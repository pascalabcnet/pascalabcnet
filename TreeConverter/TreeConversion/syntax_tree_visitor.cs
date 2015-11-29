﻿// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Посетитель синтаксического дерева.
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeRealization;

using SyntaxTreeBuilder = PascalABCCompiler.SyntaxTree.SyntaxTreeBuilder;
using SymTable = SymbolTable;
using array_const = PascalABCCompiler.TreeRealization.array_const;
using compiler_directive = PascalABCCompiler.TreeRealization.compiler_directive;
using empty_statement = PascalABCCompiler.TreeRealization.empty_statement;
using for_node = PascalABCCompiler.TreeRealization.for_node;
using goto_statement = PascalABCCompiler.TreeRealization.goto_statement;
using if_node = PascalABCCompiler.TreeRealization.if_node;
using labeled_statement = PascalABCCompiler.TreeRealization.labeled_statement;
using question_colon_expression = PascalABCCompiler.TreeRealization.question_colon_expression;
using repeat_node = PascalABCCompiler.TreeRealization.repeat_node;
using sizeof_operator = PascalABCCompiler.TreeRealization.sizeof_operator;
using typeof_operator = PascalABCCompiler.TreeRealization.typeof_operator;
using while_node = PascalABCCompiler.TreeRealization.while_node;
using TreeConverter.LambdaExpressions.Closure;
using TreeConverter.LambdaExpressions;

namespace PascalABCCompiler.TreeConverter
{
    // Сахарный узел синтаксического дерева. Не генерируется автоматически, поскольку содержит ссылку на узел семантического дерева
    // Нужен только при генерации синтаксических узлов на семантическом уровне, т.е. для синтаксического сахара
/*    public class semantic_type_node : SyntaxTree.type_definition
    {
        public type_node type { get; set; }
        public semantic_type_node(type_node t)
        {
            type = t;
        }
    }*/

    public class syntax_tree_visitor : SyntaxTree.AbstractVisitor
    {
        public convertion_data_and_alghoritms convertion_data_and_alghoritms;

        internal returner ret;

        internal document current_document;

        public compilation_context context;
		public ContextChanger contextChanger;
		
        internal using_namespace_list using_list = new using_namespace_list();
        internal using_namespace_list interface_using_list = new using_namespace_list();

        // SSM 6/08/15 Стек для хранения вложенных лямбд для одной цели - вычислить истинный тип выражения, присваиваемого Result. Это нужно для выбора перегруженной функции, у которой параметр лямбда и один из типов возвращаемого значения в точности совпадает с типом возвращаемого значения лямбды
        internal Stack<function_lambda_definition> stflambda = new Stack<function_lambda_definition>(); 

        //TODO: Загнать в request.
        motivation_keeper motivation_keeper = new motivation_keeper();

        private PascalABCCompiler.TreeRealization.unit_node_list _referenced_units;

        private common_unit_node _compiled_unit;

        private common_unit_node _system_unit;
		internal bool debug=true;
		internal bool debugging=false;
        internal List<Errors.Error> ErrorsList;
        internal List<Errors.CompilerWarning> WarningsList;
		internal Dictionary<SyntaxTree.syntax_tree_node,string> docs;
        public bool for_native_code = false;
        internal Dictionary<SyntaxTree.syntax_tree_node, SyntaxTree.compiler_directive> DirectivesToNodesLinks;
        public bool ThrowCompilationError = true;
        public bool MustVisitBody = true;
        public LambdaProcessingState lambdaProcessingState = LambdaProcessingState.None; //lroman

        #region (MikhailoMMX) Положение визитора относительно параллельных конструкций
        public ParallelPosition CurrentParallelPosition = ParallelPosition.Outside;
        #endregion

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        internal Errors.Error LastError()
        {
            Errors.Error err = ErrorsList[ErrorsList.Count-1];
            ErrorsList.RemoveAt(ErrorsList.Count - 1);
            return err;
        }

        internal void RemoveLastError()
        {
            ErrorsList.RemoveAt(ErrorsList.Count - 1);
        }

        internal void AddError(Errors.Error err, bool shouldReturn=false)
        {
            if (ThrowCompilationError || !shouldReturn /*|| err.MustThrow && !shouldReturn*/)
            {
                throw err;
            }
            else
            {
                ErrorsList.Add(err);
            }
        }

        internal void AddError(location loc, string ErrResourceString, params object[] values)
        {
            ErrorsList.Add(new SimpleSemanticError(loc, ErrResourceString, values));
        }

        internal void AddWarning(Errors.CompilerWarning err)
        {
            WarningsList.Add(err);
        }
		 
        private bool _record_created = false;
		
        private bool with_class_name=false;

        internal bool from_pabc_dll = false;

        private bool type_section_converting = false;

        private Dictionary<expression_node, expressions_list> set_intls = new Dictionary<expression_node, expressions_list>();

        //(ssyy) Дополнительные ограничения на параметры generic-типа.
        //Например, возможность объявить указатель на этот тип-параметр.
        internal Dictionary<type_node, GenericParameterAbilities> generic_param_abilities =
            new Dictionary<type_node, GenericParameterAbilities>();

        private GenericParameterAbilities get_type_abilities(type_node t)
        {
            GenericParameterAbilities gpa;
            if (generic_param_abilities.TryGetValue(t, out gpa)) return gpa;
            gpa = new GenericParameterAbilities();
            generic_param_abilities.Add(t, gpa);
            return gpa;
        }

        private void internal_reset()
        {
            PascalABCCompiler.SystemLibrary.SystemLibInitializer.initialization_properties init_properties =
                new PascalABCCompiler.SystemLibrary.SystemLibInitializer.initialization_properties();

            init_properties.break_executor = make_break_node;
            init_properties.continue_executor = make_continue_node;
            init_properties.exit_executor = make_exit_node;

            _system_unit = SystemLibrary.SystemLibInitializer.make_system_unit(convertion_data_and_alghoritms.symbol_table,
                init_properties);
            SystemLibrary.SystemLibrary.system_unit = _system_unit;
            generic_convertions.reset_generics();
            _record_created = false;
            RefTypesForCheckPointersTypeForDotNetFramework.Clear();
            reset_for_interface();
            _is_interface_part = false;
            with_class_name=false;
            generic_param_abilities.Clear();
            current_converted_method_not_in_class_defined = false;
            assign_is_converting = false;
            motivation_keeper.reset();
            SystemLibrary.SystemLibInitializer.NeedsToRestore.Clear();
            type_section_converting = false;

            #region MikhailoMMX, реинициализация класса OpenMP
            OpenMP.InternalReset();
            CurrentParallelPosition = ParallelPosition.Outside;
            #endregion

            lambdaProcessingState = LambdaProcessingState.None; //lroman
            CapturedVariablesSubstitutionClassGenerator.Reset();
        }

        public syntax_tree_visitor()
        {
            convertion_data_and_alghoritms = new convertion_data_and_alghoritms(this);
            
            ret = new returner(this);
            context = new compilation_context(convertion_data_and_alghoritms, this);
			contextChanger = new ContextChanger(context);
            internal_reset();
        }


        private void release_system_module()
        {
            SystemUnitAssigned = false;
            SystemLibrary.SystemLibInitializer.format_function = null;
            SystemLibrary.SystemLibInitializer.read_procedure = null;
            SystemLibrary.SystemLibInitializer.write_procedure = null;
            SystemLibrary.SystemLibInitializer.writeln_procedure = null;
            SystemLibrary.SystemLibInitializer.readln_procedure = null;
            SystemLibrary.SystemLibInitializer.TextFileType = null;
            SystemLibrary.SystemLibInitializer.TextFileInitProcedure = null;
            SystemLibrary.SystemLibInitializer.BinaryFileType = null;
            SystemLibrary.SystemLibInitializer.AbstractBinaryFileType = null;
            SystemLibrary.SystemLibInitializer.PointerOutputType = null;
            SystemLibrary.SystemLibInitializer.BinaryFileInitProcedure = null;
            SystemLibrary.SystemLibInitializer.BinaryFileReadProcedure = null;
            SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure = null;
            SystemLibrary.SystemLibInitializer.TypedFileType = null;
            SystemLibrary.SystemLibInitializer.TypedFileInitProcedure = null;
            SystemLibrary.SystemLibInitializer.TypedFileReadProcedure = null;
            SystemLibrary.SystemLibInitializer.ShortStringType = null;
            SystemLibrary.SystemLibInitializer.ShortStringTypeInitProcedure = null;
            SystemLibrary.SystemLibInitializer.TypedSetType = null;
            SystemLibrary.SystemLibInitializer.SetUnionProcedure = null;
            SystemLibrary.SystemLibInitializer.SetIntersectProcedure = null;
            SystemLibrary.SystemLibInitializer.SetSubtractProcedure = null;
            SystemLibrary.SystemLibInitializer.InSetProcedure = null;
            SystemLibrary.SystemLibInitializer.CreateSetProcedure = null;
            SystemLibrary.SystemLibInitializer.TypedSetInitProcedure = null;
            SystemLibrary.SystemLibInitializer.IncludeProcedure = null;
            SystemLibrary.SystemLibInitializer.ExcludeProcedure = null;
            SystemLibrary.SystemLibInitializer.DiapasonType = null;
            SystemLibrary.SystemLibInitializer.CreateDiapason = null;
            SystemLibrary.SystemLibInitializer.CreateObjDiapason = null;
            SystemLibrary.SystemLibInitializer.CompareSetEquals = null;
            SystemLibrary.SystemLibInitializer.CompareSetInEquals = null;
            SystemLibrary.SystemLibInitializer.CompareSetLess = null;
            SystemLibrary.SystemLibInitializer.CompareSetLessEqual = null;
            SystemLibrary.SystemLibInitializer.CompareSetGreater = null;
            SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual = null;
            SystemLibrary.SystemLibInitializer.IncProcedure = null;
            SystemLibrary.SystemLibInitializer.DecProcedure = null;
            SystemLibrary.SystemLibInitializer.SuccFunction = null;
            SystemLibrary.SystemLibInitializer.PredFunction = null;
            SystemLibrary.SystemLibInitializer.OrdFunction = null;
            SystemLibrary.SystemLibInitializer.TypedSetInitProcedureWithBounds = null;
            SystemLibrary.SystemLibInitializer.TypedSetInitWithShortString = null;
            SystemLibrary.SystemLibInitializer.AssignSetProcedure = null;
            SystemLibrary.SystemLibInitializer.AssignSetProcedureWithBounds = null;
            SystemLibrary.SystemLibInitializer.ClipProcedure = null;
            SystemLibrary.SystemLibInitializer.ClipFunction = null;
            SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction = null;
            SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure = null;
            SystemLibrary.SystemLibInitializer.ClipShortStringProcedure = null;
            SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure = null;
            SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure = null;
            SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure = null;
            SystemLibrary.SystemLibInitializer.SetLengthProcedure = null;
            SystemLibrary.SystemLibInitializer.read_short_string_procedure = null;
            SystemLibrary.SystemLibInitializer.read_short_string_from_file_procedure = null;
            SystemLibrary.SystemLibInitializer.InsertProcedure = null;
            SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure = null;
            SystemLibrary.SystemLibInitializer.DeleteProcedure = null;
            SystemLibrary.SystemLibInitializer.LowFunction = null;
            SystemLibrary.SystemLibInitializer.HighFunction = null;
            SystemLibrary.SystemLibInitializer.CheckCanUsePointerOnTypeProcedure = null;
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForBinaryFilesProcedure = null;
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForTypedFilesProcedure = null;
            SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction = null;
            SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction = null;
            SystemLibrary.SystemLibInitializer.PointerToStringFunction = null;
            SystemLibrary.SystemLibInitializer.GetRuntimeSizeFunction = null;
            SystemLibrary.SystemLibInitializer.PointerOutputConstructor = null;
            SystemLibrary.SystemLibInitializer.StrProcedure = null;
            SystemLibrary.SystemLibInitializer.ChrUnicodeFunction = null;
            SystemLibrary.SystemLibInitializer.AssertProcedure = null;
            SystemLibrary.SystemLibInitializer.CheckRangeFunction = null;
            SystemLibrary.SystemLibInitializer.CheckCharRangeFunction = null;
            SystemLibrary.SystemLibInitializer.CopyWithSizeFunction = null;
            SystemLibrary.SystemLibInitializer.ArrayCopyFunction = null;
            SystemLibrary.SystemLibInitializer.ObjectType = null;
            SystemLibrary.SystemLibInitializer.StringType = null;
            SystemLibrary.SystemLibInitializer.ConfigVariable = null;
        }
        
        internal bool SystemUnitAssigned = false;
        private bool typed_set_operators_added = false;
        
        public static void init_system_module(common_unit_node psystem_unit)
        {
        	SystemLibrary.SystemLibInitializer.format_function = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.format_procedure_name);
            SystemLibrary.SystemLibInitializer.read_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_procedure_name);
            SystemLibrary.SystemLibInitializer.write_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.write_procedure_name);
            SystemLibrary.SystemLibInitializer.writeln_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.writeln_procedure_name);
            SystemLibrary.SystemLibInitializer.readln_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.readln_procedure_name);
            SystemLibrary.SystemLibInitializer.TextFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.text_file_name_type_name);
            SystemLibrary.SystemLibInitializer.TextFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TextFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.BinaryFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileTypeName);
            SystemLibrary.SystemLibInitializer.AbstractBinaryFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AbstractBinaryFileTypeName);
            SystemLibrary.SystemLibInitializer.PointerOutputType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PointerOutputTypeName);
            SystemLibrary.SystemLibInitializer.BinaryFileReadProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileReadProcedureName);
            SystemLibrary.SystemLibInitializer.BinaryFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.StringDefaultPropertySetProcedureName);
            SystemLibrary.SystemLibInitializer.TypedFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileTypeName);
            SystemLibrary.SystemLibInitializer.TypedFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.TypedFileReadProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileReadProcedureName);
            SystemLibrary.SystemLibInitializer.ClipShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortString);
            SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.GetCharInShortString);
            SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.SetCharInShortString);
            //SystemLibrary.SystemLibInitializer.ShortStringType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ShortStringTypeName);
            //SystemLibrary.SystemLibInitializer.ShortStringTypeInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ShortStringTypeInitProcedure);
            SystemLibrary.SystemLibInitializer.TypedSetType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_name);
            SystemLibrary.SystemLibInitializer.TypedSetInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitProcedure);
            SystemLibrary.SystemLibInitializer.SetUnionProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.union_of_set);
            SystemLibrary.SystemLibInitializer.SetIntersectProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.intersect_of_set);
            SystemLibrary.SystemLibInitializer.SetSubtractProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.subtract_of_set);
            SystemLibrary.SystemLibInitializer.InSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.in_set);
            SystemLibrary.SystemLibInitializer.CreateSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateSetProcedure);
            SystemLibrary.SystemLibInitializer.IncludeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.IncludeProcedure);
            SystemLibrary.SystemLibInitializer.ExcludeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ExcludeProcedure);
            SystemLibrary.SystemLibInitializer.DiapasonType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.DiapasonType);
            SystemLibrary.SystemLibInitializer.CreateDiapason = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateDiapason);
            SystemLibrary.SystemLibInitializer.CreateObjDiapason = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateObjDiapason);
            SystemLibrary.SystemLibInitializer.CompareSetEquals = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetEquals);
            SystemLibrary.SystemLibInitializer.CompareSetInEquals = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetInEquals);
            SystemLibrary.SystemLibInitializer.CompareSetLess = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetLess);
            SystemLibrary.SystemLibInitializer.CompareSetLessEqual = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetLessEqual);
            SystemLibrary.SystemLibInitializer.CompareSetGreater = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetGreater);
            SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetGreaterEqual);
            SystemLibrary.SystemLibInitializer.IncProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.IncProcedure);
            SystemLibrary.SystemLibInitializer.DecProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.DecProcedure);
            SystemLibrary.SystemLibInitializer.SuccFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.SuccFunction);
			SystemLibrary.SystemLibInitializer.PredFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PredFunction);            
			SystemLibrary.SystemLibInitializer.OrdFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.OrdFunction);
			SystemLibrary.SystemLibInitializer.TypedSetInitProcedureWithBounds = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitProcedureWithBounds);
			SystemLibrary.SystemLibInitializer.TypedSetInitWithShortString = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitWithShortString);
			SystemLibrary.SystemLibInitializer.AssignSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssignSetProcedure);
			SystemLibrary.SystemLibInitializer.AssignSetProcedureWithBounds = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssignSetProcedureWithBounds);
			SystemLibrary.SystemLibInitializer.ClipProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipProcedure);
			SystemLibrary.SystemLibInitializer.ClipFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipFunction);
			SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortStringInSetFunction);
			SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortStringInSetProcedure);
			SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_length_for_short_string);
			SystemLibrary.SystemLibInitializer.SetLengthProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_length_procedure_name);
			SystemLibrary.SystemLibInitializer.read_short_string_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_short_string);
            SystemLibrary.SystemLibInitializer.read_short_string_from_file_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_short_string_from_file);
            SystemLibrary.SystemLibInitializer.InsertProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit,compiler_string_consts.Insert);
            SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit,compiler_string_consts.InsertInShortString);
            SystemLibrary.SystemLibInitializer.DeleteProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit,compiler_string_consts.Delete);
			SystemLibrary.SystemLibInitializer.LowFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit,compiler_string_consts.Low);
            SystemLibrary.SystemLibInitializer.HighFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit,compiler_string_consts.High);
            SystemLibrary.SystemLibInitializer.CheckCanUsePointerOnTypeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUsePointerOnType_proc_name);
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForBinaryFilesProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUseTypeForBinaryFiles_proc_name);
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForTypedFilesProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUseTypeForTypedFiles_proc_name);
            SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.RuntimeDetermineType_func_name);
            SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.RuntimeInitializeFunction_func_name);
            SystemLibrary.SystemLibInitializer.PointerToStringFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PointerToStringFunction_func_name);
            SystemLibrary.SystemLibInitializer.GetRuntimeSizeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.GetRuntimeSizeFunction_func_name);
            SystemLibrary.SystemLibInitializer.StrProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.StrProcedure_func_name);
            SystemLibrary.SystemLibInitializer.ChrUnicodeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ChrUnicodeFunction_func_name);
            SystemLibrary.SystemLibInitializer.AssertProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssertProcedure);
            SystemLibrary.SystemLibInitializer.CheckRangeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.check_in_range);
            SystemLibrary.SystemLibInitializer.CheckCharRangeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.check_in_range_char);
            SystemLibrary.SystemLibInitializer.CopyWithSizeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CopyWithSizeFunction);
            SystemLibrary.SystemLibInitializer.ArrayCopyFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ArrayCopyFunction);
            SystemLibrary.SystemLibInitializer.ConfigVariable = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.config_variable_name);

            //AddSpecialOperatorsToSetType();
			//SystemLibrary.SystemLibrary.make_type_conversion(SystemLibrary.SystemLibInitializer.ShortStringType.sym_info as type_node,SystemLibrary.SystemLibrary.string_type,type_compare.less_type,SemanticTree.basic_function_type.none,true);
			//SystemLibrary.SystemLibrary.make_type_conversion(SystemLibrary.SystemLibrary.string_type,SystemLibrary.SystemLibInitializer.ShortStringType.sym_info as type_node,type_compare.greater_type,SemanticTree.basic_function_type.none,true);
			if(SystemLibrary.SystemLibInitializer.TextFileType.Found)
                SystemLibrary.SystemLibInitializer.TextFileType.TypeNode.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
            if (SemanticRules.GenerateNativeCode)
            {
                SystemLibrary.SystemLibInitializer.ObjectType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ObjectType);
                if (SystemLibrary.SystemLibInitializer.ObjectType.Found)
                    SemanticRules.ClassBaseType = SystemLibrary.SystemLibInitializer.ObjectType.sym_info as TreeRealization.type_node;
                SystemLibrary.SystemLibInitializer.StringType = new PascalABCCompiler.SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.StringType);
                if (SystemLibrary.SystemLibInitializer.StringType.Found)
                    SemanticRules.StringType = SystemLibrary.SystemLibInitializer.StringType.sym_info as TreeRealization.type_node;
            }
        }

        public static void init_system_module_from_dll(dot_net_unit_node psystem_unit)
        {
            SystemLibrary.SystemLibInitializer.format_function = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.format_procedure_name);
            SystemLibrary.SystemLibInitializer.read_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_procedure_name);
            SystemLibrary.SystemLibInitializer.write_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.write_procedure_name);
            SystemLibrary.SystemLibInitializer.writeln_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.writeln_procedure_name);
            SystemLibrary.SystemLibInitializer.readln_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.readln_procedure_name);
            SystemLibrary.SystemLibInitializer.TextFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.text_file_name_type_name);
            SystemLibrary.SystemLibInitializer.TextFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TextFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.BinaryFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileTypeName);
            SystemLibrary.SystemLibInitializer.AbstractBinaryFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AbstractBinaryFileTypeName);
            SystemLibrary.SystemLibInitializer.PointerOutputType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PointerOutputTypeName);
            SystemLibrary.SystemLibInitializer.BinaryFileReadProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileReadProcedureName);
            SystemLibrary.SystemLibInitializer.BinaryFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.BinaryFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.StringDefaultPropertySetProcedureName);
            SystemLibrary.SystemLibInitializer.TypedFileType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileTypeName);
            SystemLibrary.SystemLibInitializer.TypedFileInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileInitProcedureName);
            SystemLibrary.SystemLibInitializer.TypedFileReadProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedFileReadProcedureName);
            SystemLibrary.SystemLibInitializer.ClipShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortString);
            SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.GetCharInShortString);
            SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.SetCharInShortString);
            //SystemLibrary.SystemLibInitializer.ShortStringType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ShortStringTypeName);
            //SystemLibrary.SystemLibInitializer.ShortStringTypeInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ShortStringTypeInitProcedure);
            SystemLibrary.SystemLibInitializer.TypedSetType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_name);
            SystemLibrary.SystemLibInitializer.TypedSetInitProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitProcedure);
            SystemLibrary.SystemLibInitializer.SetUnionProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.union_of_set);
            SystemLibrary.SystemLibInitializer.SetIntersectProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.intersect_of_set);
            SystemLibrary.SystemLibInitializer.SetSubtractProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.subtract_of_set);
            SystemLibrary.SystemLibInitializer.InSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.in_set);
            SystemLibrary.SystemLibInitializer.CreateSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateSetProcedure);
            SystemLibrary.SystemLibInitializer.IncludeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.IncludeProcedure);
            SystemLibrary.SystemLibInitializer.ExcludeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ExcludeProcedure);
            SystemLibrary.SystemLibInitializer.DiapasonType = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.DiapasonType);
            SystemLibrary.SystemLibInitializer.CreateDiapason = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateDiapason);
            SystemLibrary.SystemLibInitializer.CreateObjDiapason = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CreateObjDiapason);
            SystemLibrary.SystemLibInitializer.CompareSetEquals = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetEquals);
            SystemLibrary.SystemLibInitializer.CompareSetInEquals = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetInEquals);
            SystemLibrary.SystemLibInitializer.CompareSetLess = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetLess);
            SystemLibrary.SystemLibInitializer.CompareSetLessEqual = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetLessEqual);
            SystemLibrary.SystemLibInitializer.CompareSetGreater = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetGreater);
            SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CompareSetGreaterEqual);
            SystemLibrary.SystemLibInitializer.IncProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.IncProcedure);
            SystemLibrary.SystemLibInitializer.DecProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.DecProcedure);
            SystemLibrary.SystemLibInitializer.SuccFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.SuccFunction);
            SystemLibrary.SystemLibInitializer.PredFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PredFunction);
            SystemLibrary.SystemLibInitializer.OrdFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.OrdFunction);
            SystemLibrary.SystemLibInitializer.TypedSetInitProcedureWithBounds = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitProcedureWithBounds);
            SystemLibrary.SystemLibInitializer.TypedSetInitWithShortString = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.TypedSetInitWithShortString);
            SystemLibrary.SystemLibInitializer.AssignSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssignSetProcedure);
            SystemLibrary.SystemLibInitializer.AssignSetProcedureWithBounds = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssignSetProcedureWithBounds);
            SystemLibrary.SystemLibInitializer.ClipProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipProcedure);
            SystemLibrary.SystemLibInitializer.ClipFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipFunction);
            SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortStringInSetFunction);
            SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ClipShortStringInSetProcedure);
            SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_length_for_short_string);
            SystemLibrary.SystemLibInitializer.SetLengthProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.set_length_procedure_name);
            SystemLibrary.SystemLibInitializer.read_short_string_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_short_string);
            SystemLibrary.SystemLibInitializer.read_short_string_from_file_procedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.read_short_string_from_file);
            SystemLibrary.SystemLibInitializer.InsertProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.Insert);
            SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.InsertInShortString);
            SystemLibrary.SystemLibInitializer.DeleteProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.Delete);
            SystemLibrary.SystemLibInitializer.LowFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.Low);
            SystemLibrary.SystemLibInitializer.HighFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.High);
            SystemLibrary.SystemLibInitializer.CheckCanUsePointerOnTypeProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUsePointerOnType_proc_name);
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForBinaryFilesProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUseTypeForBinaryFiles_proc_name);
            SystemLibrary.SystemLibInitializer.CheckCanUseTypeForTypedFilesProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CheckCanUseTypeForTypedFiles_proc_name);
            SystemLibrary.SystemLibInitializer.RuntimeDetermineTypeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.RuntimeDetermineType_func_name);
            SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.RuntimeInitializeFunction_func_name);
            SystemLibrary.SystemLibInitializer.PointerToStringFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.PointerToStringFunction_func_name);
            SystemLibrary.SystemLibInitializer.GetRuntimeSizeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.GetRuntimeSizeFunction_func_name);
            SystemLibrary.SystemLibInitializer.StrProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.StrProcedure_func_name);
            SystemLibrary.SystemLibInitializer.ChrUnicodeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ChrUnicodeFunction_func_name);
            SystemLibrary.SystemLibInitializer.AssertProcedure = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.AssertProcedure);
            SystemLibrary.SystemLibInitializer.CheckRangeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.check_in_range);
            SystemLibrary.SystemLibInitializer.CheckCharRangeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.check_in_range_char);
            SystemLibrary.SystemLibInitializer.CopyWithSizeFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.CopyWithSizeFunction);
            SystemLibrary.SystemLibInitializer.ArrayCopyFunction = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.ArrayCopyFunction);
            SystemLibrary.SystemLibInitializer.ConfigVariable = new SystemLibrary.UnitDefinitionItem(psystem_unit, compiler_string_consts.config_variable_name);
            //AddSpecialOperatorsToSetType();
            //SystemLibrary.SystemLibrary.make_type_conversion(SystemLibrary.SystemLibInitializer.ShortStringType.sym_info as type_node,SystemLibrary.SystemLibrary.string_type,type_compare.less_type,SemanticTree.basic_function_type.none,true);
            //SystemLibrary.SystemLibrary.make_type_conversion(SystemLibrary.SystemLibrary.string_type,SystemLibrary.SystemLibInitializer.ShortStringType.sym_info as type_node,type_compare.greater_type,SemanticTree.basic_function_type.none,true);
            if (SystemLibrary.SystemLibInitializer.TextFileType.Found)
                SystemLibrary.SystemLibInitializer.TextFileType.TypeNode.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
        }

        private void get_system_module(common_unit_node psystem_unit)
        {
        	init_system_module(psystem_unit);
        	//esli zapustili v otladke, to vosstanovim mnozhestvo i procedury sozdanija diapasonov, inache ne budet rabotat
        	if (debugging)
        	{
        		SymbolInfo si = SystemLibrary.SystemLibInitializer.CreateDiapason.SymbolInfo;
        		si = SystemLibrary.SystemLibInitializer.CreateObjDiapason.SymbolInfo;
        		si = SystemLibrary.SystemLibInitializer.TypedSetType.SymbolInfo;
        	}
            //if (SystemLibrary.SystemLibInitializer.TextFileType.Found)
            //	SystemLibrary.SystemLibInitializer.TextFileType.GetTypeNodeSpecials().type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
            SystemUnitAssigned = true;
            CreateSpecialFields(psystem_unit);
        }

        private void get_system_module_from_dll(dot_net_unit_node psystem_unit)
        {
            init_system_module_from_dll(psystem_unit);
            //esli zapustili v otladke, to vosstanovim mnozhestvo i procedury sozdanija diapasonov, inache ne budet rabotat
            if (debugging)
            {
                SymbolInfo si = SystemLibrary.SystemLibInitializer.CreateDiapason.SymbolInfo;
                si = SystemLibrary.SystemLibInitializer.CreateObjDiapason.SymbolInfo;
                si = SystemLibrary.SystemLibInitializer.TypedSetType.SymbolInfo;
            }
            //if (SystemLibrary.SystemLibInitializer.TextFileType.Found)
            //	SystemLibrary.SystemLibInitializer.TextFileType.GetTypeNodeSpecials().type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
            SystemUnitAssigned = true;
            //CreateSpecialFields(psystem_unit);
        }

        private void AddSpecialOperatorsToSetType()
        {
            if (SystemLibrary.SystemLibInitializer.TypedSetType.NotFound)
                return;
            common_type_node tctn = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as common_type_node;
            tctn.type_special_kind = SemanticTree.type_special_kind.base_set_type;
            tctn.scope.AddSymbol(compiler_string_consts.plus_name, SystemLibrary.SystemLibInitializer.SetUnionProcedure.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.mul_name, SystemLibrary.SystemLibInitializer.SetIntersectProcedure.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.in_name, SystemLibrary.SystemLibInitializer.InSetProcedure.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.minus_name, SystemLibrary.SystemLibInitializer.SetSubtractProcedure.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.gr_name, SystemLibrary.SystemLibInitializer.CompareSetGreater.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.greq_name, SystemLibrary.SystemLibInitializer.CompareSetGreaterEqual.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.sm_name, SystemLibrary.SystemLibInitializer.CompareSetLess.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.smeq_name, SystemLibrary.SystemLibInitializer.CompareSetLessEqual.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.eq_name, SystemLibrary.SystemLibInitializer.CompareSetEquals.SymbolInfo);
            tctn.scope.AddSymbol(compiler_string_consts.noteq_name, SystemLibrary.SystemLibInitializer.CompareSetInEquals.SymbolInfo);
        	typed_set_operators_added = true;
        }

        private void CreateSpecialFields(common_unit_node psystem_unit)
        {
            /*SymbolInfo si = psystem_unit.scope.Find(compiler_string_consts.CommandLineArgsVariableName);
            if (si != null && si.sym_info is namespace_variable)
            {
                //location loc = (si.sym_info as namespace_variable).Location
                common_parameter args = new common_parameter(compiler_string_consts.MainArgsParamName, SystemLibrary.SystemLibrary.array_of_string, SemanticTree.parameter_type.value, null, concrete_parameter_type.cpt_none, null, null);
                namespace_variable_reference nvr = new namespace_variable_reference(si.sym_info as namespace_variable, null);
                psystem_unit.MainArgsParameter = args;
                psystem_unit.MainArgsAssignExpr = make_assign_operator(nvr, new common_parameter_reference(args, 0, null), null);
            }*/
            SymbolInfo si = psystem_unit.scope.Find(compiler_string_consts.IsConsoleApplicationVariableName);
            if (si != null && si.sym_info is namespace_variable)
            {
                namespace_variable_reference nvr = new namespace_variable_reference(si.sym_info as namespace_variable, null);
                psystem_unit.IsConsoleApplicationVariableAssignExpr = make_assign_operator(nvr, psystem_unit.IsConsoleApplicationVariableValue, null);
            }
        }

        private void UpdateUnitDefinitionItemForUnit(common_unit_node unit)
        {
            if (!from_pabc_dll)
            {
                SystemLibrary.SystemLibInitializer.read_procedure = new SystemLibrary.UnitDefinitionItem(unit, compiler_string_consts.read_procedure_name);
                SystemLibrary.SystemLibInitializer.readln_procedure = new SystemLibrary.UnitDefinitionItem(unit, compiler_string_consts.readln_procedure_name);
            }
            else
            {
                //SystemLibrary.SystemLibInitializer.read_procedure = new SystemLibrary.UnitDefinitionItem(unit, compiler_string_consts.read_procedure_name);
                //SystemLibrary.SystemLibInitializer.readln_procedure = new SystemLibrary.UnitDefinitionItem(unit, compiler_string_consts.readln_procedure_name);
            }
        }

        public void reset()
        {
            _system_unit = null;
            SystemLibrary.SystemLibrary.system_unit = null;
            //SystemLibrary.SystemLibrary.Reset();
            SystemUnitAssigned = false;
            _compiled_unit = null;
            _referenced_units = null;
            current_document = null;
            using_list.clear();
            //_referenced_units.clear();
            ret.reset();
            motivation_keeper.reset();
            context.reset();
            convertion_data_and_alghoritms.reset();
            //compiled_type_node.compiled_types.Clear();
            internal_reset();
            release_system_module();
            WaitedRefTypes.Clear();
            set_intls.Clear();
            NetHelper.NetHelper.reset();
            from_pabc_dll = false;
            compiled_type_node[] ctns = new compiled_type_node[compiled_type_node.compiled_types.Values.Count];
            compiled_type_node.compiled_types.Values.CopyTo(ctns, 0);
            null_type_node.reset();
            foreach (compiled_type_node ctn in ctns)
            {
                ctn.reinit_scope();
                ctn.clear_generated_names();
                ctn.clear_generated_intersections();
            }
        }

        public PascalABCCompiler.Errors.SyntaxError parser_error
        {
            get
            {
                return convertion_data_and_alghoritms.parser_error;
            }
            set
            {
                convertion_data_and_alghoritms.parser_error = value;
            }
        }
        public System.Collections.Hashtable bad_nodes_in_syntax_tree
        {
            get
            {
                return convertion_data_and_alghoritms.bad_nodes_in_syntax_tree;
            }
            set
            {
                convertion_data_and_alghoritms.bad_nodes_in_syntax_tree = value;
            }

        }

        public PascalABCCompiler.TreeRealization.document CurrentDocument
        {
            get
            {
                return current_document;
            }
        }

        public SymbolTable.TreeConverterSymbolTable SymbolTable
        {
            get
            {
                return convertion_data_and_alghoritms.symbol_table;
            }
        }

        public location get_location(SyntaxTree.syntax_tree_node tn)
        {
            if (tn.source_context == null)
            {
                return null;
            }
            document d = current_document;
            if (tn.source_context.FileName != null && (d == null || d.file_name != tn.source_context.FileName))
                d = new document(tn.source_context.FileName);
            return new location(tn.source_context.begin_position.line_num, tn.source_context.begin_position.column_num,
                tn.source_context.end_position.line_num, tn.source_context.end_position.column_num, d);
        }
        public location get_right_location(SyntaxTree.syntax_tree_node tn)
        {
            if (tn.source_context == null)
            {
                return null;
            }
            return new location(tn.source_context.end_position.line_num, tn.source_context.end_position.column_num,
                tn.source_context.end_position.line_num, tn.source_context.end_position.column_num, current_document);
        }

        public location get_location_with_check(SyntaxTree.syntax_tree_node tn)
        {
            if (tn == null || tn.source_context == null)
            {
                return null;
            }
            document d = current_document;
            if (tn.source_context.FileName != null)
                d = new document(tn.source_context.FileName);
            return new location(tn.source_context.begin_position.line_num, tn.source_context.begin_position.column_num,
                tn.source_context.end_position.line_num, tn.source_context.end_position.column_num, d);
        }

        public common_unit_node compiled_unit
        {
            get
            {
                return _compiled_unit;
            }
            //TODO: Нужен ли set?
            set
            {
                _compiled_unit = value;
            }
        }

        public PascalABCCompiler.TreeRealization.unit_node_list referenced_units
        {
            get
            {
                return _referenced_units;
            }
            set
            {
                _referenced_units = value;
            }
        }

        //TODO: Может вынести всю работу с returner-ами в convertion_data_and_alghoritms?
        //Методы returner-а.
        //Запрос на построение и получение конвертированного поддерева.
        //Нежесткая проверка существования узла.
        private statement_node convert_weak(SyntaxTree.statement st)
        {
            if (st == null)
            {
                return null;
            }

            convertion_data_and_alghoritms.check_node_parser_error(st);

            //st = prepare_statement(st);

            statement_node sn = ret.visit(st);
            //sn.loc=get_location(st);
            return sn;
        }

        private expression_node convert_weak(SyntaxTree.expression expr)
        {
            if (expr == null)
            {
                return null;
            }

            convertion_data_and_alghoritms.check_node_parser_error(expr);

            //Посмотреть, может вызвать ошибку
            //ssyy
            motivation_keeper.reset();
            //\ssyy

            expression_node en = ret.visit(expr);
            //en.loc=get_location(expr);
            return en;
        }

        private type_node convert_weak(SyntaxTree.type_definition type_def)
        {
            if (type_def == null)
            {
                return null;
            }

            convertion_data_and_alghoritms.check_node_parser_error(type_def);

            return ret.visit(type_def);
        }

        //Жесткая проверка существования узла. Проверка будет осуществлятся только в DEBUG версии.
        //Будем верить, что в RELEAS-е все будет работать правильно.
        private semantic_node convert_semantic_strong(SyntaxTree.syntax_tree_node tn)
        {
#if (DEBUG)
            if (tn == null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            convertion_data_and_alghoritms.check_node_parser_error(tn);
            motivation_keeper.set_motivation_to_except_semantic_node();
            return ret.visit(tn);
        }

        //(ssyy) Убрал ввиду бесполезности
        /*private SymbolInfo convert_symbol_strong(SyntaxTree.syntax_tree_node tn)
        {
#if (DEBUG)
            if (tn==null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            convertion_data_and_alghoritms.check_node_parser_error(tn);
			
            //motivation_keeper.set_motivation_to_except_symbol();
            return ret.visit_symbol_info(tn);
        }*/

        //(ssyy) DS, сделай нормальное дерево! // 2015 год - проблема по-прежнему актуальна: expression не должно быть потомком statement 
        /*private SyntaxTree.statement prepare_statement(SyntaxTree.statement st) // логика: если это вызов функции, то сделать из него вызов процедуры. И это - единственное место для преобразования
        {
            SyntaxTree.method_call mc = null; // st as SyntaxTree.method_call;
            if (mc == null)
            {
                return st;
            }
            else
            {
                return new SyntaxTree.procedure_call(mc);
            }
        } */

        internal statement_node convert_strong(SyntaxTree.statement st)
        {
#if (DEBUG)
            if (st == null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            #region MikhailoMMX, обработка критических секций OpenMP
            bool isCritical = false;
            SyntaxTree.statement st_orig = st;
            if (OpenMP.LocksFound)
                if (DirectivesToNodesLinks.ContainsKey(st) && OpenMP.IsCriticalDirective(DirectivesToNodesLinks[st]))
                {
                    //если перед узлом есть директива omp critical
                    isCritical = true;
                    OpenMP.TryConvertCritical(ref st, this, DirectivesToNodesLinks[st]);
                    OpenMP.DisableDirective(st_orig, DirectivesToNodesLinks);
                }
            #endregion

            convertion_data_and_alghoritms.check_node_parser_error(st);

            //st = prepare_statement(st);
            statement_node sn = null;
            //try
            {
                sn = ret.visit(st);
                if (sn == null)
                    sn = new empty_statement(null);
            }
            /*catch (PascalABCCompiler.Errors.Error e)
            {
                if (ThrowCompilationError)
                    throw e;
                else
                {
                    ErrorsList.Add(e);
                    return new empty_statement(null);
                }
            }
            catch (Exception e)
            {
                if (ThrowCompilationError)
                    throw e;
                else
                    return new empty_statement(null);
            }*/
            //sn.loc=get_location(st);

            #region MikhailoMMX, обработка критических секций OpenMP
            if (isCritical)
                OpenMP.EnableDirective(st_orig, DirectivesToNodesLinks);
            #endregion

            return sn;
        }

        internal expression_node convert_strong(SyntaxTree.expression expr)
        {
#if (DEBUG)
            if (expr == null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            convertion_data_and_alghoritms.check_node_parser_error(expr);

            //Посмотреть, может вызвать ошибку
            //ssyy
            motivation_keeper.reset();
            //\ssyy
            expression_node en = ret.visit(expr);

            //en.loc=get_location(expr);
            return en;
        }

        internal type_node convert_strong(SyntaxTree.type_definition type_def)
        {
#if (DEBUG)
            if (type_def == null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            convertion_data_and_alghoritms.check_node_parser_error(type_def);

            type_node tn;
            SyntaxTree.function_header fh = (type_def as SyntaxTree.function_header);
            if (fh != null)
            {
                tn = convert_function_type(fh, get_location(type_def), context.get_delegate_type_name(), null);
                return tn;
            }
            else
            {
                SyntaxTree.procedure_header ph = (type_def as SyntaxTree.procedure_header);
                if (ph != null)
                {
                    tn = convert_function_type(ph, get_location(type_def), context.get_delegate_type_name(), null);
                    return tn;
                }
            }
            if (is_direct_type_decl && !(type_def is SyntaxTree.ref_type)) is_direct_type_decl = false;
            return ret.visit(type_def);
        }

        public addressed_expression convert_address_strong(SyntaxTree.addressed_value av)
        {
#if (DEBUG)
            if (av == null)
            {
                throw new CompilerInternalError("This syntax tree node can not be null.");
            }
#endif

            convertion_data_and_alghoritms.check_node_parser_error(av);

            motivation_keeper.set_motivation_to_expect_address();
            addressed_expression ae = ret.visit(av);
            //ae.loc=get_location(av);
            return ae;
        }

        //Возвращение конвертированного поддерева.
        private void return_value(statement_node stat)
        {
            ret.return_value(stat);
        }

        private void return_value(expression_node expr)
        {
            ret.return_value(expr);
        }

        private void return_value(type_node tn)
        {
            ret.return_value(tn);
        }

        private void return_addressed_value(addressed_expression ae)
        {
            ret.return_value(ae);
        }

        private void return_semantic_value(semantic_node se)
        {
            ret.return_value(se);
        }

        //(ssyy) Убрал ввиду бесполезности
        /*private void return_symbol_value(SymbolInfo si)
        {
            ret.return_symbolinfo(si);
        }*/

        public void weak_node_test_and_visit(SyntaxTree.syntax_tree_node tn)
        {
            if (tn != null)
            {
                convertion_data_and_alghoritms.check_node_parser_error(tn);
                tn.visit(this);
            }
        }

        public void hard_node_test_and_visit(SyntaxTree.syntax_tree_node tn)
        {
#if (DEBUG)
//            try
//            {
                if (tn == null)
                {
                    throw new CompilerInternalError("This node can not be null");
                }
#endif
                convertion_data_and_alghoritms.check_node_parser_error(tn);
                if (is_direct_type_decl && !(tn is SyntaxTree.ref_type))
                    is_direct_type_decl = false;
                tn.visit(this);
/*#if (DEBUG)
            }
            catch (Exception e)
            {
                if (tn != null)
                    throw new Exception("" + tn.source_context.ToString() + System.Environment.NewLine + e.ToString());
                else throw e;
            }
#endif*/
        }
		
        private void assign_doc_info(definition_node dn, SyntaxTree.syntax_tree_node stn)
        {
        	string s = null;
        	if (docs != null && docs.TryGetValue(stn, out s) && !string.IsNullOrEmpty(s))
        	{
        		dn.documentation = s;
        	}
        	return;
        }
        
        //Последний параметер - выходной. Он слдержит список параметров с преобразованиями типов.
        private function_node find_function(string name, location loc, PascalABCCompiler.TreeRealization.expressions_list parameters)
        {
            SymbolInfo si = context.find(name);
            if (si == null)
            {
                AddError(new NoFunctionWithThisName(loc));
            }
            if (si.sym_info.general_node_type != general_node_type.function_node)
            {
                AddError(new ThisIsNotFunctionName(loc));
            }
            function_node fn = convertion_data_and_alghoritms.select_function(parameters, si, loc);
            return fn;
        }

        //TODO: Обратить внимание на эту функцию. Ее вызовы не приводят к созданиям узлов преобразования типов.
        private function_node find_function(string name, location loc, params expression_node[] exprs)
        {
            expressions_list exprs_list = new expressions_list();
            exprs_list.AddRange(exprs);
            return find_function(name, loc, exprs_list);
        }

        private expression_node find_operator(SyntaxTree.Operators ot, expression_node expr, location loc)
        {
            string name = name_reflector.get_name(ot);
            //#if (DEBUG)
            if (name == null)
            {
                if (ot == PascalABCCompiler.SyntaxTree.Operators.AddressOf)
                {
                    if (expr.is_addressed == false)
                    {
                        AddError(expr.location, "CAN_NOT_GET_ADDRESS_FROM_EXPRESSION");
                    }
                    expression_node res = new get_addr_node(expr, loc);
                    return res;
                }
                if (ot == PascalABCCompiler.SyntaxTree.Operators.Dereference)
                {
                }
                throw new CompilerInternalError("Invalid operator name");
            }
            //#endif
            SymbolInfo si = expr.type.find(name, context.CurrentScope);
            if (si == null || si.sym_info is wrapped_definition_node)
            {
            	AddError(new OperatorCanNotBeAppliedToThisType(name, expr));
            }

            expressions_list pars = new expressions_list();
            pars.AddElement(expr);

            function_node fn = convertion_data_and_alghoritms.select_function(pars, si, loc);
            expr = pars[0];
            if (fn == null)
            {
            	AddError(new OperatorCanNotBeAppliedToThisType(name, expr));
            }
#if (DEBUG)
            convertion_data_and_alghoritms.check_operator(fn);
#endif
            expression_node exp_node = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, expr);
            return exp_node;
        }

        private expression_node find_operator(SyntaxTree.Operators ot, expression_node left, expression_node right, location loc)
        {
            string name = name_reflector.get_name(ot);
            return find_operator(name, left, right, loc);
        }

        bool CheckExpressionType(expression_node expr)
        {
            if (expr.type == null)
            {
                AddError(expr.location, "TYPE_NOT_DEFINED");
                return false;
            }
            return true;
        }
        
        internal bool one_way_operation(string name)
        {
        	if (name == compiler_string_consts.plusassign_name) return true;
        	if (name == compiler_string_consts.minusassign_name) return true;
        	if (name == compiler_string_consts.multassign_name) return true;
            if (name == compiler_string_consts.assign_name) return true;
            if (name == compiler_string_consts.divassign_name) return true;
        	return false;
        }
        
        public expression_node find_operator(string name, expression_node left, expression_node right, location loc)
        {
            //if (!CheckExpressionType(left))
            //    return SystemLibrary.SystemLibrary.get_empty_method_call(left.location);
            //if (!CheckExpressionType(right))
            //    return SystemLibrary.SystemLibrary.get_empty_method_call(right.location);
            //string name=name_reflector.get_name(ot);
#if (DEBUG)
            if (name == null)
            {
                throw new CompilerInternalError("Invalid operator name");
            }
#endif

            if (right.semantic_node_type == semantic_node_type.null_const_node)
            {
            	if ( !type_table.is_with_nil_allowed(left.type) && !left.type.IsPointer)
                    AddError(right.location, "NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
            	right = null_const_node.get_const_node_with_type(left.type, (null_const_node)right);
            }

            type_node left_type = left.type;
            type_node right_type = right.type;
			
            /*if (left.type == SystemLibrary.SystemLibrary.uint64_type || right.type == SystemLibrary.SystemLibrary.uint64_type)
            	if (name == compiler_string_consts.plus_name || name == compiler_string_consts.minus_name || name == compiler_string_consts.mul_name || name == compiler_string_consts.idiv_name || name == compiler_string_consts.mod_name)
            	{
                    expression_node expr = right.type != SystemLibrary.SystemLibrary.uint64_type ? right : left;
                    if (!(expr is constant_node))
                    {
                        type_node tn = expr.type;
                        if (tn == SystemLibrary.SystemLibrary.short_type || tn == SystemLibrary.SystemLibrary.sbyte_type || tn == SystemLibrary.SystemLibrary.integer_type || tn == SystemLibrary.SystemLibrary.int64_type)
                            throw new OperatorCanNotBeAppliedToThisTypes(name, left, right, loc);
                    }
            	}
            */

            if (left_type.semantic_node_type == semantic_node_type.delegated_method)
            {
                delegated_methods dm1 = (delegated_methods)left_type;
                if (dm1.empty_param_method != null)
                {
                    if (dm1.empty_param_method.simple_function_node.return_value_type != null)
                    {
                        left_type = dm1.empty_param_method.simple_function_node.return_value_type;
                    }
                }
            }

            if (right_type.semantic_node_type == semantic_node_type.delegated_method && name != compiler_string_consts.plusassign_name && name != compiler_string_consts.minusassign_name)
            {
                delegated_methods dm2 = (delegated_methods)right_type;
                if (dm2.empty_param_method != null)
                {
                    if (dm2.empty_param_method.simple_function_node.return_value_type != null)
                    {
                        right_type = dm2.empty_param_method.simple_function_node.return_value_type;
                    }
                }
            }
			
            //zdes ne vse verno. += odnostoronnjaa operacija, a += b pochemu esli tipy ne ravny, += ishetsja v tipe b???
            //TODO: Посмотреть.
            //TODO: Не find а find_in_type.
            //SymbolInfo si=left_type.find(name, context.CurrentScope);
            SymbolInfo si = left_type.find_in_type(name, left_type.Scope);
            SymbolInfo added_symbols = null;
            SymbolInfo si2 = null;
            if (left_type != right_type && !one_way_operation(name))
            {
                //SymbolInfo si2 = right_type.find(name, context.CurrentScope);
                si2 = right_type.find_in_type(name, right_type.Scope);
                if ((si != null) && (si2 != null))
                {
                    //Важная проверка. Возможно один и тот же оператор с одними и теми же типами определен в двух разных классах.
                    //Возможно она занимает много времени, но, наверное, от нее нельзя отказаться.
                    function_node_list funcs = new function_node_list();
                    SymbolInfo sic = si;
                    SymbolInfo sic_last = null;
                    while (sic != null)
                    {
                        if (sic.sym_info.general_node_type != general_node_type.function_node)
                        {
                            BasePCUReader.RestoreSymbols(sic, name);
                        }
#if (DEBUG)
                        if (sic.sym_info.general_node_type != general_node_type.function_node)
                        {
                            throw new CompilerInternalError("Expected operator.");
                        }
#endif
                        
                        function_node fn = ((function_node)(sic.sym_info));
                        if (convertion_data_and_alghoritms.is_exist_eq_method_in_list(fn, funcs) != null)
                        {
                            break;
                        }
                        funcs.AddElement(fn);
                        sic_last = sic;
                        sic = sic.Next;
                    }
                    sic = si2;
                    //TODO: Разобраться с зацикливанием.
                    function_node_list fnl = new function_node_list();
                    while (sic != null)
                    {
                        if (sic.sym_info.general_node_type != general_node_type.function_node)
                        {
                            BasePCUReader.RestoreSymbols(sic, name);
                        }
#if (DEBUG)
                        if (sic.sym_info.general_node_type != general_node_type.function_node)
                        {
                            throw new CompilerInternalError("Expected operator.");
                        }
#endif
                        fnl.AddElement(((function_node)(sic.sym_info)));
                        sic = sic.Next;
                    }
                    added_symbols = sic_last;
                    foreach (function_node fnode in fnl)
                    {
                        function_node eq_func = convertion_data_and_alghoritms.find_eq_method_in_list(fnode, funcs);
                        if (eq_func != null)
                        {
                            //TODO: Проверить правильно ли это будет работать.
                            if (eq_func != fnode)
                            {
                                basic_function_node bbfn1 = fnode as basic_function_node;
                                basic_function_node bbfn2 = eq_func as basic_function_node;
                                if ((bbfn1 != null) && (bbfn2 != null))
                                {
                                    if (bbfn1.basic_function_type != bbfn2.basic_function_type)
                                    {
                                        AddError(new TwoOperatorsCanBeCalled(eq_func, fnode, left, right));
                                    }
                                    //sic = sic.Next;
                                    continue;
                                }
                                else
                                {
                                	if (left.type.type_special_kind == SemanticTree.type_special_kind.set_type && right.type.type_special_kind == SemanticTree.type_special_kind.set_type
                                	   || left.type.type_special_kind == SemanticTree.type_special_kind.set_type &&  right.type.type_special_kind == SemanticTree.type_special_kind.base_set_type
                                	  || left.type.type_special_kind == SemanticTree.type_special_kind.base_set_type &&  right.type.type_special_kind == SemanticTree.type_special_kind.set_type
                                	 || left.type.type_special_kind == SemanticTree.type_special_kind.base_set_type &&  right.type.type_special_kind == SemanticTree.type_special_kind.base_set_type
                                	|| left.type == SystemLibrary.SystemLibrary.string_type && right.type.type_special_kind == SemanticTree.type_special_kind.short_string
                               		 || right.type == SystemLibrary.SystemLibrary.string_type && left.type.type_special_kind == SemanticTree.type_special_kind.short_string
                               		|| left.type.type_special_kind == SemanticTree.type_special_kind.short_string && right.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                                	{
                                		//if (left.type.find_in_type(name) !=null)
                                	}
                                	else
                                	AddError(new TwoOperatorsCanBeCalled(eq_func, fnode, left, right));
                                }
                            }
                        }
                        sic_last.Next = new SymbolInfo(fnode);
                        sic_last = sic_last.Next;
                    }
                    /*while (sic!=null)
                    {
#if (DEBUG)
                        if (sic.sym_info.general_node_type!=general_node_type.function_node)
                        {
                            throw new CompilerInternalError("Expected operator.");
                        }
#endif
                        function_node fn=((function_node)(sic.sym_info));
                        function_node eq_func=convertion_data_and_alghoritms.find_eq_method_in_list(fn,funcs);
                        if (eq_func!=null)
                        {
                            basic_function_node bbfn1=fn as basic_function_node;
                            basic_function_node bbfn2=eq_func as basic_function_node;
                            if ((bbfn1!=null)&&(bbfn2!=null))
                            {
                                if (bbfn1.basic_function_type!=bbfn2.basic_function_type)
                                {
                                    throw new TwoOperatorsCanBeCalled(eq_func,fn,left,right);
                                }
                                sic=sic.Next;
                                continue;
                            }
                            else
                            {
                                throw new TwoOperatorsCanBeCalled(eq_func,fn,left,right);
                            }
                        }
                        //TODO: Разобраться с зацикливанием.
                        sic_last.Next = sic;
                        sic_last = sic;
                        sic=sic.Next;
                    }*/
                    //sic_last.Next=si2;
                }
                else
                {
                    if ((si == null) && (si2 != null))
                    {
                        si = si2;
                    }
                }
            }
            if (si == null)
            {
                if (si2 == null && left_type.semantic_node_type == semantic_node_type.delegated_method && right_type.semantic_node_type == semantic_node_type.delegated_method)
                {
                    base_function_call bfc = ((left as typed_expression).type as delegated_methods).proper_methods[0];
                    left = convertion_data_and_alghoritms.explicit_convert_type(left, CreateDelegate(bfc.simple_function_node));
                    si = left.type.find_in_type(name);
                    bfc = ((right as typed_expression).type as delegated_methods).proper_methods[0];
                    right = convertion_data_and_alghoritms.explicit_convert_type(right, CreateDelegate(bfc.simple_function_node));
                    si2 = right.type.find_in_type(name);
                    if (si == null)
                        AddError(new OperatorCanNotBeAppliedToThisTypes(name, left, right, loc));
                }
                else
                    AddError(new OperatorCanNotBeAppliedToThisTypes(name, left, right, loc));
            }

            expressions_list pars = null;
            function_node fnsel = null;
            SymbolInfo tmp_si = si;
            while (tmp_si != null)
            {
                if (tmp_si.sym_info is wrapped_definition_node)
                    BasePCUReader.RestoreSymbols(tmp_si, name);
                tmp_si = tmp_si.Next;
            }
            //try
            //{
            pars = new expressions_list();
            pars.AddElement(left);
            pars.AddElement(right);

            fnsel = convertion_data_and_alghoritms.select_function(pars, si, loc);
            /*}
            catch (CanNotConvertTypes e)
            {
                if (first)
                {
                    first = false;
                    right = convert_if_typed_expression_to_function_call(right);
                }
                else
                    throw;
            }*/

            //function_node fnsel=convertion_data_and_alghoritms.select_function(pars,si,loc);
            CheckSpecialFunctionCall(si, pars,loc);
            //TODO: А это зачем? Можно передать в create_simple_function_call pars.
            left = pars[0];
            right = pars[1];
            if (fnsel == null)
            {
                AddError(new OperatorCanNotBeAppliedToThisTypes(name, left, right, loc));
            }
            if (added_symbols != null)
                added_symbols.Next = null;
            
            if (SystemUnitAssigned && in_check_range_region() && name == compiler_string_consts.assign_name && is_range_checkable(left.type) && is_range_checkable(right.type))
            {
            	ordinal_type_interface oti = left.type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
            	if (left.type != SystemLibrary.SystemLibrary.char_type && left.type != SystemLibrary.SystemLibrary.uint64_type && !(oti.lower_value is ulong_const_node))
            	if(!(oti.lower_value is enum_const_node) && !(oti.lower_value is char_const_node))
            		right = convertion_data_and_alghoritms.convert_type(convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.CheckRangeFunction.sym_info as common_namespace_function_node, null, convertion_data_and_alghoritms.convert_type(right,SystemLibrary.SystemLibrary.int64_type), convertion_data_and_alghoritms.convert_type(oti.lower_value,SystemLibrary.SystemLibrary.int64_type),convertion_data_and_alghoritms.convert_type(oti.upper_value,SystemLibrary.SystemLibrary.int64_type)),right.type);
            	else if (oti.lower_value is enum_const_node)
            	{
            		right = convertion_data_and_alghoritms.explicit_convert_type(convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.CheckRangeFunction.sym_info as common_namespace_function_node, null, convertion_data_and_alghoritms.explicit_convert_type(right,SystemLibrary.SystemLibrary.int64_type),
            		                                                                                                               convertion_data_and_alghoritms.explicit_convert_type(oti.lower_value,SystemLibrary.SystemLibrary.int64_type),convertion_data_and_alghoritms.explicit_convert_type(oti.upper_value,SystemLibrary.SystemLibrary.int64_type)),right.type);
            	}
            	else if (oti.lower_value is char_const_node)
            	right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.CheckCharRangeFunction.sym_info as common_namespace_function_node, null, right, oti.lower_value,oti.upper_value);	
            }
            expression_node exp_node = convertion_data_and_alghoritms.create_simple_function_call(fnsel, loc, left, right);
            return exp_node;
        }
		
        internal bool in_check_range_region()
        {
        	return false;
        }
        
        internal bool is_range_checkable(type_node tn)
        {
        	ordinal_type_interface oti = tn.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        	if (oti != null && !tn.IsEnum && tn != SystemLibrary.SystemLibrary.bool_type)
        		return true;
        	return false;
        }
        
        private base_function_call_list convert_functions_to_calls(expression_node obj, function_node_list fnl, location loc, bool is_static)
        {
            base_function_call_list ret = new base_function_call_list();
            foreach (function_node fnode in fnl)
            {
                base_function_call bfc = null;
                switch (fnode.semantic_node_type)
                {
                    case semantic_node_type.common_namespace_function_node:
                        {
                            common_namespace_function_node cmfn = fnode as common_namespace_function_node;
                            common_namespace_function_call cnfc = new common_namespace_function_call(cmfn, loc);
                            if (cmfn.ConnectedToType != null)
                                cnfc.parameters.AddElement(obj);
                            if (cmfn.is_generic_function && !cmfn.is_generic_function_instance && cmfn.ConnectedToType != null && cmfn.parameters.Count == 1)
                            {
                                expressions_list parameters = new expressions_list();
                                parameters.AddElement(obj);
                                function_node inst = null;
                                try
                                {
                                    inst = generic_convertions.DeduceFunction(cmfn, parameters, true, loc);
                                }
                                catch
                                {
                                    continue;
                                }
                                cnfc = new common_namespace_function_call((common_namespace_function_node)inst, loc);
                                if (cmfn.ConnectedToType != null)
                                    cnfc.parameters.AddElement(obj);
                            }
                            /*if (cmfn.parameters.Count >= 1 && cmfn.parameters[cmfn.parameters.Count - 1].is_params)
                            {
                                convertion_data_and_alghoritms.select_function(cnfc.parameters, new SymbolInfo(cmfn), loc);
                                
                            }*/
                            bfc = cnfc;
                            break;
                        }
                    case semantic_node_type.basic_function_node:
                        {
                            //Может здесь стоит и выругаться, но я не буду пока этого делать.
                            break;
                        }
                    case semantic_node_type.common_in_function_function_node:
                        {
                            common_in_function_function_node ciffn = fnode as common_in_function_function_node;
                            int depth = convertion_data_and_alghoritms.symbol_table.GetRelativeScopeDepth(ciffn.scope,
                                context.top_function.scope);
                            common_in_function_function_call ciffc = new common_in_function_function_call(ciffn, depth, loc);
                            bfc = ciffc;
                            break;
                        }
                    case semantic_node_type.common_method_node:
                        {
                            common_method_node cmn = fnode as common_method_node;
                            //Если cmn конструктор - то плохо, но его не должно сюда попасть.
                            if (cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                            {
                                if (!is_static)
                                {
                                    if (obj == null)
                                        obj = GetCurrentObjectReference(cmn.cont_type.Scope, cmn, loc);//new this_node(context.converted_type, loc);
                                    common_method_call cmc = new common_method_call(cmn, obj, loc);
                                    cmc.virtual_call = !inherited_ident_processing;
                                    bfc = cmc;
                                }
                                //ssyy!!! Мне сложно понять предназначение данного кода, но, по-видимому,
                                //следует его переписать так.
                                else if (cmn.is_constructor)
                                {
                                    if (cmn.parameters.Count == 0)
                                    {
                                    	if (cmn.cont_type.IsAbstract)
                                            AddError(loc, "ABSTRACT_CONSTRUCTOR_{0}_CALL", cmn.cont_type.name);
                                    	ret.clear();
                                        ret.AddElement(new common_constructor_call(cmn, loc));
                                        return ret;
                                    }
                                }
                                //else if (cmn.pascal_associated_constructor != null)
                                //{
                                //    if (cmn.pascal_associated_constructor.parameters.Count == 0)
                                //    {
                                //        ret.clear();
                                //        ret.AddElement(new common_constructor_call(cmn.pascal_associated_constructor,loc));
                                //        return ret;
                                //    }
                                //}
                                //\ssyy
                            }
                            else
                            {
                                if (is_static)
                                {
                                    common_static_method_call csmc = new common_static_method_call(cmn, loc);
                                    bfc = csmc;
                                }
                            }
                            break;
                        }
                    case semantic_node_type.compiled_function_node:
                        {
                            compiled_function_node cfn = fnode as compiled_function_node;
                            if (cfn.cont_type.Scope == null && cfn is compiled_function_node)
                        	(cfn.cont_type as compiled_type_node).init_scope();
                            
                            if (cfn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                            {
                                //ispravleno ibond
                            	//if (is_static)
                                {

                                    if (cfn.is_generic_function && !cfn.is_generic_function_instance && cfn.ConnectedToType != null && cfn.parameters.Count == 1)
                                    {
                                        expressions_list parameters = new expressions_list();
                                        parameters.AddElement(obj);
                                        function_node inst = null;
                                        try
                                        {
                                            inst = generic_convertions.DeduceFunction(cfn, parameters, true, loc);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        if (inst is common_namespace_function_node)
                                            bfc = new common_namespace_function_call((common_namespace_function_node)inst, loc);
                                        else if (inst is compiled_function_node)
                                            bfc = new compiled_static_method_call((compiled_function_node)inst, loc);
                                        else
                                            bfc = new compiled_static_method_call(cfn, loc);
                                    }
                                    else
                                    {
                                        compiled_static_method_call csmc = new compiled_static_method_call(cfn, loc);
                                        bfc = csmc;
                                        
                                    }
                                    if (cfn.ConnectedToType != null)
                                        bfc.parameters.AddElement(obj);
                                }
                            }
                            else
                            {
                                if (!is_static)
                                {
                                    if (obj == null)
                                        obj = GetCurrentObjectReference(cfn.cont_type.Scope, cfn, loc);//new this_node(context.converted_type, loc);
                                    compiled_function_call cfc = new compiled_function_call(cfn, obj, loc);
                                    cfc.virtual_call = !inherited_ident_processing;
                                    bfc = cfc;
                                }
                            }
                            break;
                        }
                    case semantic_node_type.compiled_constructor_node:
                        {
                            //Этот код мы вроде не должны вызывать, но если он все-же вызовется.
                            compiled_constructor_node ccn = fnode as compiled_constructor_node;
                            compiled_constructor_call ccc = new compiled_constructor_call(ccn, loc);
                            bfc = ccc;
                            break;
                        }
                    default:
                        {
                            throw new CompilerInternalError("Undefined method type.");
                        }
                }
                if (bfc != null)
                {
                    ret.AddElement(bfc);
                }
            }
            return ret;
        }

        private base_function_call_list create_possible_delegates_list(expression_node obj, SymbolInfo si, location loc, bool is_static)
        {
            if (si == null)
            {
                return new base_function_call_list();
            }
            function_node_list fnl = new function_node_list();
            while (si != null)
            {
                function_node fn = si.sym_info as function_node;
                if (si.sym_info is compiled_property_node)
                {
                    fn = (si.sym_info as compiled_property_node).get_function; 
                }
                else if (si.sym_info is common_property_node)
                {
                    fn = (si.sym_info as common_property_node).get_function;
                }
                if (convertion_data_and_alghoritms.find_eq_method_in_list(fn, fnl) == null)
                {
                    if (!(obj != null && fn.polymorphic_state == SemanticTree.polymorphic_state.ps_static && !fn.is_extension_method))
                    {
                        if (fn.is_extension_method)
                        {
                            if (fn.parameters[0].type == obj.type || type_table.compare_types(fn.parameters[0].type, obj.type) == type_compare.greater_type)
                                fnl.AddElementFirst(fn);
                            else
                                fnl.AddElement(fn);
                        }
                        else
                            fnl.AddElement(fn);
                    }
                }
                si = si.Next;
            }
            return convert_functions_to_calls(obj, fnl, loc, is_static);
        }

        private base_function_call_list create_possible_delegates_list(expression_node obj, function_node func, location loc, bool is_static)
        {
            function_node_list fnl = new function_node_list();
            fnl.AddElement(func);
            return convert_functions_to_calls(obj, fnl, loc, is_static);
        }

        internal typed_expression make_delegate_wrapper(expression_node obj, SymbolInfo si, location loc, bool is_static)
        {
            base_function_call_list fnl = create_possible_delegates_list(obj, si, loc, is_static);
            if (fnl.Count == 0)
            {
                if (is_static)
                {
                    AddError(loc, "EXPECTED_STATIC_METHOD");
                }
                else
                {
                    AddError(loc, "EXPECTED_NON_STATIC_METHOD");
                }
            }
            delegated_methods dm = new delegated_methods();
            dm.proper_methods.AddRange(fnl);
            typed_expression te = new typed_expression(dm, loc);
            return te;
        }

        internal typed_expression make_delegate_wrapper(expression_node obj, function_node func, location loc, bool is_static)
        {
            base_function_call_list fnl = create_possible_delegates_list(obj, func, loc, is_static);
            if (fnl.Count == 0)
            {
                if (is_static)
                {
                    AddError(loc, "EXPECTED_STATIC_METHOD");
                }
                else
                {
                    AddError(loc, "EXPECTED_NON_STATIC_METHOD");
                }
            }
            delegated_methods dm = new delegated_methods();
            dm.proper_methods.AddRange(fnl);
            typed_expression te = new typed_expression(dm, loc);
            return te;
        }

        private bool check_name_in_current_scope(string name)
        {

            SymbolTable.Scope cscope = context.CurrentScope, name_scope = context.find(name).scope;
            while (cscope is SymbolTable.BlockScope)
            {
                if (name_scope == cscope)
                    return true;
                cscope = cscope.TopScope;
            }
            if (cscope is SymbolTable.UnitImplementationScope && name_scope is SymbolTable.UnitInterfaceScope)
                return cscope.TopScope == name_scope;
            return cscope == name_scope;
        }

        void checkPolymorphicState(definition_node dn,location loc)
        {
            /*common_method_node cmn = context.top_function as common_method_node;
            if(GetCurrentObjectReference()==null)
            {
                if (dn is class_field && !(dn as class_field).IsStatic)
                    AddError(new CanNotReferenceToNonStaticFromStatic(dn, loc));

            }*/
        }

        private type_node find_type(string name, location loc)
        {
            definition_node di = context.check_name_node_type(name, loc, general_node_type.type_node);
            type_node tn = (type_node)di;
            return tn;
        }

        //(ssyy) разбил
        private type_node find_type(SyntaxTree.named_type_reference names, location loc)
        {
            definition_node di;
            SyntaxTree.template_type_reference ttr = names as SyntaxTree.template_type_reference;
            if (ttr != null)
            {
                di = ret.visit(names);
                type_node tn = di as type_node;
                if (tn == null)
                {
                    AddError(loc, "TYPE_REFERENCE_EXPECTED");
                }
                return tn;
            }
            SymbolInfo si = context.find_definition_node(names, loc);
            if (si == null && attribute_converted)
            {
            	names.names[names.names.Count-1].name += "Attribute";
                si = context.find_definition_node(names, loc);
            }
            if (si != null && si.sym_info != null && si.sym_info.general_node_type == general_node_type.generic_indicator)
            {
                generic_indicator gi = si.sym_info as generic_indicator;
                AddError(loc, "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", names.names[names.names.Count - 1].name, gi.generic.generic_params.Count);
            }
            if (si == null || si.sym_info == null)
            {
                check_possible_generic_names(names, loc);
            }
            di = context.check_name_node_type(names.names[names.names.Count - 1].name, si, loc, general_node_type.type_node);
            return (type_node)di;
        }

        public void check_possible_generic_names(SyntaxTree.named_type_reference names, location loc)
        {
            string tmp = names.names[names.names.Count - 1].name;
            int num;
            string generic_name = compiler_string_consts.GetGenericTypeInformation(tmp, out num);
            List<int> counts;
            bool found = NetHelper.NetHelper.generics_names.TryGetValue(generic_name.ToLower(), out counts);
            if (found)
            {
                foreach (int k in counts)
                {
                    names.names[names.names.Count - 1].name = generic_name +
                        compiler_string_consts.generic_params_infix + k.ToString();
                    SymbolInfo sinfo = context.find_definition_node(names, loc);
                    if (sinfo != null)
                    {
                        names.names[names.names.Count - 1].name = tmp;
                        AddError(loc, "GENERIC_TYPE_{0}_NEEDS_{1}_GENERIC_PARAMETERS", generic_name, k);
                        break;
                    }
                }
                names.names[names.names.Count - 1].name = tmp;
            }
        }


        public expression_node make_break_node(location call_location, params expression_node[] parameters)
        {
            if (parameters.Length != 0)
            {
                return null;
            }
            if (context.cycle_stack.Empty)
            {
                AddError(new BreakStatementWithoutComprehensiveCycle(call_location));
            }
            statement_node sn = context.cycle_stack.top();
            switch (sn.semantic_node_type)
            {
                case semantic_node_type.while_node:
                    {
                        while_node wn = (while_node)sn;
                        return new while_break_node(wn, call_location);
                    }
                case semantic_node_type.repeat_node:
                    {
                        repeat_node rn = (repeat_node)sn;
                        return new repeat_break_node(rn, call_location);
                    }
                case semantic_node_type.for_node:
                    {
                        for_node fn = (for_node)sn;
                        return new for_break_node(fn, call_location);
                    }
            	case semantic_node_type.foreach_node:
            		{
            			foreach_node fn = (foreach_node)sn;
            			return new foreach_break_node(fn, call_location);
            		}
            }
            throw new CompilerInternalError("Undefined cycle type");
        }

        public expression_node make_continue_node(location call_location, params expression_node[] parameters)
        {
            if (parameters.Length != 0)
            {
                return null;
            }
            if (context.cycle_stack.Empty)
            {
                AddError(new ContinueStatementWithoutComprehensiveCycle(call_location));
            }
            statement_node sn = context.cycle_stack.top();
            switch (sn.semantic_node_type)
            {
                case semantic_node_type.while_node:
                    {
                        while_node wn = (while_node)sn;
                        return new while_continue_node(wn, call_location);
                    }
                case semantic_node_type.repeat_node:
                    {
                        repeat_node rn = (repeat_node)sn;
                        return new repeat_continue_node(rn, call_location);
                    }
                case semantic_node_type.for_node:
                    {
                        for_node fn = (for_node)sn;
                        return new for_continue_node(fn, call_location);
                    }
            	case semantic_node_type.foreach_node:
            		{
            			foreach_node fn = (foreach_node)sn;
            			return new foreach_continue_node(fn, call_location);
            		}	
            }
            throw new CompilerInternalError("Undefined cycle type");
        }

        public expression_node make_exit_node(location call_location, params expression_node[] parameters)
        {
            if (parameters.Length != 0)
            {
                return null;
            }
            if (!TreeConverter.SemanticRules.EnableExitProcedure)
                AddError(new TreeConverter.UndefinedNameReference(TreeConverter.compiler_string_consts.exit_procedure_name, call_location));
            return new exit_procedure(call_location);
        }

        public override void visit(SyntaxTree.inherited_message _inherited_message)
        {
            //throw new NotSupportedError(get_location(_inherited_message));
            switch (motivation_keeper.motivation)
            {
                case motivation.expression_evaluation: return_value(inherited_message_value_reciving(_inherited_message)); break;
                //case motivation.symbol_info_reciving: return_symbol_value(blocks.find(_ident.name));break;
               	case motivation.address_reciving:
                case motivation.semantic_node_reciving: AddError(get_location(_inherited_message), "CAN_NOT_ASSIGN_TO_LEFT_PART"); break;
            }
        }
		
        private expression_node current_catch_excep;
        
        public override void visit(SyntaxTree.try_stmt _try_stmt)
        {
            context.enter_code_block_without_bind();
            context.enter_exception_handlers();
            statement_node try_statements = convert_strong(_try_stmt.stmt_list);
            context.leave_code_block();
            location loc = get_location(_try_stmt);
            exception_filters_list efl = new exception_filters_list();
            SyntaxTree.try_handler_except try_hand_except = _try_stmt.handler as SyntaxTree.try_handler_except;
            if (try_hand_except != null)
            {
                if (try_hand_except.except_block.handlers != null)
                {
                    int hand_count = try_hand_except.except_block.handlers.handlers.Count;
                    for (int i = 0; i < hand_count; i++)
                    {
                        SyntaxTree.exception_handler eh = try_hand_except.except_block.handlers.handlers[i];
                        type_node filter_type = convert_strong(eh.type_name);
                        if (!SemanticRules.GenerateNativeCode && !(filter_type.is_generic_parameter ||
                            filter_type == SystemLibrary.SystemLibrary.exception_base_type ||
                            type_table.is_derived(SystemLibrary.SystemLibrary.exception_base_type, filter_type)))
                        {
                            AddError(get_location(eh.type_name), "EXCEPTION_TYPE_MUST_BE_SYSTEM_EXCEPTION_OR_DERIVED_FROM_EXCEPTION");
                        }
                        current_catch_excep = new int_const_node(2,null);//create_constructor_call(filter_type, new expressions_list(), null);
                        local_block_variable_reference lvr = null;
                        
                        context.enter_code_block_without_bind();
                        if (eh.variable != null)
                        {
                        	
                        	context.check_name_redefinition = false;
                        	
                        	local_block_variable lbv = context.add_var_definition(eh.variable.name, get_location(eh.variable), filter_type, SemanticTree.polymorphic_state.ps_common) as local_block_variable;
                            context.check_name_redefinition = true;
                        	lvr = new local_block_variable_reference(lbv, lbv.loc);
                        }
                        statement_node stm = convert_strong(eh.statements);
                        context.leave_code_block();
                        
                        /*if (eh.variable != null)
                        {
                            context.leave_scope();
                        }*/
                        exception_filter ef = new exception_filter(filter_type, lvr, stm, get_location(eh));
                        efl.AddElement(ef);
                        current_catch_excep = null;
                    }
                }
                else
                {
                    context.enter_code_block_without_bind();
                    exception_filter ef = new exception_filter(null, null, convert_strong(try_hand_except.except_block.stmt_list), get_location(try_hand_except.except_block.stmt_list));
                    context.leave_code_block();
                    efl.AddElement(ef);
                }
                if (try_hand_except.except_block.else_stmt_list != null)
                {
                    context.enter_code_block_without_bind();
                    statement_node else_stm = convert_strong(try_hand_except.except_block.else_stmt_list);
                    context.leave_code_block();
                    type_node ftype = SystemLibrary.SystemLibrary.object_type;
                    exception_filter else_ef = new exception_filter(ftype, null, else_stm, else_stm.location);
                    efl.AddElement(else_ef);
                }
            }
            else
            {
            	type_node filter_type = compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(compiler_string_consts.ExceptionName));
            	expression_node current_catch_excep = create_constructor_call(filter_type, new expressions_list(), null);
                local_block_variable_reference lvr = null;
                local_block_variable tmp_var = context.add_var_definition(context.BuildName("$try_temp" + UniqueNumStr()), null, SystemLibrary.SystemLibrary.bool_type, null) as local_block_variable;
                statements_list stm = new statements_list(null);
                SyntaxTree.try_handler_finally try_hndlr_finally = _try_stmt.handler as SyntaxTree.try_handler_finally;
                context.enter_code_block_without_bind();
                statement_node finally_stmt = convert_strong(try_hndlr_finally.stmt_list);
                context.leave_code_block();
                stm.statements.AddElement(finally_stmt);
                basic_function_call bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,null);
                bfc.parameters.AddElement(new local_block_variable_reference(tmp_var,null));
                stm.statements.AddElement(new if_node(bfc,new rethrow_statement_node(null),null,null));
            	exception_filter ef = new exception_filter(filter_type, lvr, stm, null);
                efl.AddElement(ef);
                bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,null);
                bfc.parameters.AddElement(new local_block_variable_reference(tmp_var,null));
                bfc.parameters.AddElement(new bool_const_node(true,null));
                
                (try_statements as statements_list).statements.AddElement(bfc);
                (try_statements as statements_list).statements.AddElement(new throw_statement_node(current_catch_excep,null));
                return_value(new try_block(try_statements, null, efl, loc));
                context.leave_exception_handlers();
                return;
            }
            statement_node finally_st = null;
            SyntaxTree.try_handler_finally try_handler_finally = _try_stmt.handler as SyntaxTree.try_handler_finally;
            if (try_handler_finally != null)
            {
                context.enter_code_block_without_bind();
                finally_st = convert_strong(try_handler_finally.stmt_list);
                context.leave_code_block();
            }
            try_block tb = new try_block(try_statements, finally_st, efl, loc);
            context.leave_exception_handlers();
            return_value(tb);
        }

        public override void visit(SyntaxTree.try_handler_except _try_handler_except)
        {
            throw new NotSupportedError(get_location(_try_handler_except));
        }

        public override void visit(SyntaxTree.try_handler_finally _try_handler_finally)
        {
            throw new NotSupportedError(get_location(_try_handler_finally));
        }

        public override void visit(SyntaxTree.try_handler _try_handler)
        {
            throw new NotSupportedError(get_location(_try_handler));
        }

        public override void visit(SyntaxTree.exception_block _exception_block)
        {
            throw new NotSupportedError(get_location(_exception_block));
        }

        public override void visit(SyntaxTree.exception_handler_list _exception_handler_list)
        {
            throw new NotSupportedError(get_location(_exception_handler_list));
        }

        public override void visit(SyntaxTree.exception_ident _exception_ident)
        {
            throw new NotSupportedError(get_location(_exception_ident));
        }

        public override void visit(SyntaxTree.exception_handler _exception_handler)
        {
            throw new NotSupportedError(get_location(_exception_handler));
        }

        public override void visit(SyntaxTree.known_type_ident _known_type_ident)
        {
            if (_known_type_ident.type == PascalABCCompiler.SyntaxTree.known_type.string_type)
            {
                switch (motivation_keeper.motivation)
                {
                    case motivation.address_reciving: return_value(SystemLibrary.SystemLibrary.string_type); break;
                    case motivation.expression_evaluation: return_value(SystemLibrary.SystemLibrary.string_type); break;
                    //case motivation.symbol_info_reciving: return_symbol_value(blocks.find(_ident.name));break;
                    case motivation.semantic_node_reciving: return_semantic_value(SystemLibrary.SystemLibrary.string_type); break;
                }

                return;
            }
            //throw new NotSupportedError(get_location(_known_type_ident));
        }

        public override void visit(SyntaxTree.file_type _file_type)
        {
            if (_file_type.file_of_type != null)
            {
                //Типизированый файл
                type_node el_type = convert_strong(_file_type.file_of_type);
                check_for_type_allowed(el_type,get_location(_file_type.file_of_type));
                //if (el_type == SystemLibrary.SystemLibrary.void_type)
                //	ErrorsList.Add(new VoidNotValid(get_location(_file_type.file_of_type)));
                /*if (el_type == SystemLibrary.SystemLibInitializer.TextFileType.sym_info) 
                {
                    //Это текстовый файл!!!                    
                    return_value(el_type);
                    return;
                }*/
                if (SystemLibrary.SystemLibInitializer.TypedFileType == null || SystemLibrary.SystemLibInitializer.TypedFileInitProcedure == null)
                    AddError(new NotSupportedError(get_location(_file_type)));
                if (!CanUseThisTypeForTypedFiles(el_type))
                    AddError(get_location(_file_type.file_of_type), "INVALID_ELEMENT_TYPE_FOR_TYPED_FILE", el_type);
                return_value(context.create_typed_file_type(el_type, get_location(_file_type)));
            }
            else
            {
                //Бинарнай файл
                if (SystemLibrary.SystemLibInitializer.BinaryFileType == null)
                    AddError(new NotSupportedError(get_location(_file_type)));
                ((type_node)SystemLibrary.SystemLibInitializer.BinaryFileType.sym_info).type_special_kind = SemanticTree.type_special_kind.binary_file;
                return_value((type_node)SystemLibrary.SystemLibInitializer.BinaryFileType.sym_info);
            }
        }

        private bool CanUseThisTypeForFiles(type_node el_type, bool allow_strings)
        {
            if (el_type.is_generic_parameter)
            {
                if (SemanticRules.AllowGenericParametersForFiles)
                {
                    if (allow_strings)
                        get_type_abilities(el_type).useful_for_binary_files = true;
                    else
                        get_type_abilities(el_type).useful_for_typed_files = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (el_type.type_special_kind == SemanticTree.type_special_kind.short_string)
            	return true;
        	if (el_type.type_special_kind == SemanticTree.type_special_kind.set_type)
        	{
        		if (el_type.element_type == SystemLibrary.SystemLibrary.byte_type || el_type.element_type == SystemLibrary.SystemLibrary.sbyte_type
        		   || el_type.element_type == SystemLibrary.SystemLibrary.bool_type || el_type.element_type.IsEnum) return true;
        		else if (el_type.element_type.type_special_kind == SemanticTree.type_special_kind.diap_type)
        		{
        			if (el_type.element_type.base_type == SystemLibrary.SystemLibrary.byte_type || el_type.element_type.base_type == SystemLibrary.SystemLibrary.sbyte_type
        		   || el_type.element_type.base_type == SystemLibrary.SystemLibrary.bool_type || el_type.element_type.base_type.IsEnum) return true;
        			if (el_type.element_type.base_type == SystemLibrary.SystemLibrary.integer_type)
        			{
        				ordinal_type_interface oti = el_type.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        				if (((oti.upper_value as int_const_node).constant_value-(oti.lower_value as int_const_node).constant_value) <= 256)
        					return true;
        			}
        			
        			return false;
        		}
        		return false;
        	}
        	if (el_type.is_value)
            {
                generic_instance_type_node gitn = el_type as generic_instance_type_node;
                if (gitn != null)
                {
                    List<type_node> ftypes = gitn.all_field_types;
                    foreach (type_node ftype in ftypes)
                    {
                        if (!CanUseThisTypeForFiles(ftype, allow_strings))
                            return false;
                    }
                    return true;
                }
                if (SystemLibrary.SystemLibrary.CanUseThisTypeForTypedFiles(el_type))
                    return true;
                if (el_type is common_type_node)
                {
                	if (el_type as common_type_node == context.converted_type ||
                        (el_type.original_generic != null && el_type.original_generic == context.converted_type))
                		return false;
                	common_type_node ctn = el_type as common_type_node;
                    foreach (class_field cf in ctn.fields)
                    	if (cf.type.type_special_kind != SemanticTree.type_special_kind.short_string && !CanUseThisTypeForFiles(cf.type, allow_strings))
                            return false;
                    return true;
                }
                if (el_type is compiled_type_node)
                {
                    compiled_type_node ctn = el_type as compiled_type_node;
                    if (!ctn.compiled_type.IsLayoutSequential)
                    	return false;
                    System.Reflection.FieldInfo[] fields = ctn.compiled_type.GetFields();
                    foreach (System.Reflection.FieldInfo fi in fields)
                        if (!fi.IsStatic)
                            if (!CanUseThisTypeForFiles(compiled_type_node.get_type_node(fi.FieldType), allow_strings))
                                return false;
                    return true;
                }
            }
            if (IsBoundedArray(el_type))
            {
                //это обычный массив
                bounded_array_interface bai = (bounded_array_interface)el_type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                return CanUseThisTypeForFiles(bai.element_type, allow_strings);
            }
            if (allow_strings && el_type == SystemLibrary.SystemLibrary.string_type)
            {
                return true;
            }
            return false;
        }

        private bool CanUseThisTypeForTypedFiles(type_node el_type)
        {
        	if (el_type.type_special_kind == SemanticTree.type_special_kind.short_string) return true;
        	return CanUseThisTypeForFiles(el_type, false);
        }

        private bool CanUseThisTypeForBinaryFiles(type_node el_type)
        {
        	if (el_type == SystemLibrary.SystemLibrary.string_type || el_type is short_string_type_node) return true;
            return CanUseThisTypeForFiles(el_type, true);
        }
		
        internal List<int> get_short_string_lens(type_node tn)
        {
        	List<int> lst = new List<int>();
        	if (tn.type_special_kind == SemanticTree.type_special_kind.short_string)
        	{
        		lst.Add((tn as short_string_type_node).Length);
        	}
        	if (tn is common_type_node)
        	{
        		common_type_node ctn = tn as common_type_node;
        		if (ctn.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
        		{
        			lst.AddRange(get_short_string_lens(ctn.element_type));
        		}
        		else if (ctn.type_special_kind == SemanticTree.type_special_kind.record)
        		{
        			foreach (class_field fld in ctn.fields)
        				lst.AddRange(get_short_string_lens(fld.type));
        		}
        	}
        	return lst;
        }
        
        internal int get_short_string_size(type_node tn)
        {
        	if (tn.type_special_kind == SemanticTree.type_special_kind.short_string)
        	{
        		return (tn as short_string_type_node).Length+1;
        	}
        	if (tn is common_type_node)
        	{
        		common_type_node ctn = tn as common_type_node;
        		if (ctn.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
        		{
        			int len = ((ctn.find_in_type(compiler_string_consts.internal_array_name).sym_info as class_field).type as simple_array).length;
        			return len*get_short_string_size(ctn.element_type);
        		}
        		else if (ctn.type_special_kind == SemanticTree.type_special_kind.record)
        		{
        			int off = 0;
        			foreach (class_field fld in ctn.fields)
        				off += get_short_string_size(fld.type);
        			return off;
        		}
        		return 0;
        	}
        	return 0;
        }
        
        internal expression_node get_init_call_for_typed_file(var_definition_node vdn, type_node element_type)
        {
            location loc = (vdn.type as common_type_node).loc;
            if (!SystemUnitAssigned)
                AddError(new NotSupportedError(loc));
            //Когда заработает наследование конструкторов надо вставить это
            /*
            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(new typeof_operator(element_type, loc));
            base_function_call bfc = create_constructor_call(vdn.type, exl, loc);
            expression_node expr = find_operator(compiler_string_consts.assign_name, var_ref, bfc, loc);
            */

            type_node tn = vdn.type;
            vdn.type = SystemLibrary.SystemLibInitializer.TypedFileType.sym_info as type_node;

            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(var_ref);
            exl.AddElement(new typeof_operator(element_type, loc));
            List<int> lens = get_short_string_lens(element_type);
            int sz = get_short_string_size(element_type);
            if (sz > 0)
            {
            	exl.AddElement(new int_const_node(get_short_string_size(element_type),null));
            	for (int i=0; i<lens.Count; i++)
            		exl.AddElement(new int_const_node(lens[i],null));
            }
            this.context.save_var_definitions();
            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TypedFileInitProcedure.SymbolInfo, loc);
            this.context.restore_var_definitions();
            expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());

            vdn.type = tn;

            return expr;
        }

        internal expression_node get_init_call_for_text_file(var_definition_node vdn)
        {
            location loc = null; 
            if (vdn.type is common_type_node)
                loc = (vdn.type as common_type_node).loc;
            if (!SystemUnitAssigned)
                AddError(new NotSupportedError(loc));
            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(var_ref);
            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TextFileInitProcedure.SymbolInfo, loc);
            expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());
            return expr;
        }

        internal expression_node get_init_call_for_binary_file(var_definition_node vdn)
        {
            location loc = null;
            if (vdn.type is common_type_node)
                loc = (vdn.type as common_type_node).loc;
            if (!SystemUnitAssigned)
                AddError(new NotSupportedError(loc));
            type_node tn = vdn.type;
            vdn.type = SystemLibrary.SystemLibInitializer.BinaryFileType.sym_info as type_node;

            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(var_ref);

            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.BinaryFileInitProcedure.SymbolInfo, loc);
            expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());

            vdn.type = tn;

            return expr;
        }

        internal expression_node get_init_call_for_set(var_definition_node vdn)
        {
            location loc = (vdn.type as common_type_node).loc;
            if (!SystemUnitAssigned)
                AddError(new NotSupportedError(loc));
            //Когда заработает наследование конструкторов надо вставить это
            /*
            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(new typeof_operator(element_type, loc));
            base_function_call bfc = create_constructor_call(vdn.type, exl, loc);
            expression_node expr = find_operator(compiler_string_consts.assign_name, var_ref, bfc, loc);
            */

            type_node tn = vdn.type;
            vdn.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;

            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(var_ref);
            bool short_str = false;
            //exl.AddElement(new typeof_operator(element_type, loc));
            if (tn.element_type == SystemLibrary.SystemLibrary.byte_type)
            {
            	exl.AddElement(new byte_const_node(byte.MinValue,null));
            	exl.AddElement(new byte_const_node(byte.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.sbyte_type)
            {
            	exl.AddElement(new sbyte_const_node(sbyte.MinValue,null));
            	exl.AddElement(new sbyte_const_node(sbyte.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.short_type)
            {
            	exl.AddElement(new short_const_node(short.MinValue,null));
            	exl.AddElement(new short_const_node(short.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.ushort_type)
            {
            	exl.AddElement(new ushort_const_node(ushort.MinValue,null));
            	exl.AddElement(new ushort_const_node(ushort.MaxValue,null));
            }
            else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.diap_type || tn.element_type.type_special_kind == SemanticTree.type_special_kind.enum_kind)
            {
            	ordinal_type_interface ii = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
            	exl.AddElement(ii.lower_value);
            	exl.AddElement(ii.upper_value);
            }
            else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
            {
            	short_str = true;
            	exl.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length,null));
            }
            function_node fn = null;
            if (short_str)
            	fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TypedSetInitWithShortString.SymbolInfo, loc);
            else
            if (exl.Count > 1) 
            	fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TypedSetInitProcedureWithBounds.SymbolInfo, loc);
            else
            	fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TypedSetInitProcedure.SymbolInfo, loc);
            expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());

            vdn.type = tn;

            return expr;
        }
		
        internal expression_node get_init_call_for_set_as_constr(var_definition_node vdn, expression_node init_value)
        {
            location loc = (vdn.type as common_type_node).loc;
            if (!SystemUnitAssigned)
                AddError(new NotSupportedError(loc));
            //Когда заработает наследование конструкторов надо вставить это
            /*
            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(new typeof_operator(element_type, loc));
            base_function_call bfc = create_constructor_call(vdn.type, exl, loc);
            expression_node expr = find_operator(compiler_string_consts.assign_name, var_ref, bfc, loc);
            */

            type_node tn = vdn.type;
            vdn.type = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;

            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            //exl.AddElement(var_ref);
            //exl.AddElement(new typeof_operator(element_type, loc));
            if (tn.element_type == SystemLibrary.SystemLibrary.byte_type)
            {
            	exl.AddElement(new byte_const_node(byte.MinValue,null));
            	exl.AddElement(new byte_const_node(byte.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.sbyte_type)
            {
            	exl.AddElement(new sbyte_const_node(sbyte.MinValue,null));
            	exl.AddElement(new sbyte_const_node(sbyte.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.short_type)
            {
            	exl.AddElement(new short_const_node(short.MinValue,null));
            	exl.AddElement(new short_const_node(short.MaxValue,null));
            }
            else if (tn.element_type == SystemLibrary.SystemLibrary.ushort_type)
            {
            	exl.AddElement(new ushort_const_node(ushort.MinValue,null));
            	exl.AddElement(new ushort_const_node(ushort.MaxValue,null));
            }
            else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.diap_type || tn.element_type.type_special_kind == SemanticTree.type_special_kind.enum_kind)
            {
            	ordinal_type_interface ii = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
            	exl.AddElement(ii.lower_value);
            	exl.AddElement(ii.upper_value);
            }
            exl.AddElement(init_value);
            function_node fn = null;
            if (exl.Count > 1)
            {
                fn = convertion_data_and_alghoritms.select_function(exl, (SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node).find_in_type(compiler_string_consts.default_constructor_name), loc);
            }
            else
            {
                fn = convertion_data_and_alghoritms.select_function(exl, (SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node).find_in_type(compiler_string_consts.default_constructor_name), loc);
            }
            //expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());
            expression_node expr = create_static_method_call_with_params(fn, loc, tn, false, exl);
            vdn.type = tn;

            return expr;
        }
        
        internal expression_node get_init_call_for_short_string(var_definition_node vdn)
        {
            location loc = (vdn.type as short_string_type_node).loc;

            type_node tn = vdn.type;
            //vdn.type = SystemLibrary.SystemLibInitializer.ShortStringType.TypeNode;
			vdn.type = SystemLibrary.SystemLibrary.string_type;
            int Length = (tn as short_string_type_node).Length;

            expression_node var_ref = create_variable_reference(vdn, loc);
            expressions_list exl = new expressions_list();
            exl.AddElement(var_ref);
            exl.AddElement(new int_const_node(Length, loc));
            expression_node expr = null;
            //if (SystemLibrary.SystemLibInitializer.ShortStringTypeInitProcedure != null)
            //convertion_data_and_alghoritms.create_full_function_call(exl, SystemLibrary.SystemLibInitializer.ShortStringTypeInitProcedure.SymbolInfo, null, context.converted_type, context.top_function, true);

            vdn.type = tn;

            return expr;
        }


        public override void visit(SyntaxTree.op_type_node _op_type_node)
        {
            throw new NotSupportedError(get_location(_op_type_node));
        }

        public override void visit(SyntaxTree.raise_stmt _raise_stmt)
        {
        	location loc = get_location(_raise_stmt);
        	if (_raise_stmt.expr == null)
        	{
                if (current_catch_excep == null) AddError(loc, "RAISE_WITHOUT_PARAMETERS_MUST_BE_IN_CATCH_BLOCK");
        		return_value(new rethrow_statement_node(loc));
        		return;
        	}
        	expression_node en = convert_strong(_raise_stmt.expr);
        	if (en is typed_expression) en = convert_typed_expression_to_function_call(en as typed_expression);
            if (!SemanticRules.GenerateNativeCode && (!(type_table.is_derived(SystemLibrary.SystemLibrary.exception_base_type, en.type))) &&
                (en.type != SystemLibrary.SystemLibrary.exception_base_type))
            {
                AddError(loc, "EXCEPTION_TYPE_MUST_BE_SYSTEM_EXCEPTION_OR_DERIVED_FROM_EXCEPTION");
            }
            if (_raise_stmt.address != null)
            {
                AddError(new NotSupportedError(get_location(_raise_stmt.address)));
            }
            return_value(new throw_statement_node(en, loc));
        }

        public override void visit(SyntaxTree.token_info _token_info)
        {
            throw new NotSupportedError(get_location(_token_info));
        }

        public override void visit(SyntaxTree.initfinal_part _initfinal_part)
        {
        }
		
       	private bool is_format_allowed=false;
       	
        public override void visit(SyntaxTree.format_expr _format_expr)
        {
            //TODO: Добавить проверки.
            if (!SemanticRules.AllowUseFormatExprAnywhere && !is_format_allowed)
                AddError(get_location(_format_expr.expr), "FORMAT_EXPRESSION_CAN_USE_ONLY_IN_THESE_PROCEDURES");
            expression_node expr = convert_strong(_format_expr.expr);
            if (expr is typed_expression)
                expr = convert_typed_expression_to_function_call(expr as typed_expression);
            expression_node par1 = convert_strong(_format_expr.format1);
            par1 = convertion_data_and_alghoritms.convert_type(par1, SystemLibrary.SystemLibrary.integer_type);
            expression_node par2 = convert_weak(_format_expr.format2);
            if (par2 != null)
            {
                if (expr.type != SystemLibrary.SystemLibrary.double_type && expr.type != SystemLibrary.SystemLibrary.float_type)
                {
                    AddError(expr.location, "REAL_TYPE_IN_DOUBLE_COLON_EXPRESSION_EXPECTED");
                }
                par2 = convertion_data_and_alghoritms.convert_type(par2, SystemLibrary.SystemLibrary.integer_type);
            }
            expressions_list exl = new expressions_list();
            exl.AddElement(expr);
            exl.AddElement(par1);
            if (par2 != null)
            {
                exl.AddElement(par2);
            }
            location loc = get_location(_format_expr);
            if (!SystemUnitAssigned)
                AddError( new NotSupportedError(loc));
            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.format_function.SymbolInfo,
                loc);
            expression_node ret = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, exl.ToArray());
            return_value(ret);
        }

        internal bool inherited_ident_processing = false;
        public override void visit(SyntaxTree.inherited_ident _inherited_ident)
        {
            inherited_ident_processing = true;
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: return_addressed_value(inherited_ident_address_reciving(_inherited_ident)); break;
                case motivation.expression_evaluation: return_value(inherited_ident_value_reciving(_inherited_ident)); break;
                //case motivation.symbol_info_reciving: return_symbol_value(blocks.find(_ident.name));break;
                case motivation.semantic_node_reciving: return_semantic_value(inherited_ident_semantic_reciving(_inherited_ident)); break;
            }
            inherited_ident_processing = false;
            return;
        }

        public override void visit(SyntaxTree.property_parameter_list _property_parameter_list)
        {
            throw new NotSupportedError(get_location(_property_parameter_list));
        }

        public override void visit(SyntaxTree.property_parameter _property_parameter)
        {
            throw new NotSupportedError(get_location(_property_parameter));
        }

        public override void visit(SyntaxTree.property_interface _property_interface)
        {
            //throw new NotSupportedError(get_location(_property_interface));
        }

        public override void visit(SyntaxTree.property_array_default _property_array_default)
        {
            throw new NotSupportedError(get_location(_property_array_default));
        }

        public override void visit(SyntaxTree.record_type_parts _record_type_parts)
        {
            throw new NotSupportedError(get_location(_record_type_parts));
        }

        public override void visit(SyntaxTree.var_def_list_for_record _var_def_list)
        {
            throw new NotSupportedError(get_location(_var_def_list));
        }

        public override void visit(SyntaxTree.diapason_expr _diapason_expr)
        {
            throw new NotSupportedError(get_location(_diapason_expr));
        }

        public override void visit(SyntaxTree.case_variants _case_variants)
        {
            throw new NotSupportedError(get_location(_case_variants));
        }

        public override void visit(SyntaxTree.literal _literal)
        {
            throw new NotSupportedError(get_location(_literal));
        }

        public override void visit(SyntaxTree.array_of_const_type_definition _array_of_const_type_definition)
        {
            throw new NotSupportedError(get_location(_array_of_const_type_definition));
        }

        public override void visit(SyntaxTree.array_of_named_type_definition _array_of_named_type_definition)
        {
            throw new NotSupportedError(get_location(_array_of_named_type_definition));
        }

        public override void visit(SyntaxTree.proc_block _proc_block)
        {
            throw new NotSupportedError(get_location(_proc_block));
        }

        private statements_list get_statements_list(statement_node sn)
        {
            statements_list snl = sn as statements_list;
            if (snl != null)
            {
                return snl;
            }
            snl = new statements_list(null);
            snl.statements.AddElement(sn);
            return snl;
        }

        private statements_list add_first_statement(statement_node list, statement_node statement)
        {
            statements_list snl = get_statements_list(list);
            snl.statements.AddElementFirst(statement);
            return snl;
        }

        private statements_list add_last_statement(statement_node list, statement_node statement)
        {
            statements_list snl = get_statements_list(list);
            snl.statements.AddElement(statement);
            return snl;
        }

        /*private statements_list add_self_assign_statement(statement_node sn, common_method_node cmn)
        {
            SymbolInfo si = cmn.cont_type.find_in_type(compiler_string_consts.assign_name, context.CurrentScope);
            expressions_list exl=new expressions_list();
            local_variable_reference lvr = new local_variable_reference(cmn.self_variable, 0, null);
            this_node tn = new this_node(cmn.cont_type, null);
            exl.AddElement(lvr);
            exl.AddElement(tn);
            function_node fn = convertion_data_and_alghoritms.select_function(exl, si, null);
            expression_node en = convertion_data_and_alghoritms.create_simple_function_call(fn, null, exl.ToArray());
            statements_list snl = add_first_statement(sn, en);
            return snl;
        }*/


        /*
        private expression_node create_newarr_expression(type_node arr_type, pascal_array_internal_interface paii)
        {
            int_const_node icn = new int_const_node(paii.length, null);
            expressions_list elist=new expressions_list();
            elist.AddElement(icn);
            SymbolInfo si = arr_type.find(compiler_string_consts.standart_constructor_name);
            function_node fn = convertion_data_and_alghoritms.select_function(elist, si, null);
            expression_node res = convertion_data_and_alghoritms.create_simple_function_call(fn, null, elist.ToArray());
            return res;
        }
        */
        /*
        private statements_list add_create_array_statement(statement_node sn, var_definition_node vd,
            pascal_array_internal_interface paii, expression_node obj_type)
        {
            expression_node left = null;
            switch (vd.semantic_node_type)
            {
                case semantic_node_type.local_variable:
                    {
                        left = new local_variable_reference((local_variable)vd, 0, null);
                        break;
                    }
                case semantic_node_type.class_field:
                    {
                        left = new class_field_reference((class_field)vr, new this_node(obj_type, null), null);
                        break;
                    }
                case semantic_node_type.namespace_variable:
                    {
                        left = new namespace_variable_reference((namespace_variable)vd, null);
                        break;
                    }
            }
            //new local_variable_reference(lv, 0, null);
            expression_node right = create_newarr_expression(lv.type, paii);
            SymbolInfo si = lv.type.find(compiler_string_consts.assign_name);
            expressions_list elist = new expressions_list();
            elist.AddElement(left);
            elist.AddElement(right);
            function_node assign=convertion_data_and_alghoritms.select_function(elist,si,null);
            expression_node res=convertion_data_and_alghoritms.create_simple_function_call(
                assign,null,elist);
            return add_first_statement(sn, res);
        }
        */
        /*
        private statements_list add_create_arrays_statements(statement_node sn, common_function_node cfn)
        {
            foreach (local_variable lv in cfn.var_definition_nodes_list)
            {
                type_node tn = lv.type;
                internal_interface ii = tn.get_internal_interface(internal_interface_kind.pascal_array_internal_interface);
                if (ii == null)
                {
                    continue;
                }
                pascal_array_internal_interface paii = (pascal_array_internal_interface)ii;
                sn = add_create_array_statement(sn, lv, paii);
            }
        }
        */
        public override void visit(SyntaxTree.block _block)
        {
            if (context.converting_block() != block_type.namespace_block && _block.defs != null && _block.defs.defs != null)
            {
                foreach (var def in _block.defs.defs.Where(d => d is const_definition ||
                                                                d is consts_definitions_list ||
                                                                d is variable_definitions))
                {
                    var lambdaSearcher = new LambdaSearcher(def);
                    if (lambdaSearcher.CheckIfContainsLambdas())
                    {
                        AddError(new LambdasNotAllowedInDeclarationsSection(get_location(lambdaSearcher.FoundLambda)));     // SSM 6.06.15 - Хочется убрать это. В разделе описаний лямбды нужны - пусть и без замыканий. Если внутри подпрограммы - не нужны, но Вы же запретили везде!
                    }
                }
            }

            weak_node_test_and_visit(_block.defs);

            //ssyy добавил генерацию вызова конструктора предка без параметров
            if (context.converting_block() == block_type.function_block)
            {
                common_method_node cmn = context.top_function as common_method_node;
                if (cmn != null && cmn.is_constructor && !(cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static))
                {
                    context.allow_inherited_ctor_call = true;
                    if (_block.program_code != null && _block.program_code.subnodes != null)
                    {
                        //(ssyy) Для записей конструктор предка не вызываем.
                        bool should_ctor_add = context.converted_type.is_class;
                        if (should_ctor_add)
                        {
                            if (_block.program_code.subnodes.Count > 0)
                            {
                                SyntaxTree.procedure_call pc = _block.program_code.subnodes[0] as SyntaxTree.procedure_call;
                                if (pc != null || _block.program_code.subnodes[0] is SyntaxTree.inherited_message)
                                {
                                    //SyntaxTree.inherited_ident ii = pc.func_name as SyntaxTree.inherited_ident;
                                    //SyntaxTree.method_call mc = pc.func_name as SyntaxTree.method_call;
                                    //SyntaxTree.ident id = pc.func_name as SyntaxTree.ident;
                                    //if (/*ii != null*/id != null || mc != null /*&& mc.dereferencing_value is SyntaxTree.inherited_ident*/)
                                    {
                                        //(ssyy) Не уверен, что следующий оператор необходим.
                                        convertion_data_and_alghoritms.check_node_parser_error(_block.program_code);

                                        statement_node inh = convert_strong(_block.program_code.subnodes[0]);

                                        compiled_constructor_call c1 = inh as compiled_constructor_call;
                                        if (c1 != null && !c1._new_obj_awaited)
                                        {
                                            should_ctor_add = false;
                                        }
                                        else
                                        {
                                            common_constructor_call c2 = inh as common_constructor_call;
                                            if (c2 != null && !c2._new_obj_awaited)
                                            {
                                                should_ctor_add = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (should_ctor_add)
                        {
                            //Пытаемся добавить вызов .ctor() предка...
                            //Для начала проверим, есть ли у предка таковой.
                            bool not_found = true;
                            SymbolInfo sym = context.converted_type.base_type.find_in_type(compiler_string_consts.default_constructor_name, context.CurrentScope);
                            while (not_found && sym != null)
                            {
                                compiled_constructor_node ccn = sym.sym_info as compiled_constructor_node;
                                if (ccn != null && ccn.parameters.Count == 0)
                                {
                                    not_found = false;
                                }
                                else
                                {
                                    common_method_node cnode = sym.sym_info as common_method_node;
                                    if (cnode != null && cnode.is_constructor && (cnode.parameters.Count == 0 || cnode.parameters[0].default_value != null))
                                    {
                                        not_found = false;
                                    }
                                }
                                sym = sym.Next;
                            }
                            if (not_found)
                            {
                                //У предка нет конструктора по умолчанию
                                AddError(get_location(_block.program_code), "INHERITED_CONSTRUCTOR_CALL_EXPECTED");
                            }
                            else
                            {
                                //Генерируем вызов .ctor() предка
                                SyntaxTree.inherited_ident ii = new SyntaxTree.inherited_ident();
                                ii.name = compiler_string_consts.default_constructor_name;
                                _block.program_code.subnodes.Insert(0, new SyntaxTree.procedure_call(ii));
                                //context.allow_inherited_ctor_call = false;
                            }
                        }
                    }
                }
            }
            //\ssyy

            //ssyy
            if (context.converting_block() == block_type.namespace_block)
            {
                context.check_predefinition_defined();
            }

            if (lambdaProcessingState == LambdaProcessingState.None && _block.program_code != null)
            {
                var lambdaSearcher = new LambdaSearcher(_block.program_code);
                var lambdaIsFound = lambdaSearcher.CheckIfContainsLambdas();

                if ((context.converting_block() == block_type.function_block ||
                     context.converting_block() == block_type.namespace_block) &&
                    lambdaIsFound)
                    // TODO: Пока обрабатываются только блок функции и основной блок, необходимо обрабатывать случаи появлений лямбд в var_def_statement
                {
                    if (context.converting_block() == block_type.function_block)
                    {
                        if (context.func_stack.top().functions_nodes_list.Count > 0)
                        {
                            AddError(new LambdasNotAllowedWhenNestedSubprogrammesAreUsed(get_location(lambdaSearcher.FoundLambda)));
                        }
                    }

                    lambdaProcessingState = LambdaProcessingState.TypeInferencePhase;
                    visit_program_code(_block.program_code);

                    lambdaProcessingState = LambdaProcessingState.ClosuresProcessingPhase;
                    CapturedVariablesSubstitutionsManager.Substitute(this, _block.defs, _block.program_code);
                    
                    lambdaProcessingState = LambdaProcessingState.FinishPhase;

                    context.code = visit_program_code(_block.program_code);

                    lambdaProcessingState = LambdaProcessingState.None;
                }
                else
                {
                    context.code = visit_program_code(_block.program_code);
                }
            }
            else
            {
                context.code = visit_program_code(_block.program_code);
            }
        }

        private statement_node visit_program_code(statement_list program_code)
        {
            context.enter_code_block_with_bind();
            statement_node sn = convert_strong(program_code);
            context.leave_code_block();

            return sn;
        }

        public override void visit(SyntaxTree.diap_expr _diap_expr)
        {
            throw new NotSupportedError(get_location(_diap_expr));
        }
        internal SymbolTable.Scope[] build_referenced_units(unit_node_list referenced_units, bool try_get_system_module)
        {
            List<SymbolTable.Scope> used_units = new List<SymbolTable.Scope>();
            Dictionary<System.Reflection.Assembly, dot_net_unit_node> assembly_references_dict = new Dictionary<System.Reflection.Assembly, dot_net_unit_node>();
            for (int i = 0; i < referenced_units.Count; i++)
            {
                if (referenced_units[i] is dot_net_unit_node)
                    if (!assembly_references_dict.ContainsKey((referenced_units[i] as dot_net_unit_node).dotNetScope._assembly))
                        assembly_references_dict.Add((referenced_units[i] as dot_net_unit_node).dotNetScope._assembly, referenced_units[i] as dot_net_unit_node);
            }
            dot_net_unit_node[] assembly_references = new dot_net_unit_node[assembly_references_dict.Values.Count];
            assembly_references_dict.Values.CopyTo(assembly_references, 0);
            List<NetHelper.NetScope> def_scopes = new List<NetHelper.NetScope>();
            if (SemanticRules.AllowGlobalVisibilityForPABCDll)
            {
                for (int i = 0; i < assembly_references.Length; i++)
                {
                    if (assembly_references[i].dotNetScope.entry_type != null)
                        def_scopes.Add(assembly_references[i].dotNetScope);
                }
            }
            //used_units.Add(_system_unit.scope);
            for (int i = 0; i < referenced_units.Count; i++)
            {
                if (!(referenced_units[i] is dot_net_unit_node))
                {
                    if (referenced_units[i] is namespace_unit_node)
                    {
                        for (int j = 0; j < assembly_references.Length; j++)
                        {
                            using_namespace_list unl = new using_namespace_list();
                            unl.AddElement((referenced_units[i] as namespace_unit_node).namespace_name);
                            NetHelper.NetScope ns = new PascalABCCompiler.NetHelper.NetScope(unl, assembly_references[j].dotNetScope._assembly, SymbolTable);
                            used_units.Add(ns);
                        }
                        if (_compiled_unit != null)
                            _compiled_unit.used_namespaces.Add((referenced_units[i] as namespace_unit_node).namespace_name.namespace_name);
                    }
                    else
                    {
                        common_unit_node cun = _referenced_units[i] as common_unit_node;
                        if (cun != null)
                        {
                            if (try_get_system_module && cun.IsSystemUnit)
                            {
                                get_system_module(cun);
                                for (int j = 0; j < def_scopes.Count; j++)
                                {
                                    used_units.Add(new PascalABCCompiler.NetHelper.NetScope(interface_using_list, def_scopes[j]._assembly, SymbolTable));
                                }
                            }
                            used_units.Add(cun.scope);
                        }
                        else
                        {
                            throw new CompilerInternalError("Unexpected unit type.");
                        }
                    }
                }
                else
                {
                    dot_net_unit_node dun = referenced_units[i] as dot_net_unit_node;
                    if (dun.dotNetScope.entry_type != null && dun.dotNetScope.entry_type.Name == "PABCRtl")
                    {
                        if (try_get_system_module)
                        {
                            from_pabc_dll = true;
                            get_system_module_from_dll(dun);
                        }
                    }
                }
            }
            List<SymTable.Scope> fetched_used_units = new List<SymTable.Scope>();
            Dictionary<System.Reflection.Assembly, PascalABCCompiler.NetHelper.NetScope> assm_cache = new Dictionary<System.Reflection.Assembly, PascalABCCompiler.NetHelper.NetScope>();
            for (int i = 0; i < used_units.Count; i++)
            {
                PascalABCCompiler.NetHelper.NetScope net_scope = used_units[i] as PascalABCCompiler.NetHelper.NetScope;
                if (net_scope != null)
                {
                    PascalABCCompiler.NetHelper.NetScope existing_scope = null;
                    if (assm_cache.TryGetValue(net_scope.Assembly, out existing_scope))
                    {
                        /*if (net_scope.used_namespaces)
                        {
                            foreach (using_namespace un in net_scope.used_namespaces)
                            {
                                if (un.namespace_name)
                            }
                        }*/
                        existing_scope.used_namespaces.AddRange(net_scope.used_namespaces);
                    }
                    else
                    {
                        if (NetHelper.NetHelper.PABCSystemType == null || net_scope.Assembly != NetHelper.NetHelper.PABCSystemType.Assembly)
                            assm_cache.Add(net_scope.Assembly, net_scope);
                        fetched_used_units.Add(net_scope);
                    }
                }
                else
                    fetched_used_units.Add(used_units[i]);
            }
            used_units = fetched_used_units;
            //Если компилими implementation то в начало кладем все что было в interface
            for (int i = 0; i < assembly_references.Length; i++)
            {
                used_units.Add(new PascalABCCompiler.NetHelper.NetScope(interface_using_list, (assembly_references[i] as dot_net_unit_node).dotNetScope._assembly, SymbolTable));
            }
            used_units.Add(_system_unit.scope);

            return used_units.ToArray();
        }


        public void visit_implementation(SyntaxTree.unit_module _unit_module)
        {
            if (_unit_module.initialization_part != null)
            {
                var lambdaSearcher = new LambdaSearcher(_unit_module.initialization_part);
                if (lambdaSearcher.CheckIfContainsLambdas())
                {
                    AddError(new LambdasNotAllowedInInitializationPartOfModule(get_location(lambdaSearcher.FoundLambda)));
                }
            }

            if (_unit_module.finalization_part != null)
            {
                var lambdaSearcher = new LambdaSearcher(_unit_module.finalization_part);
                if (lambdaSearcher.CheckIfContainsLambdas())
                {
                    AddError(new LambdasNotAllowedInFinalizationPartOfModule(get_location(lambdaSearcher.FoundLambda)));
                }
            }

            //using_list.AddRange(implementation_using_list);
            /*if (_unit_module.implementation_part != null)
            {
                weak_node_test_and_visit(_unit_module.implementation_part.using_namespaces);
            }*/

            SymbolTable.Scope[] used_units = build_referenced_units(referenced_units,false);            

            _compiled_unit.implementation_scope =
                convertion_data_and_alghoritms.symbol_table.CreateUnitImplementationScope(_compiled_unit.scope, used_units);

            location loc = null;
            if (_unit_module.unit_name != null)
            {
                loc = get_location(_unit_module.unit_name);
            }

            common_namespace_node cnsn = context.create_namespace(_unit_module.unit_name.idunit_name.name + compiler_string_consts.ImplementationSectionNamespaceName,
                _compiled_unit, _compiled_unit.implementation_scope, loc);

            //cnsn.scope=_compiled_unit.implementation_scope;

            if (_unit_module.implementation_part != null)
            {
                hard_node_test_and_visit(_unit_module.implementation_part);
            }

            //weak_node_test_and_visit(_unit_module.initialization_part);
            context.enter_code_block_without_bind();

            statement_node init_statements = convert_weak(_unit_module.initialization_part);
            context.leave_code_block();
            common_namespace_function_node initialization_function = null;
            if (init_statements != null)
            {
                context.check_labels(context.converted_namespace.labels);
                //(ssyy) Блокируем поставленные метки, чтобы не допустить переход из finalization-секции
                //context.block_defined_labels(context.converted_namespace.labels);
                initialization_function = new common_namespace_function_node(compiler_string_consts.initialization_function_name,
                    null, init_statements.location, context.converted_namespace, convertion_data_and_alghoritms.symbol_table.CreateScope(context.converted_namespace.scope));
                initialization_function.function_code = init_statements;
                cnsn.functions.AddElement(initialization_function);
                _compiled_unit.main_function = initialization_function;
                context.apply_special_local_vars(_compiled_unit.main_function);
            }

            context.enter_code_block_without_bind();
            statement_node final_statements = convert_weak(_unit_module.finalization_part);
            context.leave_code_block();
            common_namespace_function_node finalization_function = null;
            if (final_statements != null)
            {
                context.check_labels(context.converted_namespace.labels);
                finalization_function = new common_namespace_function_node(compiler_string_consts.finalization_function_name,
                    null, final_statements.location, context.converted_namespace, convertion_data_and_alghoritms.symbol_table.CreateScope(context.converted_namespace.scope));
                finalization_function.function_code = final_statements;
                cnsn.functions.AddElement(finalization_function);
                _compiled_unit.finalization_method = finalization_function;
                context.apply_special_local_vars(_compiled_unit.finalization_method);
            }


            //TODO: Доделать finalization.
            //weak_node_test_and_visit(_unit_module.finalization_part);

            //TODO: Доделать.
            //common_namespace_function_node main_function=new common_namespace_function_node(compiler_string_consts.main_function_name,
            //	null,loc,blocks.converted_namespace,convertion_data_and_alghoritms.symbol_table.CreateScope(blocks.converted_namespace.scope));
            //main_function.function_code=blocks.code;

            //_compiled_unit.main_function=main_function;

            //Pochemu?????
            //context.leave_block();

            context.check_all_name_unit_defined(_compiled_unit);
        }

        public override void visit(SyntaxTree.implementation_node _implementation_node)
        {
            if (_implementation_node.implementation_definitions != null && _implementation_node.implementation_definitions.defs != null)
            {
                foreach (var def in _implementation_node.implementation_definitions.defs.Where(d => d is const_definition ||
                                                                                               d is consts_definitions_list ||
                                                                                               d is variable_definitions))
                {
                    var lambdaSearcher = new LambdaSearcher(def);
                    if (lambdaSearcher.CheckIfContainsLambdas())
                    {
                        AddError(new LambdasNotAllowedInImplementationOfModuleInInit(get_location(lambdaSearcher.FoundLambda)));
                    }
                }
            }

            weak_node_test_and_visit(_implementation_node.implementation_definitions);
        }

        public override void visit(SyntaxTree.interface_node _interface_node)
        {
            if (_interface_node.interface_definitions != null && _interface_node.interface_definitions.defs != null)
            {
                foreach (var def in _interface_node.interface_definitions.defs.Where(d => d is const_definition ||
                                                                                     d is consts_definitions_list ||
                                                                                     d is variable_definitions))
                {
                    var lambdaSearcher = new LambdaSearcher(def);
                    if (lambdaSearcher.CheckIfContainsLambdas())
                    {
                        AddError(new LambdasNotAllowedInInterfacePartOfModuleInInit(get_location(lambdaSearcher.FoundLambda)));
                    }
                }
            }

            weak_node_test_and_visit(_interface_node.interface_definitions);
        }

        public override void visit(SyntaxTree.typecast_node node)
        {

            expression_node en = convert_strong(node.expr);
            location loc = get_location(node);
            type_node tp = convert_strong(node.type_def);
            //ssyy
            if (en.type.semantic_node_type == semantic_node_type.delegated_method)
            {
                try_convert_typed_expression_to_function_call(ref en);
            }
            //\ssyy
            if (!SemanticRules.IsAsForPointers && (tp.IsPointer || en.type.IsPointer))
            {
                if (node.cast_op == PascalABCCompiler.SyntaxTree.op_typecast.is_op)
                    AddError(loc, "OPERATOR_{0}_CAN_NOT_BE_APPLIED_TO_POINTER_TYPE", compiler_string_consts.is_name);
                else
                    AddError(loc, "OPERATOR_{0}_CAN_NOT_BE_APPLIED_TO_POINTER_TYPE", compiler_string_consts.as_name);
            }
            else
            	if (!(type_table.is_derived(en.type, tp) || type_table.is_derived(tp, en.type) || en.type == tp || en.type == SystemLibrary.SystemLibrary.object_type
            	      || en.type.IsInterface || tp.IsInterface))
                {
                    AddError(loc, "EXPECTED_DERIVED_CLASSES");
                }
            if (en.type.IsSealed && tp.IsInterface && !en.type.ImplementingInterfaces.Contains(tp))
            {
                AddError(loc, "CAN_NOT_CONVERT_TYPE_{0}_TO_INTERFACE_{1}", en.type.PrintableName, tp.PrintableName);
            }
            if (node.cast_op == PascalABCCompiler.SyntaxTree.op_typecast.is_op)
            {
                is_node isn = new is_node(en, tp, loc);
                return_value(isn);
            }
            else
            {
                if (tp.is_generic_parameter && !tp.is_class)
                    AddError(get_location(node.type_def), "OPERATOR_AS_CAN_NOT_BE_USED_WITH_GENERIC_PARAMETER_{0}_WITHOUT_CLASS_CONSTRAINT", tp.name);
                if (tp.is_value_type)
                    AddError(get_location(node.type_def), "OPERATOR_AS_MUST_BE_USED_WITCH_A_REFERENCE_TYPE_VALUETYPE{0}", tp.PrintableName);
                as_node asn = new as_node(en, tp, loc);
                return_value(asn);
            }
        }

        public override void visit(SyntaxTree.inherited_method_call _inherited_method_call)
        {
            throw new NotSupportedError(get_location(_inherited_method_call));
        }

        public override void visit(SyntaxTree.destructor _destructor)
        {
        	if (_destructor.name == null)
        	{
                AddError(get_location(_destructor), "DESTRUCTOR_MUST_HAVE_NAME");
        	}
        	if (context.converting_block() == block_type.type_block)
            {
                if (context.converted_type.IsInterface)
                {
                    AddError(get_location(_destructor), "DESTRUCTOR_IN_INTERFACE");
                }
                if (!context.converted_type.is_class)
                {
                    AddError(get_location(_destructor), "DESTRUCTOR_NOT_IN_CLASS");
                }
            }
            else if (!(context.converting_block() == block_type.namespace_block && _destructor.name.class_name != null))
            {
                AddError(get_location(_destructor), "DESTRUCTOR_NOT_IN_CLASS");
            }
            if (_destructor.parameters != null && _destructor.parameters.params_list.Count > 0)
            {
                AddError(get_location(_destructor), "DESTRUCTOR_CANNOT_HAVE_PARAMETERS");
            }
            visit_procedure_header(_destructor);
        }

        //ssyy изменил этот метод из-за изменения концепции конструкторов
        public override void visit(SyntaxTree.constructor _constructor)
        {
            if (context.converting_block() == block_type.type_block)
            {
                if (context.converted_type.IsInterface)
                {
                    AddError(new ConstructorInInterface(get_location(_constructor)));
                }
                /*if (!context.converted_type.is_class)
                {
                    throw new ConstructorNotInClass(get_location(_constructor));
                }*/
            }
            else if (!(context.converting_block() == block_type.namespace_block && _constructor.name != null && _constructor.name.class_name != null))
            {
                AddError(get_location(_constructor), "CONSTRUCTOR_NOT_IN_CLASS");
            }

            convertion_data_and_alghoritms.check_node_parser_error(_constructor.name);

            if (_constructor.name == null)
            {
                SyntaxTree.ident name = new SyntaxTree.ident(compiler_string_consts.default_constructor_name);
                _constructor.name = new PascalABCCompiler.SyntaxTree.method_name(null, null, name, null);
                _constructor.name.source_context = _constructor.name.meth_name.source_context = _constructor.source_context;
            }

            if (_constructor.name.meth_name.name.ToLower() != compiler_string_consts.default_constructor_name)
                AddError(get_location(_constructor.name), "CONSTRUCTOR_CAN_HAVE_ONLY_{0}_NAME", compiler_string_consts.default_constructor_name);
            if ((_constructor.name.class_name == null) && (context.converting_block() != block_type.type_block))
            {
                AddError(get_location(_constructor.name), "ONLY_CONSTRUCTOR_OF_TYPE_ALLOWED");
            }

            foreach (SyntaxTree.procedure_attribute att in _constructor.proc_attributes.proc_attributes)
            {
                if (att.attribute_type == SyntaxTree.proc_attribute.attr_virtual)
                {
                    AddError(get_location(att), "CONSTRUCTOR_CAN_NOT_BE_VIRTUAL");
                }
                else if (att.attribute_type == SyntaxTree.proc_attribute.attr_override)
                {
                    AddError(get_location(att), "CONSTRUCTOR_CAN_NOT_BE_OVERRIDE");
                }
                else if (att.attribute_type == SyntaxTree.proc_attribute.attr_abstract)
                {
                    AddError(get_location(att), "CONSTRUCTOR_CANNOT_BE_ABSTRACT");
                }
            }
            if (_constructor.class_keyword)
            {
                SyntaxTree.procedure_attribute pa = new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static);
                pa.source_context = _constructor.source_context;
                _constructor.proc_attributes.proc_attributes.Add(pa);
            }
            foreach (SyntaxTree.procedure_attribute att in _constructor.proc_attributes.proc_attributes)
            {
                if (att.attribute_type == SyntaxTree.proc_attribute.attr_static)
                {
                    //Это статический конструктор
                    if (_constructor.parameters != null)
                    {
                        AddError(get_location(_constructor.name), "STATIC_CONSTRUCTOR_MUST_BE_PARAMETERLESS");
                    }
                    _constructor.name.meth_name.name = compiler_string_consts.static_ctor_prefix + _constructor.name.meth_name.name;
                    visit_procedure_header(_constructor);
                    if (context.top_function != null)
                    {
                        (context.top_function as common_method_node).is_constructor = true;
                        (context.top_function as common_method_node).cont_type.static_constr = context.top_function as common_method_node;
                    }
                    return;
                }
            }

            if (context.converting_block() == block_type.type_block)
            {
                //Нужно для генерации унаследованных конструкторов
                context.converted_type.has_user_defined_constructor = true;

                //Нужно для генерации конструктора по умолчанию
                if (_constructor.parameters == null || _constructor.parameters.params_list[0].inital_value != null)
                {
                    context.converted_type.has_default_constructor = true;
                }
            }

            visit_procedure_header(_constructor);

            if (context.top_function == null)
            {
                return;
            }
            common_method_node cmn = context.top_function as common_method_node;

            if (cmn == null)
            {
                throw new CompilerInternalError("Constructor is not common_method_node.");
            }

            context.allow_inherited_ctor_call = true;
            cmn.is_constructor = true;
            cmn.return_value_type = context.converted_type;
        }

        public override void visit(SyntaxTree.file_type_definition _file_type_definition)
        {
            throw new NotSupportedError(get_location(_file_type_definition));
        }

        public override void visit(SyntaxTree.nil_const _nil_const)
        {
            return_value(new null_const_node(null_type_node.get_type_node(), get_location(_nil_const)));
        }

        public override void visit(SyntaxTree.class_predefinition _class_predefinition)
        {
            throw new NotSupportedError(get_location(_class_predefinition));
        }

        private bool statement_expected = false;
        private bool procedure_wait = false;
        public override void visit(SyntaxTree.procedure_call _procedure_call)
        {
            procedure_wait = true;
            SyntaxTree.method_call mc = null;
            mc = _procedure_call.func_name as SyntaxTree.method_call;
            if (mc == null)
            {
                SyntaxTree.ident id = _procedure_call.func_name as SyntaxTree.ident;
                if (id != null)
                {
                    mc = new SyntaxTree.method_call();
                    mc.dereferencing_value = id;
                    //DarkStar Add
                    //Добавил т.к. нужно для генерации отладочной инфы
                    mc.source_context = id.source_context;
                }
                else
                {
                    SyntaxTree.addressed_value adrv = _procedure_call.func_name as SyntaxTree.addressed_value;
                    if (adrv != null)
                    {
                        mc = new SyntaxTree.method_call();
                        mc.dereferencing_value = adrv;
                        //DarkStar Add
                        //Добавил т.к. нужно для генерации отладочной инфы
                        mc.source_context = adrv.source_context;
                    }
                    else
                    {
                        throw new CompilerInternalError("Undefined procedure name");
                    }
                }
            }
            //statement_node sn=convert_strong(_procedure_call.func_name);
            statement_node sn = convert_strong(mc);
            //проверяем на слукчай вызова типа real(a) ...
            if (/*((sn is base_function_call) && (sn as base_function_call).IsExplicitConversion)
                || (sn is SemanticTree.IReferenceNode)
                || (sn is SemanticTree.IGetAddrNode)
                || (sn is null_const_node)
                || (sn is simple_array_indexing)
                || (sn is basic_function_call)
                || (sn is constant_node)
                || (sn is class_field_reference)*/
                !(sn is base_function_call) && !(sn is exit_procedure) && !(sn is while_break_node) && !(sn is while_continue_node)
                && !(sn is repeat_break_node) && !(sn is repeat_continue_node) && !(sn is for_break_node) && !(sn is for_continue_node)
                && !(sn is foreach_break_node) && !(sn is foreach_continue_node)
              )
                AddError(get_location(_procedure_call), "STATEMENT_EXPECTED");

            return_value(sn);
            //_procedure_call.func_name.visit(this);
        }

        public override void visit(SyntaxTree.variant_record_type _variant_record_type)
        {
            throw new NotSupportedError(get_location(_variant_record_type));
        }

        public override void visit(SyntaxTree.variant_types _variant_types)
        {
            throw new NotSupportedError(get_location(_variant_types));
        }

        public override void visit(SyntaxTree.variant_type _variant_type)
        {
            throw new NotSupportedError(get_location(_variant_type));
        }

        public override void visit(SyntaxTree.variant_list _variant_list)
        {
            throw new NotSupportedError(get_location(_variant_list));
        }

        public override void visit(SyntaxTree.variant _variant)
        {
            throw new NotSupportedError(get_location(_variant));
        }

        public override void visit(SyntaxTree.string_num_definition node)
        {
            location loc = get_location(node);
            //if (SystemLibrary.SystemLibInitializer.ShortStringType.NotFound)
            //    throw new NotSupportedError(loc);
            type_node tn = find_type(node.name.name, loc);
            if (!(tn == SystemLibrary.SystemLibrary.string_type))
                AddError(new ExpectedType(SystemLibrary.SystemLibrary.string_type.name, loc));
            constant_node cn = convert_strong_to_constant_node(node.num_of_symbols, SystemLibrary.SystemLibrary.integer_type);
            int length = (cn as int_const_node).constant_value;
            if (length < 1 || length > 256)
                AddError(loc, "TYPE_CAN_NOT_HAVE_THIS_SIZE_{0}", SystemLibrary.SystemLibrary.string_type.name);
            return_value(context.create_short_string_type(length, loc));
        }

        public override void visit(SyntaxTree.literal_const_line _literal_const_line)
        {
            //DarkStar Add
            string s = "";
            SyntaxTree.string_const strc;
            SyntaxTree.sharp_char_const sharpcharconst;
            SyntaxTree.char_const charconst;
            SyntaxTree.syntax_tree_node tnf = _literal_const_line.literals[0], tnl = null;
            for (int i = 0; i < _literal_const_line.literals.Count; i++)
            {
                if ((strc = _literal_const_line.literals[i] as SyntaxTree.string_const) != null)
                    s = s + strc.Value;
                else
                    if ((sharpcharconst = _literal_const_line.literals[i] as SyntaxTree.sharp_char_const) != null)
                        s = s + Convert.ToChar(sharpcharconst.char_num);
                    else
                        if ((charconst = _literal_const_line.literals[i] as SyntaxTree.char_const) != null)
                            s = s + charconst.cconst;
                if (i == _literal_const_line.literals.Count - 1)
                    tnl = _literal_const_line.literals[i];
            }
            expression_node en = new string_const_node(s, new location(tnf.source_context.begin_position.line_num, tnf.source_context.begin_position.column_num,
                tnl.source_context.end_position.line_num, tnl.source_context.end_position.column_num, current_document));
            return_value(en);
        }

        public override void visit(SyntaxTree.sharp_char_const _sharp_char_const)
        {
            return_value(new char_const_node(Convert.ToChar(_sharp_char_const.char_num), get_location(_sharp_char_const)));
        }

        public override void visit(SyntaxTree.raise_statement _raise_statement)
        {
            throw new NotSupportedError(get_location(_raise_statement));
        }

        public override void visit(SyntaxTree.char_const _char_const)
        {
            expression_node en = new char_const_node(_char_const.cconst, get_location(_char_const));
            return_value(en);
        }
		
        private bool check_if_has_enum_consts(SyntaxTree.enum_type_definition _enum_type_definition)
        {
        	bool has_val = false;
        	foreach (SyntaxTree.enumerator en in _enum_type_definition.enumerators.enumerators)
            {
        		if (en.value != null)
        		{
        			has_val = true;
        		}
        		else
        		if (has_val)
                    AddError(get_location(en), "ALL_ENUM_CONSTANTS_NEEDED");
        	}
        	return has_val;
        }
        
        public override void visit(SyntaxTree.enum_type_definition _enum_type_definition)
        {
            //throw new NotSupportedError(get_location(_enum_type_definition));
            //ivan added
            int num = 0;
            List<constant_definition_node> cnsts = new List<constant_definition_node>();
            check_if_has_enum_consts(_enum_type_definition);
            foreach (SyntaxTree.enumerator en in _enum_type_definition.enumerators.enumerators)
            {
                SyntaxTree.ident id = en.name;
                constant_definition_node cdn = context.add_const_definition(id.name, get_location(id));
                if (en.value == null)
                cdn.const_value = new enum_const_node(num++, null, get_location(id));
                else
                {
                	constant_node cn = convert_strong_to_constant_node(en.value,SystemLibrary.SystemLibrary.integer_type);
                	check_for_strong_constant(cn,get_location(en.value));
                	cdn.const_value = new enum_const_node((cn as int_const_node).constant_value,null,get_location(id));
                }
                cnsts.Add(cdn);
            }
            common_type_node ctn = context.create_enum_type(null, get_location(_enum_type_definition)); //_enum_type_definition.values
            num = 0;
            foreach (SyntaxTree.enumerator en in _enum_type_definition.enumerators.enumerators)
            {
                SyntaxTree.ident id = en.name;
                constant_definition_node cdn = context.add_const_definition(id.name, get_location(id));
                if (en.value == null)
                cdn.const_value = new enum_const_node(num++, null, get_location(id));
                else
                {
                	constant_node cn = convert_strong_to_constant_node(en.value,SystemLibrary.SystemLibrary.integer_type);
                	check_for_strong_constant(cn,get_location(en.value));
                	cdn.const_value = new enum_const_node((cn as int_const_node).constant_value,null,get_location(id));
                }
                cdn.const_value.type = ctn;
            }
            foreach (constant_definition_node cdn in cnsts)
                cdn.const_value.type = ctn;
            internal_interface ii = SystemLibrary.SystemLibrary.integer_type.get_internal_interface(internal_interface_kind.ordinal_interface);
            ordinal_type_interface oti_old = (ordinal_type_interface)ii;
            enum_const_node lower_value = new enum_const_node(0, ctn, ctn.loc);
            enum_const_node upper_value = new enum_const_node(ctn.const_defs.Count - 1, ctn, ctn.loc);
            ordinal_type_interface oti_new = new ordinal_type_interface(oti_old.inc_method, oti_old.dec_method,
                oti_old.inc_value_method, oti_old.dec_value_method,
                oti_old.lower_eq_method, oti_old.greater_eq_method, oti_old.lower_method, oti_old.greater_method, lower_value, upper_value, oti_old.value_to_int, oti_old.ordinal_type_to_int);

            ctn.add_internal_interface(oti_new);
            //foreach (constant_definition_node cdn in cnsts)
            //  cdn.const_value.type = ctn;
            //context.leave_block();
            context.leave_record();
            return_value(ctn);

        }

        public override void visit(SyntaxTree.record_type _record_type)
        {
            /*
			// TODO:  Add semantic_tree_generator.SyntaxTree.IVisitor.visit implementation
            common_type_node ctn = blocks.create_record_type(get_location(_record_type));
            ctn.internal_is_value = true;
            foreach (SyntaxTree.var_def_statement var in _record_type.parts.fixed_part.vars)
            {
                hard_node_test_and_visit(var);
            }
            blocks.leave_record();
            return_value(ctn);
            //throw new NotSupportedError(get_location(_record_type));
             */
        }

        public override void visit(SyntaxTree.record_const node)
        {
        	if (is_typed_const_def)
        	{
        		record_constant rc = new record_constant(node.rec_consts, get_location(node));
            	rc.SetType(new RecordConstType(rc.location));
            	return_value(rc);
        	}
        	else
        	{
        		record_initializer ri = new record_initializer(node.rec_consts, get_location(node));
        		ri.type = new RecordConstType(ri.location);
        		return_value(ri);
        	}
        }

        public override void visit(SyntaxTree.record_const_definition _record_const_definition)
        {
            throw new NotSupportedError(get_location(_record_const_definition));
        }

        public override void visit(SyntaxTree.set_type_definition _set_type_definition)
        {
            // throw new NotSupportedError(get_location(_set_type_definition));
            if (SystemLibrary.SystemLibInitializer.TypedSetType == null || SystemLibrary.SystemLibInitializer.CreateSetProcedure == null)
            	AddError(new NotSupportedError(get_location(_set_type_definition)));
            type_node el_type = convert_strong(_set_type_definition.of_type);
            if (el_type.IsPointer)
                AddError(get_location(_set_type_definition.of_type), "POINTERS_IN_SETS_NOT_ALLOWED");
            //if (el_type == SystemLibrary.SystemLibrary.void_type)
            //	AddError(new VoidNotValid(get_location(_set_type_definition.of_type)));
            check_for_type_allowed(el_type,get_location(_set_type_definition.of_type));
            return_value(context.create_set_type(el_type, get_location(_set_type_definition)));
        }

        public override void visit(SyntaxTree.known_type_definition _known_type_definition)
        {
            throw new NotSupportedError(get_location(_known_type_definition));
        }

        public override void visit(SyntaxTree.default_indexer_property_node _default_indexer_property_node)
        {
            throw new NotSupportedError(get_location(_default_indexer_property_node));
        }

        public override void visit(SyntaxTree.class_definition _class_definition)
        {
            if ((_class_definition.attribute & PascalABCCompiler.SyntaxTree.class_attribute.Sealed) == SyntaxTree.class_attribute.Sealed)
            {
                context.converted_type.SetIsSealed(true);
                if (_class_definition.keyword == SyntaxTree.class_keyword.Interface || _class_definition.keyword == SyntaxTree.class_keyword.TemplateInterface)
                    ErrorsList.Add(new SimpleSemanticError(get_location(_class_definition), "INTERFACE_CANNOT_BE_SEALED"));
            }
            if ((_class_definition.attribute & PascalABCCompiler.SyntaxTree.class_attribute.Abstract) == SyntaxTree.class_attribute.Abstract)
            {
                context.converted_type.SetIsAbstract(true);
            }
            switch (_class_definition.keyword)
            {
                case PascalABCCompiler.SyntaxTree.class_keyword.Class:
                    if (context.converted_type.is_value || context.converted_type.IsInterface)
                    {
                        AddError(get_location(_class_definition), "FORWARD_DECLARATION_OF_{0}_MISMATCH_DECLARATION", context.converted_type.name);
                    }
                    if ((_class_definition.class_parents != null) && (_class_definition.class_parents.types.Count > 0))
                    {
                        type_node tn = ret.visit(_class_definition.class_parents.types[0]);
                        //if (tn == context.converted_type)
                        if (type_table.original_types_equals(tn, context.converted_type))
                            AddError(new UndefinedNameReference(tn.name, get_location(_class_definition.class_parents.types[0])));
                        if (tn.IsSealed)
                            AddError(get_location(_class_definition.class_parents.types[0]), "PARENT_TYPE_IS_SALED{0}", tn);
                        if (tn == SystemLibrary.SystemLibrary.value_type)
                        {
                            AddError(get_location(_class_definition.class_parents.types[0]), "CAN_NOT_INHERIT_FROM_VALUE_TYPE");
                        }
                        if (tn.IsPointer)
                        {
                            AddError(get_location(_class_definition.class_parents.types[0]), "CAN_NOT_INHERIT_FROM_POINTER");
                        }
                        if (tn.IsDelegate)
                        {
                            AddError(get_location(_class_definition.class_parents.types[0]), "CAN_NOT_INHERIT_FROM_FUNCTION_TYPE");
                        }
                        if (tn == SystemLibrary.SystemLibrary.delegate_base_type || tn == SystemLibrary.SystemLibrary.system_delegate_type)
                        {
                            AddError(get_location(_class_definition.class_parents.types[0]), "CAN_NOT_INHERIT_FROM_DELEGATE_TYPE");
                        }
                        Hashtable used_interfs = new Hashtable();
                        //Если tn - это интерфейс
                        if (tn.IsInterface)
                        {
                            //Первым в списке предков идёт интерфейс, поэтому базовым будет object.
                            context.converted_type.SetBaseType(SemanticRules.ClassBaseType);

                            //Добавляем интерфейс
                            used_interfs.Add(tn, tn);
                            type_table.AddInterface(context.converted_type, tn, get_location(_class_definition.class_parents.types[0]));
                        }
                        else
                        {
                            if (tn.ForwardDeclarationOnly)
                            {
                                AddError(get_location(_class_definition.class_parents.types[0]), "FORWARD_DECLARATION_{0}_AS_BASE_TYPE", tn.name);
                            }
                            if (tn.is_generic_parameter)
                            {
                                AddError(get_location(_class_definition.class_parents.types[0]), "CAN_NOT_INHERIT_FROM_GENERIC_PARAMETER");
                            }
                            context.converted_type.SetBaseType(tn);
                        }
                        //Теперь добавляем интерфейсы.
                        //Цикл с единицы, т.к. нулевой элемент уже был рассмотрен.
                        VisitAndAddInterfaces(context.converted_type, _class_definition.class_parents.types, 1, used_interfs);
                        //for (int i = 1; i < _class_definition.class_parents.types.Count; i++)
                        //{
                        //    type_node interf = ret.visit(_class_definition.class_parents.types[i]);
                        //    AddInterface(context.converted_type, interf, get_location(_class_definition.class_parents.types[i]));
                        //}
                    }
                    else
                    {
                        context.converted_type.SetBaseType(SemanticRules.ClassBaseType);
                    }
                    context.converted_type.is_class = true;
                    if (_class_definition.body == null &&
                        (_class_definition.class_parents == null || _class_definition.class_parents.types.Count == 0))
                    {
                        if (context.converted_type.ForwardDeclarationOnly)
                        {
                            AddError(new NameRedefinition(context.converted_type.name, context.converted_type.Location, get_location(_class_definition)));
                        }
                        context.types_predefined.Add(context.converted_type);
                        context.converted_type.ForwardDeclarationOnly = true;
                    }
                    else
                    {
                        context.converted_type.ForwardDeclarationOnly = false;
                        if (_class_definition.body == null)
                        {
                            generate_inherit_constructors();
                            if (!context.converted_type.has_default_constructor)
                            {
                                generate_default_constructor();
                            }
                        }
                        List<SyntaxTree.ident> names = new List<SyntaxTree.ident>();
                        List<SyntaxTree.type_definition> types = new List<SyntaxTree.type_definition>();
                        if (((_class_definition.attribute & PascalABCCompiler.SyntaxTree.class_attribute.Auto) == SyntaxTree.class_attribute.Auto))
                        {
                            if (_class_definition.class_parents!=null)
                                AddError(new AutoClassMustNotHaveParents(get_location(_class_definition)));
                            // добавление членов автоклассов. Не забыть сделать, что от автоклассов нельзя наследовать
                            SyntaxTreeBuilder.AddMembersForAutoClass(_class_definition,ref names,ref types);
                            for (var i = 0; i < types.Count; i++)
                            {
                                type_node tn = convert_strong(types[i]); 
                                if (tn.IsPointer)
                                    AddError(get_location(types[i]), "AUTO_CLASS_MUST_NOT_HAVE_POINTERS");
                            }
                        }
                        //if (!SemanticRules.OrderIndependedNames)

                        weak_node_test_and_visit(_class_definition.body);
                        
                    }
                    context.leave_block();
                    return;
                case PascalABCCompiler.SyntaxTree.class_keyword.Record:
                    common_type_node ctn;
                    if (_record_created)
                    {
                        _record_created = false;
                        ctn = context.converted_type;
                    }
                    else
                    {
                        ctn = context.create_record_type(get_location(_class_definition), record_type_name);
                        if (record_is_generic)
                        {
                            context.create_generic_indicator(ctn);
                            visit_generic_params(ctn, _class_definition.template_args.idents);
                        }
                        else
                        {
                            if (_class_definition.template_args != null && _class_definition.template_args.idents != null)
                            {
                                AddError(get_location(_class_definition.template_args), "GENERIC_TYPE_NOT_ALLOWED_HERE");
                            }
                        }
                        visit_where_list(_class_definition.where_section);
                    }

                    if (context.CurrentScope is SymbolTable.BlockScope)
                    {
                        AddError(get_location(_class_definition), "RECORD_CANNOT_BE_DECLARED_IN_BLOCK");
                        ctn.internal_is_value = true;
                        context.leave_record();
                        return_value(ctn);
                        return;
                    }

                    //Добавляем интерфейсы - предки.
                    if (_class_definition.class_parents != null)
                    {
                        VisitAndAddInterfaces(context.converted_type, _class_definition.class_parents.types, 0, new Hashtable());
                        //for (int i = 0; i < _class_definition.class_parents.types.Count; i++)
                        //{
                        //    type_node interf = ret.visit(_class_definition.class_parents.types[i]);
                        //    AddInterface(context.converted_type, interf, get_location(_class_definition.class_parents.types[i]));
                        //}
                    }
                    CheckWaitedRefTypes(ctn);
                    record_type_name = null;
                    record_is_generic = false;
                    context.converted_type.internal_is_value = true;
                    if (_class_definition.body == null &&
                        (_class_definition.class_parents == null || _class_definition.class_parents.types.Count == 0))
                    {
                        if (context.converted_type.ForwardDeclarationOnly)
                        {
                            AddError(new NameRedefinition(context.converted_type.name, context.converted_type.Location, get_location(_class_definition)));
                        }
                        context.types_predefined.Add(context.converted_type);
                        context.converted_type.ForwardDeclarationOnly = true;
                    }
                    else
                    {
                        context.converted_type.ForwardDeclarationOnly = false;
                        weak_node_test_and_visit(_class_definition.body);
                    }
                    /*if (_class_definition.body != null)
                        hard_node_test_and_visit(_class_definition.body);
                    else
                    {
                        context.converted_type.ForwardDeclarationOnly = true;
                    }*/
                    /*if (_class_definition.body != null)
                    foreach (SyntaxTree.var_def_statement var in (_class_definition.body.class_def_blocks[0].members))
                    {
                        hard_node_test_and_visit(var);
                    }*/
                    context.leave_record();
                    return_value(ctn);
                    return;
                //ssyy owns
                case PascalABCCompiler.SyntaxTree.class_keyword.Interface:
                    if (context.converted_type.is_value || !context.converted_type.IsInterface)
                    {
                        AddError(get_location(_class_definition), "FORWARD_DECLARATION_OF_{0}_MISMATCH_DECLARATION", context.converted_type.name);
                    }
                    if (SemanticRules.GenerateNativeCode)
                        context.converted_type.SetBaseType(SemanticRules.ClassBaseType);
                    else
                        context.converted_type.SetBaseType(SystemLibrary.SystemLibrary.object_type);
                    context.converted_type.IsInterface = true;
                    context.converted_type.is_class = true;
                    //Добавляем интерфейсы - предки.
                    if (_class_definition.class_parents != null)
                    {
                        VisitAndAddInterfaces(context.converted_type, _class_definition.class_parents.types, 0, new Hashtable());
                        //for (int i = 0; i < _class_definition.class_parents.types.Count; i++)
                        //{
                        //    type_node interf = ret.visit(_class_definition.class_parents.types[i]);
                        //    AddInterface(context.converted_type, interf, get_location(_class_definition.class_parents.types[i]));
                        //}
                    }

                    InitInterfaceScope(context.converted_type);

                    //if (_class_definition.body == null)
                    //{
                        //throw new NotSupportedError(get_location(_class_definition));
                    //    throw new InterfaceForwardDeclaration(get_location(_class_definition));
                    //}
                    if (_class_definition.body == null &&
                        (_class_definition.class_parents == null || _class_definition.class_parents.types.Count == 0))
                    {
                        if (context.converted_type.ForwardDeclarationOnly)
                        {
                            AddError(new NameRedefinition(context.converted_type.name, context.converted_type.Location, get_location(_class_definition)));
                        }
                        context.types_predefined.Add(context.converted_type);
                        context.converted_type.ForwardDeclarationOnly = true;
                    }
                    else
                    {
                        context.converted_type.ForwardDeclarationOnly = false;
                        weak_node_test_and_visit(_class_definition.body);
                    }
                    //hard_node_test_and_visit(_class_definition.body);
                    context.leave_block();
                    return;
                //\ssyy owns
                default:
                    throw new NotSupportedError(get_location(_class_definition));
            }
        }

        private void VisitAndAddInterfaces(common_type_node t, List<SyntaxTree.named_type_reference> types, int start, Hashtable used_interfaces)
        {
            for (int i = start; i < types.Count; i++)
            {
                type_node interf = ret.visit(types[i]);
                location loc = get_location(types[i]);
                if (used_interfaces[interf] != null)
                {
                    AddError(loc, "INTERFACE_{0}_ALREADY_ADDED_TO_IMPLEMENTING_LIST", interf.PrintableName);
                }
                used_interfaces.Add(interf, interf);
                type_table.AddInterface(t, interf, loc);
            }
        }

        private void InitInterfaceScope(common_type_node ctn)
        {
            List<SymbolTable.Scope> interf_scopes = new List<SymbolTable.Scope>(ctn.ImplementingInterfaces.Count);
            foreach (type_node tnode in ctn.ImplementingInterfaces)
            {
                interf_scopes.Add(tnode.Scope);
            }
            (ctn.Scope as SymbolTable.InterfaceScope).TopInterfaceScopeArray =
                interf_scopes.ToArray();
        }
		
        private void visit_function_realizations(SyntaxTree.declarations _decls)
        {
        	foreach (SyntaxTree.declaration sd in _decls.defs)
        	{
        		common_namespace_function_node cnfn = context.get_method_to_realize(sd) as common_namespace_function_node;
        		if (cnfn != null)
        		{
        			context.push_function(cnfn);
        			
        			if ((sd as SyntaxTree.procedure_definition).proc_body != null)
                	{
                    	hard_node_test_and_visit((sd as SyntaxTree.procedure_definition).proc_body);
                    	add_clip_for_set(context.top_function);
                    	context.leave_block();
                	}
        		}
        	}
        }
        
        private void visit_class_member_realizations(SyntaxTree.class_body _class_body)
        {
        	foreach (SyntaxTree.class_members clmem in _class_body.class_def_blocks)
            {
        		foreach (SyntaxTree.declaration sd in clmem.members)
            	{
        			common_method_node cmn = context.get_method_to_realize(sd) as common_method_node;
        			
        			if (cmn != null)
        			{
        				context.push_function(cmn);
        				if (!cmn.IsStatic)
                		{
                            type_node self_type = cmn.cont_type;
                            if (cmn.cont_type.is_generic_type_definition)
                                self_type = cmn.cont_type.get_instance(cmn.cont_type.generic_params.ConvertAll<type_node>(o => (type_node)o));// new generic_instance_type_node(self_type, self_type.generic_params.ConvertAll<type_node>(o => (type_node)o), self_type.base_type, self_type.name, self_type.type_access_level, self_type.comprehensive_namespace, self_type.loc);
                    		local_variable lv = new local_variable(compiler_string_consts.self_word, self_type, cmn, null);
                    		cmn.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(lv));
                    		cmn.self_variable = lv;
                		}
        				if ((sd as SyntaxTree.procedure_definition).proc_body != null)
                		{
                    		hard_node_test_and_visit((sd as SyntaxTree.procedure_definition).proc_body);
                    		add_clip_for_set(context.top_function);
                    		context.leave_block();
                		}
        			}
            	}
        	}
        	context.clear_member_bindings();
        }
        
        public override void visit(SyntaxTree.class_body _class_body)
        {
            foreach (SyntaxTree.class_members clmem in _class_body.class_def_blocks)
            {
                //hard_node_test_and_visit(clmem);
                weak_node_test_and_visit(clmem);
            }
            if (SemanticRules.OrderIndependedMethodNames)
            {
            	visit_class_member_realizations(_class_body);
            }
            if (!context.converted_type.is_value && !context.converted_type.IsInterface)
            {
                generate_inherit_constructors();
            }
            if (!context.converted_type.IsInterface && context.converted_type.static_constr == null)
            {
            	//generate_static_constructor();
            }
            if (!context.converted_type.IsInterface && !context.converted_type.has_default_constructor)
            {
                generate_default_constructor();
            }
            
        }
		
        private bool disable_order_independ;
        
        //public void generate_static_constructor()
        //{
        //    common_type_node ctn = context.converted_type;
        //    ctn.static_constr = new common_method_node(PascalABCCompiler.TreeConverter.compiler_string_consts.static_ctor_prefix+"Create",null,ctn,SemanticTree.polymorphic_state.ps_static,SemanticTree.field_access_level.fal_private,null);
        //    ctn.static_constr.is_constructor = true;
        //    ctn.static_constr.function_code = new statements_list(null);
        //    ctn.methods.AddElement(ctn.static_constr);
        //    //(ctn.static_constr.function_code as statements_list).statements.AddElement(new return_node(null,null));
        //}
        
        //ssyy
        public void generate_default_constructor()
        {
            /*SymbolInfo si = context.converted_type.base_type.find_in_type(compiler_string_consts.default_constructor_name, context.converted_type.base_type.Scope);
            bool base_default_ctor_none = true;
            while (base_default_ctor_none && si != null)
            {
                function_node fn = si.sym_info as function_node;
                compiled_constructor_node pconstr = fn as compiled_constructor_node;
                common_method_node mconstr = fn as common_method_node;
                if ((pconstr != null ||
                    mconstr != null && mconstr.is_constructor) &&
                    fn.parameters.Count == 0
                    )
                {
                    //Нашли конструктор по умолчанию у предка
                    base_default_ctor_none = false;
                }
                si = si.Next;
            }*/
            if (!context.converted_type.is_value)
            {
                if (!generic_convertions.type_has_default_ctor(context.converted_type.base_type, true))
                {
                    //У предка нет конструктора по умолчанию,
                    //невозможно сгенерировать конструктор по умолчанию.
                    AddError(context.converted_type.loc, "NO_DEFAULT_CONSTRUCTOR_INT_TYPE_{0}", context.converted_type.base_type.name);
                    return;
                }
            }

            SyntaxTree.class_members clmem = new SyntaxTree.class_members();
            if (context.converted_type.has_user_defined_constructor)
            {
                clmem.access_mod = new SyntaxTree.access_modifer_node(SyntaxTree.access_modifer.protected_modifer);
            }
            else
            {
                clmem.access_mod = new SyntaxTree.access_modifer_node(SyntaxTree.access_modifer.public_modifer);
            }
            SyntaxTree.procedure_attributes_list pal = new PascalABCCompiler.SyntaxTree.procedure_attributes_list();
            //pal.proc_attributes.Add(new PascalABCCompiler.SyntaxTree.procedure_attribute(SyntaxTree.proc_attribute.attr_overload)); attr_overload - убрал отовсюду! ССМ
            SyntaxTree.constructor constr = new PascalABCCompiler.SyntaxTree.constructor();
            constr.name = new SyntaxTree.method_name(null, null, new PascalABCCompiler.SyntaxTree.ident(compiler_string_consts.default_constructor_name), null);
            constr.proc_attributes = pal;
            SyntaxTree.block bl = new SyntaxTree.block();
            bl.program_code = new SyntaxTree.statement_list();
            bl.program_code.left_logical_bracket = new SyntaxTree.token_info("begin");
            bl.program_code.right_logical_bracket = new SyntaxTree.token_info("end");
            clmem.members.Add(new SyntaxTree.procedure_definition(constr, bl));
            disable_order_independ = true;
            clmem.visit(this);
            disable_order_independ = false;
        }
        //\ssyy

        public override void visit(SyntaxTree.access_modifer_node _access_modifer_node)
        {
            //ssyy
            if (context.converted_type != null && context.converted_type.IsInterface)
            {
                if (_access_modifer_node.access_level == SyntaxTree.access_modifer.none)
                {
                    context.set_field_access_level(SemanticTree.field_access_level.fal_public);
                }
                else
                {
                    AddError(get_location(_access_modifer_node), "ACCESS_MODIFER_IN_INTERFACE");
                }
                return;
            }
            //\ssyy
            switch (_access_modifer_node.access_level)
            {
                case SyntaxTree.access_modifer.private_modifer:
                    {
                        context.set_field_access_level(SemanticTree.field_access_level.fal_private);
                        break;
                    }
                case SyntaxTree.access_modifer.protected_modifer:
                    {
                        context.set_field_access_level(SemanticTree.field_access_level.fal_protected);
                        break;
                    }
                case SyntaxTree.access_modifer.public_modifer:
                    {
                        context.set_field_access_level(SemanticTree.field_access_level.fal_public);
                        break;
                    }
                case SyntaxTree.access_modifer.internal_modifer:
                    {
                        context.set_field_access_level(SemanticTree.field_access_level.fal_internal);
                        break;
                    }
            }
        }

        public override void visit(SyntaxTree.class_members _class_members)
        {
            foreach (var def in _class_members.members.Where(d => d is const_definition ||
                                                            d is consts_definitions_list ||
                                                            d is variable_definitions ||
                                                            d is var_def_statement))
            {
                var lambdaSearcher = new LambdaSearcher(def);
                if (lambdaSearcher.CheckIfContainsLambdas())
                {
                    AddError(new LambdasNotAllowedInFieldsInitialization(get_location(lambdaSearcher.FoundLambda)));
                }
            }

            weak_node_test_and_visit(_class_members.access_mod);
            foreach (SyntaxTree.declaration sd in _class_members.members)
            {
                hard_node_test_and_visit(sd);
            }
        }

        public override void visit(SyntaxTree.index_property _index_property)
        {
            throw new NotSupportedError(get_location(_index_property));
        }

        //TODO: Если одного из акцессоров нет?
        public override void visit(SyntaxTree.simple_property _simple_property)
        {
            if (_simple_property.accessors == null)
                AddError(get_location(_simple_property), "PROPERTYACCESSOR_{0}_OR_{1}_EXPECTED", compiler_string_consts.PascalReadAccessorName, compiler_string_consts.PascalWriteAccessorName);
            if (_simple_property.property_type == null)
                AddError(get_location(_simple_property.property_name), "TYPE_NAME_EXPECTED");
            common_property_node pn = context.add_property(_simple_property.property_name.name,
                get_location(_simple_property.property_name));
            assign_doc_info(pn, _simple_property);
            //pn.polymorphic_state=SemanticTree.polymorphic_state.ps_common;
            //pn.loc=get_location(_simple_property.property_name);
            if (_simple_property.attr == SyntaxTree.definition_attribute.Static)
            	pn.polymorphic_state = SemanticTree.polymorphic_state.ps_static;
            parameter_list pal_big = new parameter_list();
            //TODO: Спросить у Саши как получить тип параметра - var,const и т.д.
            if (_simple_property.parameter_list != null)
            {
                foreach (SyntaxTree.property_parameter pp in _simple_property.parameter_list.parameters)
                {
                    parameter_list pal_small = new parameter_list();
                    foreach (SyntaxTree.ident id in pp.names.idents)
                    {
                        common_parameter cp = new common_parameter(id.name, SemanticTree.parameter_type.value, null,
                            concrete_parameter_type.cpt_none, get_location(id));
                        pal_small.AddElement(cp);
                    }
                    type_node tn = convert_strong(pp.type);
                    foreach (parameter pr in pal_small)
                    {
                        pr.type = tn;
                    }
                    pal_big.AddRange(pal_small);
                }
            }
            pn.parameters.AddRange(pal_big);
            pn.internal_property_type = convert_strong(_simple_property.property_type);

            if (_simple_property.accessors != null)
            {

                convertion_data_and_alghoritms.check_node_parser_error(_simple_property.accessors);

                SymbolInfo si = null;
                //definition_node dn = null;

                if (_simple_property.accessors.read_accessor != null)
                {
                    convertion_data_and_alghoritms.check_node_parser_error(_simple_property.accessors.read_accessor);
                    if (_simple_property.accessors.read_accessor.accessor_name == null)
                    {
                        if (!context.converted_type.IsInterface)
                        {
                            AddError(get_location(_simple_property.accessors.read_accessor), "ACCESSOR_NAME_EXPECTED");
                        }
                        pn.internal_get_function = GenerateGetSetMethodForInterfaceProperty(pn, get_location(_simple_property.accessors.read_accessor), true);
                    }
                    else
                    {
                        convertion_data_and_alghoritms.check_node_parser_error(_simple_property.accessors.read_accessor.accessor_name);
                        si = context.converted_type.find_in_type(_simple_property.accessors.read_accessor.accessor_name.name, context.CurrentScope);

                        location loc1 = get_location(_simple_property.accessors.read_accessor.accessor_name);

                        if (si == null)
                        {
                            AddError(new UndefinedNameReference(_simple_property.accessors.read_accessor.accessor_name.name, loc1));
                        }

                        //dn = check_name_node_type(_simple_property.accessors.read_accessor.accessor_name.name,
                        //    si, loc1, general_node_type.function_node, general_node_type.variable_node);

                        function_node read_accessor = si.sym_info as function_node;

                        if (read_accessor != null)
                        {
                            bool good_func = true;
                            bool one_func = si.Next == null;
                            while (si != null)
                            {
                                good_func = true;
                                if (read_accessor.parameters.Count != pn.parameters.Count)
                                {
                                    good_func = false;
                                    if (one_func)
                                    {
                                        AddError(new PropertyAndReadAccessorParamsCountConvergence(read_accessor, pn, loc1));
                                    }
                                }
                                //TODO: Сверять типы параметров - var, const и т.д.
                                else
                                {
                                    for (int i1 = 0; good_func && i1 < read_accessor.parameters.Count; i1++)
                                    {
                                        if (read_accessor.parameters[i1].parameter_type != pn.parameters[i1].parameter_type ||
                                            read_accessor.parameters[i1].type != pn.parameters[i1].type)
                                        {
                                            good_func = false;
                                            if (one_func)
                                            {
                                                AddError(loc1, "PROPERTY_{0}_AND_READ_ACCESSOR_{1}_PARAMS_TYPE_CONVERGENCE", pn.name, read_accessor.name);
                                            }
                                        }
                                    }
                                    if (read_accessor.return_value_type == null)
                                    {
                                        good_func = false;
                                        if (one_func)
                                        {
                                            AddError(loc1, "PROPERTY_READ_ACCESSOR_CAN_NOT_BE_PROCEDURE");
                                        }
                                    }
                                    if (read_accessor.return_value_type != pn.property_type)
                                    {
                                        good_func = false;
                                        if (one_func)
                                        {
                                            AddError(loc1, "PROPERTY_{0}_AND_READ_ACCESSOR_{1}_RETURN_VALUE_TYPE_CONVERGENCE", pn.name, read_accessor.name);
                                        }
                                    }
                                    if (read_accessor is common_method_node && (read_accessor as common_method_node).is_constructor)
                                    {
                                        AddError(loc1, "ACCESSOR_CAN_BE_FIELD_OR_METHOD_ONLY");
                                    }
                                }
                                if (good_func)
                                {
                                    break;
                                }
                                si = si.Next;
                                if (si != null)
                                {
                                    read_accessor = si.sym_info as function_node;
                                }
                            }
                            if (!good_func)
                            {
                                AddError(loc1, "NO_OVERLOAD_FUNCTION_{0}_USEFUL_FOR_ACCESSOR", read_accessor.name);
                            }
                            read_accessor = GenerateGetMethod(pn,read_accessor as common_method_node,pn.loc);
                           
                        }
                        else
                        {
                            class_field cfield = si.sym_info as class_field;
                            if (cfield == null)
                            {
                                AddError(loc1, "ACCESSOR_CAN_BE_FIELD_OR_METHOD_ONLY");
                            }
                            if (_simple_property.parameter_list != null)
                            {
                                AddError(loc1, "INDEX_PROPERTY_ACCESSOR_CAN_NOT_BE_VARIABLE");
                            }
                            if (cfield.type != pn.internal_property_type)
                            {
                                AddError(loc1, "PROPERTY_TYPE_MISMATCH_ACCESSOR_FIELD_TYPE");
                            }
                            if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static && cfield.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        		AddError(get_location(_simple_property.accessors.read_accessor.accessor_name), "ACCESSOR_{0}_MUST_BE_STATIC", cfield.name);
                    		if (pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && cfield.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                                AddError(get_location(_simple_property.accessors.read_accessor.accessor_name), "ACCESSOR_{0}_CANNOT_BE_STATIC", cfield.name);
                            read_accessor = GenerateGetMethodForField(pn, compiler_string_consts.GetGetAccessorName(pn.name), cfield, loc1);
                        }
                       
                        //Вот здесь уже можем добавить акцессор для чтения.
                        pn.internal_get_function = read_accessor;
                    }
                    if (pn.internal_get_function != null && pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static && pn.internal_get_function.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        AddError(get_location(_simple_property.accessors.read_accessor.accessor_name), "ACCESSOR_{0}_MUST_BE_STATIC", pn.internal_get_function.name);
                    if (pn.internal_get_function != null && pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && pn.internal_get_function.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        AddError(get_location(_simple_property.accessors.read_accessor.accessor_name),  "ACCESSOR_{0}_CANNOT_BE_STATIC", pn.internal_get_function.name);
                }

                if (_simple_property.accessors.write_accessor != null)
                {
                    convertion_data_and_alghoritms.check_node_parser_error(_simple_property.accessors.write_accessor);
                    if (_simple_property.accessors.write_accessor.accessor_name == null)
                    {
                        if (!context.converted_type.IsInterface)
                        {
                            AddError(get_location(_simple_property.accessors.write_accessor), "ACCESSOR_NAME_EXPECTED");
                        }
                        pn.internal_set_function = GenerateGetSetMethodForInterfaceProperty(pn, get_location(_simple_property.accessors.write_accessor), false);
                    }
                    else
                    {
                        convertion_data_and_alghoritms.check_node_parser_error(_simple_property.accessors.write_accessor.accessor_name);
                        si = context.converted_type.find_in_type(_simple_property.accessors.write_accessor.accessor_name.name, context.CurrentScope);

                        location loc2 = get_location(_simple_property.accessors.write_accessor.accessor_name);

                        if (si == null)
                        {
                            AddError( new UndefinedNameReference(_simple_property.accessors.write_accessor.accessor_name.name, loc2));
                        }

                        //dn = check_name_node_type(_simple_property.accessors.write_accessor.accessor_name.name,
                        //    si, loc2, general_node_type.function_node, general_node_type.variable_node);

                        function_node write_accessor = si.sym_info as function_node;

                        if (write_accessor != null)
                        {
                            bool good_func = true;
                            bool one_func = si.Next == null;
                            while (si != null)
                            {
                                good_func = true;
                                if (write_accessor.parameters.Count != pn.parameters.Count + 1)
                                {
                                    good_func = false;
                                    if (one_func)
                                    {
                                        AddError( new PropertyAndWriteAccessorParamsCountConvergence(write_accessor, pn, loc2));
                                    }
                                }
                                else
                                {
                                    //TODO: Сверять типы параметров - var, const и т.д.
                                    for (int i2 = 0; good_func && i2 < pn.parameters.Count; i2++)
                                    {
                                        if (write_accessor.parameters[i2].parameter_type != pn.parameters[i2].parameter_type ||
                                            write_accessor.parameters[i2].type != pn.parameters[i2].type)
                                        {
                                            good_func = false;
                                            if (one_func)
                                            {
                                                AddError(loc2, "PROPERTY_{0}_AND_WRITE_ACCESSOR_{1}_PARAMS_TYPE_CONVERGENCE", pn.name ,write_accessor.name);
                                            }
                                        }
                                    }
                                    if (write_accessor.parameters[write_accessor.parameters.Count - 1].type != pn.property_type ||
                                        write_accessor.parameters[write_accessor.parameters.Count - 1].parameter_type != PascalABCCompiler.SemanticTree.parameter_type.value)
                                    {
                                        good_func = false;
                                        if (one_func)
                                        {
                                            AddError(loc2, "PROPERTY_{0}_AND_WRITE_ACCESSOR_LAST_PARAMETER_TYPE_CONVERGENCE", pn.name);
                                        }
                                    }
                                    if (write_accessor.return_value_type != null)
                                    {
                                        good_func = false;
                                        if (one_func)
                                        {
                                            AddError(loc2, "PROPERTY_WRITE_ACCESSOR_CAN_NOT_BE_FUNCTION");
                                        }
                                    }
                                }
                                if (good_func)
                                {
                                    break;
                                }
                                si = si.Next;
                                if (si != null)
                                {
                                    write_accessor = si.sym_info as function_node;
                                }
                            }
                            if (!good_func)
                            {
                                AddError(loc2, "NO_OVERLOAD_FUNCTION_{0}_USEFUL_FOR_ACCESSOR", write_accessor.name);
                            }
                            if (write_accessor is common_method_node && (write_accessor as common_method_node).is_constructor)
                            {
                                AddError(loc2, "ACCESSOR_CAN_BE_FIELD_OR_METHOD_ONLY");
                            }
                            write_accessor = GenerateSetMethod(pn, write_accessor as common_method_node, pn.loc);
                        }
                            else
                            {
                                class_field cfield = si.sym_info as class_field;
                                if (cfield == null)
                                {
                                    AddError(loc2, "ACCESSOR_CAN_BE_FIELD_OR_METHOD_ONLY");
                                }
                                if (_simple_property.parameter_list != null)
                                {
                                    AddError(loc2, "INDEX_PROPERTY_ACCESSOR_CAN_NOT_BE_VARIABLE");
                                }
                                if (cfield.type != pn.internal_property_type)
                                {
                                    AddError(loc2, "PROPERTY_TYPE_MISMATCH_ACCESSOR_FIELD_TYPE");
                                }
                                if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static && cfield.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        			AddError(get_location(_simple_property.accessors.write_accessor.accessor_name), "ACCESSOR_{0}_MUST_BE_STATIC", cfield.name);
                    			if (pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && cfield.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        			AddError(get_location(_simple_property.accessors.write_accessor.accessor_name), "ACCESSOR_{0}_CANNOT_BE_STATIC", cfield.name);
                                write_accessor = GenerateSetMethodForField(pn, compiler_string_consts.GetSetAccessorName(pn.name), cfield, loc2);
                            }
                            //Вот здесь уже можем добавить акцессор для чтения.
                            pn.internal_set_function = write_accessor;
                        }
                    if (pn.internal_set_function != null && pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static && pn.internal_set_function.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        AddError(get_location(_simple_property.accessors.write_accessor.accessor_name), "ACCESSOR_{0}_MUST_BE_STATIC", pn.internal_set_function.name);
                    if (pn.internal_set_function != null && pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && pn.internal_set_function.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        AddError(get_location(_simple_property.accessors.write_accessor.accessor_name), "ACCESSOR_{0}_CANNOT_BE_STATIC", pn.internal_set_function.name);
                }
            }
            make_attributes_for_declaration(_simple_property,pn);
            //TODO: Можно сделать множество свойств по умолчанию.
            if (_simple_property.array_default != null)
            {
                if (pn.parameters.Count == 0)
                {
                    AddError(pn.loc, "DEFAULT_PROPERTY_MUST_BE_INDEXED");
                }
                if (context.converted_type.default_property_node != null)
                {
                    AddError(pn.loc, "DUPLICATE_DEFAULT_PROPERTY_IN_CLASS");
                }
                context.converted_type.default_property = pn;
            }
        }

        private function_node GenerateGetMethod(common_property_node cpn, common_method_node accessor, location loc)
        {
            common_method_node cmn = new common_method_node(
                "get_"+cpn.name, loc, cpn.common_comprehensive_type,
                cpn.polymorphic_state, cpn.field_access_level, null);
            cpn.common_comprehensive_type.methods.AddElement(cmn);
            cmn.return_value_type = cpn.property_type;
            cmn.is_overload = true;
            foreach (common_parameter cp in accessor.parameters)
            {
                common_parameter new_cp = new common_parameter(cp.name, cp.type, cp.parameter_type, cp.common_function, cp.concrete_parameter_type, cp.default_value, loc);
                cmn.parameters.AddElement(new_cp);
            }
            expression_node meth_call;
            if (cpn.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
            {
                meth_call = new common_method_call(accessor, new this_node(cpn.common_comprehensive_type, loc), loc);
                foreach (common_parameter cp in cmn.parameters)
                {
                    (meth_call as common_method_call).parameters.AddElement(new common_parameter_reference(cp, 0, loc));
                }
            }
            else
            {
                meth_call = new common_static_method_call(accessor, loc);
                foreach (common_parameter cp in cmn.parameters)
                {
                    (meth_call as common_static_method_call).parameters.AddElement(new common_parameter_reference(cp, 0, loc));
                }
            }
            cmn.function_code = new return_node(meth_call, loc);
            cpn.common_comprehensive_type.scope.AddSymbol("get_" + cpn.name, new SymbolInfo(cmn));
            return cmn;
        }

        private function_node GenerateSetMethod(common_property_node cpn, common_method_node accessor, location loc)
        {
            common_method_node cmn = new common_method_node(
                "set_" + cpn.name, loc, cpn.common_comprehensive_type,
                cpn.polymorphic_state, cpn.field_access_level, null);
            cpn.common_comprehensive_type.methods.AddElement(cmn);
            //cmn.return_value_type = cpn.property_type;
            cmn.is_overload = true;
            foreach (common_parameter cp in accessor.parameters)
            {
                common_parameter new_cp = new common_parameter(cp.name, cp.type, cp.parameter_type, cp.common_function, cp.concrete_parameter_type, cp.default_value, loc);
                cmn.parameters.AddElement(new_cp);
            }
            expression_node meth_call;
            if (cpn.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
            {
                meth_call = new common_method_call(accessor, new this_node(cpn.common_comprehensive_type, loc), loc);
                foreach (common_parameter cp in cmn.parameters)
                {
                    (meth_call as common_method_call).parameters.AddElement(new common_parameter_reference(cp, 0, loc));
                }
            }
            else
            {
                meth_call = new common_static_method_call(accessor, loc);
                foreach (common_parameter cp in cmn.parameters)
                {
                    (meth_call as common_static_method_call).parameters.AddElement(new common_parameter_reference(cp, 0, loc));
                }
            }
            cmn.function_code = meth_call;
            cpn.common_comprehensive_type.scope.AddSymbol("set_" + cpn.name, new SymbolInfo(cmn));
            return cmn;
        }

        internal function_node GenerateSetMethodForField(common_property_node cpn, string AcessorName, class_field cf, location loc)
        {
            SymbolInfo exist_si = context.find_only_in_namespace(AcessorName);
            if (exist_si != null && exist_si.sym_info.general_node_type != general_node_type.function_node)
            {
                AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", AcessorName);
            }
            while (exist_si != null)
            {
                function_node ff = (function_node)(exist_si.sym_info);
                if (ff.parameters.Count == 1 && ff.parameters[0].type == cf.type)
                {
                    AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", AcessorName);
                }
                exist_si = exist_si.Next;
            }
            common_method_node cmn = new common_method_node(
                AcessorName, loc, cf.cont_type,
                cf.polymorphic_state, context.get_field_access_level(), null);
            cpn.common_comprehensive_type.methods.AddElement(cmn);
            common_parameter cp = new common_parameter(
                compiler_string_consts.value, cf.type, SemanticTree.parameter_type.value,
                cmn, concrete_parameter_type.cpt_none, null, loc);
            cmn.parameters.AddElement(cp);
            cmn.is_overload = true;
            common_parameter_reference cpr = new common_parameter_reference(cp, 0, loc);
            expression_node var_ref;
            if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
            {
                var_ref = new class_field_reference(cf, new this_node(cf.type, loc), loc);
            }
            else
            {
                var_ref = new static_class_field_reference(cf, loc);
            }
            cmn.function_code = find_operator(compiler_string_consts.assign_name, var_ref, cpr, loc);
            cf.cont_type.scope.AddSymbol(AcessorName, new SymbolInfo(cmn));
            return cmn;
        }

        internal expression_node make_assign_operator(addressed_expression left, expression_node right, location loc)
        {
            return find_operator(compiler_string_consts.assign_name, left, right, loc);
        }

        internal function_node GenerateGetMethodForField(common_property_node cpn, string AcessorName, class_field cf, location loc)
        {
            SymbolInfo exist_si = context.find_only_in_namespace(AcessorName);
            if (exist_si != null && exist_si.sym_info.general_node_type != general_node_type.function_node)
            {
                AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", AcessorName);
            }
            while (exist_si != null)
            {
                if (((function_node)(exist_si.sym_info)).parameters.Count == 0)
                {
                    AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", AcessorName);
                }
                exist_si = exist_si.Next;
            }
            common_method_node cmn = new common_method_node(
                AcessorName, loc, cf.cont_type,
                cf.polymorphic_state, context.get_field_access_level(), null);
            cpn.common_comprehensive_type.methods.AddElement(cmn);
            cmn.return_value_type = cf.type;
            cmn.is_overload = true;
            expression_node var_ref;
            if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
            {
                var_ref = new class_field_reference(cf, new this_node(cf.type, loc), loc);
            }
            else
            {
                var_ref = new static_class_field_reference(cf, loc);
            }
            cmn.function_code = new return_node(var_ref, loc);
            cf.cont_type.scope.AddSymbol(AcessorName, new SymbolInfo(cmn));
            return cmn;
        }

        private function_node GenerateGetSetMethodForInterfaceProperty(common_property_node pn, location loc, bool get_meth)
        {

            string AcessorName = (get_meth) ?
                compiler_string_consts.GetGetAccessorName(pn.name) :
                compiler_string_consts.GetSetAccessorName(pn.name);
            SymbolInfo exist_si = context.find_only_in_namespace(AcessorName);
            if (exist_si != null && exist_si.sym_info.general_node_type != general_node_type.function_node)
            {
                AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", AcessorName);
            }
            common_method_node cmn = new common_method_node(
                AcessorName, loc, pn.common_comprehensive_type,
                pn.polymorphic_state, context.get_field_access_level(), null);
            cmn.is_overload = true;
            foreach (common_parameter cp in pn.parameters)
            {
                common_parameter accp = new common_parameter(cp.name, cp.parameter_type, cmn, cp.concrete_parameter_type, cp.loc);
                accp.type = cp.type;
                accp.inital_value = cp.inital_value;
                accp.intrenal_is_params = cp.intrenal_is_params;
                accp.is_ret_value = cp.is_ret_value;
                cmn.parameters.AddElement(accp);
            }
            if (get_meth)
            {
                cmn.return_value_type = pn.internal_property_type;
            }
            else
            {
                //foreach (common_parameter par in cmn.parameters)
                //{
                //    if (par.name == 
                //}
                common_parameter accp = new common_parameter(compiler_string_consts.value_in_accessor_name,
                    SemanticTree.parameter_type.value, cmn, concrete_parameter_type.cpt_none, null);
                accp.type = pn.internal_property_type;
                cmn.parameters.AddElement(accp);
            }
            while (exist_si != null)
            {
                if (convertion_data_and_alghoritms.function_eq_params(cmn, (function_node)(exist_si.sym_info)))
                {
                    AddError(loc, "CAN_NOT_GENERATE_ACCESSOR_{0}", cmn.name);
                }
                exist_si = exist_si.Next;
            }
            pn.common_comprehensive_type.methods.AddElement(cmn);
            pn.common_comprehensive_type.scope.AddSymbol(AcessorName, new SymbolInfo(cmn));
            return cmn;
        }

        public override void visit(SyntaxTree.property_accessors _property_accessors)
        {
            throw new NotSupportedError(get_location(_property_accessors));
        }

        public override void visit(SyntaxTree.read_accessor_name _read_accessor_name)
        {
            throw new NotSupportedError(get_location(_read_accessor_name));
        }

        public override void visit(SyntaxTree.write_accessor_name _write_accessor_name)
        {
            throw new NotSupportedError(get_location(_write_accessor_name));
        }

        public override void visit(SyntaxTree.array_const _array_const)
        {
            if (_array_const.elements != null)
            {
                List<constant_node> elements = new List<constant_node>();
                List<expression_node> exprs = new List<expression_node>();
                foreach (SyntaxTree.expression expr in _array_const.elements.expressions)
                {
                	if (is_typed_const_def)
                		elements.Add(convert_strong_to_constant_node(expr));
                	else
                		exprs.Add(convert_strong(expr));
                }
                if (is_typed_const_def)
                {
                	array_const arrc = new array_const(elements, get_location(_array_const));
                	arrc.SetType(new ArrayConstType(elements[0].type, elements.Count, arrc.location));
                	return_value(arrc);
                }
                else
                {
                	array_initializer arrc = new array_initializer(exprs, get_location(_array_const));
                	arrc.type = new ArrayConstType(exprs[0].type, exprs.Count, arrc.location);
                	return_value(arrc);
                }
            }
            else
            {
                //это пустой инициализатор записи
                record_constant rc = new record_constant(new List<PascalABCCompiler.SyntaxTree.record_const_definition>(), get_location(_array_const));
                rc.SetType(new RecordConstType(get_location(_array_const)));
                return_value(rc);
            }
        }

        private expressions_list get_set_initializer(expression_node cnfc)
        {
            return set_intls[cnfc];
        }

        private void add_set_initializer(expression_node cnfc, expressions_list exprs)
        {
            set_intls[cnfc] = exprs;
        }

        private expression_node convert_diap_for_set(SyntaxTree.diapason_expr _diapason_expr, out type_node elem_type)
        {
            expression_node left = convert_strong(_diapason_expr.left);
            if (left is typed_expression) left = convert_typed_expression_to_function_call(left as typed_expression);
            expression_node right = convert_strong(_diapason_expr.right);
            if (right is typed_expression) right = convert_typed_expression_to_function_call(right as typed_expression);
            internal_interface ii = left.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                AddError(new OrdinalTypeExpected(left.location));
            }
            internal_interface iir = right.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (iir == null)
            {
                AddError(new OrdinalTypeExpected(right.location));
            }
            type_node_list tnl = new type_node_list();
            tnl.AddElement(left.type);
            tnl.AddElement(right.type);
            elem_type = convertion_data_and_alghoritms.select_base_type(tnl);
            expression_node l = convertion_data_and_alghoritms.explicit_convert_type(left, PascalABCCompiler.SystemLibrary.SystemLibrary.integer_type);
            expression_node r = convertion_data_and_alghoritms.explicit_convert_type(right, PascalABCCompiler.SystemLibrary.SystemLibrary.integer_type);
            if (PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateDiapason.sym_info is common_namespace_function_node)
            {
                common_namespace_function_node cnfn = PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateDiapason.sym_info as common_namespace_function_node;
                if (elem_type == PascalABCCompiler.SystemLibrary.SystemLibrary.char_type || elem_type == PascalABCCompiler.SystemLibrary.SystemLibrary.bool_type || elem_type.IsEnum)
                    cnfn = PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateObjDiapason.sym_info as common_namespace_function_node;
                common_namespace_function_call cnfc = new common_namespace_function_call(cnfn, get_location(_diapason_expr));
                if (elem_type != PascalABCCompiler.SystemLibrary.SystemLibrary.char_type && elem_type != PascalABCCompiler.SystemLibrary.SystemLibrary.bool_type && !elem_type.IsEnum)
                {
                    cnfc.parameters.AddElement(l);
                    cnfc.parameters.AddElement(r);
                }
                else
                {
                    cnfc.parameters.AddElement(left);
                    cnfc.parameters.AddElement(right);
                }
                return cnfc;
            }
            else
            {
                compiled_function_node cnfn = PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateDiapason.sym_info as compiled_function_node;
                if (elem_type == PascalABCCompiler.SystemLibrary.SystemLibrary.char_type || elem_type == PascalABCCompiler.SystemLibrary.SystemLibrary.bool_type || elem_type.IsEnum)
                    cnfn = PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateObjDiapason.sym_info as compiled_function_node;
                compiled_static_method_call cnfc = new compiled_static_method_call(cnfn, get_location(_diapason_expr));
                if (elem_type != PascalABCCompiler.SystemLibrary.SystemLibrary.char_type && elem_type != PascalABCCompiler.SystemLibrary.SystemLibrary.bool_type && !elem_type.IsEnum)
                {
                    cnfc.parameters.AddElement(l);
                    cnfc.parameters.AddElement(r);
                }
                else
                {
                    cnfc.parameters.AddElement(left);
                    cnfc.parameters.AddElement(right);
                }
                return cnfc;
            }
            
        }

        public override void visit(SyntaxTree.pascal_set_constant _pascal_set_constant)
        {
            //throw new NotSupportedError(get_location(_pascal_set_constant));
            if (SystemLibrary.SystemLibInitializer.TypedSetType == null || SystemLibrary.SystemLibInitializer.CreateSetProcedure == null)
            	AddError(new NotSupportedError(get_location(_pascal_set_constant)));
            expressions_list consts = new expressions_list();
            type_node el_type = null;
            type_node_list types = new type_node_list();
            if (_pascal_set_constant.values != null && _pascal_set_constant.values != null)
                foreach (SyntaxTree.expression e in _pascal_set_constant.values.expressions)
                {
            		if (e is SyntaxTree.nil_const)
                        ErrorsList.Add(new SimpleSemanticError(get_location(e), "NIL_IN_SET_CONSTRUCTOR_NOT_ALLOWED"));
            		else
            		if (e is SyntaxTree.diapason_expr)
                    {
                        consts.AddElement(convert_diap_for_set((e as SyntaxTree.diapason_expr), out el_type));
                        if (el_type.IsPointer)
                            ErrorsList.Add(new SimpleSemanticError(get_location(e), "POINTERS_IN_SETS_NOT_ALLOWED"));
                        types.AddElement(el_type);
                    }
                    else
                    {
                        expression_node en = convert_strong(e);
                        if (en is typed_expression) en = convert_typed_expression_to_function_call(en as typed_expression);
                        if (en.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                        	en.type = SystemLibrary.SystemLibrary.string_type;
                        consts.AddElement(en);
                        types.AddElement(en.type);
                    }
                }
            expressions_list consts_copy = new expressions_list();
            consts_copy.AddRange(consts);
            type_node ctn = null;
            if (consts.Count > 0)
            {
                el_type = convertion_data_and_alghoritms.select_base_type(types);
                ctn = context.create_set_type(el_type, get_location(_pascal_set_constant));

            }
            else ctn = SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node;
            function_node fn = convertion_data_and_alghoritms.select_function(consts, SystemLibrary.SystemLibInitializer.CreateSetProcedure.SymbolInfo, (SystemLibrary.SystemLibInitializer.CreateSetProcedure.sym_info is common_namespace_function_node)?(SystemLibrary.SystemLibInitializer.CreateSetProcedure.sym_info as common_namespace_function_node).loc:null);
            if (fn is common_namespace_function_node)
            {
                common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, get_location(_pascal_set_constant));
                add_set_initializer(cnfc, consts_copy);
                cnfc.ret_type = ctn;
                for (int i = 0; i < consts.Count; i++)
                    cnfc.parameters.AddElement(consts[i]);
                return_value(cnfc);
            }
            else
            {
                compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, get_location(_pascal_set_constant));
                add_set_initializer(cnfc, consts_copy);
                cnfc.ret_type = ctn;
                for (int i = 0; i < consts.Count; i++)
                    cnfc.parameters.AddElement(consts[i]);
                return_value(cnfc);
            }
            //return_value(new common_namespace_function_call_as_constant(cnfc,cnfc.location));
        }

        internal SymbolInfo get_function_instances(SymbolInfo si, List<SyntaxTree.type_definition> type_pars, string name, location loc, bool stop_on_error)
        {
            List<type_node> tparams = visit_type_list(type_pars);
            for (int i = 0; i < tparams.Count; i++)
            {
                CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_generic_useful(tparams[i], loc);
                if (err != null)
                {
                    AddError( err);
                }
            }
            SymbolInfo start = null;
            SymbolInfo tek = null;
            while (si != null)
            {
                function_node fn = si.sym_info as function_node;
                if (fn != null)
                {
                    function_node inst = fn.get_instance(tparams, stop_on_error, loc);
                    if (inst != null)
                    {
                        SymbolInfo s = new SymbolInfo(inst);
                        if (start == null)
                        {
                            start = tek = s;
                        }
                        else
                        {
                            tek.Next = s;
                            tek = s;
                        }
                    }
                }
                si = si.Next;
            }
            if (start == null)
            {
                AddError(loc, "NO_FUNCTIONS_{0}_CAN_BE_USED_WITH_THIS_SPECIFICATION", name);
            }
            return start;
        }

        //private SymbolInfo convert_functions_to_instances(SymbolInfo funcs, List<SyntaxTree.type_definition> type_pars, string name, location subloc, bool stop_on_error)
        //{
        //    List<type_node> tparams = visit_type_list(type_pars);
        //    SymbolInfo si = get_function_instances(funcs, tparams, stop_on_error, subloc);
        //    if (si == null)
        //    {
        //        throw new NoFunctionsCanBeUsedWithThisSpecification(name, subloc);
        //    }
        //    return si;
        //}

        private SymbolInfo get_generic_functions(SymbolInfo funcs, bool stop_on_none, location loc)
        {
            SymbolInfo start = null;
            SymbolInfo cur = null;
            while (funcs != null)
            {
                function_node f = funcs.sym_info as function_node;
                if (f.is_generic_function)
                {
                    //Проверяем на совпадение
                    bool found = false;
                    SymbolInfo fsi = start;
                    while (fsi != null && fsi != funcs)
                    {
                        if (convertion_data_and_alghoritms.function_eq_params(fsi.sym_info as function_node, f))
                        {
                            found = true;
                            break;
                        }
                        fsi = fsi.Next;
                    }
                    if (!found)
                    {
                        if (start == null)
                        {
                            start = funcs;
                        }
                        else
                        {
                            cur.Next = funcs;
                        }
                        cur = funcs;
                    }
                }
                funcs = funcs.Next;
            }
            if (start == null)
            {
                if (stop_on_none)
                {
                    AddError(loc, "TRIANGLE_BRACKETS_NOT_ALLOWED_WITH_COMMON_FUNCTIONS");
                }
                return null;
            }
            cur.Next = null;
            return start;
        }

        internal void visit_method_call(SyntaxTree.method_call _method_call)
        {
            //lroman
            if (_method_call.dereferencing_value is closure_substituting_node)
            {
                var nodeToVisit =
                    new method_call(((closure_substituting_node) _method_call.dereferencing_value).substitution,
                                    _method_call.parameters);
                visit(nodeToVisit);
                return;
            }

            bool proc_wait = procedure_wait;
			bool lambdas_are_in_parameters = false; //lroman//

            var syntax_nodes_parameters = _method_call.parameters == null
                                              ? new List<expression>()
                                              : _method_call.parameters.expressions;
            if (procedure_wait)
            {
                procedure_wait = false;
            }
            //throw new ArgumentNullException("test");

            motivation mot = motivation_keeper.motivation;

            expression_node expr_node = null;
            expressions_list exprs = new expressions_list();

            SyntaxTree.ident id = null;

            SymbolInfo si = null;

            type_node to_type = null;

            SyntaxTree.addressed_value deref_value = _method_call.dereferencing_value;
            int templ_args_count = 0;
            //bool needs_generic_type_search = false;
            SyntaxTree.ident_with_templateparams iwt = deref_value as SyntaxTree.ident_with_templateparams;
            if (iwt != null)
            {
                deref_value = iwt.name;
                templ_args_count = iwt.template_params.params_list.Count;
                //needs_generic_type_search = _method_call.parameters.expressions.Count == 1;
            }

            SyntaxTree.inherited_ident inh_id = deref_value as SyntaxTree.inherited_ident;
            if (inh_id != null)
            {
                inherited_ident_processing = true;
                si = find_in_base(inh_id);
                if (si != null) 
            	if (si.sym_info is common_method_node)
            	{
            		if ((si.sym_info as common_method_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                        AddError(get_location(inh_id), "CANNOT_CALL_ABSTRACT_METHOD");
            	}
            	else
            	if (si.sym_info is compiled_function_node)
            	{
            		if ((si.sym_info as compiled_function_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                        AddError(get_location(inh_id), "CANNOT_CALL_ABSTRACT_METHOD");
            	}
                id = inh_id;
            }
            else
            {
                id = deref_value as SyntaxTree.ident;
                if (id != null)
                {
                    if (templ_args_count != 0)
                    {
                        //Ищем generics
                        si = context.find(id.name + compiler_string_consts.generic_params_infix + templ_args_count.ToString());
                        if (si != null)
                        {
                            si = new SymbolInfo(get_generic_instance(si, iwt.template_params.params_list));
                            iwt = null;
                        }
                    }
                    if (si == null)
                    {
                        SyntaxTree.operator_name_ident oni = id as SyntaxTree.operator_name_ident;
                        if (oni != null)
                        {
                            si = context.find(name_reflector.get_name(oni.operator_type));
                        }
                        else
                        {
                            si = context.find(id.name);
                            if (templ_args_count != 0)
                            {
                                SymbolInfo conv = ConvertTypeToInstance(si, iwt.template_params.params_list, get_location(id));
                                if (conv != null)
                                {
                                    si = conv;
                                    iwt = null;
                                }
                            }
                        }
                    }
                }
                else
                {
                    SyntaxTree.dot_node _dot_node = deref_value as SyntaxTree.dot_node;
                    if (_dot_node != null)
                    {
						bool skip_first_parameter = false; //lroman//
                        bool has_extension_overload = false;
                        semantic_node sn = convert_semantic_strong(_dot_node.left);

                        //SyntaxTree.ident id_right = ConvertOperatorNameToIdent(_dot_node.right as SyntaxTree.ident);
                        SyntaxTree.ident id_right = _dot_node.right as SyntaxTree.ident;
                        switch (sn.general_node_type)
                        {
                            case general_node_type.expression:
                                {
                                    expression_node exp = (expression_node)sn;
                                    if (exp is typed_expression)
                                        try_convert_typed_expression_to_function_call(ref exp);
                                    SyntaxTree.operator_name_ident oni_right = id_right as SyntaxTree.operator_name_ident;
                                    if (oni_right != null)
                                    {
                                        si = exp.type.find_in_type(name_reflector.get_name(oni_right.operator_type), context.CurrentScope);
                                    }
                                    else
                                    {
                                        si = exp.type.find_in_type(id_right.name, context.CurrentScope);
                                        if (si != null && si.sym_info != null && si.sym_info.semantic_node_type == semantic_node_type.wrap_def)
                                            BasePCUReader.RestoreSymbols(si, id_right.name);
                                    }

                                    
                                    //definition_node ddn=check_name_node_type(id_right.name,si,get_location(id_right),
                                    //	general_node_type.function_node,general_node_type.variable_node);

                                    location subloc = get_location(id_right);

                                    if (si == null)
                                    {
                                        AddError(new UndefinedNameReference(id_right.name, subloc));
                                    }

                                    if (si.sym_info.general_node_type != general_node_type.function_node)
                                    {
                                        if (si.sym_info.general_node_type == general_node_type.type_node)
                                        {
                                            to_type = ((type_node)(si.sym_info));
                                        }
                                        else
                                        {
                                            dot_node_as_expression_dot_ident(exp, id_right, motivation.expression_evaluation);
                                            exp = ret.get_expression();
                                            internal_interface ii = exp.type.get_internal_interface(internal_interface_kind.delegate_interface);
                                            if (ii == null)
                                            {
                                                AddError(subloc, "EXPECTED_DELEGATE");
                                            }
                                            delegate_internal_interface dii = ii as delegate_internal_interface;
                                            si = new SymbolInfo(dii.invoke_method);
                                        }
                                    }

                                    if (to_type != null)
                                    {
                                        if ((_method_call.parameters == null) || (_method_call.parameters.expressions.Count != 1))
                                        {
                                            AddError(get_location(_method_call), "ONLY_ONE_PARAMETER_OF_TYPE_CONVERSION_ALLOWED" );
                                        }
                                    }
                                    SymbolInfo tmp_si = si;
                                    while (si != null)
                                    {
                                        if (si.sym_info is common_namespace_function_node)
                                        {
                                            common_namespace_function_node cnfn = si.sym_info as common_namespace_function_node;
                                            if (cnfn.ConnectedToType != null && !cnfn.IsOperator)
                                            {
                                                exprs.AddElementFirst(exp);
                                                skip_first_parameter = true;
                                                break;
                                            }
                                        }
                                        if (si.sym_info is compiled_function_node)
                                        {
                                            compiled_function_node cfn = si.sym_info as compiled_function_node;
                                            if (cfn.ConnectedToType != null)
                                            {
                                                exprs.AddElementFirst(exp);
                                                skip_first_parameter = true;
                                                if (cfn.is_generic_function)
                                                {
                                                    //generic_convertions.DeduceFunction(cfn, exprs);
                                                    //si.sym_info = cfn.get_instance(new List<type_node>(new type_node[] { exp.type }), true, get_location(_method_call));
                                                }
                                                break;
                                            }

                                        }
                                        si = si.Next;
                                    }
                                    
                                    si = tmp_si;
                                    if (skip_first_parameter)
                                    {
                                        SymbolInfo new_si = null;
                                        bool has_obj_methods = false;
                                        List<SymbolInfo> si_list = new List<SymbolInfo>();
                                        while (si != null)
                                        {
                                            if (si.sym_info is common_namespace_function_node)
                                            {
                                                common_namespace_function_node cnfn = si.sym_info as common_namespace_function_node;
                                                if (cnfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static || cnfn.ConnectedToType != null)
                                                    si_list.Add(si);
                                                if (cnfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && cnfn.ConnectedToType == null)
                                                    has_obj_methods = true;
                                            }
                                            else if (si.sym_info is compiled_function_node)
                                            {
                                                compiled_function_node cfn = si.sym_info as compiled_function_node;
                                                if (cfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static || cfn.ConnectedToType != null)
                                                    si_list.Add(si);
                                                if (cfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static && cfn.ConnectedToType == null)
                                                    has_obj_methods = true;
                                            }
                                            si = si.Next;
                                        }
                                        for (int i = 0; i < si_list.Count; i++)
                                        {
                                            if (new_si == null)
                                            {
                                                new_si = si_list[i];
                                                si = new_si;
                                            }
                                            else
                                            {
                                                si.Next = si_list[i];
                                                si = si.Next;
                                            }
                                        }
                                        if (si != null)
                                            si.Next = null;
                                        si = new_si;
                                    }
                                    if (_method_call.parameters != null)
                                    {
                                    	foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                        {
                                            #region Отмечаем флаг в лямбдах, говорящий о том, что в первый раз будем их "обходить" для вывода типов
                                            //lroman//
                                            if (en is SyntaxTree.function_lambda_definition)
                                            {
                                                lambdas_are_in_parameters = true;
                                                ((SyntaxTree.function_lambda_definition)en).lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                                            }
                                            //lroman//
                                            #endregion
                                            exprs.AddElement(convert_strong(en));
                                        }
                                    }

                                    expression_node subexpr1 = null;

                                    if (to_type != null)
                                    {
                                        subexpr1 = convertion_data_and_alghoritms.explicit_convert_type(exprs[0], to_type);
                                    }
                                    else
                                    {
                                        if (iwt != null)
                                        {
                                            si = get_generic_functions(si, true, subloc);
                                            si = get_function_instances(si, iwt.template_params.params_list, id_right.name, subloc, si.Next == null);
                                        }
										#region Если встретились лямбды в фактических параметрах, то выбираем нужную функцию из перегруженных, выводим типы, отмечаем флаг в лямбдах, говорящий о том, что мы их реально обходим
                                        //lroman//
                                        if (lambdas_are_in_parameters)
                                        {
                                            LambdaHelper.processingLambdaParametersForTypeInference++;
                                            // SSM 21.05.14 - попытка обработать перегруженные функции с параметрами-лямбдами с различными возвращаемыми значениями
                                            function_node_list spf = null;
                                            try
                                            {
                                                function_node ffn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                                int exprCounter = 0;
                                                if (skip_first_parameter)
                                                {
                                                    exprCounter++;
                                                }
                                                foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                                {
                                                    if (!(en is SyntaxTree.function_lambda_definition))
                                                    {
                                                        exprCounter++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        var enLambda = (SyntaxTree.function_lambda_definition)en;
                                                        LambdaHelper.InferTypesFromVarStmt(ffn.parameters[exprCounter].type, enLambda, this);
                                                        enLambda.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                                                        exprs[exprCounter] = convert_strong(en);
                                                        enLambda.lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                                                        exprCounter++;
                                                    }
                                                }
                                            }
                                            catch (SeveralFunctionsCanBeCalled sf)
                                            {
                                                spf = sf.set_of_possible_functions; // Возможны несколько перегруженных версий - надо выводить дальше в надежде что какие-то уйдут и останется одна
                                            }

                                            Exception lastmultex = null;
                                            if (spf != null) // пытаемся инстанцировать одну за другой и ошибки гасим try
                                            {   // exprs - глобальная, поэтому надо копировать
                                                int spfnum = -1; // первый номер правильно инстанцированной. Если потом встретился второй, то тоже ошибка
                                                                 // SSM 4.08.15. Сейчас меняю эту логику. Если будет много кандидатов, но ровно один с совпадающим типом возвращаемого значения, то его и надо выбирать.
                                                                 // не забыть, что аналогичный код есть в create_constructor_call!!!!!!!
                                                int GoodVersionsCount = 0;
                                                int GoodVersionsCountWithSameResType = 0;
                                                for (int i = 0; i < spf.Count; i++) // цикл по версиям
                                                {
                                                    function_node fnn = spf[i];
                                                    try
                                                    {
                                                        int exprCounter = 0;
                                                        if (skip_first_parameter)
                                                        {
                                                            exprCounter++;
                                                        }

                                                        expressions_list exprs1 = new expressions_list();
                                                        exprs1.AddRange(exprs); // сделали копию

                                                        foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                                        {
                                                            if (!(en is SyntaxTree.function_lambda_definition))
                                                            {
                                                                exprCounter++;
                                                                continue;
                                                            }
                                                            else
                                                            {
                                                                var fld = en as SyntaxTree.function_lambda_definition;

                                                                var lambdaName = fld.lambda_name;                           //lroman Сохранять имя необходимо
                                                                var fl = fld.lambda_visit_mode;

                                                                // запомнили типы параметров лямбды - SSM
                                                                object[] realparamstype = new object[fld.formal_parameters.params_list.Count]; // здесь хранятся выведенные типы лямбд или null если типы явно заданы
                                                                for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                                                {
                                                                    var laminftypeK = fld.formal_parameters.params_list[k].vars_type as SyntaxTree.lambda_inferred_type;
                                                                    if (laminftypeK == null)
                                                                        realparamstype[k] = null;
                                                                    else realparamstype[k] = laminftypeK.real_type;
                                                                }

                                                                // запоминаем реальный тип возвращаемого значения если он не указан явно (это должен быть any_type или null если он указан явно) - он может измениться при следующем вызове, поэтому мы его восстановим
                                                                var restype = fld.return_type as SyntaxTree.lambda_inferred_type;
                                                                object realrestype = null;
                                                                if (restype != null)
                                                                    realrestype = restype.real_type;
                                                                LambdaHelper.InferTypesFromVarStmt(fnn.parameters[exprCounter].type, fld, this);
                                                                fld.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing; //lroman
                                                                fld.lambda_name = LambdaHelper.GetAuxiliaryLambdaName(lambdaName); // поправляю имя. Думаю, назад возвращать не надо. ПРОВЕРИТЬ!

                                                                //contextChanger.SaveContextAndUpToNearestDefSect();
                                                                try
                                                                {
                                                                    exprs1[exprCounter] = convert_strong(en);

                                                                    type_node resexprtype = fld.RealSemTypeOfResExpr as type_node;
                                                                    type_node resformaltype = fld.RealSemTypeOfResult as type_node;
                                                                    var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                                                    if (bbb)
                                                                    {
                                                                        GoodVersionsCountWithSameResType += 1;
                                                                        spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                                                    }

                                                                    /*compiled_type_node tt;
                                                                    tt = fnn.parameters[exprCounter].type as compiled_type_node;
                                                                    if (tt != null && tt.compiled_type.FullName.ToLower().StartsWith("system.func"))
                                                                    {
                                                                        resformaltype = tt.instance_params[tt.instance_params.Count - 1]; // Последний параметр в записи Func<T,T1,...TN> - тип возвращаемого значения
                                                                        var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                                                        if (bbb)
                                                                        {
                                                                            GoodVersionsCountWithSameResType += 1;
                                                                            spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                                                        }
                                                                    }*/
                                                                }
                                                                catch
                                                                {
                                                                    throw;
                                                                }
                                                                finally
                                                                {
                                                                    LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, en as function_lambda_definition);
                                                                    // восстанавливаем сохраненный тип возвращаемого значения
                                                                    if (restype != null)
                                                                        restype.real_type = realrestype;
                                                                    // восстанавливаем сохраненные типы параметров лямбды, которые не были заданы явно
                                                                    for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                                                    {
                                                                        var laminftypeK = fld.formal_parameters.params_list[k].vars_type as SyntaxTree.lambda_inferred_type;
                                                                        if (laminftypeK != null)
                                                                            laminftypeK.real_type = realparamstype[k];
                                                                    }

                                                                    fld.lambda_name = lambdaName; //lroman Восстанавливаем имена
                                                                    fld.lambda_visit_mode = fl;
                                                                }

                                                                //contextChanger.RestoreCurrentContext();
                                                                exprCounter++;
                                                            }
                                                        }
                                                        /*if (spfnum >= 0) // два удачных инстанцирования - плохо. Может, одно - с более близким типом возвращаемого значения, тогда это плохо - надо доделать, но пока так
                                                        {
                                                            spfnum = -2;
                                                            break;
                                                        }*/

                                                        if (GoodVersionsCountWithSameResType == 0)
                                                            spfnum = i; // здесь запоминаем индекс только если нет подошедших, совпадающих по типу возвращаемого значения
                                                        GoodVersionsCount += 1;
                                                        for (int j = 0; j < exprs.Count; j++) // копируем назад если всё хорошо
                                                            exprs[j] = exprs1[j];
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        // если сюда попали, значит, не вывели типы в лямбде и надо эту инстанцию пропускать
                                                        //contextChanger.RestoreCurrentContext();
                                                        lastmultex = e;
                                                    }
                                                } // конец цикла по версиям
                                                if (GoodVersionsCount > 1 && GoodVersionsCountWithSameResType != 1) // подошло много, но не было ровно одной с совпадающим типом возвращаемого значения
                                                    throw new SeveralFunctionsCanBeCalled(subloc, spf);
                                                if (GoodVersionsCount == 0) // было много, но ни одна не подошла из-за лямбд
                                                {
                                                    throw lastmultex;
                                                    //throw new NoFunctionWithSameArguments(subloc2, false);
                                                }

                                                int kk = 0;
                                                if (skip_first_parameter)
                                                    kk++;
                                                foreach (SyntaxTree.expression en in _method_call.parameters.expressions) //lroman окончательно подставить типы в лямбды
                                                {
                                                    if (!(en is SyntaxTree.function_lambda_definition))
                                                    {
                                                        kk++;
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        LambdaHelper.InferTypesFromVarStmt(spf[spfnum].parameters[kk].type, en as SyntaxTree.function_lambda_definition, this);
                                                        exprs[kk] = convert_strong(en);
                                                        kk++;
                                                    }
                                                }
                                            }
                                            // SSM 21.05.14 end
                                            LambdaHelper.processingLambdaParametersForTypeInference--;
                                        }
                                        //lroman//
                                        #endregion

                                        function_node fn = null;
                                        if (!skip_first_parameter || si.Next == null)
                                            fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                        else
                                        {
                                            try
                                            {
                                                ThrowCompilationError = false;
                                                fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                                if (fn == null && skip_first_parameter)
                                                {
                                                    if (si.Next == null)
                                                    {
                                                        ThrowCompilationError = true;
                                                        throw LastError();
                                                    }
                                                    RemoveLastError();
                                                    skip_first_parameter = false;
                                                    si = tmp_si;
                                                    exprs.remove_at(0);
                                                    fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                                    if (fn == null)
                                                    {
                                                        ThrowCompilationError = true;
                                                        throw LastError();
                                                    }
//                                                    else
//                                                        RemoveLastError(); // ошибка уже убрана чуть выше
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ThrowCompilationError = true;
                                                if (skip_first_parameter)
                                                {
                                                    si = tmp_si;
                                                    exprs.remove_at(0);
                                                    fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                                }
                                                else
                                                    throw ex;
                                            }
                                            ThrowCompilationError = true;
                                        }
                                        SemanticTree.IGenericInstance igi = fn as SemanticTree.IGenericInstance;
                                        if (igi != null)
                                        {
                                            //проверяем на соответствие ограничителям
                                            int num_err;
                                            //CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(
                                        }
                                        base_function_call bfc = create_not_static_method_call(fn, exp, get_location(id_right), proc_wait);
                                        bfc.parameters.AddRange(exprs);
                                        subexpr1 = bfc;
                                    }

                                    switch (mot)
                                    {
                                        case motivation.expression_evaluation:
                                            {
                                                return_value(subexpr1);
                                                return;
                                            }
                                        case motivation.semantic_node_reciving:
                                            {
                                                return_semantic_value(subexpr1);
                                                return;
                                            }
                                        default:
                                            {
                                                AddError(subexpr1.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                                return;
                                            }
                                    }

                                }
                            case general_node_type.namespace_node:
                                {
                                    namespace_node nsn = (namespace_node)sn;
                                    if (templ_args_count != 0)
                                    {
                                        //Ищем generics
                                        si = context.find(id_right.name + compiler_string_consts.generic_params_infix + templ_args_count.ToString());
                                        if (si != null)
                                        {
                                            si = new SymbolInfo(get_generic_instance(si, iwt.template_params.params_list));
                                            iwt = null;
                                        }
                                    }
                                    if (si == null)
                                    {
                                        SyntaxTree.operator_name_ident oni_right = id_right as SyntaxTree.operator_name_ident;
                                        if (oni_right != null)
                                        {
                                            si = nsn.find(name_reflector.get_name(oni_right.operator_type));
                                        }
                                        else
                                        {
                                            si = nsn.find(id_right.name);
                                            if (templ_args_count != 0)
                                            {
                                                SymbolInfo conv = ConvertTypeToInstance(si, iwt.template_params.params_list, get_location(id_right));
                                                if (conv != null)
                                                {
                                                    si = conv;
                                                    iwt = null;
                                                }
                                            }
                                        }
                                    }
                                    id = id_right;
                                    break;
                                }
                            case general_node_type.unit_node:
                                {
                                    unit_node un = (unit_node)sn;
                                    SyntaxTree.operator_name_ident oni_right = id_right as SyntaxTree.operator_name_ident;
                                    if (oni_right != null)
                                    {
                                        si = un.find_only_in_namespace(name_reflector.get_name(oni_right.operator_type));
                                    }
                                    else
                                    {
                                        si = un.find_only_in_namespace(id_right.name);
                                    }
                                    id = id_right;
                                    break;
                                }
                            case general_node_type.type_node:
                                {
                                    type_node tn = (type_node)sn;
                                    /*if (tn == SystemLibrary.SystemLibrary.void_type)
                                    {
                                        throw new VoidNotValid(get_location(id_right));
                                    }*/
                                    check_for_type_allowed(tn,get_location(id_right));
                                    SyntaxTree.operator_name_ident oni_right = id_right as SyntaxTree.operator_name_ident;
                                    if (oni_right != null)
                                    {
                                        si = tn.find_in_type(name_reflector.get_name(oni_right.operator_type), context.CurrentScope);
                                    }
                                    else
                                    {
                                        si = tn.find_in_type(id_right.name, context.CurrentScope);//CurrentScope
                                        delete_inherited_constructors(ref si, tn);
                                        delete_extension_methods(ref si);
                                    }

                                    //definition_node ddn2=check_name_node_type(id_right.name,si,get_location(id_right),
                                    //    general_node_type.function_node);

                                    expression_node exp = null;
                                    location subloc = get_location(id_right);

                                    if (si == null)
                                    {
                                        AddError( new UndefinedNameReference(id_right.name, subloc));
                                    }

                                    if (si.sym_info.general_node_type != general_node_type.function_node)
                                    {
                                        if (si.sym_info.general_node_type == general_node_type.type_node)
                                        {
                                            to_type = ((type_node)(si.sym_info));
                                        }
                                        else
                                        {
                                            dot_node_as_type_ident(tn, id_right, motivation.expression_evaluation);
                                            exp = ret.get_expression();
                                            internal_interface ii = exp.type.get_internal_interface(internal_interface_kind.delegate_interface);
                                            if (ii == null)
                                            {
                                                AddError(subloc, "EXPECTED_DELEGATE");
                                            }
                                            delegate_internal_interface dii = ii as delegate_internal_interface;
                                            si = new SymbolInfo(dii.invoke_method);
                                        }
                                    }

                                    if (to_type != null)
                                    {
                                        if ((_method_call.parameters == null) || (_method_call.parameters.expressions.Count != 1))
                                        {
                                            AddError(get_location(_method_call), "ONLY_ONE_PARAMETER_OF_TYPE_CONVERSION_ALLOWED" );
                                        }
                                    }

                                    if (_method_call.parameters != null)
                                    {
                                        foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                        {
                                            exprs.AddElement(convert_strong(en));
                                        }
                                    }

                                    expression_node subexpr2 = null;

                                    if (to_type != null)
                                    {
                                        subexpr2 = convertion_data_and_alghoritms.explicit_convert_type(exprs[0], to_type);
                                    }
                                    else
                                    {
                                        if (iwt != null)
                                        {
                                            si = get_generic_functions(si, true, subloc);
                                            si = get_function_instances(si, iwt.template_params.params_list, id_right.name, subloc, si.Next == null);
                                        }

                                        function_node fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc, syntax_nodes_parameters);
                                        if (!debug && fn == SystemLibrary.SystemLibrary.assert_method)
                                        {
//                                        	return_value(new empty_statement(null));
//                                        	return;
                                        }
                                        /*if ((proc_wait==false)&&(fn.return_value_type==null))
                                        {
                                            throw new FunctionExpectedProcedureMeet(fn,get_location(id_right));
                                        }*/

                                        base_function_call bfc2 = null;
                                        if (exp == null)
                                        {
                                            bfc2 = create_static_method_call(fn, subloc, tn, proc_wait);
                                        }
                                        else
                                        {
                                            bfc2 = create_not_static_method_call(fn, exp, subloc, proc_wait);
                                        }
                                        bfc2.parameters.AddRange(exprs);
                                        subexpr2 = bfc2;
                                    }

                                    switch (mot)
                                    {
                                        case motivation.expression_evaluation:
                                            {
                                                return_value(subexpr2);
                                                return;
                                            }
                                        case motivation.semantic_node_reciving:
                                            {
                                                return_semantic_value(subexpr2);
                                                return;
                                            }
                                        default:
                                            {
                                                AddError(subexpr2.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                                return;
                                            }
                                    }
                                }
                        }
                    }
                    else
                    {
                        SyntaxTree.expression expr = deref_value as SyntaxTree.expression;
                        if (expr != null)
                        {
                            //throw new CompilerInternalError("Not supported");
                            expression_node exp_int = convert_strong(expr);
                            location sloc = get_location(expr);
                            internal_interface ii = exp_int.type.get_internal_interface(internal_interface_kind.delegate_interface);
                            if (ii == null)
                            {
                                AddError(sloc, "EXPECTED_DELEGATE");
                            }
                            delegate_internal_interface dii = (delegate_internal_interface)ii;
                            si = new SymbolInfo(dii.invoke_method);

                            if (_method_call.parameters != null)
                            {
                                foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                {
                                    exprs.AddElement(convert_strong(en));
                                }
                            }

                            function_node del_func = convertion_data_and_alghoritms.select_function(exprs, si, sloc, syntax_nodes_parameters);
                            base_function_call bbfc = create_not_static_method_call(del_func, exp_int, sloc, proc_wait);
                            bbfc.parameters.AddRange(exprs);

                            switch (mot)
                            {
                                case motivation.expression_evaluation:
                                    {
                                        return_value(bbfc);
                                        return;
                                    }
                                case motivation.semantic_node_reciving:
                                    {
                                        return_semantic_value(bbfc);
                                        return;
                                    }
                                default:
                                    {
                                        AddError(bbfc.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                        return;
                                    }
                            }
                        }
                        else
                        {
                            throw new CompilerInternalError("Unexpected method name kind");
                        }
                    }
                }
            }

            //definition_node dn=check_name_node_type(id.name,si,get_location(_method_call),general_node_type.function_node);

            expression_node exp2 = null;
            location subloc2 = get_location(id);

            List<TreeConverter.SymbolInfo> sibak = new List<TreeConverter.SymbolInfo>();
            TreeConverter.SymbolInfo ssi=si;
            while (ssi != null)
            {
                ssi = ssi.Next;
                sibak.Add(ssi);
            }


            if (si == null)
            {
                AddError(new UndefinedNameReference(id.name, subloc2));
            }
			is_format_allowed = false;
            if (SystemUnitAssigned)
                if (SystemLibrary.SystemLibInitializer.read_procedure.Equal(si) || SystemLibrary.SystemLibInitializer.readln_procedure.Equal(si))
                {
                    
            		expression_node bfcint = make_read_call(si, _method_call.parameters, subloc2);
                    if (!proc_wait) AddError(subloc2, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", (si.sym_info as function_node).name);
                    switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
                }
            else if (SystemLibrary.SystemLibInitializer.write_procedure.Equal(si) || SystemLibrary.SystemLibInitializer.writeln_procedure.Equal(si) || SystemLibrary.SystemLibInitializer.StrProcedure.Equal(si))
            {
            	is_format_allowed = true;
            }
            else if (SystemLibrary.SystemLibInitializer.ArrayCopyFunction.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 1)
            	{
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		if (param0.type.type_special_kind == SemanticTree.type_special_kind.array_kind)
            		{
            			location loc = get_location(_method_call);
            			expression_node en = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ArrayCopyFunction.sym_info as function_node,loc,param0);
                    	function_node fn = convertion_data_and_alghoritms.get_empty_conversion(en.type, param0.type, false);
                    	en = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, en);
                    	return_value(en);
                    	return;
            		}
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.SetLengthProcedure.Equal(si) || SystemLibrary.SystemLibrary.resize_func == si.sym_info as function_node)
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count >= 2)
            	{
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		if (!param0.is_addressed)
            			AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
            		if (param0.type.type_special_kind == SemanticTree.type_special_kind.short_string)
            		{
            			expression_node param1 = convert_strong(_method_call.parameters.expressions[1]);
            			param1 = convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type);
                        base_function_call cnfn = null; 
                        if (SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure.sym_info is common_namespace_function_node)
                            cnfn = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure.sym_info as common_namespace_function_node, get_location(_method_call));
            			else
                            cnfn = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.SetLengthForShortStringProcedure.sym_info as compiled_function_node, get_location(_method_call));
                        cnfn.parameters.AddElement(param0);
            			cnfn.parameters.AddElement(param1);
            			cnfn.parameters.AddElement(new int_const_node((param0.type as short_string_type_node).Length,null));
            			switch (mot)
                    	{
                        	case motivation.expression_evaluation:
                            {
                                return_value(cnfn);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(cnfn);
                                return;
                            }
                        	default:
                            {
                                AddError(cnfn.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    	}
            		}
            		else if (param0.type.type_special_kind == SemanticTree.type_special_kind.array_kind)
            		{
            			int rank = 1;
            			if (param0.type is compiled_type_node)
            				rank = (param0.type as compiled_type_node).rank;
            			else if (param0.type is common_type_node)
            				rank = (param0.type as common_type_node).rank;
            			if (_method_call.parameters.expressions.Count-1 != rank)
            				AddError(get_location(_method_call), "{0}_PARAMETERS_EXPECTED", rank+1);
            			if (rank > 1)
            			{
            				type_node tn = param0.type.element_type;
            				location loc = get_location(_method_call);
            				typeof_operator to = new typeof_operator(tn, loc);
                    		List<expression_node> lst = new List<expression_node>();
                    		//размер
                    		for (int i=1; i<_method_call.parameters.expressions.Count; i++)
                    		{
                    			expression_node expr = convert_strong(_method_call.parameters.expressions[i]);;
                    			expr = convertion_data_and_alghoritms.convert_type(expr, SystemLibrary.SystemLibrary.integer_type);
                    			if (expr is int_const_node && (expr as int_const_node).constant_value < 0)
                    				AddError(expr.location,"NEGATIVE_ARRAY_LENGTH_({0})_NOT_ALLOWED", (expr as int_const_node).constant_value);
                    			lst.Add(expr);
                    		}
                    		//это вызов спецфункции
                    		
                    		expression_node retv = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.NewArrayProcedureDecl, loc, to, new int_const_node(lst.Count,loc));
                    		common_namespace_function_call cnfc = retv as common_namespace_function_call;
                    		foreach (expression_node e in lst)
                    			cnfc.parameters.AddElement(e);
                    		expression_node en = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.CopyWithSizeFunction.sym_info as function_node,loc,param0,cnfc);
                    		function_node fn = convertion_data_and_alghoritms.get_empty_conversion(en.type, retv.type, false);
                    		en = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, en);
                    		basic_function_call bfc = new basic_function_call(tn.find(compiler_string_consts.assign_name).sym_info as basic_function_node,loc,param0,en);
                    		return_value(bfc);
                    		return;
            			}
            		}
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.InsertProcedure.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 3)
            	{
            		//expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		expression_node param1 = convert_strong(_method_call.parameters.expressions[1]);
            		//expression_node param2 = convert_strong(_method_call.parameters.expressions[2]);
            		if (param1.type.type_special_kind == SemanticTree.type_special_kind.short_string)
            		{
            			expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            			expression_node param2 = convert_strong(_method_call.parameters.expressions[2]);
            			param0 = convertion_data_and_alghoritms.convert_type(param0,SystemLibrary.SystemLibrary.string_type);
            			param2 = convertion_data_and_alghoritms.convert_type(param2,SystemLibrary.SystemLibrary.integer_type);
                        base_function_call cnfn = null;
                        if (SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure.sym_info is common_namespace_function_node)
                            cnfn = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure.sym_info as common_namespace_function_node, get_location(_method_call));
            			else
                            cnfn = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.InsertInShortStringProcedure.sym_info as compiled_function_node, get_location(_method_call));
                        cnfn.parameters.AddElement(param0);
            			cnfn.parameters.AddElement(param1);
            			cnfn.parameters.AddElement(param2);
            			cnfn.parameters.AddElement(new int_const_node((param1.type as short_string_type_node).Length,null));
            			switch (mot)
                    	{
                        	case motivation.expression_evaluation:
                            {
                                return_value(cnfn);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(cnfn);
                                return;
                            }
                        	default:
                            {
                                AddError(cnfn.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    	}
            		}
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.DeleteProcedure.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 3)
            	{
            		//expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		//expression_node param2 = convert_strong(_method_call.parameters.expressions[2]);
            		if (param0.type.type_special_kind == SemanticTree.type_special_kind.short_string)
            		{
            			expression_node param1 = convert_strong(_method_call.parameters.expressions[1]);
            			expression_node param2 = convert_strong(_method_call.parameters.expressions[2]);
            			param1 = convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type);
            			param2 = convertion_data_and_alghoritms.convert_type(param2,SystemLibrary.SystemLibrary.integer_type);
                        base_function_call cnfn = null;
                        if (SystemLibrary.SystemLibInitializer.DeleteProcedure.sym_info is common_namespace_function_node) 
                            cnfn = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.DeleteProcedure.sym_info as common_namespace_function_node, get_location(_method_call));
                        else
                            cnfn = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.DeleteProcedure.sym_info as compiled_function_node, get_location(_method_call));
            			cnfn.parameters.AddElement(param0);
            			cnfn.parameters.AddElement(param1);
            			cnfn.parameters.AddElement(param2);
            			switch (mot)
                    	{
                        	case motivation.expression_evaluation:
                            {
                                return_value(cnfn);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(cnfn);
                                return;
                            }
                        	default:
                            {
                                AddError(cnfn.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    	}
            		}
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.IncludeProcedure.Equal(si)
                            || SystemLibrary.SystemLibInitializer.ExcludeProcedure.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 2)
            	{
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		expression_node param1 = convert_strong(_method_call.parameters.expressions[1]);
            		expressions_list args = new expressions_list();
            		args.AddElement(param0);
            		args.AddElement(param1);
            		CheckSpecialFunctionCall(si,args,get_location(_method_call));
                    expression_node en_cnfn = null;
                    if (SystemLibrary.SystemLibInitializer.IncludeProcedure.sym_info is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfn = null;
                        if (SystemLibrary.SystemLibInitializer.IncludeProcedure.Equal(si)) cnfn = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.IncludeProcedure.sym_info as common_namespace_function_node, get_location(_method_call));
                        else cnfn = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ExcludeProcedure.sym_info as common_namespace_function_node, get_location(_method_call));
                        cnfn.parameters.AddElement(param0);
                        cnfn.parameters.AddElement(param1);
                        en_cnfn = cnfn;
                    }
                    else
                    {
                        compiled_static_method_call cnfn = null;
                        if (SystemLibrary.SystemLibInitializer.IncludeProcedure.Equal(si)) cnfn = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.IncludeProcedure.sym_info as compiled_function_node, get_location(_method_call));
                        else cnfn = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ExcludeProcedure.sym_info as compiled_function_node, get_location(_method_call));
                        cnfn.parameters.AddElement(param0);
                        cnfn.parameters.AddElement(param1);
                        en_cnfn = cnfn;
                    }
            		switch (mot)
                    {
                        	case motivation.expression_evaluation:
                            {
                                return_value(en_cnfn);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(en_cnfn);
                                return;
                            }
                        	default:
                            {
                                AddError(en_cnfn.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.IncProcedure.Equal(si))
            {
            	expression_node bfcint = make_inc_call(si, _method_call.parameters, subloc2);
            	if (!proc_wait) AddError(subloc2, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", (si.sym_info as function_node).name);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (SystemLibrary.SystemLibInitializer.DecProcedure.Equal(si))
            {
            	expression_node bfcint = make_dec_call(si, _method_call.parameters, subloc2);
            	if (!proc_wait) AddError(subloc2, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", (si.sym_info as function_node).name);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (SystemLibrary.SystemLibInitializer.SuccFunction.Equal(si))
            {
            	expression_node bfcint = make_succ_call(si, _method_call.parameters, subloc2);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (SystemLibrary.SystemLibInitializer.PredFunction.Equal(si))
            {
            	expression_node bfcint = make_pred_call(si, _method_call.parameters, subloc2);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (SystemLibrary.SystemLibInitializer.OrdFunction.Equal(si))
            {
            	expression_node bfcint = make_ord_call(si, _method_call.parameters, subloc2);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (SystemLibrary.SystemLibInitializer.LowFunction.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 1)
            	{
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		if (param0.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            		{
            			bounded_array_interface bai = param0.type.get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
            			expression_node en = new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value),get_location(_method_call));
            			switch (mot)
                    	{
                        	case motivation.expression_evaluation:
                            {
                                return_value(en);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(en);
                                return;
                            }
                        	default:
                            {
                                AddError(en.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    	}
            		}
            	}
            }
            else if (SystemLibrary.SystemLibInitializer.HighFunction.Equal(si))
            {
            	if (_method_call.parameters != null && _method_call.parameters.expressions.Count == 1)
            	{
            		expression_node param0 = convert_strong(_method_call.parameters.expressions[0]);
            		if (param0.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            		{
            			bounded_array_interface bai = param0.type.get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
            			expression_node en = new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.upper_value),get_location(_method_call));
            			switch (mot)
                    	{
                        	case motivation.expression_evaluation:
                            {
                                return_value(en);
                                return;
                            }
                        	case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(en);
                                return;
                            }
                        	default:
                            {
                                AddError(en.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    	}
            		}
            	}
            }
            else if (si == SystemLibrary.SystemLibInitializer.NewProcedure)
            {
            	expression_node bfcint = make_new_call(si, _method_call.parameters, subloc2);
            	if (!proc_wait) AddError(subloc2, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", (si.sym_info as function_node).name);
            	switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
            }
            else if (si == SystemLibrary.SystemLibInitializer.DisposeProcedure)
            {
                //if (convertion_data_and_alghoritms.select_function(convert_expression_list(_method_call.parameters.expressions), si, subloc2) == SystemLibrary.SystemLibInitializer.DisposeProcedure.sym_info)
                //{
                    expression_node bfcint = make_dispose_call(si, _method_call.parameters, subloc2);
                    if (!proc_wait) AddError(subloc2, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", (si.sym_info as function_node).name);
                    switch (mot)
                    {
                        case motivation.expression_evaluation:
                            {
                                return_value(bfcint);
                                return;
                            }
                        case motivation.semantic_node_reciving:
                            {
                                return_semantic_value(bfcint);
                                return;
                            }
                        default:
                            {
                                AddError(bfcint.location, "EXPRESSION_IS_NOT_ADDRESSED");
                                return;
                            }
                    }

                    return;
               // }
            }
            else if (!debug && SystemLibrary.SystemLibInitializer.AssertProcedure.Equal(si))
            {
//            	return_value(new empty_statement(null));
//            	return;
            }
            /*else if (SystemLibrary.SystemLibInitializer.IncProcedure != null && SystemLibrary.SystemLibInitializer.IncProcedure.Equal(si))
            	{
            		expression_node bfcint = make_inc_call(si, _method_call.parameters, subloc2);
            	}*/

            if (si.sym_info.general_node_type != general_node_type.function_node)
            {
                if (si.sym_info.general_node_type == general_node_type.type_node)
                {
                    to_type = ((type_node)(si.sym_info));
                    /*if (to_type == SystemLibrary.SystemLibrary.void_type)
                    {
                        throw new VoidNotValid(subloc2);
                    }*/
                    check_for_type_allowed(to_type,subloc2);
                }
                else
                {
                    exp2 = ident_value_reciving(si, id);
                    internal_interface ii = exp2.type.get_internal_interface(internal_interface_kind.delegate_interface);
                    if (ii == null)
                    {
                        AddError(subloc2, "EXPECTED_DELEGATE");
                    }
                    if (exp2 is nonstatic_event_reference)
                    {
                    	nonstatic_event_reference nser = exp2 as nonstatic_event_reference;
                    	common_event ce = nser.en as common_event;
                    	if (ce != null)
                    		exp2 = new class_field_reference(ce.field,new this_node(ce.cont_type,null),null);
                    	else
                    		AddError(get_location(_method_call), "EVENT_{0}_MUST_BE_IN_LEFT_PART", nser.en.name);
                    }
                    else if (exp2 is static_event_reference)
                    {
                    	static_event_reference nser = exp2 as static_event_reference;
                    	common_event ce = nser.en as common_event;
                        if (ce != null)
                            exp2 = new static_class_field_reference(ce.field, null);
                        else if (nser.en is common_namespace_event && _compiled_unit.namespaces.IndexOf((nser.en as common_namespace_event).namespace_node) != -1)
                            exp2 = new namespace_variable_reference((nser.en as common_namespace_event).field, null);
                        else
                            AddError(get_location(_method_call), "EVENT_{0}_MUST_BE_IN_LEFT_PART", nser.en.name);
                    }
                    delegate_internal_interface dii = ii as delegate_internal_interface;
                    si = new SymbolInfo(dii.invoke_method);
                }
            }

            if (to_type != null)
            {
                if ((_method_call.parameters == null) || (_method_call.parameters.expressions.Count != 1))
                {
                    AddError(get_location(_method_call), "ONLY_ONE_PARAMETER_OF_TYPE_CONVERSION_ALLOWED");
                }
            }

            if (_method_call.parameters != null)
            {
                foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                {
                	bool tmp = is_format_allowed;
					
					#region Отмечаем флаг в лямбдах, говорящий о том, что в первый раз будем их "обходить" для вывода типов
                    //lroman//
                    if (en is SyntaxTree.function_lambda_definition)
                    {
                        lambdas_are_in_parameters = true;
                        ((SyntaxTree.function_lambda_definition)en).lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                    }
                    //lroman//
                    #endregion
					
                	exprs.AddElement(convert_strong(en));
                	is_format_allowed = tmp;
                }
            }
			is_format_allowed = false;
            CheckSpecialFunctionCall(si, exprs,get_location(_method_call));

            ssi=si;
            foreach (SymbolInfo sii in sibak)
            {
                ssi.Next = sii;
                ssi = sii;
            }

            if (to_type != null)
            {
                //(ssyy) К вызову функции здесь явно не приводим, т.к. это излишне.
                expression_node ee = exprs[0];
                bool del = to_type is common_type_node && (to_type as common_type_node).IsDelegate;
                if (!del)
                try_convert_typed_expression_to_function_call(ref ee);
                else
                	ee = convert_strong(_method_call.parameters.expressions[0]);
                expr_node = convertion_data_and_alghoritms.explicit_convert_type(ee, to_type);
                //expression_node expr = convert_if_typed_expression_to_function_call(exprs[0]);
                //expr_node = convertion_data_and_alghoritms.explicit_convert_type(expr, to_type);
            }
            else
            {
                if (exp2 == null)
                {
                    location mcloc = get_location(_method_call);
                    if (iwt != null)
                    {
                        si = get_generic_functions(si, true, mcloc);
                        si = get_function_instances(si, iwt.template_params.params_list, id.name, mcloc, si.Next == null);
                    }
					
					#region Если встретились лямбды в фактических параметрах, то выбираем нужную функцию из перегруженных, выводим типы, отмечаем флаг в лямбдах, говорящий о том, что мы их реально обходим 
                    //lroman//
                    if (lambdas_are_in_parameters)
                    {
                        LambdaHelper.processingLambdaParametersForTypeInference++;
                        // SSM 21.05.14 - попытка обработать перегруженные функции с параметрами-лямбдами с различными возвращаемыми значениями
                        function_node_list spf = null;
                        try
                        {
                            function_node fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc2, syntax_nodes_parameters);
                            int exprCounter = 0;
                            foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                            {
                                if (!(en is SyntaxTree.function_lambda_definition))
                                {
                                    exprCounter++;
                                    continue;
                                }
                                else
                                {
                                    var enLambda = (SyntaxTree.function_lambda_definition) en;
                                    LambdaHelper.InferTypesFromVarStmt(fn.parameters[exprCounter].type, enLambda, this);
                                    enLambda.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                                    exprs[exprCounter] = convert_strong(en);
                                    enLambda.lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                                    exprCounter++;
                                }
                            }
                        }
                        catch (SeveralFunctionsCanBeCalled sf)
                        {
                            spf = sf.set_of_possible_functions; // Возможны несколько перегруженных версий - надо выводить дальше в надежде что какие-то уйдут и останется одна
                        }

                        Exception lastmultex = null;
                        if (spf != null) // пытаемся инстанцировать одну за другой и ошибки гасим try
                        {   // exprs - глобальная, поэтому надо копировать
                            int spfnum = -1; // первый номер правильно инстанцированной. Если потом встретился второй, то тоже ошибка
                                             // SSM 4.08.15. Сейчас меняю эту логику. Если будет много кандидатов, но ровно один с совпадающим типом возвращаемого значения, то его и надо выбирать.
                                             // не забыть, что аналогичный код есть в create_constructor_call!!!!!!! И еще выше по коду!!! кошмар!!!
                            int GoodVersionsCount = 0;
                            int GoodVersionsCountWithSameResType = 0;
                            for (int i = 0; i < spf.Count; i++) // цикл по версиям
                            {
                                function_node fn = spf[i];
                                try // внутренний try регенерирует исключение, а этот гасит
                                {
                                    int exprCounter = 0;
                                    expressions_list exprs1 = new expressions_list();
                                    exprs1.AddRange(exprs); // сделали копию

                                    foreach (SyntaxTree.expression en in _method_call.parameters.expressions)
                                    {
                                        if (!(en is SyntaxTree.function_lambda_definition))
                                        {
                                            exprCounter++;
                                            continue;
                                        }
                                        else
                                        {
                                            var fld = en as SyntaxTree.function_lambda_definition;

                                            var lambdaName = fld.lambda_name;                           //lroman Сохранять имя необходимо
                                            var fl = fld.lambda_visit_mode;

                                            // запомнили типы параметров лямбды - SSM
                                            object[] realparamstype = new object[fld.formal_parameters.params_list.Count]; // здесь хранятся выведенные типы лямбд или null если типы явно заданы
                                            for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                            {
                                                var laminftypeK = fld.formal_parameters.params_list[k].vars_type as SyntaxTree.lambda_inferred_type;
                                                if (laminftypeK == null)
                                                    realparamstype[k] = null;
                                                else realparamstype[k] = laminftypeK.real_type;
                                            }

                                            // запоминаем реальный тип возвращаемого значения если он не указан явно (это должен быть any_type или null если он указан явно) - он может измениться при следующем вызове, поэтому мы его восстановим
                                            var restype = fld.return_type as SyntaxTree.lambda_inferred_type;
                                            object realrestype = null;
                                            if (restype != null) 
                                                realrestype = restype.real_type;  

                                            LambdaHelper.InferTypesFromVarStmt(fn.parameters[exprCounter].type, fld, this);
                                            fld.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing; //lroman
                                            fld.lambda_name = LambdaHelper.GetAuxiliaryLambdaName(lambdaName); // поправляю имя. Думаю, назад возвращать не надо. ПРОВЕРИТЬ!

                                            //contextChanger.SaveContextAndUpToNearestDefSect();
                                            try
                                            {
                                                exprs1[exprCounter] = convert_strong(en);

                                                // SSM 7/08/15

                                                type_node resexprtype = fld.RealSemTypeOfResExpr as type_node;
                                                type_node resformaltype = fld.RealSemTypeOfResult as type_node;
                                                var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                                if (bbb)
                                                {
                                                    GoodVersionsCountWithSameResType += 1;
                                                    spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                                }

                                                /*var tt = fn.parameters[exprCounter].type as compiled_type_node;
                                                if (tt != null && tt.compiled_type.FullName.ToLower().StartsWith("system.func"))
                                                {
                                                    resformaltype = tt.instance_params[tt.instance_params.Count - 1]; // Последний параметр в записи Func<T,T1,...TN> - тип возвращаемого значения
                                                    var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                                    if (bbb)
                                                    {
                                                        GoodVersionsCountWithSameResType += 1;
                                                        spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                                    }
                                                }*/
                                            }
                                            catch
                                            {
                                                throw;
                                            }
                                            finally
                                            {
                                                LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, en as function_lambda_definition);
                                                // восстанавливаем сохраненный тип возвращаемого значения
                                                if (restype != null)
                                                    restype.real_type = realrestype;
                                                // восстанавливаем сохраненные типы параметров лямбды, которые не были заданы явно
                                                for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                                {
                                                    var laminftypeK = fld.formal_parameters.params_list[k].vars_type as SyntaxTree.lambda_inferred_type;
                                                    if (laminftypeK != null)
                                                        laminftypeK.real_type = realparamstype[k];
                                                }

                                                fld.lambda_name = lambdaName; //lroman Восстанавливаем имена
                                                fld.lambda_visit_mode = fl;
                                            }

                                            //contextChanger.RestoreCurrentContext();
                                            exprCounter++;
                                        }
                                    }
                                    /*if (spfnum >= 0) // два удачных инстанцирования - плохо. Может, одно - с более близким типом возвращаемого значения, тогда это плохо - надо доделать, но пока так
                                    {
                                        spfnum = -2;
                                        break;
                                    }*/

                                    if (GoodVersionsCountWithSameResType==0)
                                        spfnum = i; // здесь запоминаем индекс только если нет подошедших, совпадающих по типу возвращаемого значения
                                    GoodVersionsCount += 1;
                                    for (int j = 0; j < exprs.Count; j++) // копируем назад если всё хорошо
                                        exprs[j] = exprs1[j];
                                }
                                catch (Exception e)
                                {
                                    // если сюда попали, значит, не вывели типы в лямбде и надо эту инстанцию пропускать
                                    //contextChanger.RestoreCurrentContext();
                                    lastmultex = e;
                                }
                            } //--------------- конец цикла по версиям

                            if (GoodVersionsCount>1 && GoodVersionsCountWithSameResType!=1) // подошло много, но не было ровно одной с совпадающим типом возвращаемого значения
                                throw new SeveralFunctionsCanBeCalled(subloc2, spf);
                            if (GoodVersionsCount == 0) // было много, но ни одна не подошла из-за лямбд
                            {
                                throw lastmultex;
                                //throw new NoFunctionWithSameArguments(subloc2, false);
                            }

                            var kk = 0;
                            foreach (SyntaxTree.expression en in _method_call.parameters.expressions) //lroman окончательно подставить типы в лямбды
                            {
                                if (!(en is SyntaxTree.function_lambda_definition))
                                {
                                    kk++;
                                    continue;
                                }
                                else
                                {
                                    LambdaHelper.InferTypesFromVarStmt(spf[spfnum].parameters[kk].type, en as SyntaxTree.function_lambda_definition, this);
                                    exprs[kk] = convert_strong(en);
                                    kk++;
                                }
                            }    
                        }
                        // SSM 21.05.14 end
                        LambdaHelper.processingLambdaParametersForTypeInference--;
                    }
                    //lroman//
                    #endregion
					
                    expr_node = convertion_data_and_alghoritms.create_full_function_call(exprs, si, mcloc,
                        context.converted_type, context.top_function, proc_wait);
                }
                else
                {
                    function_node fn = convertion_data_and_alghoritms.select_function(exprs, si, subloc2, syntax_nodes_parameters);
                    base_function_call bbffcc = create_not_static_method_call(fn, exp2, subloc2, proc_wait);
                    bbffcc.parameters.AddRange(exprs);
                    expr_node = bbffcc;
                }
            }

            /*if ((proc_wait==false)&&(expr_node.type==null))
            {
                throw new FunctionExpectedProcedureMeet((function_node)dn,get_location(_method_call));
            }*/

            switch (mot)
            {
                case motivation.expression_evaluation:
                    {
                        return_value(expr_node);
                        return;
                    }
                case motivation.semantic_node_reciving:
                    {
                        return_semantic_value(expr_node);
                        return;
                    }
            	case motivation.address_reciving:
            		{
                        AddError(get_location(_method_call), "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
                        return;
            		}
                default:
                    {
                        //throw new CompilerInternalError("Can not recive address from method call");//!
                        AddError(get_location(_method_call), "EXPECTED_VARIABLE");
                        return;
                    }
            }
            //throw new CompilerInternalError("Error in creation method call");

        }

        private void delete_extension_methods(ref SymbolInfo si)
        {
            List<SymbolInfo> si_list = new List<SymbolInfo>();
            SymbolInfo tmp_si = si;
            while (tmp_si != null)
            {
                if (!(tmp_si.sym_info is function_node && (tmp_si.sym_info as function_node).is_extension_method))
                    si_list.Add(tmp_si);
                tmp_si = tmp_si.Next;
            }
            SymbolInfo new_si = null;
            for (int i = 0; i < si_list.Count; i++)
            {
                if (new_si == null)
                {
                    new_si = si_list[i];
                    si = new_si;
                }
                else
                {
                    si.Next = si_list[i];
                    si = si.Next;
                }
            }
            if (si != null)
                si.Next = null;
            si = new_si;
        }

        //ssyy
        //Удаляет конструкторы предков в списке si
        private void delete_inherited_constructors(ref SymbolInfo si, type_node tn)
        {
            SymbolInfo last_sym = null;
            SymbolInfo cur_sym = si;
            while (cur_sym != null)
            {
                type_node comph_type = null;
                bool is_ctor = false;
                common_method_node mnode = cur_sym.sym_info as common_method_node;
                if (mnode != null)
                {
                    comph_type = mnode.cont_type;
                    is_ctor = mnode.is_constructor;
                }
                else
                {
                    compiled_function_node tnode = cur_sym.sym_info as compiled_function_node;
                    if (tnode != null)
                    {
                        comph_type = tnode.cont_type;
                        is_ctor = tnode.method_info.IsConstructor;
                    }
                }
                //Итак, comph_type - класс, содержащий si.sym_info;
                //is_ctor - является ли оно конструктором
                if (is_ctor && comph_type != tn)
                {
                    //Удаляем
                    if (last_sym == null)
                    {
                        si = si.Next;
                    }
                    else
                    {
                        last_sym.Next = cur_sym.Next;
                    }
                }
                else
                {
                    last_sym = cur_sym;
                }
                cur_sym = cur_sym.Next;
            }
        }
        //\ssyy

        //TODO: Возможно следует переписать этот метод в более простой форме.
        public override void visit(SyntaxTree.method_call _method_call)
        {
            visit_method_call(_method_call);
            inherited_ident_processing = false;
        }

        private void ConvertPointersForWrite(expressions_list exprs)
        {
            for (int i = 0; i < exprs.Count; ++i)
            {
                if (exprs[i].type.IsPointer)
                {
                    if (SystemLibrary.SystemLibInitializer.PointerOutputConstructor == null)
                    {
                        SymbolInfo si = (SystemLibrary.SystemLibInitializer.PointerOutputType.sym_info as common_type_node).find_in_type(compiler_string_consts.default_constructor_name);
                        common_method_node cnode = null;
                        do
                        {
                            cnode = si.sym_info as common_method_node;
                            if (cnode.parameters.Count != 1)
                            {
                                cnode = null;
                                si = si.Next;
                            }
                        }
                        while (cnode == null);
                        SystemLibrary.SystemLibInitializer.PointerOutputConstructor = cnode;
                    }
                    common_constructor_call cnc = new common_constructor_call(SystemLibrary.SystemLibInitializer.PointerOutputConstructor as common_method_node, exprs[i].location);
                    //common_namespace_function_call cnfc = new common_namespace_function_call(
                    //    SystemLibrary.SystemLibInitializer.PointerToStringFunction.sym_info as common_namespace_function_node,
                    //    exprs[i].location);
                    cnc.parameters.AddElement(exprs[i]);
                    exprs[i] = cnc;
                }
            }
        }

        private void ConvertPointersForWriteFromDll(expressions_list exprs)
        {
            for (int i = 0; i < exprs.Count; ++i)
            {
                if (exprs[i].type.IsPointer)
                {
                    if (SystemLibrary.SystemLibInitializer.PointerOutputConstructor == null)
                    {
                        SymbolInfo si = (SystemLibrary.SystemLibInitializer.PointerOutputType.sym_info as compiled_type_node).find_in_type(compiler_string_consts.default_constructor_name);
                        compiled_constructor_node cnode = null;
                        do
                        {
                            cnode = si.sym_info as compiled_constructor_node;
                            if (cnode.parameters.Count != 1)
                            {
                                cnode = null;
                                si = si.Next;
                            }
                        }
                        while (cnode == null);
                        SystemLibrary.SystemLibInitializer.PointerOutputConstructor = cnode;
                    }
                    compiled_constructor_call cnc = new compiled_constructor_call(SystemLibrary.SystemLibInitializer.PointerOutputConstructor as compiled_constructor_node, exprs[i].location);
                    //common_namespace_function_call cnfc = new common_namespace_function_call(
                    //    SystemLibrary.SystemLibInitializer.PointerToStringFunction.sym_info as common_namespace_function_node,
                    //    exprs[i].location);
                    cnc.parameters.AddElement(exprs[i]);
                    exprs[i] = cnc;
                }
            }
        }

        private void CheckSpecialFunctionCall(SymbolInfo si, expressions_list exprs, location loc)
        {
            if (SystemUnitAssigned)
            {
                bool write_proc = SystemLibrary.SystemLibInitializer.write_procedure.Equal(si);
                if (write_proc && exprs.Count > 1)
                {
                    //возможно это запись в типизированый файл
                    if (exprs[0].type.type_special_kind == SemanticTree.type_special_kind.typed_file)
                    //да, точно это типизированый файл!
                    {
                        //Проверим типы параметров вызова на совпадение с типом элементов файла
                        type_node element_type = exprs[0].type.element_type;
                        for (int i = 1; i < exprs.Count; i++)
                            convertion_data_and_alghoritms.check_convert_type(exprs[i], element_type, exprs[i].location);
                        if (element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                        {
                        	for (int i=1; i<exprs.Count; i++)
                        	{
                        		//common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node,null);
                        		//cmc.parameters.AddElement(exprs[i]);
                        		//cmc.parameters.AddElement(new int_const_node((element_type as short_string_type_node).Length,null));
                        		expression_node cmc = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(exprs[i],SystemLibrary.SystemLibrary.string_type),new int_const_node((element_type as short_string_type_node).Length,null));
                        		exprs[i] = cmc;
                        	}
                        }
                        //if (exprs[i].type != element_type)
                        //  throw new ExpectedExprHaveTypeTypedFile(element_type, exprs[i].type, false, get_location(_method_call.parameters.expressions[i]));
                    }
                    else
                        //(ssyy) а может быть, в бинарный!
                        if (exprs[0].type.type_special_kind == SemanticTree.type_special_kind.binary_file)
                        {
                            //(ssyy) действительно, бинарный
                            for (int i = 1; i < exprs.Count; i++)
                            {
                                if (!CanUseThisTypeForBinaryFiles(exprs[i].type))
                                {
                                    AddError(exprs[i].location, "CAN_NOT_WRITE_REFERENCE_DATA_TO_BINARY_FILE");
                                }
                            }
                        }
                        else
                        {
                            if (SystemLibrary.SystemLibInitializer.write_procedure.sym_info is common_namespace_function_node)
                                ConvertPointersForWrite(exprs);
                            else
                                ConvertPointersForWriteFromDll(exprs);
                        }
                }
                else if (write_proc || SystemLibrary.SystemLibInitializer.writeln_procedure.Equal(si))
                {
                    //(ssyy) Преобразуем указатели в строки для вывода write/writeln
                    if (SystemLibrary.SystemLibInitializer.writeln_procedure.sym_info is common_namespace_function_node)
                        ConvertPointersForWrite(exprs);
                    else
                        ConvertPointersForWriteFromDll(exprs);
                }
                else if (SystemLibrary.SystemLibInitializer.IncludeProcedure.Equal(si)
                            || SystemLibrary.SystemLibInitializer.ExcludeProcedure.Equal(si))
                {
                    if (exprs.Count != 2) AddError( new NoFunctionWithSameArguments(loc, true));
                    type_node element_type = exprs[0].type.element_type;
                    if (exprs[0].type.type_special_kind != SemanticTree.type_special_kind.base_set_type && exprs[0].type.type_special_kind != SemanticTree.type_special_kind.set_type)
                    {
                    	AddError( new NoFunctionWithSameArguments(loc, true));
                    }
                    if (element_type != null)
                        convertion_data_and_alghoritms.check_convert_type(exprs[1], element_type, exprs[1].location);
                    else convertion_data_and_alghoritms.check_convert_type(exprs[1], exprs[0].type, exprs[0].location);
                    if (!exprs[0].is_addressed) AddError( new ThisExpressionCanNotBePassedAsVarParameter(exprs[0]));
                }
                else if (SystemLibrary.SystemLibInitializer.InSetProcedure.Equal(si))
                {
                    if (exprs.Count != 2) AddError( new NoFunctionWithSameArguments(loc, true));
                    type_node element_type = exprs[1].type.element_type;
                    if (element_type != null)
                    {
                        //exprs[0] = convertion_data_and_alghoritms.convert_type(exprs[0], element_type, exprs[0].location);
                        convertion_data_and_alghoritms.check_convert_type(exprs[0], element_type, exprs[0].location);
                    }
                    else convertion_data_and_alghoritms.check_convert_type(exprs[0], exprs[1].type, exprs[0].location);
                }
                else if (SystemLibrary.SystemLibInitializer.SetUnionProcedure.Equal(si)
                   || SystemLibrary.SystemLibInitializer.SetIntersectProcedure.Equal(si) || SystemLibrary.SystemLibInitializer.SetSubtractProcedure.Equal(si))
                {
                    if (exprs.Count != 2) AddError( new NoFunctionWithSameArguments(loc, true));
                    convertion_data_and_alghoritms.check_convert_type(exprs[1], exprs[0].type, exprs[1].location);
                }

                if (si == SystemLibrary.SystemLibInitializer.NewProcedure || si == SystemLibrary.SystemLibInitializer.DisposeProcedure)
                {
                    if (exprs[0].type == SystemLibrary.SystemLibrary.pointer_type)
                        AddError(exprs[0].location, "EXPECTED_TYPED_POINTER_IN_PROCEDURE{0}", (si.sym_info as function_node).name);
                }
            }
        }

        // Пробую разобраться и упростить. ССМ.
        //TODO надо какнибуть ускорить!
        private void CheckCanRead(expression_node en)
        {
            /*if (en.type != SystemLibrary.SystemLibrary.integer_type &&
                en.type != SystemLibrary.SystemLibrary.real_type &&
                en.type != SystemLibrary.SystemLibrary.string_type &&
                en.type != SystemLibrary.SystemLibrary.char_type &&
                en.type != SystemLibrary.SystemLibrary.byte_type &&
                en.type != SystemLibrary.SystemLibrary.short_type &&
                en.type != SystemLibrary.SystemLibrary.sbyte_type &&
                en.type != SystemLibrary.SystemLibrary.ushort_type &&
                en.type != SystemLibrary.SystemLibrary.uint_type &&
                en.type != SystemLibrary.SystemLibrary.long_type &&
                en.type != SystemLibrary.SystemLibrary.ulong_type &&
                en.type != SystemLibrary.SystemLibrary.float_type)
                    throw new CanNotRead(en.location);*/
        }


        /*private expression_node make_write_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            expression_node last_call = null;
            expressions_list exl = new expressions_list();
            expression_node text_file_to_out = null;
            type_node typed_file_to_out = null;
            type_node typed_file_element_type = null;
            if (parameters != null) // если список параметров не пуст
            {
                if (parameters.expressions.Count >= 1)
                {
                    expression_node param0 = convert_strong(parameters.expressions[0]);
                    if (param0.type == (SystemLibrary.SystemLibInitializer.file_type.sym_info))
                        text_file_to_out = param0;
                    if (param0.type.type_special_kind == SemanticTree.type_special_kind.typed_file)
                    {
                        typed_file_to_out = param0.type;
                        typed_file_element_type = typed_file_to_out.element_type;
                    }
                }


                foreach (SyntaxTree.expression ex in parameters.expressions)
                {
                //ex = convertion_data_and_alghoritms.convert_type(ex, typed_file_element_type);

                }
            }
            //
        }*/
		
        private expression_node make_new_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
        	if (parameters == null) AddError( new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node));
            if (parameters.expressions.Count == 1)
            {
                expression_node param0 = convert_strong(parameters.expressions[0]);
                if (!param0.type.IsPointer || param0.type == SystemLibrary.SystemLibrary.pointer_type)
                    AddError(new NoFunctionWithSameArguments(loc, true));
                if (!param0.is_addressed)
                {
                    bool is_pascal_array_ref = false;
                    if (param0.semantic_node_type == semantic_node_type.common_method_call)
                    {
                        common_method_call cmc = (common_method_call)param0;
                        internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                        if (ii != null)
                        {
                            if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                            {
                                bounded_array_interface bai = (bounded_array_interface)ii;
                                class_field cf = bai.int_array;
                                expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                expression_node right = cmc.parameters[0];
                                //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                    new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                                param0 = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                is_pascal_array_ref = true;
                            }
                        }
                        if (!is_pascal_array_ref)
                        {
                            AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                        }

                    }
                    else AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                }
                if (param0.type is ref_type_node && !can_evaluate_size((param0.type as ref_type_node).pointed_type))
                    AddError(loc, "CANNOT_CALL_NEW_BECAUSE_OF_UNDEFINED_SIZE");
                function_node fn = si.sym_info as function_node;
                common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                cnfc.parameters.AddElement(param0);
                return cnfc;

            }
            else
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
        }
        
        private expression_node make_dispose_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            if (parameters == null)
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
            if (parameters.expressions.Count == 1)
            {
                expression_node param0 = convert_strong(parameters.expressions[0]);
                if (!param0.type.IsPointer || param0.type == SystemLibrary.SystemLibrary.pointer_type)
                    AddError(new NoFunctionWithSameArguments(loc, true));
                if (!param0.is_addressed)
                {
                    bool is_pascal_array_ref = false;
                    if (param0.semantic_node_type == semantic_node_type.common_method_call)
                    {
                        common_method_call cmc = (common_method_call)param0;
                        internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                        if (ii != null)
                        {
                            if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                            {
                                bounded_array_interface bai = (bounded_array_interface)ii;
                                class_field cf = bai.int_array;
                                expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                expression_node right = cmc.parameters[0];
                                //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                    new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                                param0 = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                is_pascal_array_ref = true;
                            }
                        }
                        if (!is_pascal_array_ref)
                        {
                            AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                            return null;
                        }

                    }
                    else
                    {
                        AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                        return null;
                    }
                }
                function_node fn = si.sym_info as function_node;
                common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                cnfc.parameters.AddElement(param0);
                return cnfc;

            }
            else
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
        }
        
        private expression_node make_ord_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            if (parameters == null)
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
            if (parameters.expressions.Count == 1)
            {
                expression_node param0 = convert_strong(parameters.expressions[0]);
                if (param0 is typed_expression) param0 = convert_typed_expression_to_function_call(param0 as typed_expression);

                expressions_list el = new expressions_list();
                el.AddElement(param0);
                function_node fn = null;
                try
                {
                    fn = convertion_data_and_alghoritms.select_function(el, si, loc);
                }
                catch (Exception e)
                {

                }
                if (fn != null)
                {
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                }
                else if (param0.type.IsEnum || param0.type.type_special_kind == SemanticTree.type_special_kind.diap_type)
                {
                    el = new expressions_list();
                    el.AddElement(new int_const_node(1, null));
                    fn = convertion_data_and_alghoritms.select_function(el, si, loc);
                    //bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
                    //bfc.ret_type = param0.type;
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                }

                else
                {
                    AddError(new NoFunctionWithSameArguments(loc, false));
                    return null;
                }
            }
            else
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
		}
        
        private expression_node make_succ_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            if (parameters == null)
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
        	if (parameters.expressions.Count == 1)
        	{
        		expression_node param0 = convert_strong(parameters.expressions[0]);
        		if (param0 is typed_expression) param0 = convert_typed_expression_to_function_call(param0 as typed_expression);
        		basic_function_call bfc = null;
        		int type_flag = 0;
        		expressions_list el = new expressions_list();
        		el.AddElement(param0);
        		function_node fn = null;
        		try
        		{
        			fn = convertion_data_and_alghoritms.select_function(el,si,loc);
        		}
        		catch(Exception e)
        		{
        			
        		}
        		if (fn != null)
        		{
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
        		}
        		if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || param0.type.IsEnum)
        		{
        			el = new expressions_list();
        			el.AddElement(new int_const_node(1,null));
        			fn = convertion_data_and_alghoritms.select_function(el,si,loc);
        			//bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        			//bfc.ret_type = param0.type;
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        cnfc.type = param0.type;
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        cnfc.type = param0.type;
                        return cnfc;
                    }
        		}
        		else AddError(new NoFunctionWithSameArguments(loc,false));
        		bfc.parameters.AddElement(param0);
        		if (type_flag == 0 || type_flag == 3)
        		bfc.parameters.AddElement(new int_const_node(1,null));
        		else if (type_flag == 1)
        		bfc.parameters.AddElement(new long_const_node(1,null));
        		else if (type_flag == 2)
        		bfc.parameters.AddElement(new ulong_const_node(1,null));
        		return bfc;
        	}
        	AddError(new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node));
            return null;
        }
        
        private expression_node make_pred_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            if (parameters == null)
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
        	if (parameters.expressions.Count == 1)
        	{
        		expression_node param0 = convert_strong(parameters.expressions[0]);
        		if (param0 is typed_expression) param0 = convert_typed_expression_to_function_call(param0 as typed_expression);
        		basic_function_call bfc = null;
        		int type_flag = 0;
        		expressions_list el = new expressions_list();
        		el.AddElement(param0);
        		function_node fn = null;
        		try
        		{
        			fn = convertion_data_and_alghoritms.select_function(el,si,loc);
        		}
        		catch(Exception e)
        		{
        			
        		}
        		if (fn != null)
        		{
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        return cnfc;
                    }
        		}
        		else if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || param0.type.IsEnum)
        		{
        			el = new expressions_list();
        			el.AddElement(new int_const_node(1,null));
        			fn = convertion_data_and_alghoritms.select_function(el,si,loc);
        			//bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        			//bfc.ret_type = param0.type;
                    if (fn is common_namespace_function_node)
                    {
                        common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        cnfc.type = param0.type;
                        return cnfc;
                    }
                    else
                    {
                        compiled_static_method_call cnfc = new compiled_static_method_call(fn as compiled_function_node, loc);
                        cnfc.parameters.AddElement(param0);
                        cnfc.type = param0.type;
                        return cnfc;
                    }
        		}
        		else AddError(new NoFunctionWithSameArguments(loc,false));
        		bfc.parameters.AddElement(param0);
        		if (type_flag == 0 || type_flag == 3)
        		bfc.parameters.AddElement(new int_const_node(1,null));
        		else if (type_flag == 1)
        		bfc.parameters.AddElement(new long_const_node(1,null));
        		else if (type_flag == 2)
        		bfc.parameters.AddElement(new ulong_const_node(1,null));
        		return bfc;
        	}
        	AddError(new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node));
            return null;
        }
        
        private expression_node make_inc_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
            if (parameters == null)
            {
                AddError(new NoFunctionWithSameParametresNum(loc, false, si.sym_info as function_node));
                return null;
            }
        	if (parameters.expressions.Count == 1 || parameters.expressions.Count == 2)
        	{
        		expression_node param0 = convert_strong(parameters.expressions[0]);
        		if (!param0.is_addressed)
        		{
        			bool is_pascal_array_ref = false;
                    if (param0.semantic_node_type == semantic_node_type.common_method_call)
                    {
                        common_method_call cmc = (common_method_call)param0;
                        internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                        if (ii != null)
                        {
                            if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                            {
                                bounded_array_interface bai = (bounded_array_interface)ii;
                                class_field cf = bai.int_array;
                                expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                expression_node right = cmc.parameters[0];
                                //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                    new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                                param0 = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                is_pascal_array_ref = true;
                            }
                        }
                        if (!is_pascal_array_ref)
                        {
							AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                    	}
        				
        			}
                    else if (param0 is compiled_function_call)
                    {
                    	compiled_function_call cfc = param0 as compiled_function_call;
                    	if ((cfc.function_node.return_value_type == SystemLibrary.SystemLibrary.char_type && cfc.function_node.cont_type == SystemLibrary.SystemLibrary.string_type
                    	      && cfc.function_node == cfc.function_node.cont_type.default_property_node.get_function))
                    	{
                    		expressions_list exl = new expressions_list();
                            exl.AddElement(cfc.obj);
                            exl.AddElement(cfc.parameters[0]);
                            basic_function_call bfc2 = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
                            bfc2.parameters.AddElement(param0);
                            expression_node _param1 = null;
                            if (parameters.expressions.Count == 2) 
        					{
        						_param1 = convert_strong(parameters.expressions[1]);
        					}
                            if (_param1 != null)
                            	bfc2.parameters.AddElement(convertion_data_and_alghoritms.convert_type(_param1,SystemLibrary.SystemLibrary.integer_type));
                            else
                            	bfc2.parameters.AddElement(new int_const_node(1,null));
                            expressions_list el = new expressions_list();
                            el.AddElement(convertion_data_and_alghoritms.convert_type(bfc2,SystemLibrary.SystemLibrary.ushort_type));
                            function_node chr_func = convertion_data_and_alghoritms.select_function(el, SystemLibrary.SystemLibInitializer.ChrUnicodeFunction.SymbolInfo, loc);
                            exl.AddElement(convertion_data_and_alghoritms.create_simple_function_call(chr_func, loc, el.ToArray()));
                            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure.SymbolInfo, loc);
                            expression_node ret = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, exl.ToArray());
                            return ret;
                    	}
                    	else
                    	{
                            AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
                    	}
                    }
                    else AddError(new ThisExpressionCanNotBePassedAsVarParameter(param0));
        		}
        		else 
        			check_on_loop_variable(param0);
        		expression_node param1 = null;
        		int type_flag = 0;
        		bool is_uint = false;
        		bool is_enum = false;
        		if (parameters.expressions.Count == 2) 
        		{
        			param1 = convert_strong(parameters.expressions[1]);
        			//param1 = convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type);
        		}
        		basic_function_call ass = null;
        		basic_function_call bfc = null;
        		if (param0.type == SystemLibrary.SystemLibrary.integer_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.char_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.char_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.byte_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.byte_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.sbyte_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.sbyte_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.short_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.short_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.ushort_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.ushort_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.uint_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.uint_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.uint_add as basic_function_node,loc);
        			type_flag = 2;
        			is_uint = true;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.int64_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.long_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.long_add as basic_function_node,loc);
        			type_flag = 1;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.uint64_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.ulong_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.ulong_add as basic_function_node,loc);
        			type_flag = 2;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.bool_type)
        		{
        			if (param1 == null)
        			{
        				ass = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,loc);
        				bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,loc);
        				type_flag = 4;
        			}
        			else
        			{
        				basic_function_call mod_expr = new basic_function_call(SystemLibrary.SystemLibrary.int_mod as basic_function_node,loc);
        				mod_expr.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type));
        				mod_expr.parameters.AddElement(new int_const_node(2,loc));
        				basic_function_call condition = new basic_function_call(SystemLibrary.SystemLibrary.int_eq as basic_function_node,loc);
        				condition.parameters.AddElement(mod_expr);
        				condition.parameters.AddElement(new int_const_node(0,loc));
        				basic_function_call not_expr = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,loc);
        				not_expr.parameters.AddElement(param0);
        				question_colon_expression qce = new question_colon_expression(condition, param0, not_expr, loc);
        				ass = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,loc);
        				ass.parameters.AddElement(param0);
        				ass.parameters.AddElement(qce);
        				type_flag = 5;
        			}
        		}
        		else if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || param0.type.IsEnum)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,loc);
        			if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
        			{
        				type_node bt = param0.type.base_type;
        				if (bt == SystemLibrary.SystemLibrary.int64_type) type_flag = 1;
        				else if (bt == SystemLibrary.SystemLibrary.uint64_type) type_flag = 2;
        				else if (bt == SystemLibrary.SystemLibrary.uint_type) 
        				{
        					type_flag =2;
        					is_uint = true;
        				}
        				else if (bt.IsEnum)
        				{
        					is_enum = true;
        				}
        			}
        			else if (param0.type.IsEnum) is_enum = true;
        		}
        		else AddError(new NoFunctionWithSameArguments(loc,false));
                if (type_flag != 5)
                {
                    if (param1 != null)
                        switch (type_flag)
                        {
                            case 0: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.integer_type); break;
                            case 3:
                            case 1: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.int64_type); break;
                            case 2: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.uint64_type); break;
                        }
                    if (param0.type != SystemLibrary.SystemLibrary.char_type && !is_enum && param0.type != SystemLibrary.SystemLibrary.bool_type)
                        switch (type_flag)
                        {
                            case 0: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.integer_type)); break;
                            case 3:
                            case 1: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.int64_type)); break;
                            case 2: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.uint64_type)); break;
                        }

                    else bfc.parameters.AddElement(param0);
                    //bfc.parameters.AddElement(param0);

                    if (param1 == null)
                    {
                        if (type_flag == 0 || type_flag == 3)
                            bfc.parameters.AddElement(new int_const_node(1, null));
                        else if (type_flag == 1)
                            bfc.parameters.AddElement(new long_const_node(1, null));
                        else if (type_flag == 2)
                            bfc.parameters.AddElement(new ulong_const_node(1, null));
                    }
                    else
                        bfc.parameters.AddElement(param1);
                    ass.parameters.AddElement(param0);
                    if (is_uint)
                        ass.parameters.AddElement(convertion_data_and_alghoritms.convert_type(bfc, SystemLibrary.SystemLibrary.uint_type));
                    else
                        if (param0.type == SystemLibrary.SystemLibrary.char_type)
                        {
                            expressions_list el = new expressions_list();
                            el.AddElement(convertion_data_and_alghoritms.convert_type(bfc, SystemLibrary.SystemLibrary.ushort_type));
                            function_node chr_func = convertion_data_and_alghoritms.select_function(el, SystemLibrary.SystemLibInitializer.ChrUnicodeFunction.SymbolInfo, loc);
                            ass.parameters.AddElement(convertion_data_and_alghoritms.create_simple_function_call(chr_func, loc, el.ToArray()));
                        }
                        else
                            ass.parameters.AddElement(bfc);
                }
        		return ass;
        	}
        	
        	AddError(new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node));
            return null;
        }
		
        private expression_node make_dec_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {
        	if (parameters == null) throw new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node);
        	if (parameters.expressions.Count == 1 || parameters.expressions.Count == 2)
        	{
        		expression_node param0 = convert_strong(parameters.expressions[0]);
        		if (!param0.is_addressed)
        		{
        			bool is_pascal_array_ref = false;
                    if (param0.semantic_node_type == semantic_node_type.common_method_call)
                    {
                        common_method_call cmc = (common_method_call)param0;
                        internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                        if (ii != null)
                        {
                            if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                            {
                                bounded_array_interface bai = (bounded_array_interface)ii;
                                class_field cf = bai.int_array;
                                expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                expression_node right = cmc.parameters[0];
                                //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                    new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                                param0 = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                is_pascal_array_ref = true;

                            }
                        }
                        if (!is_pascal_array_ref)
                        {
							throw new ThisExpressionCanNotBePassedAsVarParameter(param0);
                    	}
        				
        			}
                    else if (param0 is compiled_function_call)
                    {
                    	compiled_function_call cfc = param0 as compiled_function_call;
                    	if ((cfc.function_node.return_value_type == SystemLibrary.SystemLibrary.char_type && cfc.function_node.cont_type == SystemLibrary.SystemLibrary.string_type
                    	      && cfc.function_node == cfc.function_node.cont_type.default_property_node.get_function))
                    	{
                    		expressions_list exl = new expressions_list();
                            exl.AddElement(cfc.obj);
                            exl.AddElement(cfc.parameters[0]);
                            basic_function_call bfc2 = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
                            bfc2.parameters.AddElement(param0);
                            expression_node _param1 = null;
                            if (parameters.expressions.Count == 2) 
        					{
        						_param1 = convert_strong(parameters.expressions[1]);
        					}
                            if (_param1 != null)
                            	bfc2.parameters.AddElement(convertion_data_and_alghoritms.convert_type(_param1,SystemLibrary.SystemLibrary.integer_type));
                            else
                            	bfc2.parameters.AddElement(new int_const_node(1,null));
                            expressions_list el = new expressions_list();
                            el.AddElement(convertion_data_and_alghoritms.convert_type(bfc2,SystemLibrary.SystemLibrary.ushort_type));
                            function_node chr_func = convertion_data_and_alghoritms.select_function(el, SystemLibrary.SystemLibInitializer.ChrUnicodeFunction.SymbolInfo, loc);
                            exl.AddElement(convertion_data_and_alghoritms.create_simple_function_call(chr_func, loc, el.ToArray()));
                            function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure.SymbolInfo, loc);
                            expression_node ret = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, exl.ToArray());
                            return ret;
                    	}
                    	else
                    	{
                    		throw new ThisExpressionCanNotBePassedAsVarParameter(param0);
                    	}
                    }
                    else throw new ThisExpressionCanNotBePassedAsVarParameter(param0);
        		}
        		else
        			check_on_loop_variable(param0);
        		expression_node param1 = null;
        		int type_flag = 0;
        		bool is_uint=false;
        		bool is_enum=false;
        		if (parameters.expressions.Count == 2) 
        		{
        			param1 = convert_strong(parameters.expressions[1]);
        			param1 = convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type);
        		}
        		basic_function_call ass = null;
        		basic_function_call bfc = null;
        		if (param0.type == SystemLibrary.SystemLibrary.integer_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.char_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.char_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.byte_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.byte_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.sbyte_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.sbyte_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.short_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.short_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.ushort_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.ushort_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.uint_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.uint_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.uint_sub as basic_function_node,loc);
        			type_flag = 2;
        			is_uint = true;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.int64_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.long_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.long_sub as basic_function_node,loc);
        			type_flag = 1;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.uint64_type)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.ulong_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.ulong_sub as basic_function_node,loc);
        			type_flag = 2;
        		}
        		else if (param0.type == SystemLibrary.SystemLibrary.bool_type)
        		{
        			if (param1 == null)
        			{
        				ass = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,loc);
        				bfc = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,loc);
        				type_flag = 4;
        			}
        			else
        			{
        				basic_function_call mod_expr = new basic_function_call(SystemLibrary.SystemLibrary.int_mod as basic_function_node,loc);
        				mod_expr.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param1,SystemLibrary.SystemLibrary.integer_type));
        				mod_expr.parameters.AddElement(new int_const_node(2,loc));
        				basic_function_call condition = new basic_function_call(SystemLibrary.SystemLibrary.int_eq as basic_function_node,loc);
        				condition.parameters.AddElement(mod_expr);
        				condition.parameters.AddElement(new int_const_node(0,loc));
        				basic_function_call not_expr = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node,loc);
        				not_expr.parameters.AddElement(param0);
        				question_colon_expression qce = new question_colon_expression(condition, param0, not_expr, loc);
        				ass = new basic_function_call(SystemLibrary.SystemLibrary.bool_assign as basic_function_node,loc);
        				ass.parameters.AddElement(param0);
        				ass.parameters.AddElement(qce);
        				type_flag = 5;
        			}
        		}
        		else if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || param0.type.IsEnum)
        		{
        			ass = new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,loc);
        			bfc = new basic_function_call(SystemLibrary.SystemLibrary.int_sub as basic_function_node,loc);
        			if (param0.type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
        			{
        				type_node bt = param0.type.base_type;
        				if (bt == SystemLibrary.SystemLibrary.int64_type) type_flag = 1;
        				else if (bt == SystemLibrary.SystemLibrary.uint64_type) type_flag = 2;
        				else if (bt == SystemLibrary.SystemLibrary.uint_type) 
        				{
        					type_flag =2;
        					is_uint = true;
        				}
        				else if (bt.IsEnum) is_enum = true;
        			}
        			else if (param0.type.IsEnum) is_enum = true;
        		}
        		else throw new NoFunctionWithSameArguments(loc,false);
                if (type_flag != 5)
                {
                    if (param1 != null)
                        switch (type_flag)
                        {
                            case 0: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.integer_type); break;
                            case 3:
                            case 1: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.int64_type); break;
                            case 2: param1 = convertion_data_and_alghoritms.convert_type(param1, SystemLibrary.SystemLibrary.uint64_type); break;
                        }
                    //bfc.parameters.AddElement(param0);
                    if (param0.type != SystemLibrary.SystemLibrary.char_type && !is_enum && param0.type != SystemLibrary.SystemLibrary.bool_type)
                        switch (type_flag)
                        {
                            case 0: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.integer_type)); break;
                            case 3:
                            case 1: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.int64_type)); break;
                            case 2: bfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(param0, SystemLibrary.SystemLibrary.uint64_type)); break;
                        }
                    else bfc.parameters.AddElement(param0);
                    if (param1 == null)
                    {
                        if (type_flag == 0 || type_flag == 3)
                            bfc.parameters.AddElement(new int_const_node(1, null));
                        else if (type_flag == 1)
                            bfc.parameters.AddElement(new long_const_node(1, null));
                        else if (type_flag == 2)
                            bfc.parameters.AddElement(new ulong_const_node(1, null));
                    }
                    else bfc.parameters.AddElement(param1);
                    ass.parameters.AddElement(param0);
                    if (is_uint)
                        ass.parameters.AddElement(convertion_data_and_alghoritms.convert_type(bfc, SystemLibrary.SystemLibrary.uint_type));
                    else
                        ass.parameters.AddElement(bfc);
                }
        		return ass;
        	}
        	
        	throw new NoFunctionWithSameParametresNum(loc,false,si.sym_info as function_node);
        }
        
        private void check_on_loop_variable(expression_node en)
        {
        	if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable)
        		if (en.semantic_node_type == semantic_node_type.namespace_variable_reference)
        		{
        			if (context.is_loop_variable((en as namespace_variable_reference).var))
                        AddError(en.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
        		}
        		else if (en.semantic_node_type == semantic_node_type.local_variable_reference)
        		{
        			if (context.is_loop_variable((en as local_variable_reference).var))
                        AddError(en.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
        		}
        		else if (en.semantic_node_type == semantic_node_type.local_block_variable_reference)
        		{
        			if (context.is_loop_variable((en as local_block_variable_reference).var))
                        AddError(en.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
        		}
        		
        }
        
        private expression_node make_read_call(SymbolInfo si, SyntaxTree.expression_list parameters, location loc)
        {

            // вызов read(a,b,c) разбивается на несколько вызовов, последний возвращается узлом, а остальные добавляются здесь в теле            expressions_list exl = new expressions_list();
            expression_node last_call = null;
            expressions_list exl = new expressions_list();
            expression_node file = null;
            bool read_from_file = false;
            bool read_from_text_file = false;
            bool read_from_typed_file = false;
            bool read_from_binary_file = false;
            bool readln_string_file = false;
            if (parameters != null) // если список параметров не пуст
            {
                if (parameters.expressions.Count >= 1)
                {
                    expression_node param0 = convert_strong(parameters.expressions[0]);
                    if (param0.type != null)
                    {
                        if (SystemLibrary.SystemLibInitializer.TextFileType.Found && param0.type == SystemLibrary.SystemLibInitializer.TextFileType.sym_info)
                        {
                            file = param0;
                            read_from_text_file = true;
                        }
                        if (param0.type.type_special_kind == SemanticTree.type_special_kind.typed_file
                            && parameters.expressions.Count > 1                        //это можно внести во внутрь
                            )//и побросать приличные исключения
                        {
                            if (!SystemLibrary.SystemLibInitializer.read_procedure.FromDll)
                            {
                                if (si == SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo)
                                {
                                    file = param0;
                                    read_from_typed_file = true;
                                }
                            }
                            else if (SystemLibrary.SystemLibInitializer.read_procedure.Equal(si))
                            {
                                file = param0;
                                read_from_typed_file = true;
                            }
                        }
                        if (param0.type.type_special_kind == SemanticTree.type_special_kind.binary_file
                            && parameters.expressions.Count > 1                        //это можно внести во внутрь
                            )//и побросать приличные исключения
                        {
                            if (!SystemLibrary.SystemLibInitializer.read_procedure.FromDll)
                            {
                                if (si == SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo)
                                {
                                    file = param0;
                                    read_from_binary_file = true;
                                }
                            }
                            else if (SystemLibrary.SystemLibInitializer.read_procedure.Equal(si))
                            {
                                file = param0;
                                read_from_binary_file = true;
                            }
                        }
                    }
                    read_from_file = read_from_text_file || read_from_typed_file || read_from_binary_file;
                }
                bool first_iteration = true;
                
                foreach (SyntaxTree.expression ex in parameters.expressions)
                {
                    if (first_iteration && read_from_file)
                    {
                        first_iteration = false;
                        continue;
                    }
                    if (last_call != null && convertion_data_and_alghoritms.statement_list_stack.size > 0)
                        convertion_data_and_alghoritms.statement_list_stack.top().statements.AddElement(last_call);
                    expression_node en = convert_strong(ex);
                    check_on_loop_variable(en);
                    //if (en.type == null)
                    //throw new CanNotRead(en.location);
                    exl.clear();
                    if (read_from_file)
                        exl.AddElement(file);
                    if (read_from_typed_file)
                    {
                        if (SystemLibrary.SystemLibInitializer.TypedFileReadProcedure == null)
                            AddError(new NotSupportedError(loc));
                        if (en.type != file.type.element_type)
                            AddError(new ExpectedExprHaveTypeTypedFile(file.type.element_type, en.type, true, get_location(ex)));
                        bool is_char_getter = false;
                        if (!en.is_addressed)
                        {
                            if (en is compiled_function_call)
                            {
                                compiled_function_call cfc = en as compiled_function_call;
                                
                                if ((cfc.function_node.return_value_type == SystemLibrary.SystemLibrary.char_type && cfc.function_node.cont_type == SystemLibrary.SystemLibrary.string_type
                                      && cfc.function_node == cfc.function_node.cont_type.default_property_node.get_function))
                                {
                                    en = new simple_array_indexing((en as compiled_function_call).obj, (en as compiled_function_call).parameters[0], SystemLibrary.SystemLibrary.char_type, en.location);
                                    is_char_getter = true;
                                }
                                
                            }
                        }
                        function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.TypedFileReadProcedure.SymbolInfo, loc);
                        expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, get_location(ex), file);
                        //expression_node expr = convertion_data_and_alghoritms.create_full_function_call(exl, SystemLibrary.SystemLibInitializer.TypedFileReadProcedure.SymbolInfo, loc, context.converted_type, context.top_function, true);
                        expr = convertion_data_and_alghoritms.explicit_convert_type(expr, file.type.element_type);
                        if (is_char_getter)
                            expr = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure.sym_info as function_node, loc, (en as simple_array_indexing).simple_arr_expr, (en as simple_array_indexing).ind_expr, expr);
                        else
                            expr = find_operator(compiler_string_consts.assign_name, en, expr, loc);
                        last_call = expr;
                    }
                    else if (read_from_binary_file)
                    {
                        if (SystemLibrary.SystemLibInitializer.BinaryFileReadProcedure == null)
                            AddError(new NotSupportedError(loc));
                        if (!CanUseThisTypeForBinaryFiles(en.type))
                        {
                            AddError(en.location, "CAN_NOT_READ_REFERENCE_DATA_FROM_BINARY_FILE");
                        }
                        exl.AddElement(new typeof_operator(en.type, loc));
                        function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.BinaryFileReadProcedure.SymbolInfo, loc);
                        expression_node expr = convertion_data_and_alghoritms.create_simple_function_call(fn, get_location(ex), exl.ToArray());
                        expr = convertion_data_and_alghoritms.explicit_convert_type(expr, en.type);
                        expr = find_operator(compiler_string_consts.assign_name, en, expr, loc);
                        last_call = expr;
                    }
                    else
                    {
                        exl.AddElement(en);
                        try
                        {
                        	function_node fn = null;
                        	if (en.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                        	{
                        		exl.AddElement(new int_const_node((en.type as short_string_type_node).Length,null));
                        		if (!read_from_file)
                        		last_call = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.read_short_string_procedure.sym_info as function_node,get_location(ex),exl.ToArray());
                        		else
                        		last_call = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.read_short_string_from_file_procedure.sym_info as function_node,get_location(ex),exl.ToArray());
                        	}
                        	else if (en.type.type_special_kind == SemanticTree.type_special_kind.diap_type)
                        	{
                        		exl.remove(en);
                        		en.type = en.type.base_type;
                        		exl.AddElement(en);
                        		fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo, loc);
                            	last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, get_location(ex), exl.ToArray());
                        	}
                        	else
                        	{
                                if (SystemLibrary.SystemLibInitializer.readln_procedure.Equal(si) && parameters.expressions.Count == 2 && en.type == SystemLibrary.SystemLibrary.string_type)
                                {
                                    fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.readln_procedure.SymbolInfo, loc);
                                    readln_string_file = true;
                                }
                                else
                                    fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo, loc);
                            	last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, get_location(ex), exl.ToArray());
                        	}
                            //last_call = convertion_data_and_alghoritms.create_full_function_call(exl, SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo, loc, context.converted_type, context.top_function, true);
                        }
                        catch (NoFunctionWithSameArguments)
                        {
                            AddError(en.location, "CAN_NOT_READ");
                        }
                    }
                    first_iteration = false;
                }
            }
            if ((parameters == null) || (parameters.expressions.Count == 0)) // read(), readln()
            {
                if (SystemLibrary.SystemLibInitializer.read_procedure.Equal(si))
                {
                    if (read_from_file)
                        exl.AddElement(file);
                    function_node fn = convertion_data_and_alghoritms.select_function(exl,
                        SystemLibrary.SystemLibInitializer.read_procedure.SymbolInfo, loc);
                    if (read_from_file)
                        last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, file);
                    else
                        last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, loc);
                }
            }
            if (SystemLibrary.SystemLibInitializer.readln_procedure.Equal(si)) // readln(...)
            {
                if (!readln_string_file)
                {
                    if (last_call != null && convertion_data_and_alghoritms.statement_list_stack.size > 0)
                        convertion_data_and_alghoritms.statement_list_stack.top().statements.AddElement(last_call);
                    exl.clear();

                    if (read_from_file)
                        exl.AddElement(file);

                    function_node fn = convertion_data_and_alghoritms.select_function(exl,
                            SystemLibrary.SystemLibInitializer.readln_procedure.SymbolInfo, loc);
                    if (read_from_file)
                        last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, file);
                    else
                        last_call = convertion_data_and_alghoritms.create_simple_function_call(fn, loc);
                }
            }
            return last_call;
        }

        internal expression_node GetCurrentObjectReference(SymbolTable.Scope scope, definition_node dn, location loc)
        {
            if (WithSection)
                if (scope != null)
                    if (WithVariables.ContainsKey(scope))
                        return WithVariables[scope];
            		else
            if (WithTypes.Contains(scope))
            {
            	switch (dn.semantic_node_type)
            	{
            		case semantic_node_type.class_field : if (!(dn as class_field).IsStatic) AddError(new CanNotReferenceToNonStaticFieldWithType(dn as class_field,loc,null)); break;
            		case semantic_node_type.compiled_variable_definition : if (!(dn as compiled_variable_definition).IsStatic) AddError(new CanNotReferenceToNonStaticFieldWithType(dn as class_field,loc,null)); break;
            		case semantic_node_type.compiled_property_node:
            		case semantic_node_type.common_property_node : if ((dn as property_node).polymorphic_state != SemanticTree.polymorphic_state.ps_static) AddError(new CanNotReferenceToNonStaticPropertyWithType(dn as property_node,loc,null)); break;
            		case semantic_node_type.compiled_function_node:
            		case semantic_node_type.common_method_node:  if ((dn as function_node).polymorphic_state != SemanticTree.polymorphic_state.ps_static) AddError(new CanNotReferenceToNonStaticMethodWithType((dn as function_node).name,loc)); break;
            	}
            }
            if (context.inStaticArea())
            {
                AddError(new CanNotReferenceToNonStaticFromStatic(dn,loc));
                return null;
            }
            else
                return new this_node(context.converted_type, loc);
        }
        bool WithSection = false;
        Dictionary<SymbolTable.Scope, expression_node> WithVariables = new Dictionary<SymbolTable.Scope, expression_node>();
		Stack<SymbolTable.Scope> WithTypes = new Stack<SymbolTable.Scope>();
        
		private expression_node create_with_expression(expression_node expr)
		{
			location sl = expr.location;
			switch (expr.semantic_node_type)
			{
				case semantic_node_type.common_namespace_function_call:
				case semantic_node_type.common_in_function_function_call:
				case semantic_node_type.common_method_call:
				case semantic_node_type.common_static_method_call:
				case semantic_node_type.compiled_function_call:
				case semantic_node_type.compiled_constructor_call:
				case semantic_node_type.common_constructor_call:
				case semantic_node_type.compiled_static_method_call:
					return convertion_data_and_alghoritms.CreateVariableReference(context.add_var_definition(compiler_string_consts.GetTempVariableName(), sl, expr.type, expr), sl);
				case semantic_node_type.class_field_reference:
					(expr as class_field_reference).obj = create_with_expression((expr as class_field_reference).obj);
					return expr;
				case semantic_node_type.compiled_variable_reference:
					(expr as compiled_variable_reference).obj = create_with_expression((expr as compiled_variable_reference).obj);
					return expr;
				case semantic_node_type.simple_array_indexing:
					simple_array_indexing sar = expr as simple_array_indexing;
					sar.simple_arr_expr = create_with_expression(sar.simple_arr_expr);
					sar.ind_expr = create_with_expression(sar.ind_expr);
					return sar;
				case semantic_node_type.deref_node:
					(expr as dereference_node).deref_expr = create_with_expression((expr as dereference_node).deref_expr);
					return expr;
				case semantic_node_type.basic_function_call:
					return convertion_data_and_alghoritms.CreateVariableReference(context.add_var_definition(compiler_string_consts.GetTempVariableName(), sl, expr.type, expr), sl);
				case semantic_node_type.as_node:
					return convertion_data_and_alghoritms.CreateVariableReference(context.add_var_definition(compiler_string_consts.GetTempVariableName(), sl, expr.type, expr), sl);
			}
			return expr;
		}
		
		public override void visit(SyntaxTree.with_statement _with_statement)
        {
            WithSection = true;
            List<SymbolTable.Scope> Withs = new List<SymbolTable.Scope>();
            List<expression_node> VarRefs = new List<expression_node>();
            List<SymbolTable.Scope> WithTypesList = new List<SymbolTable.Scope>();
            statements_list stl = new statements_list(get_location(_with_statement.do_with), null, null);
            SymbolTable.WithScope WithScope = new SymbolTable.WithScope(SymbolTable, context.CurrentScope, null);
            convertion_data_and_alghoritms.statement_list_stack_push(stl, WithScope);
            foreach (SyntaxTree.expression s_expr in _with_statement.do_with.expressions)
            {
            	//expression_node expr = ret.visit(s_expr);
            	motivation mot = motivation_keeper.motivation;
            	motivation_keeper.set_motivation_to_except_semantic_node();
            	semantic_node sn = ret.visit(s_expr as SyntaxTree.syntax_tree_node);
            	motivation_keeper.reset();
            	expression_node expr = sn as expression_node;
            	if (expr is null_const_node)
                    AddError(expr.location, "NIL_IN_THIS_CONTEXT_NOT_ALLOWED");
                if (expr is typed_expression) expr = convert_typed_expression_to_function_call(expr as typed_expression);
                if (expr != null)
                {
                	variable_reference vr = expr as variable_reference;
                	if (expr.type == null)
                        AddError(get_location(s_expr), "UNEXPECTED_EXPRESSION_IN_WITH");
                	if (expr.type is ref_type_node)
                        AddError(get_location(s_expr), "UNEXPECTED_EXPRESSION_IN_WITH");
                	if (vr == null)
                	{
                    	location sl = get_location(s_expr);
                    	if (expr.type.type_special_kind != SemanticTree.type_special_kind.record)
                    	vr = convertion_data_and_alghoritms.CreateVariableReference(context.add_var_definition(compiler_string_consts.GetTempVariableName(), sl, expr.type, expr), sl);
                    	else
                    		expr = create_with_expression(expr);
                	}
                	if (vr != null && vr is class_field_reference && vr.type.type_special_kind == SemanticTree.type_special_kind.record)
                	{
                		vr = null;
                		expr = create_with_expression(expr);
                	}
                    if (vr != null)
                    {
                        VarRefs.Add(vr);
                        if (vr.type.Scope == null)
                        {
                            if (vr.type is compiled_type_node)
                                (vr.type as compiled_type_node).scope = new NetHelper.NetTypeScope((vr.type as compiled_type_node).compiled_type, SystemLibrary.SystemLibrary.symtab);
                            else
                                throw new CompilerInternalError("Cannot create With statement because variable reference scope == null");
                        }
                        //если не нужны локальные перекрытия, здесь нужна проверка
                        Withs.Add(vr.type.Scope);
                    }
                    else
                    {
                        VarRefs.Add(expr);
                        Withs.Add(expr.type.Scope);
                    }
                }
                else if (sn is type_node)
                {
                	type_node tn = sn as type_node;
                	Withs.Add(tn.Scope);
                	while (tn != null)
                	{
                		if (tn.Scope == null && tn is compiled_type_node)
                    	{
                    		(tn as compiled_type_node).init_scope();
                    	}
                		WithTypesList.Add(tn.Scope);
                		tn = tn.base_type;
                	}
                }
            }
            WithScope.WithScopes = Withs.ToArray();

            Dictionary<SymbolTable.Scope, expression_node> OldWithVariables = new Dictionary<SymbolTable.Scope, expression_node>();
            foreach (expression_node vr in VarRefs)
            {
                if (WithVariables.ContainsKey(vr.type.Scope))
                {
                    //перекрытие области
                    if (OldWithVariables.ContainsKey(vr.type.Scope))
                        //Это локальные перекрытия
                        OldWithVariables[vr.type.Scope] = WithVariables[vr.type.Scope];
                    else
                        OldWithVariables.Add(vr.type.Scope, WithVariables[vr.type.Scope]);
                    WithVariables[vr.type.Scope] = vr;
                }
                else
                {
                    //добавляем текущие with
                    WithVariables.Add(vr.type.Scope, vr);
                    
                }
                //ivan dobavil dlja bazovyh klassov
                type_node tn = vr.type.base_type;
                    while (tn != null)
                    {
                    	if (tn.Scope == null && tn is compiled_type_node)
                    	{
                    		(tn as compiled_type_node).init_scope();
                    	}
                    	if (tn.Scope != null)
                    	if (!WithVariables.ContainsKey(tn.Scope))
                    		WithVariables.Add(tn.Scope, vr);
                    	else
                    	{
                    		if (OldWithVariables.ContainsKey(tn.Scope))
                        	//Это локальные перекрытия
                        		OldWithVariables[tn.Scope] = WithVariables[tn.Scope];
                    		else
                        		OldWithVariables.Add(tn.Scope, WithVariables[tn.Scope]);
                    		WithVariables[tn.Scope] = vr;
                    	}
                    	tn = tn.base_type;
                    }
            }
			
            //Ivan dobavil dlja tipov
            
            foreach (SymbolTable.Scope sc in WithTypesList)
            {
            	if (!WithTypes.Contains(sc))
            	WithTypes.Push(sc);
            }
                
            stl.statements.AddElement(ret.visit(_with_statement.what_do));

            //восстанавливаем перекрытия
            foreach (SymbolTable.Scope sc in OldWithVariables.Keys)
                WithVariables[sc] = OldWithVariables[sc];
            //удаляем текущие with
            foreach (expression_node vr in VarRefs)
            {
                WithVariables.Remove(vr.type.Scope);
                type_node tn = vr.type.base_type;
                while (tn != null)
                {
                	if (tn.Scope != null && WithVariables.ContainsKey(tn.Scope))
                		WithVariables.Remove(tn.Scope);
                	tn = tn.base_type;
                }
            }
            foreach (SymbolTable.Scope sc in WithTypesList)
            {
            	WithTypes.Pop();
            }
            
            WithSection = WithVariables.Count > 0;

            convertion_data_and_alghoritms.statement_list_stack_pop();
            return_value(stl);
        }

        public override void visit(SyntaxTree.labeled_statement _labeled_statement)
        {
            convertion_data_and_alghoritms.check_node_parser_error(_labeled_statement.label_name);
            SymbolInfo si = context.CurrentScope.FindOnlyInScopeAndBlocks(_labeled_statement.label_name.name);
            if (_labeled_statement.to_statement is SyntaxTree.var_statement)
            {
                AddError(get_location(_labeled_statement.label_name), "LABELED_DECLARATION_NOT_ALLOWED");
            }
            if (si == null)
            {
                AddError(new UndefinedNameReference(_labeled_statement.label_name.name, get_location(_labeled_statement.label_name)));
            }
            label_node lab = si.sym_info as label_node;
            if (lab == null)
            {
                AddError(get_location(_labeled_statement.label_name), "IDENT_{0}_IS_NOT_LABEL", _labeled_statement.label_name.name);
            }
            if (lab.is_defined)
            {
                AddError(get_location(_labeled_statement.label_name), "LABEL_{0}_REDEFINED", _labeled_statement.label_name.name);
            }
            lab.is_defined = true;
            lab.comprehensive_code_block = context.block_stack.Peek();
            foreach (goto_statement gs in lab.goto_statements)
            {
                if (!context.check_can_goto(lab.comprehensive_code_block, gs.comprehensive_code_block))
                {
                    //AddError(gs.location, "BLOCKED_LABEL_{0}_GOTO", lab.name);
                }
            }
            convertion_data_and_alghoritms.check_node_parser_error(_labeled_statement.to_statement);
            statement_node snode = ret.visit((SyntaxTree.statement)_labeled_statement.to_statement);
            labeled_statement las = new labeled_statement(lab, snode, get_location(_labeled_statement));
            ret.return_value((statement_node)las);
        }

        public override void visit(SyntaxTree.goto_statement _goto_statement)
        {
            SymbolInfo si = context.CurrentScope.FindOnlyInScopeAndBlocks(_goto_statement.label.name);
            if (si == null)
            {
                AddError(new UndefinedNameReference(_goto_statement.label.name, get_location(_goto_statement.label)));
            }
            label_node lab = si.sym_info as label_node;
            if (lab == null)
            {
                AddError(get_location(_goto_statement.label), "IDENT_{0}_IS_NOT_LABEL", _goto_statement.label.name);
            }
            //if (lab.goto_blocked)
            //{
            //    throw new BlockedLabelGoto(_goto_statement.label.name, get_location(_goto_statement.label));
            //}
            goto_statement gs = new goto_statement(lab, get_location(_goto_statement));
            gs.comprehensive_code_block = context.block_stack.Peek();
            lab.goto_statements.Add(gs);
            if (lab.comprehensive_code_block != null)
            {
                if (!context.check_can_goto(lab.comprehensive_code_block, gs.comprehensive_code_block))
                {
                    //AddError(gs.location, "BLOCKED_LABEL_{0}_GOTO", lab.name);
                }
            }
            ret.return_value((statement_node)gs);
        }

        public override void visit(SyntaxTree.empty_statement _empty_statement)
        {
            return_value(new empty_statement(get_location(_empty_statement)));
        }

        private addressed_expression create_class_static_field_reference(type_node tn, definition_node dn,
            SyntaxTree.ident id_right)
        {
            switch (dn.semantic_node_type)
            {
                case semantic_node_type.class_field:
                    {
                        class_field cf = (class_field)dn;
                        if (cf.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToNonStaticFieldWithType(cf, get_location(id_right), tn));
                        }
                        static_class_field_reference scfr = new static_class_field_reference(cf, get_location(id_right));
                        return scfr;
                    }
                case semantic_node_type.compiled_variable_definition:
                    {
                        compiled_variable_definition cvd = (compiled_variable_definition)dn;
                        if (cvd.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToNonStaticFieldWithType(cvd, get_location(id_right), tn));
                        }
                        static_compiled_variable_reference scvr = new static_compiled_variable_reference(cvd, get_location(id_right));
                        return scvr;
                    }
            }
            throw new CompilerInternalError("Error in create static class field reference.");
        }

        private addressed_expression create_addressed_with_type_expression(type_node tn, SyntaxTree.ident id_right,
            SymbolInfo si_right)
        {
            definition_node dn = null;
            if (!internal_is_assign)
            dn = context.check_name_node_type(id_right.name, si_right, get_location(id_right),
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node);
            else
                dn = context.check_name_node_type(id_right.name, si_right, get_location(id_right),
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node,general_node_type.constant_definition);
            if (dn.general_node_type == general_node_type.constant_definition)
                AddError(get_location(id_right), "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
            switch (dn.general_node_type)
            {
                case general_node_type.variable_node:
                    {
                        return create_class_static_field_reference(tn, dn, id_right);
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        if (pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToNonStaticPropertyWithType(pn, get_location(id_right), tn));
                        }
                        static_property_reference pr = new static_property_reference(pn, get_location(id_right));
                        return pr;
                    }
                case general_node_type.event_node:
                    {
                        if (dn.semantic_node_type == semantic_node_type.compiled_event)
                        {
                            compiled_event ce = (compiled_event)dn;
                            if (!ce.is_static)
                            {
                                //throw new NonStaticAndStaticEvevnt();
                            }
                            static_event_reference ser = new static_event_reference(ce, get_location(id_right));
                            return ser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_event)
                        {
                            common_event ce = (common_event)dn;
                            if (!ce.is_static)
                            {
                                //throw new NonStaticAndStaticEvevnt();
                            }
                            static_event_reference ser = new static_event_reference(ce, get_location(id_right));
                            return ser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_namespace_event)
                        {
                            common_namespace_event cne = dn as common_namespace_event;
                            static_event_reference ser = new static_event_reference(cne, get_location(id_right));
                            return ser;
                        }
                        break;
                    }
            		default : return null;
            }
            throw new CompilerInternalError("Invalid right type expression");
        }

        private expressions_list convert_expression_list(List<SyntaxTree.expression> expr_list)
        {
            expressions_list exprs = new expressions_list();
            foreach (SyntaxTree.expression en in expr_list)
                exprs.AddElement(convert_strong(en));
            return exprs;
        }
        

        private base_function_call create_static_method_call_with_params(function_node fn, location loc, type_node tn, bool procedure_allowed, expressions_list parametrs)
        {
            base_function_call bfc = create_static_method_call(fn, loc, tn, procedure_allowed);
            bfc.parameters.AddRange(parametrs);
            return bfc;
        }
        private base_function_call create_static_method_call(function_node fn, location loc, type_node tn, bool procedure_allowed)
        {
            if ((!procedure_allowed) && (fn.return_value_type == null))
            {
                AddError(loc, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", fn.name);
            }
            if (fn.semantic_node_type == semantic_node_type.common_method_node)
            {
                common_method_node cmn = (common_method_node)fn;
                if (cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                {
                    //ssyy изменил
                    if (!cmn.is_constructor)
                    {
                        AddError(new CanNotCallNonStaticMethodWithClass(tn, loc, fn));
                    }
                    if (cmn.cont_type.IsAbstract)
                    	ErrorsList.Add(new SimpleSemanticError(loc, "ABSTRACT_CONSTRUCTOR_{0}_CALL", cmn.name));
                    common_constructor_call csmc2 = new common_constructor_call(cmn, loc);
                    return csmc2;
                    //if (cmn.pascal_associated_constructor==null)
                    //{
                    //	throw new CanNotCallNonStaticMethodWithClass(tn,loc,fn);
                    //}
                    //common_constructor_call csmc2=new common_constructor_call(cmn.pascal_associated_constructor,loc);
                    //return csmc2;
                    //\ssyy
                }
                common_static_method_call csmc = new common_static_method_call(cmn, loc);
                return csmc;
            }
            if (fn.semantic_node_type == semantic_node_type.compiled_function_node)
            {
                compiled_function_node cfn = (compiled_function_node)fn;
                if (cfn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(new CanNotCallNonStaticMethodWithClass(tn, loc, fn));
                }
                compiled_static_method_call csmc2 = new compiled_static_method_call(cfn, loc);
                return csmc2;
            }
            if (fn.semantic_node_type == semantic_node_type.compiled_constructor_node)
            {
                compiled_constructor_node ccn = (compiled_constructor_node)fn;
                compiled_constructor_call ccc = new compiled_constructor_call(ccn, loc);
                return ccc;
            }
            if (fn.semantic_node_type == semantic_node_type.basic_function_node)
            {
                return new basic_function_call(fn as basic_function_node, loc);
            }
            if (fn.semantic_node_type == semantic_node_type.common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null)
            {
            	return new common_namespace_function_call(fn as common_namespace_function_node,loc);
            }
            if (fn.semantic_node_type == semantic_node_type.indefinite_definition_node)
            {
                return new indefinite_function_call(fn, loc);
            }
            throw new CompilerInternalError("Invalid method kind");
        }

        private expression_node create_static_expression(type_node tn, SyntaxTree.ident id_right,
            SymbolInfo si_right)
        {
            definition_node dn = context.check_name_node_type(id_right.name, si_right, get_location(id_right), general_node_type.variable_node,
                general_node_type.function_node, general_node_type.property_node, general_node_type.constant_definition);
            switch (dn.general_node_type)
            {
                case general_node_type.variable_node:
                    {
                        return create_class_static_field_reference(tn, dn, id_right);
                    }
                case general_node_type.constant_definition:
                    {
                        constant_definition_node cdn = (constant_definition_node)dn;
                        return cdn.const_value.get_constant_copy(get_location(id_right));
                    }
                case general_node_type.function_node:
                    {
                        //TODO: Передается пустой список выражений.
                        /*
                        function_node fn=convertion_data_and_alghoritms.select_function(new expressions_list(),
                            si_right,get_location(id_right));
                        return create_static_method_call(fn,get_location(id_right),tn,false);
                        */
                       //if (!(si_right.sym_info is common_in_function_function_node))
                        if (dn.semantic_node_type == semantic_node_type.indefinite_definition_node)
                        {
                            return new indefinite_reference(dn as indefinite_definition_node, get_location(id_right));
                        }
                       return make_delegate_wrapper(null, si_right, get_location(id_right), true);
                      
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        if (pn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToNonStaticPropertyWithType(pn, get_location(id_right), tn));
                        }
                        function_node fn = pn.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(pn, get_location(id_right)));
                        }
                        location lloc = get_location(id_right);
                        check_property_no_params(pn, lloc);
                        return create_static_method_call(fn, lloc, tn, false);
                    }
            }
            throw new CompilerInternalError("Error in creating static expression.");
        }

        private void dot_node_as_type_ident(type_node tn, SyntaxTree.ident id_right, motivation mot)
        {
            SymbolInfo si_right = tn.find_in_type(id_right.name, context.CurrentScope);
            if (si_right == null)
            {
                AddError(new MemberIsNotDeclaredInType(id_right, get_location(id_right), tn));
            }
            switch (mot)
            {
                case motivation.address_reciving:
                    {
                        return_addressed_value(create_addressed_with_type_expression(tn, id_right, si_right));
                        return;
                    }
                case motivation.expression_evaluation:
                    {
                        return_value(create_static_expression(tn, id_right, si_right));
                        return;
                    }
                case motivation.semantic_node_reciving:
                    {
                        if (si_right.sym_info.general_node_type == general_node_type.type_node)
                            return_semantic_value(si_right.sym_info as type_node);
                        else
                            return_semantic_value(create_static_expression(tn, id_right, si_right));
                        return;
                    }
            }
            throw new CompilerInternalError("Invalid motivation");
        }

        private void dot_node_as_namespace_ident(namespace_node nn, SyntaxTree.ident id_right, motivation mot)
        {
            SymbolInfo si_right = nn.find(id_right.name);
            if (si_right == null)
            {
                AddError(new MemberIsNotDeclaredInNamespace(id_right, get_location(id_right), nn));
            }
            switch (mot)
            {
                case motivation.address_reciving:
                    {
                        return_addressed_value(ident_address_reciving(si_right, id_right));
                        return;
                    }
                case motivation.expression_evaluation:
                    {
                        return_value(ident_value_reciving(si_right, id_right));
                        return;
                    }
                case motivation.semantic_node_reciving:
                    {
                        return_semantic_value(ident_semantic_reciving(si_right, id_right));
                        return;
                    }
            }
            throw new CompilerInternalError("Unsupported motivation");
        }

        private void dot_node_as_unit_ident(common_unit_node cun, SyntaxTree.ident id_right, motivation mot)
        {
            dot_node_as_namespace_ident(cun.namespaces[0], id_right, mot);
        }

        private void dot_node_as_ident_dot_ident(SyntaxTree.ident id_left, SyntaxTree.ident id_right,
            motivation mot)
        {
            SymbolInfo si_left = context.find(id_left.name);
            if (si_left == null)
            {
                AddError(new UndefinedNameReference(id_left.name, get_location(id_left)));
            }
            definition_node dn = context.check_name_node_type(id_left.name, si_left, get_location(id_left),
                general_node_type.constant_definition, general_node_type.function_node,
                general_node_type.namespace_node, general_node_type.property_node,
                general_node_type.type_node, general_node_type.variable_node,
                general_node_type.unit_node);
            switch (dn.general_node_type)
            {
                case general_node_type.constant_definition:
                case general_node_type.function_node:
                case general_node_type.property_node:
                case general_node_type.variable_node:
                    {
                        expression_node ex_nd = ident_value_reciving(si_left, id_left);
                        dot_node_as_expression_dot_ident(ex_nd, id_right, mot);
                        return;
                    }
                case general_node_type.namespace_node:
                    {
                        //throw new CompilerInternalError("Unsupported now.");
                        namespace_node nn = dn as namespace_node;
                        dot_node_as_namespace_ident(nn, id_right, mot);
                        return;
                    }
                case general_node_type.type_node:
                    {
                        type_node tn = dn as type_node;
                        dot_node_as_type_ident(tn, id_right, mot);
                        return;
                    }
                case general_node_type.unit_node:
                    {
                        //throw new NotSupportedError(get_location(id_left));
                        dot_node_as_unit_ident((common_unit_node)dn, id_right, mot);
                        return;
                    }
            }
            return;
        }

        private expression_node convert_if_typed_expression_to_function_call(expression_node expr)
        {
            if (expr is typed_expression)
                return convert_typed_expression_to_function_call(expr as typed_expression);
            return expr;
        }

        internal expression_node convert_typed_expression_to_function_call(typed_expression te)
        {
            delegated_methods dm = te.type as delegated_methods;
            if (dm == null)
            {
                return te;
            }
            base_function_call bfc = dm.empty_param_method;
            if (bfc == null)
            {
                return te;
            }
            if (bfc.type == null) 
            	return te;
            function_node fn = bfc.simple_function_node;
            common_namespace_function_node cnfn = fn as common_namespace_function_node;
            if ((fn.parameters.Count == 1 || cnfn != null && fn.parameters.Count == 2 && cnfn.ConnectedToType != null)
                && (fn.parameters[fn.parameters.Count - 1].is_params || fn.parameters[fn.parameters.Count - 1].default_value != null))
            {
                fn = convertion_data_and_alghoritms.select_function(bfc.parameters, new SymbolInfo(fn), bfc.location);
                bfc = convertion_data_and_alghoritms.create_simple_function_call(fn, bfc.location, bfc.parameters.ToArray()) as base_function_call;
            }
            else
            {
                compiled_function_node cfn = fn as compiled_function_node;
                if ((fn.parameters.Count == 1 || cfn != null && fn.parameters.Count == 2 && cfn.ConnectedToType != null)
                    && (fn.parameters[fn.parameters.Count - 1].is_params || fn.parameters[fn.parameters.Count - 1].default_value != null))
                {
                    fn = convertion_data_and_alghoritms.select_function(bfc.parameters, new SymbolInfo(fn), bfc.location);
                    bfc = create_static_method_call_with_params(fn, bfc.location, fn.return_value_type, true, bfc.parameters);
                }
                else if (fn.parameters.Count == 1 && cfn != null && cfn.ConnectedToType != null)
                {
                    fn = convertion_data_and_alghoritms.select_function(bfc.parameters, new SymbolInfo(fn), bfc.location);
                    bfc = create_static_method_call_with_params(fn, bfc.location, fn.return_value_type, true, bfc.parameters);
                }
            }
            return bfc;
        }

        private void try_convert_typed_expression_to_function_call(ref expression_node en)
        {
            if (en.semantic_node_type == semantic_node_type.typed_expression)
            {
                en = convert_typed_expression_to_function_call((typed_expression)en);
            }
        }

        private addressed_expression create_class_field_reference(expression_node en, definition_node dn, SyntaxTree.ident id_right)
        {
            try_convert_typed_expression_to_function_call(ref en);
            if (dn.semantic_node_type == semantic_node_type.class_field)
            {
                class_field cf = (class_field)dn;
                if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(new CanNotReferanceToStaticFieldWithExpression(cf, get_location(id_right), en));
                }
                class_field_reference cfr = new class_field_reference(cf, en, get_location(id_right));
                //return_value(cfr);
                return cfr;
            }
            if (dn.semantic_node_type == semantic_node_type.compiled_variable_definition)
            {
                compiled_variable_definition cfd = (compiled_variable_definition)dn;
                if (cfd.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(new CanNotReferanceToStaticFieldWithExpression(cfd, get_location(id_right), en));
                }
                compiled_variable_reference cfr2 = new compiled_variable_reference(cfd, en, get_location(id_right));
                //return_value(cfr2);
                return cfr2;
            }
#if (DEBUG)
            throw new CompilerInternalError("Invalid variable kind");
#endif
            return null;
        }

        private base_function_call create_not_static_method_call(function_node fn, expression_node en, location loc, bool procedure_allowed)
        {
            try_convert_typed_expression_to_function_call(ref en);
            if ((!procedure_allowed) && (fn.return_value_type == null))
            {
                AddError(loc, "FUNCTION_EXPECTED_PROCEDURE_{0}_MEET", fn);
            }
            if (fn.semantic_node_type == semantic_node_type.common_method_node)
            {
                common_method_node cmn = (common_method_node)fn;
                if (cmn.is_constructor)
                {
                    AddError(loc, "CAN_NOT_CALL_CONSTRUCTOR_AS_PROCEDURE");
                }
                if (cmn.original_function != null && cmn.original_function is compiled_function_node && (cmn.original_function as compiled_function_node).ConnectedToType != null)
                {
                    common_static_method_call csmc = new common_static_method_call(cmn, loc);
                    return csmc;
                }
                if (cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(new CanNotCallStaticMethodWithExpression(en, fn));
                }
                
                common_method_call cmc = new common_method_call(cmn, en, loc);
                cmc.virtual_call = !inherited_ident_processing;
                return cmc;
            }
            if (fn.semantic_node_type == semantic_node_type.compiled_function_node)
            {
                compiled_function_node cfn = (compiled_function_node)fn;
                if (cfn.ConnectedToType != null)
                {
                    compiled_static_method_call csmc = new compiled_static_method_call(cfn, loc);
                    return csmc;
                }
                else
                {
                    if (cfn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                    {
                        if (!cfn.is_extension_method)
                            AddError(new CanNotCallStaticMethodWithExpression(en, fn));
                        else
                            return new compiled_static_method_call(cfn, loc);
                    }
                    compiled_function_call cfc = new compiled_function_call(cfn, en, loc);
                    cfc.virtual_call = !inherited_ident_processing;
                    return cfc;
                }
            }
            if (fn.semantic_node_type == semantic_node_type.compiled_constructor_node)
            {
                AddError(loc, "CAN_NOT_CALL_CONSTRUCTOR_AS_PROCEDURE");
            }
            if (fn.semantic_node_type == semantic_node_type.common_namespace_function_node && (fn as common_namespace_function_node).ConnectedToType != null)
            {
            	common_namespace_function_call cnfc = new common_namespace_function_call(fn as common_namespace_function_node,loc);
            	//cnfc.parameters.AddElement(en);
            	return cnfc;
            }
            if (fn.semantic_node_type == semantic_node_type.indefinite_definition_node)
            {
                indefinite_function_call ifc = new indefinite_function_call(fn, loc);
                return ifc;
            }
            throw new CompilerInternalError("Invalid method kind");
        }

        private expression_node expression_value_reciving(SyntaxTree.ident id_right, SymbolInfo si, expression_node en, bool expected_delegate)
        {
            definition_node dn = context.check_name_node_type(id_right.name, si, get_location(id_right), general_node_type.variable_node,
                general_node_type.function_node, general_node_type.property_node, general_node_type.constant_definition);
            switch (dn.general_node_type)
            {
                /*case general_node_type.constant_defenition:
                {
                    constant_definition_node cdn=(constant_definition_node)dn;
                    return cdn.const_value;
                }*/
                case general_node_type.variable_node:
                    {
                        return create_class_field_reference(en, dn, id_right);
                    }
                case general_node_type.function_node:
                    {
                        if (dn.semantic_node_type == semantic_node_type.indefinite_definition_node)
                        {
                            return new indefinite_reference(dn as indefinite_definition_node, get_location(id_right));
                        }
                        if (expected_delegate)
                            return make_delegate_wrapper(en, si, get_location(id_right), false);
                        else
                        {
                            function_node fn = convertion_data_and_alghoritms.select_function(new expressions_list(),
                                si, get_location(id_right));
                            return create_not_static_method_call(fn, en, get_location(id_right), false);
                        }
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToStaticPropertyWithExpression(pn, get_location(id_right), pn.comprehensive_type));
                        }
                        function_node fn = pn.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(pn, get_location(id_right)));
                        }
                        location lloc = get_location(id_right);
                        check_property_no_params(pn, lloc);
                        return create_not_static_method_call(fn, en, lloc, false);
                    }
                case general_node_type.constant_definition:
                    {
                        //throw new ConstMemberCannotBeAccessedWithAnInstanceReference((class_constant_definition)dn, get_location(id_right));
                        return ((constant_definition_node)dn).const_value;
                    }
            }
            throw new CompilerInternalError("Invalid class member");
        }
 
        private addressed_expression address_expression_reciving(SyntaxTree.ident id_right, SymbolInfo si,
            expression_node en)
        {
            if (si != null && si.sym_info != null &&
                si.sym_info.semantic_node_type == semantic_node_type.indefinite_definition_node)
            {
                return new indefinite_reference(si.sym_info as indefinite_definition_node, get_location(id_right));
            }
        	definition_node dn = null;
        	if (!internal_is_assign)
                dn = context.check_name_node_type(id_right.name, si, get_location(id_right),
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node);
            else
                dn = context.check_name_node_type(id_right.name, si, get_location(id_right),
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node, general_node_type.constant_definition);
            if (dn.general_node_type == general_node_type.constant_definition)
            {
                AddError(get_location(id_right), "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
            }
            switch (dn.general_node_type)
            {
                case general_node_type.variable_node:
                    {
                        return create_class_field_reference(en, dn, id_right);
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            AddError(new CanNotReferenceToStaticPropertyWithExpression(pn, get_location(id_right), pn.comprehensive_type));
                        }
                        try_convert_typed_expression_to_function_call(ref en);
                        non_static_property_reference pr = new non_static_property_reference(pn, en, get_location(id_right));
                        return pr;
                    }
                case general_node_type.event_node:
                    {
                        if (dn.semantic_node_type == semantic_node_type.compiled_event)
                        {
                            compiled_event ce = (compiled_event)dn;
                            if (ce.is_static)
                            {
                                //throw new NonStaticAndStaticEvevnt();
                            }
                            nonstatic_event_reference ser = new nonstatic_event_reference(en, ce, get_location(id_right));
                            return ser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_event)
                        {
                            common_event ce = (common_event)dn;
                            if (ce.is_static)
                            {
                                //throw new NonStaticAndStaticEvevnt();
                            }
                            nonstatic_event_reference ser = new nonstatic_event_reference(en, ce, get_location(id_right));
                            return ser;
                        }
                        break;
                    }
            	default : return null;
            }
            throw new CompilerInternalError("Undefined expression to address reciving");
        }

        private void dot_node_as_expression_dot_ident(expression_node en, SyntaxTree.ident id_right, motivation mot)
        {
            if (en is typed_expression)
                try_convert_typed_expression_to_function_call(ref en);
            SymbolInfo si = en.type.find_in_type(id_right.name, context.CurrentScope);
            if (si == null)
            {
                AddError(new MemberIsNotDeclaredInType(id_right, get_location(id_right), en.type));
            }

            try_convert_typed_expression_to_function_call(ref en);

            /*
            if (en.semantic_node_type == semantic_node_type.typed_expression)
            {
                type_node tn11=en.type;
                delegated_methods dm11=tn11 as delegated_methods;
                if (dm11!=null)
                {
                    en = dm11.empty_param_method;
                }
            }
            */

            switch (mot)
            {
                case motivation.address_reciving:
                    {
                        return_addressed_value(address_expression_reciving(id_right, si, en));
                        return;
                    }
                case motivation.expression_evaluation:
                    {
                        //en = expression_value_reciving(id_right, si, en, true);
            			//try_convert_typed_expression_to_function_call(ref en);
            			//return_value(en);
                        return_value(expression_value_reciving(id_right, si, en, true));
                        return;
                    }
                case motivation.semantic_node_reciving:
                    {
                        return_semantic_value(expression_value_reciving(id_right, si, en, true));
                        return;
                    }
            }
            throw new CompilerInternalError("Invalid motivation.");
        }

        private void dot_node_as_dot_node_dot_ident(SyntaxTree.dot_node left_dot, SyntaxTree.ident id_right,
            motivation mot)
        {
            semantic_node sn = convert_semantic_strong(left_dot);
            switch (sn.general_node_type)
            {
                case general_node_type.expression:
                    {
                        expression_node en = (expression_node)sn;
                        dot_node_as_expression_dot_ident(en, id_right, mot);
                        return;
                    }
                case general_node_type.namespace_node:
                    {
                        //throw new CompilerInternalError("Not supported");
                        /***************modified********************/
                        namespace_node nn = (namespace_node)sn;
                        dot_node_as_namespace_ident(nn, id_right, mot);
                        return;
                        /*******************************************/
                    }
                case general_node_type.type_node:
                    {
                        type_node tn = (type_node)sn;
                        dot_node_as_type_ident(tn, id_right, mot);
                        return;
                    }
                case general_node_type.unit_node:
                    {
                        throw new CompilerInternalError("Not supported");
                    }
            }
            throw new CompilerInternalError("Invalid left dot node kind");
        }

        public override void visit(SyntaxTree.dot_node _dot_node)
        {
            motivation mot = motivation_keeper.motivation;
            SyntaxTree.ident id_left = _dot_node.left as SyntaxTree.ident;
            SyntaxTree.ident id_right = _dot_node.right as SyntaxTree.ident;
#if (DEBUG)
            if (id_right == null)
            {
                throw new CompilerInternalError("Ident expected in left part of dot operator.");
            }
#endif

            //lroman
            if (_dot_node.left is closure_substituting_node)
            {
                var left = (closure_substituting_node)_dot_node.left;
                var dotNodeToVisit = new dot_node(left.substitution, _dot_node.right);
                visit(dotNodeToVisit);
                return;
            }

            if (id_left != null)
            {
                dot_node_as_ident_dot_ident(id_left, id_right, mot);
                return;
            }
            else
            {
                SyntaxTree.ident_with_templateparams iwt = _dot_node.left as SyntaxTree.ident_with_templateparams;
                if (iwt != null)
                {
                    type_node tn = ret.visit((SyntaxTree.syntax_tree_node)iwt) as type_node;
                    dot_node_as_type_ident(tn, id_right, mot);
                    return;
                }
                SyntaxTree.dot_node left_dot = _dot_node.left as SyntaxTree.dot_node;
                if (left_dot != null)
                {
                    dot_node_as_dot_node_dot_ident(left_dot, id_right, mot);
                    return;
                }
                expression_node en = convert_strong(_dot_node.left);
                dot_node_as_expression_dot_ident(en, id_right, mot);
                return;
            }
        }

        private SyntaxTree.ident ConvertOperatorNameToIdent(SyntaxTree.ident opname)
        {
            if (opname is SyntaxTree.operator_name_ident)
            {
                SyntaxTree.ident id = new SyntaxTree.ident(name_reflector.get_name((opname as SyntaxTree.operator_name_ident).operator_type));
                id.source_context = opname.source_context;
                return id;
            }
            return opname;
        }



        private void visit_method_name(SyntaxTree.method_name _method_name)
        {
            convertion_data_and_alghoritms.check_node_parser_error(_method_name);
            convertion_data_and_alghoritms.check_node_parser_error(_method_name.meth_name);
            common_type_node tp = null;
            definition_node def_temp = null;
            SyntaxTree.template_type_name ttn = null;
            /*SyntaxTree.operator_name_ident on = _method_name as SyntaxTree.operator_name;
            if (on != null)
            {
                _method_name.meth_name = new SyntaxTree.ident(name_reflector.get_name(on.operator_type.type));
            }
            */
            bool is_operator = _method_name.meth_name is SyntaxTree.operator_name_ident;
            SyntaxTree.Operators op = PascalABCCompiler.SyntaxTree.Operators.Undefined;
            if(is_operator)
                op=(_method_name.meth_name as SyntaxTree.operator_name_ident).operator_type;
            _method_name.meth_name = ConvertOperatorNameToIdent(_method_name.meth_name);

            if ((context.converting_block() != block_type.namespace_block) && (_method_name.class_name != null))
            {
                AddError(new OnlyProcedureNameAllowedInClassFunctionDefinition(_method_name.class_name.name,
                    get_location(_method_name.class_name)));
            }
            if (is_operator && op==SyntaxTree.Operators.AddressOf &&  SemanticRules.AddressOfOperatorNonOverloaded)
            {
                AddError(get_location(_method_name.meth_name), "NOT_OVERLOADED_OPERATOR");
            }
            bool common_extension_meth = false;
            if (_method_name.class_name != null)
            {
                convertion_data_and_alghoritms.check_node_parser_error(_method_name.class_name);
                location loc_type_name = get_location(_method_name.class_name);
                ttn = _method_name.class_name as SyntaxTree.template_type_name;
                int num_template_args = 0;
                if (ttn != null)
                {
                    num_template_args = ttn.template_args.idents.Count;
                }
                def_temp = context.enter_in_type_method(_method_name, _method_name.class_name.name, loc_type_name, num_template_args);
                tp = def_temp as common_type_node;
                template_class tc = def_temp as template_class;
                //SyntaxTree.template_type_name ttn = _method_name.class_name as SyntaxTree.template_type_name;
                if (ttn != null && !(def_temp is compiled_type_node))
                {
                    int count;
                    if (tp == null)
                    {
                        if (tc != null)
                        {
                            if (tc.is_synonym)
                            {
                                AddError(loc_type_name, "CAN_NOT_DEFINE_METHOD_OF_TEMPLATE_TYPE_SYNONYM");
                            }
                            SyntaxTree.class_definition cl_def = tc.type_dec.type_def as SyntaxTree.class_definition;
                            count = cl_def.template_args.idents.Count;
                            if (count != ttn.template_args.idents.Count)
                            {
                                AddError(get_location(ttn), "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", tc.name, count);
                            }
                            for (int i = 0; i < count; ++i)
                            {
                                if (string.Compare(cl_def.template_args.idents[i].name, ttn.template_args.idents[i].name, true) != 0)
                                {
                                    AddError(get_location(ttn.template_args.idents[i]), "PARAMETER_{0}_MUST_BE_NAMED_{1}", cl_def.template_args.idents[i].name, i + 1);
                                }
                            }
                            _method_name.class_name = new SyntaxTree.ident(ttn.name);
                            _method_name.class_name.source_context = ttn.source_context;
                            return;
                        }
                        AddError(get_location(ttn), "COULD_NOT_DEFINE_GENERIC_FUNCTION_OF_COMPILED_TYPE");
                    }
                    if (tp.generic_params == null)
                    {
                        AddError(get_location(ttn), "{0}_IS_NOT_TEMPLATE_CLASS", tp.PrintableName);
                    }
                    count = tp.generic_params.Count;
                    if (count != ttn.template_args.idents.Count)
                    {
                        AddError(get_location(ttn), "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", tp.PrintableName, count);
                    }
                    for (int i = 0; i < count; ++i)
                    {
                        if (string.Compare(tp.generic_params[i].name, ttn.template_args.idents[i].name, true) != 0)
                        {
                            AddError(get_location(ttn.template_args.idents[i]), "PARAMETER_{0}_MUST_BE_NAMED_{1}", tp.generic_params[i].name, i+1);
                        }
                    }
                }
                else
                {
                    if (tp != null && tp.is_generic_type_definition)
                    {
                        AddError(get_location(_method_name.class_name), "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", tp.PrintableName, tp.generic_params.Count);
                    }
                    if (tc != null)
                    {
                        AddError(get_location(_method_name.class_name), "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", tc.name, (tc.type_dec.type_def as SyntaxTree.class_definition).template_args.idents.Count);
                    }
                }
                //(ds)проверяем на компилированный тип
                if (!(def_temp is compiled_type_node))
                {
                    //(ssyy) проверяем на шаблон
                    if (tp == null)
                    {
                        return;
                    }
                    if (tp is compiled_generic_instance_type_node || tp is common_generic_instance_type_node)
                        AddError(get_location(_method_name), "EXTENSION_METHOD_FOR_GENERIC_INSTANCES");
                    SymbolInfo si = tp.scope.FindOnlyInScope(_method_name.meth_name.name);

                    if (tp.original_template == null && ((si == null && this._compiled_unit.namespaces.IndexOf(tp.comprehensive_namespace) == -1) || (si != null && this._compiled_unit.namespaces.IndexOf(tp.comprehensive_namespace) == -1)))
                    {
                        
                        if (def_temp is common_type_node)
                        {
                            common_extension_meth = true;
                            common_type_node ctn = def_temp as common_type_node;
                            /*if (ctn.IsInterface)
                                AddError(new ExtensionMethodForInterfaceNotAllowed(get_location(_method_name.class_name)));
                            else*/ if (ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.array_wrapper || 
                                ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type ||
                                ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.base_set_type ||
                                ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.binary_file ||
                                ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.text_file ||
                                ctn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.typed_file)
                                AddError(get_location(_method_name), "NO_METHOD_{0}_IN_CLASS_{1}", _method_name.meth_name.name, tp.PrintableName);
                        }
                        else
                            AddError(get_location(_method_name), "NO_METHOD_{0}_IN_CLASS_{1}", _method_name.meth_name.name, tp.PrintableName);
                    }
                }
                else if (def_temp as compiled_type_node == SystemLibrary.SystemLibrary.void_type)
                    AddError(get_location(_method_name.class_name), "VOID_NOT_VALID");
                //else if ((def_temp as compiled_type_node).IsInterface)
                //	AddError(new ExtensionMethodForInterfaceNotAllowed(get_location(_method_name.class_name)));
                //else
                //{
                //    throw new CanNotDefineMethodOfCompiledType(get_location(_method_name));
                //}
            }
            else
            {
                if (is_operator && context.converted_type == null && context.converted_template_type == null)
                {
                    AddError(get_location(_method_name), "OVERLOADED_OPERATOR_MUST_BE_STATIC_FUNCTION");
                }
            }
            convertion_data_and_alghoritms.check_node_parser_error(_method_name.meth_name);
            location loc_name = get_location(_method_name.meth_name);
            context.extension_method = common_extension_meth;
            if (LambdaHelper.IsLambdaName(_method_name.meth_name) && lambdaProcessingState != LambdaProcessingState.ClosuresProcessingVisitGeneratedClassesPhase)  //lroman//
            {
                if (lambdaProcessingState == LambdaProcessingState.None &&
                    context.converting_block() == block_type.namespace_block)
                {
                    context.create_function(_method_name.meth_name.name, loc_name);
                }
                else
                {
                    var addSymbol = (lambdaProcessingState == LambdaProcessingState.TypeInferencePhase &&
                                 LambdaHelper.IsAuxiliaryLambdaName(_method_name.meth_name)) || lambdaProcessingState == LambdaProcessingState.FinishPhase;
                    context.create_lambda_function(_method_name.meth_name.name, loc_name, addSymbol, context.CurrentScope);
                }
            }
            else
            {
                context.create_function(_method_name.meth_name.name, loc_name);
            }
            context.extension_method = false;
            context.top_function.IsOperator = is_operator;
            if (ttn != null && def_temp is compiled_type_node)
            {
                visit_generic_params(context.top_function, ttn.template_args.idents);
            }
            //context.top_function.IsOperator = _method_name.meth_name is SyntaxTree.operator_name_ident;

        }

        public override void visit(SyntaxTree.method_name _method_name)
        {
            visit_method_name(_method_name);
        }

        private void is_case_variant_intersection_with_another(switch_node sn, int_const_node defined_const, location curloc)
        {
            foreach (case_variant_node cv in sn.case_variants)
            {
                foreach (int_const_node cn in cv.case_constants)
                {
                    if (cn.constant_value == defined_const.constant_value)
                    {
                        AddError(curloc, "CASE_CONSTANT_VARIANT_COINCIDE_WITH_ANOTHER");
                    }
                }
                foreach (case_range_node cr in cv.case_ranges)
                {
                    if ((defined_const.constant_value <= cr.high_bound.constant_value) &&
                        (defined_const.constant_value >= cr.lower_bound.constant_value))
                    {
                        AddError(curloc, "CASE_CONSTANT_VARIANT_COINCIDE_WITH_DIAPASON");
                    }
                }
            }
        }

        private void is_case_range_intersection_with_another(switch_node sn, case_range_node defined_range, location curloc)
        {
            foreach (case_variant_node cv in sn.case_variants)
            {
                foreach (int_const_node icn in cv.case_constants)
                {
                    if ((icn.constant_value >= defined_range.lower_bound.constant_value) &&
                        (icn.constant_value <= defined_range.high_bound.constant_value))
                    {
                        AddError(curloc, "CASE_CONSTANT_VARIANT_COINCIDE_WITH_DIAPASON");
                    }
                }
                foreach (case_range_node cr in cv.case_ranges)
                {
                    if ((cr.lower_bound.constant_value <= defined_range.high_bound.constant_value) &&
                        (cr.lower_bound.constant_value >= defined_range.lower_bound.constant_value))
                    {
                        AddError(curloc, "CASE_DIAPASON_VARIANT_INTERSECTION");
                    }
                    if ((cr.high_bound.constant_value <= defined_range.high_bound.constant_value) &&
                        (cr.high_bound.constant_value >= defined_range.lower_bound.constant_value))
                    {
                        AddError(curloc, "CASE_DIAPASON_VARIANT_INTERSECTION");
                    }
                }
            }
        }

        private string_const_node convert_string_const_to_switch(expression_node switch_expr, location loc)
        {
            if (switch_expr is string_const_node)
                return switch_expr as string_const_node;
            string_const_node scn = null;
            if (switch_expr is static_compiled_variable_reference && (switch_expr as static_compiled_variable_reference).var.compiled_field.IsLiteral)
                scn = new string_const_node((string)(switch_expr as static_compiled_variable_reference).var.compiled_field.GetRawConstantValue(), loc);
            if (scn == null)
            {
                AddError(loc, "CONSTANT_EXPRESSION_EXPECTED");
            }
            return scn;
        }

        private int_const_node convert_const_to_switch(expression_node switch_expr,
            ordinal_type_interface oti, type_node case_expr_type, location loc)
        {
            convertion_data_and_alghoritms.convert_type(switch_expr, case_expr_type, loc);
            if (switch_expr is int_const_node)
                return switch_expr as int_const_node;
            //switch_expr = convertion_data_and_alghoritms.create_simple_function_call(oti.value_to_int,
            //    loc, switch_expr);
            int_const_node icn = null;//switch_expr as constant_node;

            if (switch_expr is byte_const_node)
                icn = new int_const_node((switch_expr as byte_const_node).constant_value, loc);
            else if (switch_expr is sbyte_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as sbyte_const_node).constant_value), loc);
            else if (switch_expr is short_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as short_const_node).constant_value), loc);
            else if (switch_expr is ushort_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as ushort_const_node).constant_value), loc);
            /*else if (switch_expr is uint_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as uint_const_node).constant_value),loc);
            else if (switch_expr is long_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as long_const_node).constant_value),loc);
            else if (switch_expr is ulong_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as ulong_const_node).constant_value),loc);*/
            else if (switch_expr is bool_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as bool_const_node).constant_value), loc);
            else if (switch_expr is char_const_node)
                icn = new int_const_node(Convert.ToInt32((switch_expr as char_const_node).constant_value), loc);
            else if (switch_expr is enum_const_node)
                icn = new int_const_node((switch_expr as enum_const_node).constant_value, loc);
            else if (switch_expr is static_compiled_variable_reference && (switch_expr as static_compiled_variable_reference).var.compiled_field.IsLiteral)
                icn = new int_const_node((int)(switch_expr as static_compiled_variable_reference).var.compiled_field.GetRawConstantValue(), loc);

            //учти здесь модет быть и long!
            //-DarkStar Add
            if (icn == null)
            {
                AddError(loc, "CONSTANT_EXPRESSION_EXPECTED");
            }
            return icn;
        }

        //TODO: Добавить приведение типов.
        public override void visit(SyntaxTree.case_node _case_node)
        {
            convertion_data_and_alghoritms.check_node_parser_error(_case_node);
            switch_node sn = new switch_node(get_location(_case_node));
            expression_node en = convert_strong(_case_node.param);
            if (en is typed_expression) en = convert_typed_expression_to_function_call(en as typed_expression);
            type_node case_expr_type = en.type;

            internal_interface ii = en.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null && en.type != SystemLibrary.SystemLibrary.string_type)
            {
                AddError(new OrdinalOrStringTypeExpected(en.location));
            }
            if (ii != null)
            {
                ordinal_type_interface oti = (ordinal_type_interface)ii;
                en = convertion_data_and_alghoritms.create_simple_function_call(oti.value_to_int, en.location, en);

                sn.condition = en;

                foreach (SyntaxTree.case_variant cv in _case_node.conditions.variants)
                {
                    convertion_data_and_alghoritms.check_node_parser_error(cv);
                    case_variant_node sem_cv = new case_variant_node(get_location(cv));
                    sn.case_variants.AddElement(sem_cv);
                    foreach (SyntaxTree.expression expr in cv.conditions.expressions)
                    {
                        convertion_data_and_alghoritms.check_node_parser_error(expr);
                        SyntaxTree.diapason_expr diap = expr as SyntaxTree.diapason_expr;
                        if (diap == null)
                        {
                            expression_node cn = convert_strong(expr);
                            int_const_node icn = convert_const_to_switch(cn, oti, case_expr_type, get_location(expr));
                            is_case_variant_intersection_with_another(sn, icn, get_location(expr));
                            sem_cv.case_constants.AddElement(icn);
                        }
                        else
                        {
                            convertion_data_and_alghoritms.check_node_parser_error(diap.left);
                            expression_node left = convert_strong(diap.left);
                            /*
                            constant_node cons_left = left as constant_node;
                            if (cons_left==null)
                            {
                                throw new ConstantExpressionExpected(get_location(diap.left));
                            }
                            */
                            int_const_node icn_left = convert_const_to_switch(left, oti, case_expr_type, get_location(diap.left));

                            convertion_data_and_alghoritms.check_node_parser_error(diap.right);
                            expression_node right = convert_strong(diap.right);
                            /*
                            constant_node cons_right = right as constant_node;
                            if (cons_right == null)
                            {
                                throw new ConstantExpressionExpected(get_location(diap.right));
                            }
                            */
                            int_const_node icn_right = convert_const_to_switch(right, oti, case_expr_type, get_location(diap.right));
                            //ssyy
                            if (icn_left.constant_value > icn_right.constant_value)
                            {
                                AddError(get_location(diap.right), "LEFT_RANGE_GREATER_THEN_RIGHT");
                            }
                            //\ssyy
                            case_range_node crn = new case_range_node(icn_left, icn_right, get_location(diap));
                            is_case_range_intersection_with_another(sn, crn, get_location(expr));
                            sem_cv.case_ranges.AddElement(crn);
                        }
                    }
                    context.enter_code_block_with_bind();
                    statement_node stmn = convert_strong(cv.exec_if_true);
                    context.leave_code_block();
                    sem_cv.case_statement = stmn;
                    //sn.case_variants.AddElement(sem_cv);
                }
                context.enter_code_block_with_bind();
                statement_node else_statement = convert_weak(_case_node.else_statement);
                context.leave_code_block();
                sn.default_statement = else_statement;
                return_value(sn);
            }
            else
            {
                if_node main_ifn = null;
                if_node ifn = null;
                compiled_function_node str_eq_meth = SystemLibrary.SystemLibrary.string_type.find_in_type("=").sym_info as compiled_function_node;
                Dictionary<string, string> case_constants = new Dictionary<string, string>();
                foreach (SyntaxTree.case_variant cv in _case_node.conditions.variants)
                {
                    convertion_data_and_alghoritms.check_node_parser_error(cv);
                    location loc = get_location(cv);
                    expression_node eq_node = null;
                    List<compiled_static_method_call> eq_calls = new List<compiled_static_method_call>();
                    foreach (SyntaxTree.expression expr in cv.conditions.expressions)
                    {
                        expression_node cn = convert_strong(expr);
                        if (cn.type != SystemLibrary.SystemLibrary.string_type)
                            AddError(new CanNotConvertTypes(cn, cn.type, SystemLibrary.SystemLibrary.string_type, cn.location));
                        string_const_node scn = convert_string_const_to_switch(cn, cn.location);
                        if (!case_constants.ContainsKey(scn.constant_value))
                            case_constants.Add(scn.constant_value, scn.constant_value);
                        else
                            AddError(cn.location, "CASE_CONSTANT_VARIANT_COINCIDE_WITH_ANOTHER");
                        compiled_static_method_call eq_call = new compiled_static_method_call(str_eq_meth, cn.location);
                        eq_call.parameters.AddElement(en);
                        eq_call.parameters.AddElement(cn);
                        eq_calls.Add(eq_call);
                    }
                    foreach (compiled_static_method_call eq_call in eq_calls)
                    {
                        if (eq_node == null)
                            eq_node = eq_call;
                        else
                            eq_node = new basic_function_call(SystemLibrary.SystemLibrary.bool_or as basic_function_node, null, eq_node, eq_call);
                    }
                    context.enter_code_block_with_bind();
                    statement_node stmn = convert_strong(cv.exec_if_true);
                    context.leave_code_block();
                    if (ifn != null)
                    {
                        ifn.else_body = new if_node(eq_node, stmn, null, loc);
                        ifn = ifn.else_body as if_node;
                    }
                    else
                        ifn = new if_node(eq_node, stmn, null, loc);
                    if (main_ifn == null)
                        main_ifn = ifn;
                }
                context.enter_code_block_with_bind();
                statement_node else_statement = convert_weak(_case_node.else_statement);
                context.leave_code_block();
                if (ifn == null)
                {
                    ifn = new if_node(en, new statements_list(null), null, get_location(_case_node));
                    main_ifn = ifn;
                }
                ifn.else_body = else_statement;
                return_value(main_ifn);
            }
        }

        public override void visit(SyntaxTree.case_variant _case_variant)
        {
            throw new NotSupportedError(get_location(_case_variant));
        }

        public override void visit(SyntaxTree.get_address _get_address)
        {
            expression_node exp = convert_strong(_get_address.address_of);
            if (!exp.is_addressed)
            {
                AddError(exp.location, "CAN_NOT_GET_ADDRESS_FROM_EXPRESSION");
            }
            expression_node res = new get_addr_node(exp, get_location(_get_address));
            return_value(res);
        }

        public override void visit(SyntaxTree.hex_constant _hex_constant)
        {
            expression_node en = null;
            if (_hex_constant.val <= Int32.MaxValue && _hex_constant.val >= Int32.MinValue)
                en = new int_const_node((int)_hex_constant.val, get_location(_hex_constant));
            else
                en = new long_const_node(_hex_constant.val, get_location(_hex_constant));
            return_value(en);
        }

        private System.Collections.Generic.List<compiler_directive> ConvertDirectives(SyntaxTree.compilation_unit cu)
        {
            System.Collections.Generic.List<compiler_directive> list = new System.Collections.Generic.List<compiler_directive>();
            foreach (SyntaxTree.compiler_directive sncd in cu.compiler_directives)
            {
            	list.Add(new compiler_directive(sncd.Name.text, sncd.Directive!=null?sncd.Directive.text:"", get_location(sncd)));
            }
            return list;
        }

        /*private void PrepareDirectives(System.Collections.Generic.List<compiler_directive> directives)
        {
            foreach (compiler_directive cd in directives)
            {
                if (cd.name.ToLower() == compiler_string_consts.compiler_directive_reference)
                { 

                }
            }
        }*/


        //TODO: Слить как нибудь два следующих метода.
        public override void visit(SyntaxTree.program_module _program_module)
        {
            //MikhailoMMX Инициализация OpenMP
            OpenMP.InitOpenMP(_program_module.compiler_directives, this, _program_module);
            //\MikhailoMMX

            //current_document=new document(_program_module.file_name);
            string namespace_name = "";
            location loc = get_location(_program_module);
            if (_program_module.program_name != null)
            {
                namespace_name = _program_module.program_name.prog_name.name;
                loc = get_location(_program_module.program_name.prog_name);
                //if (namespace_name.ToLower() != System.IO.Path.GetFileNameWithoutExtension(_program_module.file_name).ToLower())
                //    throw new ProgramNameMustBeEqualProgramFileName(loc);
            }


            _compiled_unit = new common_unit_node();

            _compiled_unit.compiler_directives = ConvertDirectives(_program_module);
            //PrepareDirectives(_compiled_unit.compiler_directives);

            //using_list.AddRange(interface_using_list);
            //weak_node_test_and_visit(_program_module.using_namespaces);
            //weak_node_test_and_visit(_program_module.used_units);

            //compiled_main_unit=new unit_node();
            //SymbolTable.Scope[] used_units=new SymbolTable.Scope[used_assemblyes.Count+1];


            SymbolTable.Scope[] used_units = build_referenced_units(referenced_units,true);

            _compiled_unit.scope = convertion_data_and_alghoritms.symbol_table.CreateUnitInterfaceScope(used_units);

            common_namespace_node cnsn = context.create_namespace(namespace_name, _compiled_unit, _compiled_unit.scope, loc);
            cnsn.is_main = true;
            //compiled_program=new program_node();
            //compiled_program.units.Add(compiled_main_unit);

            reset_for_interface();

            UpdateUnitDefinitionItemForUnit(_compiled_unit);

            //_program_module.program_block.visit(this);
            hard_node_test_and_visit(_program_module.program_block);
            context.check_labels(context.converted_namespace.labels);

            //TODO: Доделать.
            common_namespace_function_node main_function = new common_namespace_function_node(compiler_string_consts.temp_main_function_name,
                null, null, cnsn, null);
            main_function.function_code = context.code;
            context.apply_special_local_vars(main_function);
            cnsn.functions.AddElement(main_function);

            context.leave_block();

            _compiled_unit.main_function = main_function;
            //cnsn.SetNamespaceName(System.IO.Path.GetFileNameWithoutExtension(_program_module.file_name));
        }

        private void reset_for_interface()
        {
            body_exists = false;
        }
		
        private bool check_for_dll_entry_module(List<compiler_directive> directives)
        {
        	foreach (compiler_directive cd in directives)
        	{
        		if (string.Compare(cd.name,"apptype",true)==0 && string.Compare(cd.directive,"dll",true)==0)
        			return true;
        	}
        	return false;
        }
        
        

        internal bool _is_interface_part = false;
        public override void visit(SyntaxTree.unit_module _unit_module)
        {
            //MikhailoMMX Инициализация OpenMP
            OpenMP.InitOpenMP(_unit_module.compiler_directives, this, _unit_module);
            //\MikhailoMMX
            
            //current_document=new document(_unit_module.file_name);
            string namespace_name = "";
            _is_interface_part = true;
            location loc = null;
            if (_unit_module.unit_name != null)
            {
                namespace_name = _unit_module.unit_name.idunit_name.name;
                loc = get_location(_unit_module.unit_name.idunit_name);
                if (namespace_name.ToLower() != System.IO.Path.GetFileNameWithoutExtension(_unit_module.file_name).ToLower())
                    AddError(loc, "UNIT_NAME_MUST_BE_EQUAL_UNIT_FILE_NAME");
            }


            _compiled_unit = new common_unit_node(_unit_module.unit_name.idunit_name.name);
            assign_doc_info(_compiled_unit,_unit_module.unit_name);
            _compiled_unit.compiler_directives = ConvertDirectives(_unit_module);
            //PrepareDirectives(_compiled_unit.compiler_directives);

            //using_list.AddRange(interface_using_list);
            //weak_node_test_and_visit(_unit_module.interface_part.using_namespaces);

            //weak_node_test_and_visit(_program_module.used_units);

            //compiled_main_unit=new unit_node();
            //SymbolTable.Scope[] used_units=new SymbolTable.Scope[used_assemblyes.Count+1];

            SymbolTable.Scope[] used_units = build_referenced_units(referenced_units,true);

            _compiled_unit.scope = convertion_data_and_alghoritms.symbol_table.CreateUnitInterfaceScope(used_units);

            common_namespace_node cnsn = context.create_namespace(namespace_name, _compiled_unit, _compiled_unit.scope, loc);

            _compiled_unit.add_unit_name_to_namespace();

            if (check_for_dll_entry_module(_compiled_unit.compiler_directives))
            {
            	cnsn.is_main = true;
            }
            UpdateUnitDefinitionItemForUnit(_compiled_unit);


            //cnsn.scope=_compiled_unit.scope;
            reset_for_interface();
            hard_node_test_and_visit(_unit_module.interface_part);

            //compiled_program=new program_node();
            //compiled_program.units.Add(compiled_main_unit);

            //_program_module.program_block.visit(this);
            if (_unit_module.attributes != null)
                make_attributes_for_declaration(_unit_module, _compiled_unit);
            context.leave_interface_part();
            _is_interface_part = false;
           
        }

        public override void visit(SyntaxTree.compilation_unit _compilation_unit)
        {
            if (_compilation_unit.source_context == null)
                throw new NotSupportedError();
            throw new NotSupportedError(get_location(_compilation_unit));
        }

        public override void visit(SyntaxTree.program_body _program_body)
        {
            throw new NotSupportedError(get_location(_program_body));
        }

        public override void visit(SyntaxTree.uses_list _uses_list)
        {
            throw new NotSupportedError(get_location(_uses_list));
        }

        public override void visit(SyntaxTree.uses_unit_in _uses_unit_in)
        {
            throw new NotSupportedError(get_location(_uses_unit_in));
        }

        /*public override void visit(SyntaxTree.unit_or_namespace node)
        {
            throw new NotSupportedError(get_location(node));
        }*/

        public override void visit(SyntaxTree.unit_name _unit_name)
        {
            throw new NotSupportedError(get_location(_unit_name));
        }

        public override void visit(SyntaxTree.consts_definitions_list _consts_definitions_list)
        {
            //ssyy
            if (context.converting_block() == block_type.type_block &&
                context.converted_type.IsInterface)
            {
                AddError(get_location(_consts_definitions_list), "INVALID_INTERFACE_MEMBER");
            }
            //\ssyy
            // TODO:  Add semantic_tree_generator.SyntaxTree.IVisitor.visit implementation
            /***********************modified*********************/
            foreach (SyntaxTree.const_definition cnst in _consts_definitions_list.const_defs)
                cnst.visit(this);
            /****************************************************/
        }

        public override void visit(SyntaxTree.const_definition _const_definition)
        {
            throw new NotSupportedError(get_location(_const_definition));
        }

        private bool IsBoundedArray(type_node tn)
        {
            return tn.get_internal_interface(internal_interface_kind.bounded_array_interface) != null;
        }

        private bool IsUnsizedArray(type_node tn)
        {
            return tn.get_internal_interface(internal_interface_kind.unsized_array_interface) != null;
        }
		
        private type_node const_def_type=null;
        private bool is_typed_const_def=false;
        
        public override void visit(SyntaxTree.typed_const_definition _typed_const_definition)
        {
            location cons_loc = get_location(_typed_const_definition.const_value);
            constant_definition_node cdn = context.add_const_definition(_typed_const_definition.const_name.name, get_location(_typed_const_definition.const_name));
            type_node tn = convert_strong(_typed_const_definition.const_type);
            assign_doc_info(cdn,_typed_const_definition);
            //if (tn == SystemLibrary.SystemLibrary.void_type)
            //    AddError(new VoidNotValid(get_location(_typed_const_definition.const_type)));
            check_for_type_allowed(tn,get_location(_typed_const_definition.const_type));
            if (context.converted_type != null && context.converted_func_stack.Empty)
            	if (!constant_in_class_valid(tn))//proverka na primitivnost konstanty v klasse
                    AddError(get_location(_typed_const_definition), "CLASS_CONSTANT_CAN_HAVE_ONLY_PRIMITIVE_VALUE");
            if (tn is common_type_node)
            {
                if (!SemanticRules.DefineMethodsInConstantRecord)
                    CheckConstantRecordNotBeContainsMethods(tn as common_type_node, get_location(_typed_const_definition.const_type));
                if (!SemanticRules.InheritanceConstantRecord)
                	//mnozhestvo zdes iskljuchenie
                    if (tn.base_type is common_type_node && tn.base_type.type_special_kind != SemanticTree.type_special_kind.base_set_type/*tn.base_type != SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node*/)
                        AddError(get_location(_typed_const_definition.const_type), "CONSTANT_RECORD_CAN_NOT_BE_INHERITANCE");
            }
            const_def_type = tn.element_type;
            is_typed_const_def = true;
            _typed_const_definition.const_value = get_possible_array_const(_typed_const_definition.const_value,tn);
            cdn.const_value = convert_strong_to_constant_node(_typed_const_definition.const_value, tn);
            const_def_type = null;
            is_typed_const_def = false;
            if (cdn is class_constant_definition && cdn.constant_value.value == null)
                AddError(new NotSupportedError(cons_loc));
        }

        private void CheckConstantRecordNotBeContainsMethods(common_type_node ctn, location loc)
        {
            if (ctn.is_value)
            {
                if (ctn.methods.Count > 0)
                {
                	for (int i=0; i<ctn.methods.Count; i++)
                	if (ctn.methods[i].loc != null)
                        AddError(loc, "CONSTANT_RECORD_CAN_NOT_CONTAINS_METHODS");
                }
                    //if (!(ctn.methods.Count == 2 && ctn.methods[0] is common_method_node && (ctn.methods[0] as common_method_node).is_constructor && ctn.methods[0].loc == null
                	//      && ctn.methods[1] is common_method_node && ctn.methods[1].loc == null))
                    //    throw new ConstantRecordCanNotContainsMethods(loc);
                if (ctn.base_type is common_type_node)
                    CheckConstantRecordNotBeContainsMethods(ctn.base_type as common_type_node, loc);
            }
            else
            {
                if (IsBoundedArray(ctn) && ctn.element_type is common_type_node)
                    CheckConstantRecordNotBeContainsMethods(ctn.element_type as common_type_node, loc);
            }
        }

        private bool constant_in_class_valid(type_node tn)
        {
        	if (tn is common_type_node)
        	if (tn.IsEnum) return true;
        	else return false;
        	if (tn is compiled_type_node)
        	{
        		return true;
        	}
        	return false;
        }

        public override void visit(SyntaxTree.simple_const_definition _simple_const_definition)
        {
            constant_definition_node cdn = context.add_const_definition(_simple_const_definition.const_name.name, get_location(_simple_const_definition.const_name));
            cdn.const_value = convert_strong_to_constant_node(_simple_const_definition.const_value);
            cdn.const_value.SetType(DeduceType(cdn.const_value.type, cdn.const_value.location));
            assign_doc_info(cdn,_simple_const_definition);
            if (context.converted_type != null && context.converted_func_stack.Empty)
            	if (!constant_in_class_valid(cdn.const_value.type))
                    AddError(get_location(_simple_const_definition), "CLASS_CONSTANT_CAN_HAVE_ONLY_PRIMITIVE_VALUE");
        }

        public override void visit(SyntaxTree.type_declarations _type_declarations)
        {
            EnterTypeDeclarationsSection();
            foreach (SyntaxTree.type_declaration td in _type_declarations.types_decl)
            {
                hard_node_test_and_visit(td);
            }
            LeaveTypeDeclarationsSection();
        }

        private common_type_node convert_function_type(SyntaxTree.function_header func_header, location loc, string type_name, common_type_node del_type)
        {
            return convert_function_type(func_header.parameters, func_header.return_type, loc, type_name, del_type);
        }

        private common_type_node convert_function_type(SyntaxTree.procedure_header proc_header, location loc, string type_name, common_type_node del_type)
        {
            return convert_function_type(proc_header.parameters, null, loc, type_name, del_type);
        }

        private common_type_node convert_function_type(SyntaxTree.formal_parameters syn_parametres, SyntaxTree.type_definition return_type,
            location loc, string type_name, common_type_node del_type)
        {
            //TODO: Добавить аттрибуты функции. Или не надо?
            //TODO: Сообщать о том что class keyword и of object игнорируется.
            parameter_list parameters = new parameter_list();
            bool params_met=false;
            if (syn_parametres != null)
            {
                parameter_list temp_params = new parameter_list();
                foreach (SyntaxTree.typed_parameters tpars in syn_parametres.params_list)
                {
                    temp_params.clear();
                    SemanticTree.parameter_type pt = SemanticTree.parameter_type.value;
                    concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
                    switch (tpars.param_kind)
                    {
                        //TODO: Params parameter.
                        case SyntaxTree.parametr_kind.var_parametr:
                            {
                                pt = SemanticTree.parameter_type.var;
                                cpt = concrete_parameter_type.cpt_var;
                                break;
                            }
                        case PascalABCCompiler.SyntaxTree.parametr_kind.const_parametr:
                            {
                                pt = SemanticTree.parameter_type.value;
                                cpt = concrete_parameter_type.cpt_const;
                                break;
                            }
                        case PascalABCCompiler.SyntaxTree.parametr_kind.out_parametr:
                            {
                                pt = SemanticTree.parameter_type.var;
                                cpt = concrete_parameter_type.cpt_var;
                                break;
                            }
                    }
                    foreach (SyntaxTree.ident id in tpars.idents.idents)
                    {
                        common_parameter cp = new common_parameter(id.name, pt, null, cpt, get_location(id));
                        parameters.AddElement(cp);
                        if (tpars.param_kind == SyntaxTree.parametr_kind.params_parametr)
                        {
                        	cp.intrenal_is_params = true;
                        }
                        temp_params.AddElement(cp);
                    }
                    if (tpars.param_kind == SyntaxTree.parametr_kind.params_parametr)
            		{
                		if (tpars.idents.idents.Count > 1)
                		{
                            AddError(get_location(tpars.idents.idents[1]), "ONLY_ONE_PARAMS_PARAMETER_ALLOWED");
                		}
            		}

            		SyntaxTree.array_type arr = tpars.vars_type as SyntaxTree.array_type;
            		if (tpars.vars_type is SyntaxTree.class_definition || tpars.vars_type is SyntaxTree.enum_type_definition ||
                    (arr != null && arr.indexers != null && arr.indexers.indexers.Count > 0 && arr.indexers.indexers[0] != null))
            		{
                        AddError(get_location(tpars.vars_type), "STRUCT_TYPE_DEFINITION_IN_FORMAL_PARAM");
            		}
            		
                    type_node tp = convert_strong(tpars.vars_type);

                    /*if (depended_from_generic_parameter(tp)) // SSM 25/07/13 - закомментировал и пока всё работает
                    {
                        AddError(new DelegateCanNotUseComprehensiveGenericParameter(get_location(tpars.vars_type)));
                    }*/

                    if (tpars.param_kind == SyntaxTree.parametr_kind.params_parametr)
            		{
                		internal_interface ii = tp.get_internal_interface(internal_interface_kind.unsized_array_interface);
                		if (ii == null)
                		{
                            AddError(get_location(tpars.vars_type), "ONLY_UNSIZED_ARRAY_PARAMS_PARAMETER_ALLOWED");
                		}
            		}
                    foreach (common_parameter ccp in temp_params)
                    {
                        ccp.type = tp;
                    }
                   
                    
                    if (params_met)
                	{
                        AddError(get_location(tpars), "ONLY_LAST_PARAMETER_CAN_BE_PARAMS");
                	}
                	if (tpars.param_kind == PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr)
                	{
                    	params_met = true;
//                    	if (default_value_met)
//                    	{
//                        	AddError(new FunctionWithParamsParameterCanNotHaveDefaultParameters(get_location(tp)));
//                    	}
                	}
                }
                
            }
            type_node ret_val_type = null;
            if (return_type != null)
            {
                ret_val_type = convert_strong(return_type);
                /*if (depended_from_generic_parameter(ret_val_type))  // SSM 25/07/13 - закомментировал и пока всё работает
                {
                    AddError(new DelegateCanNotUseComprehensiveGenericParameter(get_location(return_type)));
                }*/
            }
            common_type_node del;
            if (del_type == null)
            {
                del = convertion_data_and_alghoritms.type_constructor.create_delegate(type_name,
                        ret_val_type, parameters, context.converted_namespace, loc);
            }
            else
            {
                del = del_type;
                convertion_data_and_alghoritms.type_constructor.init_delegate(del, ret_val_type, parameters, loc);
            }
            context.converted_namespace.types.AddElement(del);
            return del;
        }

        private string get_template_instance_name(string template_name, System.Collections.Generic.List<string> args)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("$");
            sb.Append("template_");
            sb.Append(template_name);
            foreach (string s in args)
            {
                sb.Append("$");
                sb.Append(s);
            }
            return sb.ToString();
        }
        
        private string record_type_name = null;
        private bool record_is_generic = false;

        private void check_where_section(SyntaxTree.class_definition cl_def)
        {
            if (cl_def.where_section != null && cl_def.where_section.defs != null)
            {
                AddError(get_location(cl_def.where_section), "WHERE_SECTION_ALLOWED_ONLY_IN_GENERICS");
            }
        }

        //(ssyy) Проверяет параметры шаблона на совпадение друг с другом
        private void check_param_redeclared(List<SyntaxTree.ident> idents)
        {
            for (int i = 0; i < idents.Count; i++)
                for (int j = i + 1; j < idents.Count; j++)
                {
                    if (string.Equals(idents[i].name, idents[j].name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        AddError(get_location(idents[j]), "TEMPLATE_PARAMETER_{0}_REDEFINITION", idents[j].name);
                    }
                }
            if (context.top_function != null && context.converted_type != null && context.converted_type.is_generic_type_definition)
            {
                //List<SemanticTree.ICommonTypeNode> gparams = context.converted_type.generic_params;
                foreach (SyntaxTree.ident id in idents)
                    foreach(SemanticTree.ICommonTypeNode ictn in context.converted_type.generic_params)
                        if (string.Equals(id.name, ictn.name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            AddError(get_location(id), "TEMPLATE_PARAMETER_{0}_REDEFINITION", id.name);
                        }
            }
        }

        private void visit_generic_params(common_type_node ctn, List<SyntaxTree.ident> idents)
        {
            if (SemanticRules.RuntimeInitVariablesOfGenericParameters)
            {
                if (SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction != null)   // SSM 12/05/15 - из-за отсутствия этого падало при наличии обобщенных классов в системном модуле! Ужас!
                    SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.Restore(); 
            }
            ctn.is_generic_type_definition = true;
            check_param_redeclared(idents);
            ctn.generic_params = new List<SemanticTree.ICommonTypeNode>();
            foreach (SyntaxTree.ident id in idents)
            {
                common_type_node par = new common_type_node(
                    id.name, SemanticTree.type_access_level.tal_public, context.converted_namespace,
                    convertion_data_and_alghoritms.symbol_table.CreateInterfaceScope(null, SystemLibrary.SystemLibrary.object_type.Scope, null),
                    get_location(id));
                SystemLibrary.SystemLibrary.init_reference_type(par);
                par.SetBaseType(SystemLibrary.SystemLibrary.object_type);
                par.generic_type_container = ctn;
                ctn.generic_params.Add(par);
                ctn.scope.AddSymbol(id.name, new SymbolInfo(par));
                if (SemanticRules.RuntimeInitVariablesOfGenericParameters && !ctn.IsInterface && !ctn.IsDelegate)
                {
                    class_field cf = new class_field(
                        compiler_string_consts.generic_param_kind_prefix + id.name,
                        SystemLibrary.SystemLibrary.byte_type,
                        ctn, PascalABCCompiler.SemanticTree.polymorphic_state.ps_static,
                        SemanticTree.field_access_level.fal_public, null);
                    ctn.fields.AddElement(cf);
                    par.runtime_initialization_marker = cf;
                }
            }
        }

        private void visit_generic_params(common_function_node cfn, List<SyntaxTree.ident> idents)
        {
            /*if (SemanticRules.RuntimeInitVariablesOfGenericParameters)
            {
                SystemLibrary.SystemLibInitializer.RuntimeInitializeFunction.Restore();
            }*/
            //ctn.is_generic_type_definition = true;
            check_param_redeclared(idents);
            cfn.generic_params = new List<SemanticTree.ICommonTypeNode>();
            foreach (SyntaxTree.ident id in idents)
            {
                common_type_node par = new common_type_node(
                    id.name, SemanticTree.type_access_level.tal_public, context.converted_namespace,
                    convertion_data_and_alghoritms.symbol_table.CreateInterfaceScope(null, SystemLibrary.SystemLibrary.object_type.Scope, null),
                    get_location(id));
                SystemLibrary.SystemLibrary.init_reference_type(par);
                par.SetBaseType(SystemLibrary.SystemLibrary.object_type);
                par.generic_function_container = cfn;
                cfn.generic_params.Add(par);
                cfn.scope.AddSymbol(id.name, new SymbolInfo(par));
                /*if (SemanticRules.RuntimeInitVariablesOfGenericParameters && !ctn.IsInterface)
                {
                    class_field cf = new class_field(
                        compiler_string_consts.generic_param_kind_prefix + id.name,
                        SystemLibrary.SystemLibrary.byte_type,
                        ctn, PascalABCCompiler.SemanticTree.polymorphic_state.ps_static,
                        SemanticTree.field_access_level.fal_public, null);
                    ctn.fields.AddElement(cf);
                    par.runtime_initialization_marker = cf;
                }*/
            }
        }

        public override void visit(SyntaxTree.type_declaration _type_declaration)
        {
            //bool is_template_synonym = false;
            //SyntaxTree.array_type at=_type_declaration.type_def as SyntaxTree.array_type;
            template_class tc = null;
            SyntaxTree.template_type_name ttn = _type_declaration.type_name as SyntaxTree.template_type_name;
            SyntaxTree.class_definition cl_def = _type_declaration.type_def as SyntaxTree.class_definition;
            SyntaxTree.function_header function_header = _type_declaration.type_def as SyntaxTree.function_header;
            SyntaxTree.procedure_header procedure_header = _type_declaration.type_def as SyntaxTree.procedure_header;
            if (cl_def != null && (cl_def.attribute & SyntaxTree.class_attribute.Partial) == SyntaxTree.class_attribute.Partial && cl_def.keyword != SyntaxTree.class_keyword.Class)
            {
                AddError(get_location(_type_declaration), "ONLY_CLASS_CAN_BE_PARTIAL");
            }
            if (ttn != null && cl_def == null && function_header == null && procedure_header == null)
            {
                location t_loc = get_location(ttn);
                context.check_name_free(ttn.name, t_loc);
                tc = new template_class(_type_declaration, ttn.name,
                    context.converted_namespace,
                    current_document, using_list);
                tc.is_synonym = true;
                //Проверяем параметры шаблона на совпадение друг с другом
                check_param_redeclared(ttn.template_args.idents);
                context.AddTemplate(ttn.name, tc, t_loc);
                return;

                //is_template_synonym = true;
                //AddError(new TemplateCanBeClassOnly(get_location(ttn)));
            }
            if ((function_header != null) || (procedure_header != null))
            {
                location loc;
                if (function_header != null)
                    loc = get_location(function_header);
                else
                    loc = get_location(procedure_header);
                string del_name = _type_declaration.type_name.name;
                if (ttn != null)
                {
                    if (context.top_function != null)
                    {
                        AddError(loc, "GENERIC_DELEGATE_INNER_FUNCTION");
                    }
                    del_name += compiler_string_consts.generic_params_infix + ttn.template_args.idents.Count;
                }
                context.check_name_free(del_name, loc);
                common_type_node del_type = convertion_data_and_alghoritms.type_constructor.create_delegate_without_init(
                    del_name, context.converted_namespace, loc);
                //(ssyy) Обработка дженериков
                common_type_node tmp_converted_type = null;
                if (ttn != null)
                {
                    tmp_converted_type = context.converted_type;
                    context.converted_type = del_type;
                    visit_generic_params(del_type, ttn.template_args.idents);
                }
                if (function_header != null)
                {
                    del_type = convert_function_type(function_header, loc, del_name, del_type);
                }
                else
                {
                    del_type = convert_function_type(procedure_header, loc, del_name, del_type);
                }
                if (ttn != null)
                {
                    context.converted_type = tmp_converted_type;
                }
                //blocks.converted_namespace.types.AddElement(del_type);
                CheckWaitedRefTypes(del_type);
                if (_type_declaration.attributes != null)
            	{
                	make_attributes_for_declaration(_type_declaration,del_type);
                }
                context.converted_namespace.scope.AddSymbol(del_name, new SymbolInfo(del_type));
                return;
            }
            if (cl_def == null && ttn == null)
            {
                string name = _type_declaration.type_name.name;
                location loc = get_location(_type_declaration.type_name);
                context.check_name_free(name, loc);
                is_direct_type_decl = true;
                type_node tn = convert_strong(_type_declaration.type_def);
                assign_doc_info(tn,_type_declaration);
                is_direct_type_decl = false;
                if (_type_declaration.type_def is SyntaxTree.named_type_reference||
                    _type_declaration.type_def is SyntaxTree.ref_type || _type_declaration.type_def is SyntaxTree.string_num_definition ||
                    tn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.array_kind)// ||
                    /*tn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type*/
                {
                	if (context.converted_func_stack.Empty)
                	{
                		type_synonym ts = new type_synonym(name, tn, loc);
                		assign_doc_info(ts,_type_declaration);
                		context.converted_namespace.type_synonyms.Add(ts);
                	}
                }
                else
                {
                    tn.SetName(context.BuildName(name));
                }
                if (_type_declaration.attributes != null)
                if (_type_declaration.type_def is SyntaxTree.enum_type_definition)
                {
                	make_attributes_for_declaration(_type_declaration,tn);
                	(tn as common_type_node).add_additional_enum_operations();
                }
                else
                    AddError(get_location(_type_declaration.attributes), "ATTRIBUTES_APPLICABLE_ONLY_TO_THESE_TYPES");
                context.add_type(name, tn, loc);
                return;
            }
            //Проверим, что список template-ов не пуст
            bool is_generic = false;
            if (ttn != null && cl_def != null && cl_def.template_args == null)
            {
                cl_def.template_args = ttn.template_args;
            }
            if (cl_def != null && cl_def.template_args != null && cl_def.template_args.idents != null &&
                cl_def.template_args.idents.Count > 0)
            {
                switch (cl_def.keyword)
                {
                    case PascalABCCompiler.SyntaxTree.class_keyword.TemplateClass:
                        cl_def.keyword = PascalABCCompiler.SyntaxTree.class_keyword.Class;
                        is_generic = false;
                        check_where_section(cl_def);
                        break;
                    case PascalABCCompiler.SyntaxTree.class_keyword.TemplateRecord:
                        cl_def.keyword = PascalABCCompiler.SyntaxTree.class_keyword.Record;
                        is_generic = false;
                        check_where_section(cl_def);
                        break;
                    case PascalABCCompiler.SyntaxTree.class_keyword.TemplateInterface:
                        cl_def.keyword = PascalABCCompiler.SyntaxTree.class_keyword.Interface;
                        is_generic = false;
                        check_where_section(cl_def);
                        break;
                    default:
                        is_generic = true;
                        break;
                }

                //if (is_generic)
                //{
                //    throw new NotSupportedError(get_location(_type_declaration.type_name));
                //}
                if (!is_generic)
                {
                    string t_name = _type_declaration.type_name.name;
                    location t_loc = get_location(_type_declaration.type_name);
                    SymbolInfo si = context.find_only_in_namespace(t_name);

                    tc = null;
                    //Ищем предописание
                    if (si != null)
                    {
                        tc = si.sym_info as template_class;
                        if (tc == null || !tc.ForwardDeclarationOnly)
                        {
                            AddError(new NameRedefinition(t_name, convertion_data_and_alghoritms.get_location(si.sym_info), t_loc));
                        }
                        SyntaxTree.class_definition prev_cl_def = tc.type_dec.type_def as SyntaxTree.class_definition;
                        if (prev_cl_def.keyword != cl_def.keyword || prev_cl_def.template_args.idents.Count != cl_def.template_args.idents.Count)
                        {
                            AddError(t_loc, "FORWARD_TEMPLATE_{0}_DECLARATION_MISMATCH_DECLARATION", t_name);
                        }
                        for (int k = 0; k < cl_def.template_args.idents.Count; k++)
                        {
                            if (string.Compare(cl_def.template_args.idents[k].name, prev_cl_def.template_args.idents[k].name, true) != 0)
                            {
                                AddError(t_loc, "FORWARD_TEMPLATE_{0}_DECLARATION_MISMATCH_DECLARATION", t_name);
                            }
                        }
                        if (cl_def.body == null)
                        {
                            AddError(new NameRedefinition(t_name, convertion_data_and_alghoritms.get_location(si.sym_info), t_loc));
                        }
                        tc.type_dec = _type_declaration;
                        tc.ForwardDeclarationOnly = false;
                        return;
                    }

                    tc = new template_class(_type_declaration, t_name,
                        context.converted_namespace, 
                        current_document, using_list);

                    //Проверяем параметры шаблона на совпадение друг с другом
                    check_param_redeclared(cl_def.template_args.idents);

                    tc.ForwardDeclarationOnly = cl_def.body == null;
                    context.AddTemplate(t_name, tc, t_loc);
                    if (!tc.ForwardDeclarationOnly && template_class.check_template_definitions)
                    {
                        int count = cl_def.template_args.idents.Count;
                        List<type_node> test_types = new List<type_node>(count);
                        for (int i = 0; i < count; i++)
                        {
                            test_types.Add(new indefinite_type_node(cl_def.template_args.idents[i].name));
                        } 
                        instance(tc, test_types, t_loc);
                    }
                    return;
                }
            }
            //(ssyy) Теперь проверим на модификатор template
            if (cl_def.keyword == PascalABCCompiler.SyntaxTree.class_keyword.TemplateClass ||
                cl_def.keyword == PascalABCCompiler.SyntaxTree.class_keyword.TemplateRecord ||
                cl_def.keyword == PascalABCCompiler.SyntaxTree.class_keyword.TemplateInterface)
            {
                //Ошибка, т.к. нет списка шаблонных параметров.
                AddError(get_location(_type_declaration.type_name), "TEMPLATE_PARAMS_EXPECTED");
            }
            if (is_generic)
            {
                context.check_name_free(_type_declaration.type_name.name, get_location(_type_declaration.type_name));
                _type_declaration.type_name.name += compiler_string_consts.generic_params_infix +
                    cl_def.template_args.idents.Count.ToString();
            }
            if (cl_def.keyword == SyntaxTree.class_keyword.Record)
            {
                string name = record_type_name = _type_declaration.type_name.name;
                record_is_generic = is_generic;
                location loc = get_location(_type_declaration.type_name);
                context.check_name_free(name, loc);
                //context.create_record_type(loc, record_type_name);
                is_direct_type_decl = true;
                type_node tn = convert_strong(_type_declaration.type_def);
                assign_doc_info(tn,_type_declaration);
                if (_type_declaration.attributes != null)
            	{
            		make_attributes_for_declaration(_type_declaration, tn);
            	}
                is_direct_type_decl = false;
                record_type_name = null;
                record_is_generic = false;
                tn.SetName(context.BuildName(name));
                return;
            }

            //(ssyy) Флаг показывает, создаём ли мы интерфейс
            bool interface_creating = (/*cl_def != null &&*/ cl_def.keyword == PascalABCCompiler.SyntaxTree.class_keyword.Interface);

            common_type_node ctn = context.advanced_create_type(_type_declaration.type_name.name, get_location(_type_declaration.type_name), interface_creating, (cl_def.attribute & SyntaxTree.class_attribute.Partial) == SyntaxTree.class_attribute.Partial);
            assign_doc_info(ctn,_type_declaration);
            if (is_generic)
            {
                context.create_generic_indicator(ctn);
                visit_generic_params(ctn, cl_def.template_args.idents);
            }
            visit_where_list(cl_def.where_section);
            CheckWaitedRefTypes(ctn);
            is_direct_type_decl = true;
            hard_node_test_and_visit(_type_declaration.type_def);
            is_direct_type_decl = false;
            if (_type_declaration.attributes != null)
            {
            	make_attributes_for_declaration(_type_declaration, ctn);
            }
        }
		
        private bool attribute_converted = false;
        private AttributeTargets get_usage_attrs(type_node tn, out bool allowMultiple)
        {
        	allowMultiple = false;
        	if (tn is common_type_node)
        	{
        		common_type_node ctn = tn as common_type_node;
        		for (int i=0; i<ctn.attributes.Count; i++)
        		{
        			if (ctn.attributes[i].attribute_type == SystemLibrary.SystemLibrary.usage_attribute_type)
        			{
        				int ind = ctn.attributes[i].prop_names.IndexOf(SystemLibrary.SystemLibrary.usage_attribute_type.find_in_type("AllowMultiple").sym_info as property_node);
        				if (ind != -1)
        					allowMultiple = (bool)ctn.attributes[i].prop_initializers[ind].get_object_value();
        				return (AttributeTargets)ctn.attributes[i].args[0].get_object_value();
        			}
        		}
        		return AttributeTargets.All;
        	}
        	else 
        	{
        		compiled_type_node ctn = tn as compiled_type_node;
        		Attribute attr = Attribute.GetCustomAttribute(ctn.compiled_type,SystemLibrary.SystemLibrary.usage_attribute_type.compiled_type);
        		if (attr != null)
        		{
        			allowMultiple = (attr as AttributeUsageAttribute).AllowMultiple;
        			return (attr as AttributeUsageAttribute).ValidOn;
        		}
        		else
        			return AttributeTargets.All;
        	}
        }
        
        private void check_for_usage_attribute(definition_node dn, AttributeTargets targets, string name, location loc, SemanticTree.attribute_qualifier_kind qualifier)
        {
        	switch (dn.semantic_node_type)
        	{
        		case semantic_node_type.common_type_node : 
        			{
        				common_type_node ctn = dn as common_type_node;
        				if (ctn.IsDelegate)
        				{
        					if ((targets & (AttributeTargets.Delegate)) != (AttributeTargets.Delegate))
        						AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_DELEGATE", name);
        				}
        				else
        				if (ctn.IsInterface)
        				{
        					if ((targets & AttributeTargets.Interface) != AttributeTargets.Interface)
        						AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_INTERFACE", name);
        				}
        				else
        				if (ctn.IsEnum)
        				{
        					if ((targets & (AttributeTargets.Enum)) != (AttributeTargets.Enum))
        						AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_ENUM", name);
        				}
        				else
        				if (ctn.is_value_type)
        				{
        					if ((targets & AttributeTargets.Struct) != AttributeTargets.Struct)
        						AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_STRUCT", name);
        				}
        				else
        					if ((targets & AttributeTargets.Class) != AttributeTargets.Class)
        						AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_CLASS", name);
        			}
        			break;
        		case semantic_node_type.common_method_node:
        			if ((dn as common_method_node).is_constructor)
        			{
        				if ((targets & AttributeTargets.Constructor) != AttributeTargets.Constructor)
        					AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_CONSTRUCTOR", name);
        			}
        			else
        				if ((targets & AttributeTargets.Method) != AttributeTargets.Method && qualifier != SemanticTree.attribute_qualifier_kind.return_kind)
        					AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_METHOD", name);
        			break;
        		case semantic_node_type.common_namespace_function_node:
        			if ((targets & AttributeTargets.Method) != AttributeTargets.Method && qualifier != SemanticTree.attribute_qualifier_kind.return_kind)
        				AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_METHOD", name);
        			break;
        		case semantic_node_type.class_field:
        			if ((targets & AttributeTargets.Field) != AttributeTargets.Field)
        				AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_FIELD", name);
        			break;
        		case semantic_node_type.common_property_node:
        			if ((targets & AttributeTargets.Property) != AttributeTargets.Property)
        				AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_PROPERTY", name);
        			break;
                case semantic_node_type.common_namespace_event:
        		case semantic_node_type.common_event:
        			if ((targets & AttributeTargets.Event) != AttributeTargets.Event)
        				AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_EVENT", name);
        			break;
        		case semantic_node_type.common_parameter:
        			if ((targets & AttributeTargets.Parameter) != AttributeTargets.Parameter)
                        AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_FIELD", name);
					break;
                case semantic_node_type.common_unit_node:
                    if ((targets & AttributeTargets.Assembly) != AttributeTargets.Assembly)
        				AddError(loc, "ATTRIBUTE_{0}_NOT_APPLICABLE_TO_ASSEMBLY", name);
        			break;
        	}
        }
        
        private bool has_field_offset_attribute(attributes_list attrs)
        {
        	foreach (attribute_node attr in attrs)
        	{
        		if (attr.attribute_type == SystemLibrary.SystemLibrary.field_offset_attribute_type)
        			return true;
        	}
        	return false;
        }
        
        private void check_all_fields_have_field_offset_attr(common_type_node ctn)
        {
        	foreach (class_field cf in ctn.fields)
        	{
        		if (!has_field_offset_attribute(cf.attributes))
                    AddError(cf.loc, "FIELD_MUST_HAVE_FIELD_OFFSET_ATTRIBUTE");
        	}
        }

        private void make_attributes_for_declaration(SyntaxTree.declaration td, definition_node ctn)
        {
            if (td.attributes != null)
            {
                Hashtable ht = new Hashtable();
                foreach (SyntaxTree.simple_attribute_list sal in td.attributes.attributes)
                    for (int j = 0; j < sal.attributes.Count; j++)
                    {
                        SyntaxTree.attribute attr = sal.attributes[j];
                        attribute_converted = true;
                        type_node tn = convert_strong(attr.type);
                        bool is_attr = false;
                        type_node tmp = tn;
                        while (tmp.base_type != null && !is_attr)
                        {
                            is_attr = tmp.base_type == SystemLibrary.SystemLibrary.attribute_type;
                            tmp = tmp.base_type;
                        }
                        if (!is_attr)
                            AddError(get_location(attr), "CLASS_{0}_NOT_ATTRIBUTE", tn.name);
                        bool allowMultiple = false;
                        AttributeTargets targets = get_usage_attrs(tn, out allowMultiple);
                        if (ht.Contains(tn) && !allowMultiple)
                        {
                            AddError(get_location(attr), "DUPLICATE_ATTRIBUTE_{0}_APPLICATION", tn.name);
                        }
                        else
                            ht[tn] = tn;
                        SemanticTree.attribute_qualifier_kind qualifier = SemanticTree.attribute_qualifier_kind.none_kind;
                        if (attr.qualifier != null)
                            if (j == 0)
                            {
                                if (string.Compare(attr.qualifier.name, "return", true) == 0)
                                {
                                    if (context.top_function == null)
                                        AddError(get_location(attr), "ATTRIBUTE_APPLICABLE_ONLY_TO_METHOD");
                                    if (context.top_function.return_value_type == null || context.top_function.return_value_type == SystemLibrary.SystemLibrary.void_type)
                                        AddError(get_location(attr), "EXPECTED_RETURN_VALUE_FOR_ATTRIBUTE");
                                    throw new NotSupportedError(get_location(attr.qualifier));
                                    qualifier = SemanticTree.attribute_qualifier_kind.return_kind;
                                }
                                else
                                    throw new NotSupportedError(get_location(attr.qualifier));
                            }
                            else
                                AddError(get_location(attr.qualifier), "ATTRIBUTE_QUALIFIER_MUST_BE_FIRST");
                        check_for_usage_attribute(ctn, targets, tn.name, get_location(attr), qualifier);

                        attribute_converted = false;
                        SyntaxTree.expression_list cnstr_args = new SyntaxTree.expression_list();
                        if (attr.arguments != null)
                        {
                            foreach (SyntaxTree.expression e in attr.arguments.expressions)
                            {
                                if (e is SyntaxTree.bin_expr && (e as SyntaxTree.bin_expr).operation_type == SyntaxTree.Operators.Equal
                                    && (e as SyntaxTree.bin_expr).left is SyntaxTree.ident)
                                {
                                    break;
                                }
                                else
                                {
                                    cnstr_args.expressions.Add(e);
                                }
                            }
                        }
                        expressions_list args = new expressions_list();
                        for (int i = 0; i < cnstr_args.expressions.Count; i++)
                        {
                            constant_node cn = convert_strong_to_constant_node(cnstr_args.expressions[i]);
                            check_for_strong_constant(cn, get_location(cnstr_args.expressions[i]));
                            args.AddElement(cn);
                        }

                        base_function_call bfc = create_constructor_call(tn, args, get_location(attr));
                        if (ctn is common_type_node && tn == SystemLibrary.SystemLibrary.struct_layout_attribute_type)
                        {
                            if ((System.Runtime.InteropServices.LayoutKind)Convert.ToInt32((args[0] as constant_node).get_object_value()) == System.Runtime.InteropServices.LayoutKind.Explicit)
                            {
                                check_all_fields_have_field_offset_attr(ctn as common_type_node);
                            }
                        }
                        attribute_node attr_node = new attribute_node(bfc.simple_function_node, tn, get_location(td));
                        foreach (expression_node en in bfc.parameters)
                        {
                            constant_node cn = convert_strong_to_constant_node(en, en.type);
                            check_for_strong_constant(cn, en.location);
                            attr_node.args.Add(cn);
                        }
                        if (attr.arguments != null)
                        {
                            for (int i = cnstr_args.expressions.Count; i < attr.arguments.expressions.Count; i++)
                            {
                                SyntaxTree.expression e = attr.arguments.expressions[i];
                                if (!(e is SyntaxTree.bin_expr && (e as SyntaxTree.bin_expr).operation_type == SyntaxTree.Operators.Equal
                                      && (e as SyntaxTree.bin_expr).left is SyntaxTree.ident))
                                {
                                    AddError(get_location(e), "EXPECTED_ATTRIBUTE_INITIALIZER");
                                }
                                else
                                {
                                    SyntaxTree.ident id = (e as SyntaxTree.bin_expr).left as SyntaxTree.ident;
                                    SymbolInfo si = tn.find_in_type(id.name);
                                    definition_node dn = context.check_name_node_type(id.name, si, get_location(id), general_node_type.property_node, general_node_type.variable_node);
                                    type_node mem_tn = null;
                                    if (dn is property_node)
                                    {
                                        property_node pn = dn as property_node;
                                        attr_node.prop_names.Add(pn);
                                        if (pn.set_function == null)
                                            AddError(new ThisPropertyCanNotBeWrited(pn, get_location(id)));
                                        if (pn.set_function.parameters.Count != 1)
                                            AddError(get_location(id), "INDEX_PROPERTY_INITIALIZING_NOT_VALID");
                                        mem_tn = pn.set_function.parameters[0].type;
                                    }
                                    else if (dn is var_definition_node)
                                    {
                                        attr_node.field_names.Add(dn as var_definition_node);
                                        mem_tn = (dn as var_definition_node).type;
                                    }
                                    else
                                    {
                                        throw new CompilerInternalError("Bad general node type for attribute initializer");
                                    }
                                    //SyntaxTree.assign tmp_ass = new SyntaxTree.assign(id,(e as SyntaxTree.bin_expr).right,SyntaxTree.Operators.Assignment);
                                    //tmp_ass.source_context = e.source_context;
                                    //basic_function_call tmp_bfc = convert_strong(tmp_ass) as basic_function_call;
                                    constant_node cn = convert_strong_to_constant_node((e as SyntaxTree.bin_expr).right, mem_tn);
                                    check_for_strong_constant(cn, get_location((e as SyntaxTree.bin_expr).right));
                                    if (dn is property_node)
                                        attr_node.prop_initializers.Add(cn);
                                    else
                                        attr_node.field_initializers.Add(cn);
                                }
                            }
                        }
                        attr_node.qualifier = qualifier;
                        ctn.attributes.AddElement(attr_node);
                    }
            }
        }

        private void make_attributes_for_declaration(SyntaxTree.unit_module un, common_unit_node cun)
        {
            if (cun.attributes != null)
            {
                Hashtable ht = new Hashtable();
                foreach (SyntaxTree.simple_attribute_list sal in un.attributes.attributes)
                    for (int j = 0; j < sal.attributes.Count; j++)
                    {
                        SyntaxTree.attribute attr = sal.attributes[j];
                        attribute_converted = true;
                        type_node tn = convert_strong(attr.type);
                        bool is_attr = false;
                        type_node tmp = tn;
                        while (tmp.base_type != null && !is_attr)
                        {
                            is_attr = tmp.base_type == SystemLibrary.SystemLibrary.attribute_type;
                            tmp = tmp.base_type;
                        }
                        if (!is_attr)
                            AddError(get_location(attr), "CLASS_{0}_NOT_ATTRIBUTE", tn.name);
                        bool allowMultiple = false;
                        AttributeTargets targets = get_usage_attrs(tn, out allowMultiple);
                        if (ht.Contains(tn) && !allowMultiple)
                        {
                            AddError(get_location(attr), "DUPLICATE_ATTRIBUTE_{0}_APPLICATION", tn.name);
                        }
                        else
                            ht[tn] = tn;
                        SemanticTree.attribute_qualifier_kind qualifier = SemanticTree.attribute_qualifier_kind.none_kind;
                        if (attr.qualifier != null)
                            if (j == 0)
                            {
                                if (string.Compare(attr.qualifier.name, "return", true) == 0)
                                {
                                    if (context.top_function == null)
                                        AddError(get_location(attr), "ATTRIBUTE_APPLICABLE_ONLY_TO_METHOD");
                                    if (context.top_function.return_value_type == null || context.top_function.return_value_type == SystemLibrary.SystemLibrary.void_type)
                                        AddError(get_location(attr), "EXPECTED_RETURN_VALUE_FOR_ATTRIBUTE");
                                    throw new NotSupportedError(get_location(attr.qualifier));
                                    qualifier = SemanticTree.attribute_qualifier_kind.return_kind;
                                }
                                else
                                    throw new NotSupportedError(get_location(attr.qualifier));
                            }
                            else
                                AddError(get_location(attr.qualifier), "ATTRIBUTE_QUALIFIER_MUST_BE_FIRST");
                        check_for_usage_attribute(cun, targets, tn.name, get_location(attr), qualifier);

                        attribute_converted = false;
                        SyntaxTree.expression_list cnstr_args = new SyntaxTree.expression_list();
                        if (attr.arguments != null)
                        {
                            foreach (SyntaxTree.expression e in attr.arguments.expressions)
                            {
                                if (e is SyntaxTree.bin_expr && (e as SyntaxTree.bin_expr).operation_type == SyntaxTree.Operators.Equal
                                    && (e as SyntaxTree.bin_expr).left is SyntaxTree.ident)
                                {
                                    break;
                                }
                                else
                                {
                                    cnstr_args.expressions.Add(e);
                                }
                            }
                        }
                        expressions_list args = new expressions_list();
                        for (int i = 0; i < cnstr_args.expressions.Count; i++)
                        {
                            constant_node cn = convert_strong_to_constant_node(cnstr_args.expressions[i]);
                            check_for_strong_constant(cn, get_location(cnstr_args.expressions[i]));
                            args.AddElement(cn);
                        }

                        base_function_call bfc = create_constructor_call(tn, args, get_location(attr));
                        attribute_node attr_node = new attribute_node(bfc.simple_function_node, tn, get_location(un));
                        foreach (expression_node en in bfc.parameters)
                        {
                            constant_node cn = convert_strong_to_constant_node(en, en.type);
                            check_for_strong_constant(cn, en.location);
                            attr_node.args.Add(cn);
                        }
                        if (attr.arguments != null)
                        {
                            for (int i = cnstr_args.expressions.Count; i < attr.arguments.expressions.Count; i++)
                            {
                                SyntaxTree.expression e = attr.arguments.expressions[i];
                                if (!(e is SyntaxTree.bin_expr && (e as SyntaxTree.bin_expr).operation_type == SyntaxTree.Operators.Equal
                                      && (e as SyntaxTree.bin_expr).left is SyntaxTree.ident))
                                {
                                    AddError(get_location(e), "EXPECTED_ATTRIBUTE_INITIALIZER");
                                }
                                else
                                {
                                    SyntaxTree.ident id = (e as SyntaxTree.bin_expr).left as SyntaxTree.ident;
                                    SymbolInfo si = tn.find(id.name);
                                    definition_node dn = context.check_name_node_type(id.name, si, get_location(id), general_node_type.property_node, general_node_type.variable_node);
                                    type_node mem_tn = null;
                                    if (dn is property_node)
                                    {
                                        property_node pn = dn as property_node;
                                        attr_node.prop_names.Add(pn);
                                        if (pn.set_function == null)
                                            AddError(new ThisPropertyCanNotBeWrited(pn, get_location(id)));
                                        if (pn.set_function.parameters.Count != 1)
                                            AddError(get_location(id), "INDEX_PROPERTY_INITIALIZING_NOT_VALID");
                                        mem_tn = pn.set_function.parameters[0].type;
                                    }
                                    else if (dn is var_definition_node)
                                    {
                                        attr_node.field_names.Add(dn as var_definition_node);
                                        mem_tn = (dn as var_definition_node).type;
                                    }
                                    else
                                    {
                                        throw new CompilerInternalError("Bad general node type for attribute initializer");
                                    }
                                    //SyntaxTree.assign tmp_ass = new SyntaxTree.assign(id,(e as SyntaxTree.bin_expr).right,SyntaxTree.Operators.Assignment);
                                    //tmp_ass.source_context = e.source_context;
                                    //basic_function_call tmp_bfc = convert_strong(tmp_ass) as basic_function_call;
                                    constant_node cn = convert_strong_to_constant_node((e as SyntaxTree.bin_expr).right, mem_tn);
                                    check_for_strong_constant(cn, get_location((e as SyntaxTree.bin_expr).right));
                                    if (dn is property_node)
                                        attr_node.prop_initializers.Add(cn);
                                    else
                                        attr_node.field_initializers.Add(cn);
                                }
                            }
                        }
                        attr_node.qualifier = qualifier;
                        cun.attributes.AddElement(attr_node);
                        if (cun.namespaces.Count > 0)
                            cun.namespaces[0].attributes.AddElement(attr_node);
                    }
            }
        }
        
        private void check_for_strong_constant(constant_node cn, location loc)
        {
        	switch (cn.semantic_node_type)
        	{
        		case semantic_node_type.bool_const_node :
        		case semantic_node_type.byte_const_node :
        		case semantic_node_type.char_const_node :
        		case semantic_node_type.sbyte_const_node :
        		case semantic_node_type.short_const_node :
        		case semantic_node_type.ushort_const_node :
        		case semantic_node_type.int_const_node :
        		case semantic_node_type.uint_const_node :
        		case semantic_node_type.long_const_node :
        		case semantic_node_type.ulong_const_node :
        		case semantic_node_type.float_const_node :
        		case semantic_node_type.double_const_node :
        		case semantic_node_type.string_const_node :
        		case semantic_node_type.enum_const :
        		case semantic_node_type.null_const_node :
        			return;
        		default:
                    AddError(loc, "CONSTANT_EXPRESSION_EXPECTED");
        			break;
        	}
        }
        
        internal void check_for_type_allowed(type_node tn, location loc)
        {
        	if (tn == SystemLibrary.SystemLibrary.void_type)
        		AddError(loc, "TYPE_{0}_NOT_VALID", tn.name);
        }
        
        private void check_cycle_inheritance(common_type_node cnode, type_node base_of_cnode)
        {
            List<common_type_node> base_params = new List<common_type_node>();
            base_params.Add(cnode);
            common_type_node bt = base_of_cnode as common_type_node;
            while (bt != null && bt.is_generic_parameter)
            {
                if (base_params.Contains(bt))
                {
                    AddError(bt.loc, "TYPE_{0}_DERIVED_FROM_ITSELF", bt.name);
                }
                base_params.Add(bt);
                bt = bt.base_type as common_type_node;
            }
        }

        private void visit_where_list(SyntaxTree.where_definition_list where_list)
        {
            if (where_list == null)
            {
                return;
            }
            context.BeginSkipGenericInstanceChecking();
            List<SemanticTree.ICommonTypeNode> gparams = null;
            common_function_node cfn = context.top_function;
            if (cfn != null)
            {
                if (!cfn.is_generic_function)
                {
                    AddError(get_location(where_list), "WHERE_SECTION_ALLOWED_ONLY_IN_GENERICS");
                    return;
                }
                gparams = cfn.generic_params;
            }
            else
            {
                if (!context.converted_type.is_generic_type_definition)
                {
                    AddError(get_location(where_list), "WHERE_SECTION_ALLOWED_ONLY_IN_GENERICS");
                    return;
                }
                gparams = context.converted_type.generic_params;
            }
            List<common_type_node> used_types = new List<common_type_node>(where_list.defs.Count);
            foreach (SyntaxTree.where_definition wd in where_list.defs)
            {
                bool param_not_found = true;
                foreach (common_type_node param in gparams)
                {
                    if (String.Equals(param.name, wd.names.idents[0].name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Нашли нужный шаблонный параметр
                        if (used_types.Contains(param))
                        {
                            AddError(get_location(wd.names), "SPECIFICATORS_FOR_{0}_ALREADY_EXIST", wd.names.idents[0].name);
                        }
                        add_generic_eliminations(param, wd.types.defs);
                        used_types.Add(param);
                        param_not_found = false;
                        break;
                    }
                }
                if (param_not_found)
                {
                    AddError(new UndefinedNameReference(wd.names.idents[0].name, get_location(wd.names)));
                }
            }
            context.EndSkipGenericInstanceChecking();
        }

        //Добавляет ограничители для параметра шаблона
        private void add_generic_eliminations(common_type_node param, List<SyntaxTree.type_definition> specificators)
        {
            Hashtable used_interfs = new Hashtable();
            for (int i = 0; i < specificators.Count; ++i)
            {
                SyntaxTree.declaration_specificator ds = specificators[i] as SyntaxTree.declaration_specificator;
                if (ds != null)
                {
                    switch (ds.specificator)
                    {
                        case SyntaxTree.DeclarationSpecificator.WhereDefClass:
                            if (i == 0)
                            {
                                param.is_class = true;
                            }
                            else
                            {
                                AddError(get_location(specificators[i]), "CLASS_OR_RECORD_SPECIFICATOR_MUST_BE_FIRST");
                            }
                            break;
                        case SyntaxTree.DeclarationSpecificator.WhereDefValueType:
                            if (i == 0)
                            {
                                param.internal_is_value = true;
                            }
                            else
                            {
                                AddError(get_location(specificators[i]), "CLASS_OR_RECORD_SPECIFICATOR_MUST_BE_FIRST");
                            }
                            break;
                        case SyntaxTree.DeclarationSpecificator.WhereDefConstructor:
                            if (i == specificators.Count - 1)
                            {
                                generic_parameter_eliminations.add_default_ctor(param);
                                //common_method_node cnode = new common_method_node(
                                //    compiler_string_consts.default_constructor_name, param, null,
                                //    param, SemanticTree.polymorphic_state.ps_common,
                                //    SemanticTree.field_access_level.fal_public, null);
                                //cnode.is_constructor = true;
                                //param.methods.AddElement(cnode);
                                //param.add_name(compiler_string_consts.default_constructor_name, new SymbolInfo(cnode));
                            }
                            else
                            {
                                AddError(get_location(specificators[i]), "CONSTRUCTOR_SPECIFICATOR_MUST_BE_LAST");
                            }
                            break;
                    }
                }
                else
                {
                    SyntaxTree.named_type_reference ntr = specificators[i] as SyntaxTree.named_type_reference;
                    if (ntr == null)
                    {
                        AddError(get_location(specificators[i]), "SPECIFICATOR_MUST_BE_TYPE_REFERENCE");
                    }
                    else
                    {
                        type_node spec_type = ret.visit(ntr);
                        if (spec_type is ref_type_node || spec_type == SystemLibrary.SystemLibrary.void_type || spec_type == SystemLibrary.SystemLibrary.pointer_type)
                            ErrorsList.Add(new SimpleSemanticError(get_location(specificators[i]), "INAVLID_TYPE"));
                        if (spec_type.IsInterface)
                        {
                            if (used_interfs[spec_type] != null)
                            {
                                AddError(get_location(specificators[i]), "INTERFACE_{0}_ALREADY_ADDED_TO_IMPLEMENTING_LIST", spec_type.PrintableName);
                            }
                            else
                            {
                                used_interfs.Add(spec_type, spec_type);
                            }
                            //Добавляем интерфейс
                            type_table.AddInterface(param, spec_type, get_location(specificators[i]));
                        }
                        else
                        {
                            if (i != 0)
                            {
                                AddError(get_location(specificators[i]), "PARENT_SPECIFICATOR_MUST_BE_FIRST");
                            }
                            else
                            {
                                //Тип-предок
                                if (spec_type == SystemLibrary.SystemLibrary.object_type)
                                {
                                    AddError(get_location(specificators[i]), "OBJECT_CAN_NOT_BE_USED_AS_PARENT_SPECIFICATOR");
                                }
                                if (spec_type == SystemLibrary.SystemLibrary.enum_base_type)
                                {
                                    AddError(get_location(specificators[i]), "ENUM_CAN_NOT_BE_USED_AS_PARENT_SPECIFICATOR");
                                }
                                check_cycle_inheritance(param, spec_type);
                                param.SetBaseType(spec_type);
                            }
                        }
                    }
                }
            }
            InitInterfaceScope(param);
        }

        private bool is_direct_type_decl = false;
        
        private bool FunctionExsistsInSymbolInfo(function_node fn, SymbolInfo si)
        {
            while (si != null)
            {
                if (si.sym_info is function_node)
                    if (convertion_data_and_alghoritms.function_eq_params(fn, si.sym_info as function_node))
                        return true;
                si = si.Next;
            }
            return false;
        }

        internal bool body_exists;

        internal bool current_converted_method_not_in_class_defined = false;
        internal bool assign_is_converting = false;
		
		//lroman//
        public void visit_lambda_header(SyntaxTree.function_header _function_header, SyntaxTree.proc_block proc_body)
        {
            hard_node_test_and_visit(_function_header.name);
            if (context.converted_template_type != null)
            {
                return;
            }
            if (_function_header.template_args != null)
            {
                visit_generic_params(context.top_function, _function_header.template_args.idents);
            }
            SymbolInfo si = context.create_special_names();
            weak_node_test_and_visit(_function_header.parameters);
			type_node tn = null;

            #region Вывод типа возвращаемого значения лямбды

            //lroman//
            if (LambdaHelper.IsLambdaName(_function_header.name.meth_name))
            {
                if (_function_header.return_type != null && _function_header.return_type is SyntaxTree.lambda_inferred_type)
                    LambdaHelper.InferResultType(_function_header, proc_body, this);
            }
            //\lroman//

            #endregion

            if (_function_header.return_type == null)
            {
            	if (context.top_function.IsOperator)
                    AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            }
            if (_function_header.return_type != null)
            {
            	check_parameter_on_complex_type(_function_header.return_type);
            	tn = convert_strong(_function_header.return_type);
				//if (tn == SystemLibrary.SystemLibrary.void_type)
            	//    AddError(new VoidNotValid(get_location(_function_header.return_type)));
            	check_for_type_allowed(tn,get_location(_function_header.return_type));
            }
            //(ssyy) moved up, так как при проверке аттрибута override надо знать тип возвращаемого значения
            context.top_function.return_value_type = tn;
            assign_doc_info(context.top_function,_function_header);
            if (_function_header.attributes != null)
            {
            	make_attributes_for_declaration(_function_header, context.top_function);
            }
            if (context.converted_type != null && has_dll_import_attribute(context.top_function))
                AddError(get_dll_import_attribute(context.top_function).location, "DLLIMPORT_ATTRIBUTE_CANNOT_BE_APPLIED_TO_METHOD");
            if (_function_header.name.class_name != null) 
            	with_class_name = true;
            if (_function_header.class_keyword && !has_static_attr(_function_header.proc_attributes.proc_attributes))
            {
                SyntaxTree.procedure_attribute pa = new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static);
                pa.source_context = _function_header.source_context;
                _function_header.proc_attributes.proc_attributes.Add(pa);
            }
            weak_node_test_and_visit(_function_header.proc_attributes);
            if (context.top_function.IsOperator)
            {
                common_method_node cmmn = context.top_function as common_method_node;
                //if (cmmn == null)
                //{
                //    throw new OverloadOperatorMustBeStaticFunction(get_location(_function_header), context.top_function);
                //}
                if (cmmn != null && cmmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(get_location(_function_header), "OVERLOADED_OPERATOR_MUST_BE_STATIC_FUNCTION");
                }
                if (cmmn != null && (cmmn.name == compiler_string_consts.implicit_operator_name || cmmn.name == compiler_string_consts.explicit_operator_name))
                if (!convertion_data_and_alghoritms.eq_type_nodes(tn, cmmn.comperehensive_type as type_node) && !convertion_data_and_alghoritms.eq_type_nodes(cmmn.comperehensive_type as type_node, cmmn.parameters[0].type))
                {
                    AddError(get_location(_function_header.return_type), "RETURN_VALUE_SHOULD_HAVE_TYPE_{0}", (cmmn.comperehensive_type as type_node).PrintableName);
                }
                else if (convertion_data_and_alghoritms.eq_type_nodes(tn,cmmn.parameters[0].type))
                {
                    AddError(get_location(_function_header), "CIRCURAL_TYPE_CONVERSION_DEFINITION");
                }
            }
			with_class_name = false;
			if (context.top_function != null && context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null && !context.top_function.IsOperator)
            {
                concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
                SemanticTree.parameter_type pt = PascalABCCompiler.SemanticTree.parameter_type.value;
                if ((context.top_function as common_namespace_function_node).ConnectedToType.is_value_type)
                {
                    cpt = concrete_parameter_type.cpt_var;
                    pt = PascalABCCompiler.SemanticTree.parameter_type.var;
                }
                if (!has_extensionmethod_attr(_function_header.proc_attributes.proc_attributes))
                {
                    common_parameter cp = new common_parameter(compiler_string_consts.self_word, (context.top_function as common_namespace_function_node).ConnectedToType, pt,
                                                                                    context.top_function, cpt, null, null);
                    context.top_function.parameters.AddElementFirst(cp);
                    context.top_function.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(cp));
                }
            }
            CheckOverrideOrReintroduceExpectedWarning(get_location(_function_header));

            bool unique = context.close_function_params(body_exists);
            if (context.top_function.return_value_type == null)
                AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            if (_function_header.where_defs != null)
            {
                if (unique)
                {
                    visit_where_list(_function_header.where_defs);
                }
                else
                {
                    AddError(get_location(_function_header.where_defs), "WHERE_SECTION_MUST_BE_ONLY_IN_FIRST_DECLARATION");
                }
            }
            convertion_data_and_alghoritms.create_function_return_variable(context.top_function, si);

            /*if (_function_header.name != null && context.converted_compiled_type != null && context.top_function is common_namespace_function_node)
            {
                if (context.FindMethodToOverride(context.top_function as common_namespace_function_node) != null)
                    AddError(new CanNotDeclareExtensionMethodAsOverrided(get_location(_function_header)));
            }*/
            //TODO: Разобрать подробнее.
            if (!body_exists)
            {
                if ((context.top_function.semantic_node_type == semantic_node_type.common_method_node)
                    || ((context.func_stack_size_is_one()) && (_is_interface_part)))
                {
                    //context.leave_block();
                }
            }
            body_exists = false;
        }
        //\lroman//
		
        public override void visit(SyntaxTree.procedure_definition _procedure_definition)
        {
            var proc_name = _procedure_definition.proc_header != null && _procedure_definition.proc_header.name != null
                                ? _procedure_definition.proc_header.name.meth_name
                                : null;
            // SSM 20.07.13 если это - узел с коротким определением функции без типа возвращаемого значения, то вывести его
            var fh = (_procedure_definition.proc_header as SyntaxTree.function_header);
            if (fh != null && fh.return_type == null)
            {
                var bl = _procedure_definition.proc_body as SyntaxTree.block;
                if (bl != null && bl.program_code != null)
                {
                    var ass = bl.program_code.subnodes[0] as SyntaxTree.assign;
                    if (ass != null)
                    {
                        var typ = SyntaxTreeBuilder.BuildSameType(ass.from);
                        fh.return_type = typ;
                    }
                }
            }
            //\ SSM
            if (_procedure_definition.proc_header.attributes != null && context.converted_func_stack.size >= 1)
                AddError(get_location(_procedure_definition.proc_header), "ATTRIBUTES_FOR_NESTED_FUNCTIONS_NOT_ALLOWED");
            if (context.top_function != null && context.top_function.generic_params != null && !LambdaHelper.IsLambdaName(proc_name))
                AddError(get_location(_procedure_definition.proc_header), "NESTED_FUNCTIONS_IN_GENERIC_FUNCTIONS_NOT_ALLOWED");
            if (context.top_function != null && _procedure_definition.proc_header.template_args != null && !LambdaHelper.IsLambdaName(proc_name))
                AddError(get_location(_procedure_definition.proc_header), "GENERIC_NESTED_FUNCTIONS_NOT_ALLOWED");
        	if (_procedure_definition.proc_header.name != null)
                current_converted_method_not_in_class_defined = _procedure_definition.proc_header.name.class_name != null;
            else
            {
                if (_procedure_definition.proc_header is SyntaxTree.constructor)
                {
                    current_converted_method_not_in_class_defined = false;
                }
            }
            bool must_visit_body = true;
            if (SemanticRules.OrderIndependedMethodNames && !disable_order_independ && context.converting_block() == block_type.type_block && _procedure_definition.proc_body != null)
            {
            	if (_procedure_definition.proc_header.name != null)
            	{
            		if (_procedure_definition.proc_header.name.class_name == null)
            		{
            			must_visit_body = false;
            			context.is_order_independed_method_description = true;
            		}
            	}
            	else
            	{
            		must_visit_body = false;
            		context.is_order_independed_method_description = true;
            	}
            }
            if (SemanticRules.OrderIndependedFunctionNames && !disable_order_independ && context.converting_block() == block_type.namespace_block && _procedure_definition.proc_body != null && _procedure_definition.proc_header.name.class_name == null)
            {
            	if (_procedure_definition.proc_header.name != null)
            	{
            		if (_procedure_definition.proc_header.name.class_name == null)
            		{
            			must_visit_body = false;
            			context.is_order_independed_method_description = true;
            		}
            	}
            	else
            	{
            		must_visit_body = false;
            		context.is_order_independed_method_description = true;
            	}
            }
            //ssyy
            if (context.converting_block() == block_type.type_block &&
                context.converted_type.IsInterface)
            {
                AddError(get_location(_procedure_definition), "INVALID_INTERFACE_MEMBER");
            }
            //\ssyy

            //ssyy добавил
            if (context.converting_block() == block_type.function_block)
            {
                common_method_node cmnode = context.top_function as common_method_node;
                if (cmnode != null && cmnode.is_constructor)
                {
                    //throw new NotSupportedError(get_location(_procedure_definition));
                }
            }
            //\ssyy

            if (_procedure_definition.proc_body == null)
            {
                body_exists = false;
            }
            else
            {
                body_exists = true;
            }

            //lroman//
            if (_procedure_definition != null &&
                _procedure_definition.proc_header != null &&
                _procedure_definition.proc_header is SyntaxTree.function_header && 
                _procedure_definition.proc_header.name != null &&
                _procedure_definition.proc_header.name.meth_name != null &&
                LambdaHelper.IsLambdaName(_procedure_definition.proc_header.name.meth_name))
            {
                //!!!!!!!!!!!!!!!!!! 
                convertion_data_and_alghoritms.check_node_parser_error(_procedure_definition.proc_header);
                visit_lambda_header(_procedure_definition.proc_header as SyntaxTree.function_header, _procedure_definition.proc_body);
            }
            else
            {
                hard_node_test_and_visit(_procedure_definition.proc_header);
            }
            //\lroman//


            //ssyy
            if (context.converted_template_type != null)
            {
                if ((context.converted_template_type.type_dec.type_def as SyntaxTree.class_definition).keyword == PascalABCCompiler.SyntaxTree.class_keyword.Interface)
                {
                    AddError(new InterfaceFunctionWithBody(get_location(_procedure_definition)));
                }
                context.converted_template_type.external_methods.Add(
                    new procedure_definition_info(context.converted_namespace, _procedure_definition));
                context.converted_template_type = null;
                return;
            }
            //\ssyy
            if (context.top_function == null)
            {
                return;
            }
            if (has_dll_import_attribute(context.top_function))
            {
                if (_procedure_definition.proc_body != null)
                    if (!is_pinvoke(_procedure_definition.proc_body))
                        AddError(get_location(_procedure_definition.proc_body), "EXPECTED_EXTERNAL_STATEMENT");
            }

            if (contextChanger.IsActive())
            {
                must_visit_body = true;
            }

           // try
            {

                if (context.top_function.is_forward)
                {
                    if (_procedure_definition.proc_body != null)
                    {
                        AddError(context.top_function.loc, "FORWARD_DEFINITION_WITH_FUNCTION_BODY");
                    }
                }
                else if (must_visit_body)
                {
                    common_method_node cmn = context.top_function as common_method_node;
                    if (cmn != null && !cmn.IsStatic)
                    {
                        //if (cmn.find_only_in_namespace(compiler_string_consts.self_word) == null)
                        //self variable
                        //добавляем self, обращение к ней заменяется на this_node
                        type_node self_type = cmn.cont_type;
                        if (cmn.cont_type.is_generic_type_definition)
                            self_type = cmn.cont_type.get_instance(cmn.cont_type.generic_params.ConvertAll<type_node>(o => (type_node)o));
                        local_variable lv = new local_variable(compiler_string_consts.self_word, self_type, cmn, null);
                        cmn.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(lv));
                        cmn.self_variable = lv;
                    }
                    if (_procedure_definition.proc_body != null)
                    {
                        hard_node_test_and_visit(_procedure_definition.proc_body);
                    }

                    add_clip_for_set(context.top_function);
                }

                if (!must_visit_body)
                {
                    context.add_method_header(_procedure_definition, context.top_function);
                }

            }
            //finally
            {
                if (_procedure_definition.proc_header.name.class_name != null)
                {
                    context.leave_type_method();
                }
                context.is_order_independed_method_description = false;
                context.leave_block();
            }
        }
		
        internal void add_clip_for_set(common_function_node cfn)
        {
        	statements_list sl = cfn.function_code as statements_list;
        	if (sl == null) return;
        	foreach (common_parameter prm in cfn.parameters)
        	{
        		if (prm.type.type_special_kind == SemanticTree.type_special_kind.set_type && prm.parameter_type == SemanticTree.parameter_type.value ||
        		   prm.is_params && prm.type.element_type.type_special_kind == SemanticTree.type_special_kind.set_type)
        		{
        			if (!prm.is_params)
        			{
        				ordinal_type_interface oti = prm.type.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        				if (oti == null)
        				if (prm.type.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
        				{
                            base_function_call cmc2 = null;
                            if (SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info is common_namespace_function_node)
                                cmc2 = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info as common_namespace_function_node, null);
                            else
                                cmc2 = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info as compiled_function_node, null);
        					cmc2.parameters.AddElement(new common_parameter_reference(prm,0,null));
        					cmc2.parameters.AddElement(new int_const_node((prm.type.element_type as short_string_type_node).Length,null));
        					sl.statements.AddElementFirst(cmc2);
        					continue;
        				}
        				else continue;
                        base_function_call cmc = null;
                        if (SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info is common_namespace_function_node)
                            cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info as common_namespace_function_node, null);
                        else
                            cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info as compiled_function_node, null);
        				cmc.parameters.AddElement(new common_parameter_reference(prm,0,null));
        			
        				cmc.parameters.AddElement(oti.lower_value);
        				cmc.parameters.AddElement(oti.upper_value);
        				sl.statements.AddElementFirst(cmc);
        			}
        			else
        			{
        				var_definition_node var = context.create_for_temp_variable(prm.type.element_type,null);
        				expression_node in_what = new common_parameter_reference(prm,0,null);
        				statements_list what_do = new statements_list(null);
        				ordinal_type_interface oti = prm.type.element_type.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        				bool short_str = false;
        				if (oti == null)
        				if (prm.type.element_type.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
        				{
        					short_str = true;
        				}
        				else continue;
        				base_function_call cmc = null;
                        if (!short_str)
                        {
                            if (SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info is common_namespace_function_node)
                                cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info as common_namespace_function_node, null);
                            else
                                cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipProcedure.sym_info as compiled_function_node, null);
                        }
                        else
                        {
                            if (SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info is common_namespace_function_node)
                                cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info as common_namespace_function_node, null);
                            else
                                cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetProcedure.sym_info as compiled_function_node, null);
                        }
                        if (var is local_block_variable)
        				    cmc.parameters.AddElement(new local_block_variable_reference(var as local_block_variable,null));
        				else
        				    cmc.parameters.AddElement(new local_variable_reference(var as local_variable,0,null));	
        				if (!short_str)
        				{
        					cmc.parameters.AddElement(oti.lower_value);
        					cmc.parameters.AddElement(oti.upper_value);
        				}
        				else
        				{
        					cmc.parameters.AddElement(new int_const_node((prm.type.element_type.element_type as short_string_type_node).Length,null));
        				}
        				what_do.statements.AddElement(cmc);
        				foreach_node fn = new foreach_node(var,in_what,what_do,null);
        				sl.statements.AddElementFirst(fn);
        			}
        		}
        		else if (prm.type.type_special_kind == SemanticTree.type_special_kind.short_string && prm.parameter_type == SemanticTree.parameter_type.value
        		        || prm.is_params && prm.type.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
        		{
                    if (!prm.is_params)
                    {
                        base_function_call cmc = null;
                        if (SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info is common_namespace_function_node)
                            cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node, null);
                        else
                            cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as compiled_function_node, null);
                        cmc.parameters.AddElement(new common_parameter_reference(prm, 0, null));
                        cmc.parameters.AddElement(new int_const_node((prm.type as short_string_type_node).Length, null));
                        concrete_parameter_type tmp_cpt = prm.concrete_parameter_type;
                        prm.concrete_parameter_type = concrete_parameter_type.cpt_none;
                        sl.statements.AddElementFirst(find_operator(compiler_string_consts.assign_name, new common_parameter_reference(prm, 0, null), cmc, null));
                        prm.concrete_parameter_type = tmp_cpt;
                    }
                    else
                    {
                        common_parameter_reference cpr = new common_parameter_reference(prm, 0, null);
                        compiled_function_node get_func = (prm.type.find_in_type("Length").sym_info as compiled_property_node).get_function as compiled_function_node;
                        local_variable var = context.create_for_temp_variable(SystemLibrary.SystemLibrary.integer_type, null) as local_variable;
                        local_variable len_var = context.create_for_temp_variable(SystemLibrary.SystemLibrary.integer_type, null) as local_variable;
                        statements_list body = new statements_list(null);
                        //compiled_property_node item_prop = prm.type.find_in_type("Item").sym_info as compiled_property_node;
                        base_function_call cmc = null;
                        if (SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info is common_namespace_function_node)
                            cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node, null);
                        else
                            cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as compiled_function_node, null);
                        expression_node cond = new basic_function_call(SystemLibrary.SystemLibrary.int_sm as basic_function_node, null, new local_variable_reference(var, 0, null), new local_variable_reference(len_var, 0, null));
                        //compiled_function_call get_item = new compiled_function_call(item_prop.get_function as compiled_function_node,cpr,null);
                        //get_item.parameters.AddElement(new local_variable_reference(var,0,null));
                        cmc.parameters.AddElement(new simple_array_indexing(cpr, new local_variable_reference(var, 0, null), prm.type.element_type, null));
                        cmc.parameters.AddElement(new int_const_node((prm.type.element_type as short_string_type_node).Length, null));

                        body.statements.AddElement(find_operator(compiler_string_consts.assign_name, new simple_array_indexing(cpr, new local_variable_reference(var, 0, null), prm.type.element_type, null), cmc, null));
                        body.statements.AddElement(new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node, null, new local_variable_reference(var, 0, null),
                                                                           new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node, null, new local_variable_reference(var, 0, null), new int_const_node(1, null))));


                        while_node wn = new while_node(cond, body, null);
                        sl.statements.AddElementFirst(wn);
                        sl.statements.AddElementFirst(new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node, null,
                                                                           new local_variable_reference(len_var, 0, null), new compiled_function_call(get_func, cpr, null)));
                    }	
        			
        		}
        	}
        }
        
        internal void CheckOverrideOrReintroduceExpectedWarning(location loc)
        {
            common_method_node cmn = context.top_function as common_method_node;
            if (!current_converted_method_not_in_class_defined && cmn != null && !cmn.IsReintroduce && cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_common)
                if (context.FindMethodToOverride(cmn) != null)
                {
                    WarningsList.Add(new OverrideOrReintroduceExpected(loc));
                }
        }

        public override void visit(SyntaxTree.function_header _function_header)
        {
            hard_node_test_and_visit(_function_header.name);
            if (context.converted_template_type != null)
            {
                return;
            }
            if (_function_header.template_args != null)
            {
                visit_generic_params(context.top_function, _function_header.template_args.idents);
            }
            SymbolInfo si = context.create_special_names();
            weak_node_test_and_visit(_function_header.parameters);
			type_node tn = null;
            if (_function_header.return_type == null)
            {
            	if (context.top_function.IsOperator)
                    AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            }
            if (_function_header.return_type != null)
            {
            	check_parameter_on_complex_type(_function_header.return_type);
            	tn = convert_strong(_function_header.return_type);
				//if (tn == SystemLibrary.SystemLibrary.void_type)
            	//    AddError(new VoidNotValid(get_location(_function_header.return_type)));
            	check_for_type_allowed(tn,get_location(_function_header.return_type));
            }
            //(ssyy) moved up, так как при проверке аттрибута override надо знать тип возвращаемого значения
            context.top_function.return_value_type = tn;
            assign_doc_info(context.top_function,_function_header);
            if (_function_header.attributes != null)
            {
            	make_attributes_for_declaration(_function_header, context.top_function);
            }
            if (context.converted_type != null && has_dll_import_attribute(context.top_function))
                AddError(get_dll_import_attribute(context.top_function).location, "DLLIMPORT_ATTRIBUTE_CANNOT_BE_APPLIED_TO_METHOD");
            if (_function_header.name.class_name != null) 
            	with_class_name = true;
            if (_function_header.class_keyword && !has_static_attr(_function_header.proc_attributes.proc_attributes))
            {
                SyntaxTree.procedure_attribute pa = new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static);
                pa.source_context = _function_header.source_context;
                _function_header.proc_attributes.proc_attributes.Add(pa);
            }
            weak_node_test_and_visit(_function_header.proc_attributes);
            if (context.top_function.IsOperator)
            {
                //if (cmmn == null)
                //{
                //    throw new OverloadOperatorMustBeStaticFunction(get_location(_function_header), context.top_function);
                //}
                if (context.top_function is common_method_node)
                {
                    common_method_node cmmn = context.top_function as common_method_node;
                    if (cmmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                    {
                        AddError(get_location(_function_header), "OVERLOADED_OPERATOR_MUST_BE_STATIC_FUNCTION");
                    }

                    if ((cmmn.name == compiler_string_consts.implicit_operator_name || cmmn.name == compiler_string_consts.explicit_operator_name))
                    {
                        if (!convertion_data_and_alghoritms.eq_type_nodes(tn, cmmn.comperehensive_type as type_node) && !convertion_data_and_alghoritms.eq_type_nodes(cmmn.comperehensive_type as type_node, cmmn.parameters[0].type))
                        {
                            AddError(get_location(_function_header.return_type), "RETURN_VALUE_SHOULD_HAVE_TYPE_{0}", (cmmn.comperehensive_type as type_node).PrintableName);
                        }
                        else if (convertion_data_and_alghoritms.eq_type_nodes(tn, cmmn.parameters[0].type))
                        {
                            AddError(get_location(_function_header), "CIRCURAL_TYPE_CONVERSION_DEFINITION");
                        }
                    }
                    else
                    {
                        int expected_params = 2;
                        if (cmmn.name == "not")
                            expected_params = 1;
                        if ((cmmn.name == "+" || cmmn.name == "-"))
                        {
                            if (cmmn.parameters.Count != 2 && cmmn.parameters.Count != 1)
                                AddError(new SimpleSemanticError(cmmn.loc, "OPERATORS_SHOULD_HAVE_1_OR_2_PARAMETERS"));
                        }
                        else if (cmmn.parameters.Count != expected_params)
                            AddError(new SimpleSemanticError(cmmn.loc, "OPERATORS_SHOULD_HAVE_{0}_PARAMETERS",expected_params));
                        bool has_types = false;
                        foreach (parameter p in cmmn.parameters)
                        {
                            type_node ptn = p.type;
                            if (ptn.is_generic_type_instance)
                                ptn = ptn.original_generic;
                            if (ptn == cmmn.cont_type)
                                has_types = true;
                        }
                        if (!has_types)
                            AddError(new SimpleSemanticError(cmmn.loc, "LEAST_ONE_PARAMETER_TYPE_SHOULD_EQ_DECLARING_TYPE_{0}",cmmn.cont_type.name));
                    }
                }
                else if (context.top_function is common_namespace_function_node)
                {
                    common_namespace_function_node cnfn = context.top_function as common_namespace_function_node;

                    if ((cnfn.name == compiler_string_consts.implicit_operator_name || cnfn.name == compiler_string_consts.explicit_operator_name))
                    {
                        if (!convertion_data_and_alghoritms.eq_type_nodes(tn, cnfn.ConnectedToType) && !convertion_data_and_alghoritms.eq_type_nodes(cnfn.ConnectedToType as type_node, cnfn.parameters[0].type))
                        {
                            AddError(get_location(_function_header.return_type), "RETURN_VALUE_SHOULD_HAVE_TYPE_{0}", (cnfn.ConnectedToType as type_node).PrintableName);
                        }
                        else if (convertion_data_and_alghoritms.eq_type_nodes(tn, cnfn.parameters[0].type))
                        {
                            AddError(get_location(_function_header), "CIRCURAL_TYPE_CONVERSION_DEFINITION");
                        }
                    }
                    else
                    {
                        int expected_params = 2;
                        if (cnfn.name == "not")
                            expected_params = 1;
                        if (cnfn.name == "+" || cnfn.name == "-")
                        {
                            if (cnfn.parameters.Count != 2 && cnfn.parameters.Count != 1)
                                AddError(new SimpleSemanticError(cnfn.loc, "OPERATORS_SHOULD_HAVE_1_OR_2_PARAMETERS"));
                        }
                        else if (cnfn.parameters.Count != expected_params)
                            AddError(new SimpleSemanticError(cnfn.loc, "OPERATORS_SHOULD_HAVE_{0}_PARAMETERS", expected_params));
                        bool has_types = false;
                        foreach (parameter p in cnfn.parameters)
                        {
                            type_node ptn = p.type;
                            if (ptn.is_generic_type_instance)
                                ptn = ptn.original_generic;
                            if (ptn == cnfn.ConnectedToType)
                                has_types = true;
                        }
                        if (!has_types)
                            AddError(new SimpleSemanticError(cnfn.loc, "LEAST_ONE_PARAMETER_TYPE_SHOULD_EQ_DECLARING_TYPE_{0}",cnfn.ConnectedToType.name));
                    }
                }
            }
			with_class_name = false;
			if (context.top_function != null && context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null && !context.top_function.IsOperator)
            {
                concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
                SemanticTree.parameter_type pt = PascalABCCompiler.SemanticTree.parameter_type.value;
                if ((context.top_function as common_namespace_function_node).ConnectedToType.is_value_type)
                {
                    cpt = concrete_parameter_type.cpt_var;
                    pt = PascalABCCompiler.SemanticTree.parameter_type.var;
                }
                
                type_node cur_tn = (context.top_function as common_namespace_function_node).ConnectedToType;
                /*if (cur_tn.is_generic_type_definition && cur_tn is compiled_type_node)
                {
                    List<type_node> instance_params = new List<type_node>();
                    compiled_type_node cur_ctn = cur_tn as compiled_type_node;
                    for (int i = 0; i < cur_ctn.instance_params.Count; i++)
                    {
                        instance_params.Add(find_type(cur_ctn.instance_params[i].name, null));
                    }
                    cur_tn = cur_tn.get_instance(instance_params);
                }*/
                type_node self_type = cur_tn;
                if (context.top_function.generic_params != null && self_type.is_generic_type_definition)
                {
                    self_type = self_type.get_instance(context.top_function.get_generic_params_list());
                }
                if (!has_extensionmethod_attr(_function_header.proc_attributes.proc_attributes))
                {
                    common_parameter cp = new common_parameter(compiler_string_consts.self_word, self_type, pt,
                                                                                context.top_function, cpt, null, null);
                    context.top_function.parameters.AddElementFirst(cp);
                    context.top_function.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(cp));
                }
            }
            CheckOverrideOrReintroduceExpectedWarning(get_location(_function_header));

            bool unique = context.close_function_params(body_exists);
            
            if (context.top_function.return_value_type == null)
                AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            if (_function_header.where_defs != null)
            {
                if (unique)
                {
                    visit_where_list(_function_header.where_defs);
                }
                else
                {
                    AddError(get_location(_function_header.where_defs), "WHERE_SECTION_MUST_BE_ONLY_IN_FIRST_DECLARATION");
                }
            }
            convertion_data_and_alghoritms.create_function_return_variable(context.top_function, si);

            /*if (_function_header.name != null && context.converted_compiled_type != null && context.top_function is common_namespace_function_node)
            {
                if (context.FindMethodToOverride(context.top_function as common_namespace_function_node) != null)
                    AddError(new CanNotDeclareExtensionMethodAsOverrided(get_location(_function_header)));
            }*/
            //TODO: Разобрать подробнее.
            if (!body_exists)
            {
                if ((context.top_function.semantic_node_type == semantic_node_type.common_method_node)
                    || ((context.func_stack_size_is_one()) && (_is_interface_part)))
                {
                    context.leave_block();
                }
            }
            body_exists = false;
        }

        bool has_static_attr(List<SyntaxTree.procedure_attribute> attrs)
        {
            foreach (SyntaxTree.procedure_attribute attr in attrs)
            {
                if (attr.attribute_type == PascalABCCompiler.SyntaxTree.proc_attribute.attr_static)
                {
                    return true;
                }
            }
            return false;
        }

        bool has_extensionmethod_attr(List<SyntaxTree.procedure_attribute> attrs)
        {
            foreach (SyntaxTree.procedure_attribute attr in attrs)
            {
                if (attr.attribute_type == PascalABCCompiler.SyntaxTree.proc_attribute.attr_extension)
                {
                    return true;
                }
            }
            return false;
        }

        private bool has_var_char_parameters(common_function_node fn)
        {
        	for (int i=0; i<fn.parameters.Count; i++)
        	{
        		if (fn.parameters[i].type == SystemLibrary.SystemLibrary.char_type && fn.parameters[i].parameter_type == SemanticTree.parameter_type.var)
        			return true;
        	}
        	return false;
        }

        private void visit_procedure_header(SyntaxTree.procedure_header _procedure_header)
        {
            if (_procedure_header.name != null && _procedure_header.name.meth_name is SyntaxTree.operator_name_ident &&
                !SyntaxTree.OperatorServices.IsAssigmentOperator((_procedure_header.name.meth_name as SyntaxTree.operator_name_ident).operator_type))
            {
                AddError(new OverloadOperatorCanNotBeProcedure(get_location(_procedure_header)));
            }
            if (_procedure_header is SyntaxTree.constructor &&
                (_procedure_header.template_args != null || (_procedure_header.name != null && _procedure_header.name.meth_name != null && _procedure_header.name.meth_name is SyntaxTree.template_type_name)))
            {
                AddError(get_location(_procedure_header), "CONSTRUCTOR_CAN_NOT_BE_GENERIC");
            }
            hard_node_test_and_visit(_procedure_header.name);
            if (context.converted_template_type != null)
            {
                return;
            }
            if (_procedure_header.template_args != null)
            {
                visit_generic_params(context.top_function, _procedure_header.template_args.idents);
            }
            weak_node_test_and_visit(_procedure_header.parameters);
            if (_procedure_header.name.class_name != null) 
            	with_class_name = true;
            if (_procedure_header.class_keyword && !has_static_attr(_procedure_header.proc_attributes.proc_attributes))
            {
                SyntaxTree.procedure_attribute pa = new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static);
                pa.source_context = _procedure_header.source_context;
                _procedure_header.proc_attributes.proc_attributes.Add(pa);
            }
            
            weak_node_test_and_visit(_procedure_header.proc_attributes);
			with_class_name = false;
            CheckOverrideOrReintroduceExpectedWarning(get_location(_procedure_header));
            if (context.top_function != null && context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null && !context.top_function.IsOperator)
            {
                concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
                SemanticTree.parameter_type pt = PascalABCCompiler.SemanticTree.parameter_type.value;
                if ((context.top_function as common_namespace_function_node).ConnectedToType.is_value_type)
                {
                    cpt = concrete_parameter_type.cpt_var;
                    pt = PascalABCCompiler.SemanticTree.parameter_type.var;
                }
                type_node cur_tn = (context.top_function as common_namespace_function_node).ConnectedToType;
                type_node self_type = cur_tn;
                if (context.top_function.generic_params != null && self_type.is_generic_type_definition)
                {
                    self_type = self_type.get_instance(context.top_function.get_generic_params_list());
                }
                if (!has_extensionmethod_attr(_procedure_header.proc_attributes.proc_attributes))
                {
                    common_parameter cp = new common_parameter(compiler_string_consts.self_word, self_type, pt,
                                                                                context.top_function, cpt, null, null);

                    context.top_function.parameters.AddElementFirst(cp);
                    context.top_function.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(cp));
                }
            }
            if (_procedure_header is SyntaxTree.constructor)
            {
                common_method_node cmnode = context.top_function as common_method_node;
                if (cmnode != null)
                {
                    cmnode.is_constructor = true;
                    cmnode.return_value_type = context.converted_type;
                }
                else if (context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null)
                    AddError(get_location(_procedure_header), "EXTENSION_CONSTRUCTOR_NOT_ALLOWED");
                else
                if (context.converted_compiled_type != null)
                    AddError(get_location(_procedure_header), "EXTENSION_CONSTRUCTOR_NOT_ALLOWED");
            }

            bool unique = context.close_function_params(body_exists);

            if (context.converted_type != null && context.converted_type.IsInterface)
            {
                if (body_exists)
                {
                    AddError(new InterfaceFunctionWithBody(get_location(_procedure_header)));
                }
                common_method_node cmnode = context.top_function as common_method_node;
                context.set_virtual_abstract(cmnode);
            }
			
            if (context.converted_type != null)
            {
            	common_method_node cmnode = context.top_function as common_method_node;
            	if (cmnode != null && cmnode.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract && body_exists)
            	{
            		AddError(new AbstractMethodWithBody(get_location(_procedure_header)));
            	}
            }
            if (_procedure_header.where_defs != null)
            {
                if (unique)
                {
                    visit_where_list(_procedure_header.where_defs);
                }
                else
                {
                    AddError(get_location(_procedure_header.where_defs), "WHERE_SECTION_MUST_BE_ONLY_IN_FIRST_DECLARATION");
                }
            }
            assign_doc_info(context.top_function,_procedure_header);
            if (_procedure_header.attributes != null)
            {
            	make_attributes_for_declaration(_procedure_header, context.top_function);
            }
            /*if (_procedure_header.name != null && context.converted_compiled_type != null && context.top_function is common_namespace_function_node)
            {
                if (context.FindMethodToOverride(context.top_function as common_namespace_function_node) != null)
                    AddError(new CanNotDeclareExtensionMethodAsOverrided(get_location(_procedure_header)));
            }*/
            if (context.converted_type != null && has_dll_import_attribute(context.top_function))
            	AddError(get_dll_import_attribute(context.top_function).location, "DLLIMPORT_ATTRIBUTE_CANNOT_BE_APPLIED_TO_METHOD");
            if (!body_exists)
            {
                if ((context.top_function.semantic_node_type == semantic_node_type.common_method_node)
                    || ((context.func_stack_size_is_one()) && (_is_interface_part)))
                {
                    context.leave_block();
                    if (_procedure_header.name.class_name != null && context.converted_compiled_type != null)
                        context.converted_compiled_type = null;
                }
            }
            body_exists = false;
           
        }

        public override void visit(SyntaxTree.procedure_header _procedure_header)
        {
            visit_procedure_header(_procedure_header);
        }

        public override void visit(SyntaxTree.procedure_attributes_list _procedure_attributes_list)
        {
            bool is_override = false;
            bool is_virtual = false;
            bool is_abstract = false;
            string override_proc_attr=null;
            string virtual_proc_attr=null;
            string abstract_proc_attr = null;
            for (int i = 0; i < _procedure_attributes_list.proc_attributes.Count; i++)
            {
                switch (_procedure_attributes_list.proc_attributes[i].attribute_type)
                {
                    case SyntaxTree.proc_attribute.attr_abstract:
                        {
                            is_abstract = true;
                            abstract_proc_attr = _procedure_attributes_list.proc_attributes[i].name;
                        }
                        break;
                    case SyntaxTree.proc_attribute.attr_override:
                        {
                            is_override = true;
                            override_proc_attr = _procedure_attributes_list.proc_attributes[i].name;
                        }
                        break;
                    case SyntaxTree.proc_attribute.attr_virtual:
                        {
                            is_virtual = true;
                            virtual_proc_attr = _procedure_attributes_list.proc_attributes[i].name;
                        }
                        break;
                }
            }
            context.top_function.is_overload = true;                                       // SSM 12/08/15
            context.last_created_function.symbol_kind = symbol_kind.sk_overload_function;

            for (int i = 0; i < _procedure_attributes_list.proc_attributes.Count; i++)
            {
                convertion_data_and_alghoritms.check_node_parser_error(_procedure_attributes_list.proc_attributes[i]);
                for (int j = 0; j < i; j++)
                {
                    if (_procedure_attributes_list.proc_attributes[i].attribute_type ==
                        _procedure_attributes_list.proc_attributes[j].attribute_type)
                    {
                        AddError(new DuplicateAttributeDefinition(get_location(_procedure_attributes_list.proc_attributes[j]),
                            get_location(_procedure_attributes_list.proc_attributes[i])));
                    }
                }
                
                switch (_procedure_attributes_list.proc_attributes[i].attribute_type)
                {
                    case SyntaxTree.proc_attribute.attr_overload:// ничего не делать - это и так есть
                        {
                		//	if (with_class_name && _procedure_attributes_list.proc_attributes[i].source_context != null) 
                		//		AddError(get_location(_procedure_attributes_list.proc_attributes[i]),"DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                		//	context.top_function.is_overload = true;
                        //    context.last_created_function.symbol_kind = symbol_kind.sk_overload_function;
                            break;
                        }
                    case SyntaxTree.proc_attribute.attr_forward:
                        {
                            if (with_class_name) AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                			//(ssyy) В интерфейсах аттрибуты недопустимы
                            if (context.converted_type != null && context.converted_type.IsInterface)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ATTRIBUTES_IN_INTERFACE_MEMBER");
                            }

                            break;
                        }
                    case SyntaxTree.proc_attribute.attr_virtual:
                        {
                            
                            if (with_class_name) AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                			//(ssyy) В интерфейсах аттрибуты недопустимы
                            if (context.converted_type != null && context.converted_type.IsInterface)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ATTRIBUTES_IN_INTERFACE_MEMBER");
                            }

                            //(ssyy) В записях нет виртуальных функций
                            if (context.converted_type != null && context.converted_type.is_value)
                            {
                                AddError(new VirtualMethodInRecord(
                                    get_location(_procedure_attributes_list.proc_attributes[i])));
                            }

                            common_function_node ccfn = context.top_function;
                            if (ccfn.semantic_node_type != semantic_node_type.common_method_node)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "{0}_KEYWORD_ALLOWED_ONLY_WITH_METHOD", _procedure_attributes_list.proc_attributes[i].name);
                            }
                            common_method_node cmn = ccfn as common_method_node;
                            if (cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "CAN_NOT_BE_VIRTUAL_STATIC_METHOD");
                            }
                            if (is_override)
                            {
                            	AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "USING_MODIFIERS{0}_{1}_TOGETHER_NOT_ALLOWED", cmn,_procedure_attributes_list.proc_attributes[i].name,override_proc_attr);
                            }
                            if (!context.converted_type.IsAbstract || true)
                            {
                                is_virtual = true;
                                virtual_proc_attr = _procedure_attributes_list.proc_attributes[i].name;
                                if (!is_abstract)
                                context.set_virtual(cmn);
                            }
                            break;
                        }
                    case SyntaxTree.proc_attribute.attr_override:
                    case SyntaxTree.proc_attribute.attr_reintroduce:
                        {
                			if (with_class_name) AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                			//(ssyy) В интерфейсах аттрибуты недопустимы
                            if (context.converted_type != null && context.converted_type.IsInterface)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ATTRIBUTES_IN_INTERFACE_MEMBER");
                            }

                            /*if (_procedure_attributes_list.proc_attributes[i].attribute_type == SyntaxTree.proc_attribute.attr_reintroduce && is_abstract)
                            {
                                AddError(new UsingModifiersTogetherNotAllowed(context.top_function, _procedure_attributes_list.proc_attributes[i].name, virtual_proc_attr, get_location(_procedure_attributes_list.proc_attributes[i])));
                            }*/

                            common_function_node ccfn = context.top_function;
                            if (ccfn.semantic_node_type != semantic_node_type.common_method_node)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "{0}_KEYWORD_ALLOWED_ONLY_WITH_METHOD", _procedure_attributes_list.proc_attributes[i].name);
                            }
                            common_method_node cmn = ccfn as common_method_node;
                            if (cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "CAN_NOT_BE_VIRTUAL_STATIC_METHOD");
                            }
                            if (_procedure_attributes_list.proc_attributes[i].attribute_type == SyntaxTree.proc_attribute.attr_reintroduce)
                            {
                                cmn.IsReintroduce = true;
                                break;
                            }
                            if (_procedure_attributes_list.proc_attributes[i].attribute_type == SyntaxTree.proc_attribute.attr_override && is_virtual)
                			{
                            	AddError(get_location(_procedure_attributes_list.proc_attributes[i]),"USING_MODIFIERS{0}_{1}_TOGETHER_NOT_ALLOWED", cmn,_procedure_attributes_list.proc_attributes[i].name,virtual_proc_attr);
                            }
                            if (_procedure_attributes_list.proc_attributes[i].attribute_type == SyntaxTree.proc_attribute.attr_override)
                			{
                				is_override = true;
                				override_proc_attr = _procedure_attributes_list.proc_attributes[i].name;
                			}
                            
                            context.set_override(cmn);
                            if (is_abstract)
                                cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual_abstract;
                            if (cmn.field_access_level < cmn.overrided_method.field_access_level)
                                AddError(cmn.loc, "CAN_NOT_BE_DOWN_ACCESS_LEVEL_FOR_METH");
                            break;
                        }
                    case SyntaxTree.proc_attribute.attr_static:
                        {
                            if (with_class_name)
                            {
                                if (context.top_function != null)
                                {
                                    //if (context.top_function.name.Length > compiler_string_consts.static_ctor_prefix.Length)
                                    //{
                                    //    string prefix = context.top_function.name.Substring(0, compiler_string_consts.static_ctor_prefix.Length);
                                    //    if (prefix == compiler_string_consts.static_ctor_prefix)
                                    //        break;
                                    //}
                                    context.top_function.polymorphic_state = PascalABCCompiler.SemanticTree.polymorphic_state.ps_static;
                                    break;
                                }
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                            }
                			//(ssyy) В интерфейсах аттрибуты недопустимы
                            if (context.converted_type != null && context.converted_type.IsInterface)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ATTRIBUTES_IN_INTERFACE_MEMBER");
                            }

                            common_function_node ccfn = context.top_function;
                            if (ccfn.semantic_node_type != semantic_node_type.common_method_node)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "STATIC_KEYWORD_ALLOWED_ONLY_WITH_METHOD");
                            }
                            common_method_node cmn = ccfn as common_method_node;
                            if (cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "CAN_NOT_BE_VIRTUAL_STATIC_METHOD");
                            }
                            if (cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "CANNOT_BE_ABSTRACT_STATIC_METHOD");
                            }
                            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_static;
                            break;
                        }
                	case SyntaxTree.proc_attribute.attr_abstract:
                		{
                			if (with_class_name) AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                			if (context.converted_type != null && context.converted_type.IsInterface)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ATTRIBUTES_IN_INTERFACE_MEMBER");
                            }
                			common_function_node ccfn = context.top_function;
                			common_method_node cmn = ccfn as common_method_node;
                			if (cmn.is_constructor)
                				AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "DIRECTIVE_{0}_NOT_ALLOWED", _procedure_attributes_list.proc_attributes[i].name);
                            if (ccfn.semantic_node_type != semantic_node_type.common_method_node)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "{0}_KEYWORD_ALLOWED_ONLY_WITH_METHOD", _procedure_attributes_list.proc_attributes[i].name);
                            }
                            if (context.converted_type != null && context.converted_type.is_value)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ABSTRACT_METHOD_IN_RECORD");
                            }
                            /*if (is_override)
                            {
                            	AddError(new UsingModifiersTogetherNotAllowed(ccfn,_procedure_attributes_list.proc_attributes[i].name,override_proc_attr,get_location(_procedure_attributes_list.proc_attributes[i])));
                            }*/
                			if (ccfn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                            {
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "CANNOT_BE_ABSTRACT_STATIC_METHOD");
                			}
                            cmn.polymorphic_state = SemanticTree.polymorphic_state.ps_virtual_abstract;
                            if (context.converted_type.IsSealed)
                                AddError(get_location(_procedure_attributes_list.proc_attributes[i]), "ABSTRACT_METHOD_IN_SEALED_CLASS");
                            context.converted_type.SetIsAbstract(true);
                            break;
                		}
                    case proc_attribute.attr_extension:
                        {
                            if (!(context.top_function is common_namespace_function_node))
                                AddError(get_location(_procedure_attributes_list), "EXTENSION_ATTRIBUTE_ONLY_FOR_NAMESPACE_FUNCTIONS_ALLOWED");
                            if (context.top_function.parameters.Count == 0)
                                AddError(context.top_function.loc, "EXTENSION_METHODS_MUST_HAVE_LEAST_ONE_PARAMETER");
                            if (context.top_function.parameters[0].parameter_type != SemanticTree.parameter_type.value)
                                AddError(context.top_function.loc, "FIRST_PARAMETER_SHOULDBE_ONLY_VALUE_PARAMETER");
                            if (context.top_function.parameters[0].name.ToLower() != compiler_string_consts.self_word)
                                AddError(context.top_function.loc,"FIRST_PARAMETER_MUST_HAVE_NAME_SELF");
                            common_namespace_function_node top_function = context.top_function as common_namespace_function_node;
                            top_function.ConnectedToType = context.top_function.parameters[0].type;
                            if (top_function.ConnectedToType.Scope == null && top_function.ConnectedToType is compiled_type_node)
                            {
                                (top_function.ConnectedToType as compiled_type_node).init_scope();
                            }
                            if (top_function.ConnectedToType.Scope == null)
                                AddError(context.top_function.loc, "EXTENSION_METHODS_FOR_CONSTRUCTED_TYPES_NOT_ALLOWED");
                            /*SymbolTable.Scope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(context.converted_namespace.scope, top_function.ConnectedToType.Scope);
                            top_function.scope = scope;*/
                            top_function.ConnectedToType.Scope.AddSymbol(top_function.name, new SymbolInfo(context.top_function));
                            if (top_function.ConnectedToType.type_special_kind == SemanticTree.type_special_kind.array_kind && top_function.ConnectedToType.element_type.is_generic_parameter)
                                top_function.ConnectedToType.base_type.Scope.AddSymbol(top_function.name, new SymbolInfo(context.top_function));
                            
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedError(get_location(_procedure_attributes_list.proc_attributes[i]));
                        }
                }
            }
        }
		
        private bool first_param=false;
        public override void visit(SyntaxTree.formal_parameters _formal_parametres)
        {
            bool default_value_met = false;
            bool params_met = false;
            first_param = true;
            foreach (SyntaxTree.typed_parameters tp in _formal_parametres.params_list)
            {
                if (params_met)
                {
                    AddError(get_location(tp), "ONLY_LAST_PARAMETER_CAN_BE_PARAMS");
                }
                if (tp.param_kind == PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr)
                {
                    params_met = true;
                    if (default_value_met)
                    {
                        AddError(get_location(tp), "FUNCTION_WITH_PARAMS_PARAMETER_CAN_NOT_HAVE_DEFAULT_PARAMETERS");
                    }
                }
                hard_node_test_and_visit(tp);
                first_param = false;
                if ((default_value_met) && (tp.inital_value == null))
                {
                    AddError(new NeedDefaultValueForParameter(get_location(tp)));
                }

                if (tp.inital_value != null)
                {
                    default_value_met = true;
                }
            }
            first_param = false;
            common_method_node cnode = context.top_function as common_method_node;
            if (cnode != null && cnode.IsOperator)
            {
                parameter_list pars = context.top_function.parameters;
                if (cnode.name != compiler_string_consts.implicit_operator_name && cnode.name != compiler_string_consts.explicit_operator_name)
                {
                    bool all_types_mismatch = true;
                    foreach (parameter par in pars)
                    {
                        //if (par.type == cnode.comperehensive_type)
                        if (convertion_data_and_alghoritms.eq_type_nodes(par.type, cnode.comperehensive_type as type_node))
                        {
                            all_types_mismatch = false;
                            break;
                        }
                    }
                    if (all_types_mismatch)
                    {
                        AddError(cnode.loc, "TYPE_OF_ONE_OR_MORE_OPERATOR_PARAMETERS_MUST_BE_{0}", ((type_node)(cnode.comperehensive_type)).PrintableName);
                    }
                }
                if (pars.Count > 0 && pars[pars.Count - 1].is_params)
                {
                    AddError(convertion_data_and_alghoritms.get_location(pars[pars.Count - 1]), "PARAMS_IN_OPERATOR");
                }
                int pcount = name_reflector.get_params_count(cnode.name);
                if (pcount != pars.Count)
                {
                    if (cnode.name != compiler_string_consts.minus_name && cnode.name != compiler_string_consts.plus_name)
                        AddError(cnode.loc, "OPERATOR_{0}_PARAMETERS_COUNT_MUST_EQUAL_{1}", cnode.name, pcount);
                    else
                    if (pars.Count != 1 && pars.Count != 2)
                        AddError(cnode.loc, "OPERATOR_{0}_PARAMETERS_COUNT_MUST_EQUAL_{1}", cnode.name, pcount);
                }
            }
        }

        public void check_parameter_on_complex_type(SyntaxTree.type_definition type)
        {
            SyntaxTree.array_type arr = type as SyntaxTree.array_type;
            if (type is SyntaxTree.class_definition || type is SyntaxTree.enum_type_definition ||
                (arr != null && arr.indexers != null && arr.indexers.indexers.Count > 0 && arr.indexers.indexers[0] != null))
            {
                AddError(get_location(type), "STRUCT_TYPE_DEFINITION_IN_FORMAL_PARAM");
            }
            if (context.top_function is common_method_node && (context.converted_type.IsInterface || context.top_function.polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract ) 
                && (type is function_header || type is procedure_header))
                AddError(get_location(type), "ANONYMOUS_DELEGATE_IN_INTERFACE_NOT_ALLOWED");
        }

        public override void visit(SyntaxTree.typed_parameters _typed_parametres)
        {
            SemanticTree.parameter_type par_type = SemanticTree.parameter_type.value;
            concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
            bool is_params = false;
            switch (_typed_parametres.param_kind)
            {
                case SyntaxTree.parametr_kind.const_parametr:
                    {
                        par_type = SemanticTree.parameter_type.value;
                        cpt = concrete_parameter_type.cpt_const;
                        break;
                    }
                case SyntaxTree.parametr_kind.none:
                    {
                        par_type = SemanticTree.parameter_type.value;
                        cpt = concrete_parameter_type.cpt_none;
                        break;
                    }
                case SyntaxTree.parametr_kind.out_parametr:
                    {
                        par_type = SemanticTree.parameter_type.var;
                        cpt = concrete_parameter_type.cpt_var;
                        break;
                    }
                case SyntaxTree.parametr_kind.var_parametr:
                    {
                        par_type = SemanticTree.parameter_type.var;
                        cpt = concrete_parameter_type.cpt_var;
                        break;
                    }
                case PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr:
                    {
                        is_params = true;
                        break;
                    }
            }
            common_parameter com_par = null;
            if (is_params)
            {
                if (_typed_parametres.idents.idents.Count != 1)
                {
                    AddError(get_location(_typed_parametres.idents.idents[1]), "ONLY_ONE_PARAMS_PARAMETER_ALLOWED");
                }
            }
            foreach (SyntaxTree.ident id in _typed_parametres.idents.idents)
            {
                com_par = context.add_parameter(id.name, par_type, cpt, get_location(id));
                if (_typed_parametres.attributes != null)
            	{
            		make_attributes_for_declaration(_typed_parametres, com_par);
            	}
                if (is_params)
                {
                    com_par.intrenal_is_params = true;
                }
            }

            if (_typed_parametres.vars_type == null)
                throw new NotSupportedError(get_location(_typed_parametres));

            //SyntaxTree.array_type arr = _typed_parametres.vars_type as SyntaxTree.array_type;
            //if (_typed_parametres.vars_type is SyntaxTree.class_definition || _typed_parametres.vars_type is SyntaxTree.enum_type_definition ||
            //    (arr!=null && arr.indexers!=null))
            //{
            //    AddError(new StructTypeDefinitionInFormalParam(get_location(_typed_parametres.vars_type)));
            //}
            check_parameter_on_complex_type(_typed_parametres.vars_type);
            type_node tn = convert_strong(_typed_parametres.vars_type);
            //if (tn == SystemLibrary.SystemLibrary.void_type)
            //    	AddError(new VoidNotValid(get_location(_typed_parametres.vars_type)));
			check_for_type_allowed(tn,get_location(_typed_parametres.vars_type));
            /*if (context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null && first_param)
            {
               if (tn != (context.top_function as common_namespace_function_node).ConnectedToType || _typed_parametres.param_kind != SyntaxTree.parametr_kind.none)
               {
               		AddError(new FirstParameterOfExtensionsMethodMustBeConnectedType(get_location(_typed_parametres.idents.idents[0])));
               } 	
            }*/
            if (is_params)
            {
                internal_interface ii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface);
                if (ii == null)
                {
                    AddError(get_location(_typed_parametres.vars_type), "ONLY_UNSIZED_ARRAY_PARAMS_PARAMETER_ALLOWED");
                }
            }
            com_par.type = tn;
            context.in_parameters_block = true;
            //TODO: Доделать параметры со значениями по умолчанию.
            if (_typed_parametres.inital_value != null)
            {
                if (_typed_parametres.idents.idents.Count != 1)
                {
                    AddError(new OnlyOneParameterNameWithDefaultValueAllowed(get_location(_typed_parametres)));
                }
                if (cpt == concrete_parameter_type.cpt_var)
                {
                    AddError(new VarParametersCanNotHaveDefaultValue(get_location(_typed_parametres)));
                }
                if (cpt == concrete_parameter_type.cpt_const)
                {
                    AddError(get_location(_typed_parametres), "CONST_PARAMETERS_CANNOT_HAVE_DEFAULT_VALUE");
                }
                context.top_function.num_of_default_variables = context.top_function.num_of_default_variables + 1;
                expression_node def_val_expr = convert_strong_to_constant_node(_typed_parametres.inital_value);
                com_par.default_value = convertion_data_and_alghoritms.convert_type(def_val_expr, tn);
            }
            context.in_parameters_block = false;
            context.close_var_definition_list(tn, null);
        }

        public override void visit(SyntaxTree.procedure_attribute _procedure_attribute)
        {
            throw new NotSupportedError(get_location(_procedure_attribute));
        }

        public override void visit(SyntaxTree.label_definitions _label_definitions)
        {
            /*if (context.converting_block() == block_type.type_block &&
                context.converted_type.IsInterface)
            {
                throw new InvalidInterfaceMember(get_location(_label_definitions));
            }*/
            foreach (SyntaxTree.ident id in _label_definitions.labels.idents)
            {
                context.add_label_declaration(id.name, get_location(id));
            }
        }

        private bool depended_from_generic_parameter(type_node tn)
        {
        	//Ivan 1.09.08 added
        	if (tn == null)
        		return false;
        	if (tn.is_generic_parameter)
                return true;
            if (tn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.set_type)
            {
                return depended_from_generic_parameter(tn.element_type);
            }
            if (tn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.array_kind)
            {
                return depended_from_generic_parameter(tn.element_type);
            }
            ref_type_node rtn = tn as ref_type_node;
            if (rtn != null)
            {
                return depended_from_generic_parameter(rtn.pointed_type);
            }
            generic_instance_type_node gitn = tn as generic_instance_type_node;
            if (gitn == null)
                return false;
            foreach (type_node param in gitn.generic_parameters)
            {
                if (depended_from_generic_parameter(param))
                    return true;
            }
            return false;
        }
		
        private bool is_unsized_array(SyntaxTree.indexers_types indexes, out int rank)
        {
        	rank = 1;
        	if (indexes == null)
        		return true;
        	bool wait_empty = false;
        	for (int i=0; i<indexes.indexers.Count; i++)
        	{
        		if (i == 0)
        		{
        			if (indexes.indexers[0] == null)
        			{
        				if (indexes.indexers.Count == 1)
                            AddError(get_location(indexes), "INVALID_ARRAY_REPRESENTATION");
        				wait_empty=true;
        			}
        		}
        		else
        		if (indexes.indexers[i] == null && !wait_empty || indexes.indexers[i] != null && wait_empty)
                    AddError(get_location(indexes), "INVALID_ARRAY_REPRESENTATION");
        		else
        		rank++;
        	}
        	return wait_empty;
        }
        
        public override void visit(SyntaxTree.array_type _array_type)
        {
        	int rank = 1;
        	if (is_unsized_array(_array_type.indexers, out rank))
            {
                type_node ret = null;
                type_node et = convert_strong(_array_type.elements_type);
                //if (et == SystemLibrary.SystemLibrary.void_type)
            	//AddError(new VoidNotValid(get_location(_array_type.elemets_types)));
                check_for_type_allowed(et,get_location(_array_type.elements_type));
                ret = convertion_data_and_alghoritms.type_constructor.create_unsized_array(et,
                    context.converted_namespace, rank, get_location(_array_type));
                return_value(ret);
                return;
            }
            type_node_list ind_types = new type_node_list();
            foreach (SyntaxTree.type_definition td in _array_type.indexers.indexers)
            {
                type_node tn = convert_strong(td);
                internal_interface ii = tn.get_internal_interface(internal_interface_kind.ordinal_interface);
                if (ii == null /*|| ((tn is common_type_node) && (tn as common_type_node).IsEnum)*/)
                {
                    AddError(new OrdinalTypeExpected(get_location(td)));
                }
                if (tn.type_special_kind == SemanticTree.type_special_kind.diap_type)
                {
                	if (tn.base_type == SystemLibrary.SystemLibrary.uint_type || tn.base_type == SystemLibrary.SystemLibrary.int64_type || 
                	   tn.base_type == SystemLibrary.SystemLibrary.uint64_type)
                	//ordinal_type_interface oti = tn.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    AddError(get_location(td), "RANGE_TOO_LARGE");
                }
                else if (tn == SystemLibrary.SystemLibrary.uint_type || tn == SystemLibrary.SystemLibrary.int64_type ||
                        tn == SystemLibrary.SystemLibrary.uint64_type)
                    AddError(get_location(td), "RANGE_TOO_LARGE");
                ind_types.AddElement(tn);
            }
            type_node elem_type = convert_strong(_array_type.elements_type);
            //TODO: Сделать и массивы, связанные с generics!
            if (depended_from_generic_parameter(elem_type))
            {
                throw new NotSupportedError(get_location(_array_type.elements_type));
            }
            //if (elem_type == SystemLibrary.SystemLibrary.void_type)
            //	AddError(new VoidNotValid(get_location(_array_type.elemets_types)));
            check_for_type_allowed(elem_type,get_location(_array_type.elements_type));
            for (int i = ind_types.Count - 1; i >= 0; i--)
            {
                internal_interface ii = ind_types[i].get_internal_interface(internal_interface_kind.ordinal_interface);
                ordinal_type_interface oti_ind = (ordinal_type_interface)ii;
                elem_type = convertion_data_and_alghoritms.type_constructor.get_array_type(oti_ind, elem_type,
                    context.converted_namespace, get_location(_array_type));
            }
            return_value(elem_type);
        }

        public override void visit(SyntaxTree.indexers_types _indexers_types)
        {
            throw new NotSupportedError(get_location(_indexers_types));
        }

        private constant_node convert_strong_to_constant_node(SyntaxTree.expression expr)
        {
            expression_node exp = convert_strong(expr);
            return convert_strong_to_constant_node(exp, exp.type);
        }

        public expression_node convert_strong_to_constant_or_function_call_for_varinit(expression_node exp)
        {
            return convert_strong_to_constant_or_function_call_for_varinit(exp, exp.type);
        }
        
        private expression_node convert_strong_to_constant_or_function_call_for_varinit(expression_node exp, type_node tn)
        {
            if (exp is constant_node)
                return convert_strong_to_constant_node(exp, tn);
            if (exp is array_initializer)
            {
            	//if (tn.type_special_kind != SemanticTree.type_special_kind.array_wrapper)
            	{
            		//exp.type = DeduceType(exp.type,exp.location);
            		exp = ConvertArrayInitializer(tn, exp as array_initializer);
            		return exp;
            		//return convertion_data_and_alghoritms.convert_type(exp,tn);
            	}
//            	else
//            	{
//            		exp.type = tn;
//            		return exp;
//            	}
            }
            else if (exp is compiled_static_method_call && (exp.type.type_special_kind == SemanticTree.type_special_kind.set_type || exp.type.type_special_kind == SemanticTree.type_special_kind.base_set_type))
            {
                if (!(tn == exp.type) && !type_table.is_derived(tn, exp.type))
                {
                    AddError(new CanNotConvertTypes(exp, exp.type, tn, exp.location));
                }
                if (tn.element_type != null)
                {
                    ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    if (oti != null)
                    {
                        compiled_static_method_call cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as compiled_function_node, null);
                        cmc.parameters.AddElement(exp);
                        cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
                        cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
                        cmc.ret_type = tn;
                        return new compiled_static_method_call_as_constant(cmc, null);
                    }
                    else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    {
                        compiled_static_method_call cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as compiled_function_node, null);
                        cmc.parameters.AddElement(exp);
                        cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length, null));
                        cmc.ret_type = tn;
                        return new compiled_static_method_call_as_constant(cmc, null);
                    }
                }
                return new compiled_static_method_call_as_constant(exp as compiled_static_method_call, null);
            }
            else if (exp is common_namespace_function_call && (exp.type.type_special_kind == SemanticTree.type_special_kind.set_type || exp.type.type_special_kind == SemanticTree.type_special_kind.base_set_type))
            {
                if (!(tn == exp.type) && !type_table.is_derived(tn, exp.type))
                {
                    AddError(new CanNotConvertTypes(exp, exp.type, tn, exp.location));
                }
                if (tn.element_type != null)
                {
                    ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    if (oti != null)
                    {
                        common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as common_namespace_function_node, null);
                        cmc.parameters.AddElement(exp);
                        cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
                        cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
                        cmc.ret_type = tn;
                        return new common_namespace_function_call_as_constant(cmc, null);
                    }
                    else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    {
                        common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as common_namespace_function_node, null);
                        cmc.parameters.AddElement(exp);
                        cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length, null));
                        cmc.ret_type = tn;
                        return new common_namespace_function_call_as_constant(cmc, null);
                    }
                }
                return new common_namespace_function_call_as_constant(exp as common_namespace_function_call, null);
            }
            else if (exp is record_initializer)
            {
                if (tn is common_type_node)
                    exp = ConvertRecordInitializer(tn as common_type_node, exp as record_initializer);
                else if (tn is compiled_type_node)
                    throw new NotSupportedError(exp.location);
                return exp;
            }
            switch (SemanticRules.VariableInitializationParams)
            {
                case VariableInitializationParams.ConstructorCall:
                    if ((exp is common_constructor_call) || (exp is compiled_constructor_call))
                        return convertion_data_and_alghoritms.convert_type(exp, tn);
                    break;
                case VariableInitializationParams.Expression:
                    if (exp.type != null)
                        return convertion_data_and_alghoritms.convert_type(exp, tn);
                    break;
            }
            return convert_strong_to_constant_node(exp, tn);
        }

        private constant_node convert_strong_to_constant_node(SyntaxTree.expression expr, type_node tn)
        {
        	return convert_strong_to_constant_node(convert_strong(expr), tn);
        }

        private void check_set_for_constant(common_namespace_function_call cnfc)
        {
            expressions_list exprs = null;
            if (cnfc.function_node == SystemLibrary.SystemLibInitializer.CreateSetProcedure.sym_info as common_namespace_function_node)
                exprs = get_set_initializer(cnfc);
            else exprs = cnfc.parameters;
            foreach (expression_node en in exprs)
                if (en is common_namespace_function_call)
                {
                    cnfc = en as common_namespace_function_call;
                    if (cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.SetUnionProcedure.sym_info as common_namespace_function_node
                                              || cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.SetSubtractProcedure.sym_info as common_namespace_function_node
                                              || cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateSetProcedure.sym_info as common_namespace_function_node
                                              || cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.SetIntersectProcedure.sym_info as common_namespace_function_node
                                              || cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateDiapason.sym_info as common_namespace_function_node
                                              || cnfc.function_node == PascalABCCompiler.SystemLibrary.SystemLibInitializer.CreateObjDiapason.sym_info as common_namespace_function_node)
                        check_set_for_constant(en as common_namespace_function_call);
                    else AddError(en.location, "CONSTANT_EXPRESSION_EXPECTED");
                }
                else
                    if (!(en is constant_node /*|| en is statements_expression_node*/)) AddError(en.location, "CONSTANT_EXPRESSION_EXPECTED");
        }

        private constant_node convert_strong_to_constant_node(expression_node expr, type_node tn)
        {
            location loc = expr.location;
            constant_node constant = null;
            try_convert_typed_expression_to_function_call(ref expr);
            if (expr is null_const_node) 
            {
            	if (!(tn is null_type_node) && !type_table.is_with_nil_allowed(tn))
                    AddError(loc, "NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
            	return null_const_node.get_const_node_with_type(tn, expr as null_const_node);
            }
            if (expr is compiled_static_method_call)
            {
                compiled_static_method_call csmc = expr as compiled_static_method_call;

                if (/*csmc.parameters.Count == 0 &&*/ csmc.type != null && csmc.type != SystemLibrary.SystemLibrary.void_type)
                    constant = new compiled_static_method_call_as_constant(csmc, expr.location);
            }
            else if (expr is common_namespace_function_call && (expr as common_namespace_function_call).function_node == SystemLibrary.SystemLibInitializer.CreateSetProcedure.sym_info as common_namespace_function_node)
            {
                common_namespace_function_call cnfc = expr as common_namespace_function_call;
                expressions_list exprs = get_set_initializer(cnfc);
                check_set_for_constant(cnfc);
                foreach (expression_node en in exprs)
                {
                    if (en is common_namespace_function_call)
                    {
                        common_namespace_function_call cnfc2 = en as common_namespace_function_call;
                        check_set_for_constant(cnfc2);
                    }
                    else
                        if (!(en is constant_node)) AddError(loc, "CONSTANT_EXPRESSION_EXPECTED");
                }
                constant = new common_namespace_function_call_as_constant(cnfc, loc);
            }
            else if (expr is common_namespace_function_call)
            {
                common_namespace_function_call cnfc=expr as common_namespace_function_call;
                //if (cnfc.function_node.namespace_node == context.converted_namespace)
                  //  throw new ConstantExpressionExpected(loc);
                constant = new common_namespace_function_call_as_constant(expr as common_namespace_function_call, loc);
            }
            else if (expr is basic_function_call)
            {
            	basic_function_call cnfc=expr as basic_function_call;
                //if (cnfc.function_node.namespace_node == context.converted_namespace)
                  //  throw new ConstantExpressionExpected(loc);
                constant = new basic_function_call_as_constant(expr as basic_function_call, loc);
            }
            else if (expr is typed_expression)
            {
            	expr = convertion_data_and_alghoritms.convert_type(expr, tn);
            	if (expr is common_constructor_call)
            	{
            		constant = new common_constructor_call_as_constant(expr as common_constructor_call, null);
            	}
            	else
            	if (expr is typed_expression)
            	{
            		if (const_def_type != null)
            		{
            			expr = convertion_data_and_alghoritms.convert_type(expr, const_def_type);
            			tn = const_def_type;
            			constant = new common_constructor_call_as_constant(expr as common_constructor_call, null);
            		}
            		else
            		{
            			base_function_call bfc = ((expr as typed_expression).type as delegated_methods).proper_methods[0];
            			common_type_node del =
            				convertion_data_and_alghoritms.type_constructor.create_delegate(context.get_delegate_type_name(), bfc.simple_function_node.return_value_type, bfc.simple_function_node.parameters, context.converted_namespace, null);
            			context.converted_namespace.types.AddElement(del);
            			tn = del;
            			expr = convertion_data_and_alghoritms.explicit_convert_type(expr, del);
            			expr.type = tn;
            			constant = new common_constructor_call_as_constant(expr as common_constructor_call, null);
            		}
            	}
            }
            
            else if (expr is namespace_constant_reference)
            {
            	constant = (expr as namespace_constant_reference).constant.const_value;
            	convertion_data_and_alghoritms.check_convert_type(constant,tn,expr.location);
            	if ((tn.type_special_kind == SemanticTree.type_special_kind.set_type || tn.type_special_kind == SemanticTree.type_special_kind.base_set_type) && tn.element_type != null)
                {
                    ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    if (oti != null)
                    {
                    	common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as common_namespace_function_node,null);
        				cmc.parameters.AddElement(expr);
        				cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
        				cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
        				cmc.ret_type = tn;
        				constant = new common_namespace_function_call_as_constant(cmc,null);
                    }
                    else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    {
                    	common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as common_namespace_function_node,null);
        				cmc.parameters.AddElement(expr);
        				cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length,null));
        				cmc.ret_type = tn;
        				constant = new common_namespace_function_call_as_constant(cmc,null);
                    }
                 }
            	else
            	if (tn.type_special_kind == SemanticTree.type_special_kind.short_string)
            	{
            		/*common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node,null);
        			cmc.parameters.AddElement(expr);
        			cmc.parameters.AddElement(new int_const_node((tn as short_string_type_node).Length,null));*/
            		expression_node cmc = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(expr,SystemLibrary.SystemLibrary.string_type),new int_const_node((tn as short_string_type_node).Length,null));
        			constant = new common_namespace_function_call_as_constant(cmc as common_namespace_function_call,null);
            	}
            	/*expression_node e = convertion_data_and_alghoritms.convert_type(constant.get_constant_copy(expr.location), tn);
            	switch (e.semantic_node_type)
                {
                   case semantic_node_type.compiled_constructor_call:
                      constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                      break;
                   default: 
                      constant = e as constant_node;
                      break;
               }*/
            	/*if (constant.get_object_value() != null)
            	{
            		//if (const_def_type != null)
            		{
            			expression_node e = convertion_data_and_alghoritms.convert_type(constant.get_constant_copy(expr.location), tn);
            			switch (e.semantic_node_type)
                    	{
                        	case semantic_node_type.compiled_constructor_call:
                            	constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                            	break;
                        	default: 
                            	constant = e as constant_node;
                           	 	break;
                    	}
            		}
            	}
            	else
            	{
            		//if (const_def_type != null)
            		{
            			expression_node e = convertion_data_and_alghoritms.convert_type(expr, tn);
            			switch (e.semantic_node_type)
                    	{
                        	case semantic_node_type.compiled_constructor_call:
                            	constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                            	break;
                        	default: 
                            	constant = e as constant_node;
                           	 	break;
                    	}
            		}
            	}*/
            	return constant;
            }
            else if (expr is function_constant_reference)
            {
            	constant = (expr as function_constant_reference).constant.const_value;
            	convertion_data_and_alghoritms.check_convert_type(constant,tn,expr.location);
            	if ((tn.type_special_kind == SemanticTree.type_special_kind.set_type || tn.type_special_kind == SemanticTree.type_special_kind.base_set_type) && tn.element_type != null)
                {
                    ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    if (oti != null)
                    {
                    	common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as common_namespace_function_node,null);
        				cmc.parameters.AddElement(expr);
        				cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
        				cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
        				cmc.ret_type = tn;
        				constant = new common_namespace_function_call_as_constant(cmc,null);
                    }
                    else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    {
                    	common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as common_namespace_function_node,null);
        				cmc.parameters.AddElement(expr);
        				cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length,null));
        				cmc.ret_type = tn;
        				constant = new common_namespace_function_call_as_constant(cmc,null);
                    }
                 }
            	else
            	if (tn.type_special_kind == SemanticTree.type_special_kind.short_string)
            	{
            		/*common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node,null);
        			cmc.parameters.AddElement(expr);
        			cmc.parameters.AddElement(new int_const_node((tn as short_string_type_node).Length,null));*/
            		expression_node cmc = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(expr,SystemLibrary.SystemLibrary.string_type),new int_const_node((tn as short_string_type_node).Length,null));
        			constant = new common_namespace_function_call_as_constant(cmc as common_namespace_function_call,null);
            	}
            	/*expression_node e = convertion_data_and_alghoritms.convert_type(constant.get_constant_copy(expr.location), tn);
            	switch (e.semantic_node_type)
                {
                        	case semantic_node_type.compiled_constructor_call:
                            	constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                            	break;
                        	default: 
                            	constant = e as constant_node;
                           	 	break;
                }*/
            	/*if (constant.get_object_value() != null)
            	{
            		//if (const_def_type != null)
            		{
            			expression_node e = convertion_data_and_alghoritms.convert_type(constant.get_constant_copy(expr.location), tn);
            			switch (e.semantic_node_type)
                    	{
                        	case semantic_node_type.compiled_constructor_call:
                            	constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                            	break;
                        	default: 
                            	constant = e as constant_node;
                           	 	break;
                    	}
            		}
            	}
            	else
            	{
            		//if (const_def_type != null)
            		{
            			expression_node e = convertion_data_and_alghoritms.convert_type(expr, tn);
            			switch (e.semantic_node_type)
                    	{
                        	case semantic_node_type.compiled_constructor_call:
                            	constant = new compiled_constructor_call_as_constant(e as compiled_constructor_call, loc);
                            	break;
                        	default: 
                            	constant = e as constant_node;
                           	 	break;
                    	}
            		}
            	}*/
            	return constant;
            }
            else if (expr is static_compiled_variable_reference && !(expr as static_compiled_variable_reference).var.IsLiteral)
            {
                constant = new compiled_static_field_reference_as_constant(expr as static_compiled_variable_reference, null);
                return constant;
            }
            else
            {
                constant = expr as constant_node;
                if (tn.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                    /*common_namespace_function_call cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node,null);
                    cmc.parameters.AddElement(expr);
                    cmc.parameters.AddElement(new int_const_node((tn as short_string_type_node).Length,null));*/
                    expression_node cmc = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node, null, convertion_data_and_alghoritms.convert_type(expr, SystemLibrary.SystemLibrary.string_type), new int_const_node((tn as short_string_type_node).Length, null));
                    if (cmc is common_namespace_function_call)
                        constant = new common_namespace_function_call_as_constant(cmc as common_namespace_function_call, null);
                    else
                        constant = new compiled_static_method_call_as_constant(cmc as compiled_static_method_call, null);
                }
                //else
                //constant = convertion_data_and_alghoritms.convert_type(constant, tn) as constant_node;
                if (expr is static_compiled_variable_reference)
                {
                    compiled_class_constant_definition cccd = NetHelper.NetHelper.ConvertToConstant(((static_compiled_variable_reference)expr).var);
                    if (cccd != null)
                        constant = cccd.const_value;
                    else
                        constant = new compiled_static_field_reference_as_constant(expr as static_compiled_variable_reference, null);
                }

            }
            if (constant == null)
                AddError(loc, "CONSTANT_EXPRESSION_EXPECTED");
            if (IsBoundedArray(tn) || IsUnsizedArray(tn))
            {
                if (!(constant is array_const))
                    AddError(loc, "ARRAY_CONST_EXPECTED");
                constant = ConvertArrayConst(tn, constant as array_const);
            }
            else
                if (tn is common_type_node)
                {
                    if (tn.type_special_kind != SemanticTree.type_special_kind.set_type && tn.type_special_kind != SemanticTree.type_special_kind.base_set_type && tn.type_special_kind != SemanticTree.type_special_kind.diap_type)
                    {
                        if (!tn.is_value)
                        {
                        	if (expr is common_constructor_call)
                        		return constant;
                        	if (expr is common_constructor_call_as_constant)
                        		return expr as common_constructor_call_as_constant;
                        	convertion_data_and_alghoritms.check_convert_type(expr,tn,expr.location);
                        	//AddError(new CanNotConvertTypes(expr,expr.type,tn,expr.location));
                        	//throw new NotSupportedError(loc);
                        }
                        if ((tn as common_type_node).IsEnum)
                        {
                            convertion_data_and_alghoritms.check_convert_type(expr,tn,expr.location);
                        	enum_const_node ecn = expr as enum_const_node;
                            if (ecn == null) AddError(expr.location, "CONSTANT_EXPRESSION_EXPECTED");
                            return ecn;
                        }
                        if (!(constant is record_constant))
                            AddError(loc, "RECORD_CONST_EXPECTED");
                        constant = ConvertRecordConst(tn as common_type_node, constant as record_constant);
                    }
                    else if (tn.type_special_kind == SemanticTree.type_special_kind.diap_type)
                    {
                    	constant = expr as constant_node;
                    	if (constant == null)
                            AddError(expr.location, "CONSTANT_EXPRESSION_EXPECTED");
                    	//constant = (expr as namespace_constant_reference).constant.const_value;
            			convertion_data_and_alghoritms.check_convert_type(constant,tn,expr.location);
                        ordinal_type_interface oti = tn.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                        bool success = false;
                        int val = convertion_data_and_alghoritms.convert_type_to_int_constant(constant, out success);
                        if (success)
                        {
                            int left = convertion_data_and_alghoritms.convert_type_to_int_constant(oti.lower_value, out success);
                            if (success)
                            {
                                int right = convertion_data_and_alghoritms.convert_type_to_int_constant(oti.upper_value, out success);
                                if (success)
                                if (val < left || val > right)
                                    AddError(loc, "OUT_OF_RANGE");
                            }
                        }
                    }
                    else
                    {
                    	//obrezka konstantnogo mnozhestva
                    	if (!(tn == expr.type) && !type_table.is_derived(tn,expr.type))
                    	{
                    		AddError(new CanNotConvertTypes(expr,expr.type,tn,expr.location));
                    	}
                    	if (tn.element_type != null)
                    	{
                    		ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                    		if (oti != null)
                    		{
                                base_function_call cmc = null;
                                if (SystemLibrary.SystemLibInitializer.ClipFunction.sym_info is common_namespace_function_node)
                                    cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as common_namespace_function_node, null);
        						else
                                    cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as compiled_function_node, null);
                                cmc.parameters.AddElement(expr);
        						cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
        						cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
        						cmc.ret_type = tn;
                                if (cmc is common_namespace_function_call)
                                    constant = new common_namespace_function_call_as_constant(cmc as common_namespace_function_call, null);
                                else
                                    constant = new compiled_static_method_call_as_constant(cmc as compiled_static_method_call, null);
                    		}
                    		else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                    		{
                                base_function_call cmc = null;
                                if (SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info is common_namespace_function_node)
                                    cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as common_namespace_function_node, null);
        						else
                                    cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as compiled_function_node, null);
                                cmc.parameters.AddElement(expr);
        						cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length,null));
        						cmc.ret_type = tn;
                                if (cmc is common_namespace_function_call)
                                    constant = new common_namespace_function_call_as_constant(cmc as common_namespace_function_call, null);
                                else
                                    constant = new compiled_static_method_call_as_constant(cmc as compiled_static_method_call, null);
                    		}
                    	}
                    }
                }
                else
                {
                    expression_node exprc = convertion_data_and_alghoritms.convert_type(constant, tn);
                    switch (exprc.semantic_node_type)
                    {
                        case semantic_node_type.compiled_constructor_call:
                            constant = new compiled_constructor_call_as_constant(exprc as compiled_constructor_call, loc);
                            break;
                        case semantic_node_type.compiled_static_method_call:
                            constant = new compiled_static_method_call_as_constant(exprc as compiled_static_method_call, loc);
                            break;
                        case semantic_node_type.basic_function_call:
                            constant = new basic_function_call_as_constant(exprc as basic_function_call, loc);
                            break;
                        default: 
                            constant = exprc as constant_node;
                            break;
                    }
                }
            return constant;
        }

        internal common_type_node CreateDelegate(function_node fn)
        {
            common_type_node del =
                convertion_data_and_alghoritms.type_constructor.create_delegate(context.get_delegate_type_name(), fn.return_value_type, fn.parameters, context.converted_namespace, null);
            context.converted_namespace.types.AddElement(del);
            return del;
        }

        internal expression_node CreateDelegateCall(base_function_call fn_call)
        {
            common_type_node del = CreateDelegate(fn_call.simple_function_node);
            common_constructor_call deleg_costructor_call = new common_constructor_call(del.methods[0], fn_call.location);
            deleg_costructor_call.parameters.AddElement((base_function_call)fn_call);
            return deleg_costructor_call;
        }

        private record_initializer ConvertRecordInitializer(common_type_node ctn, record_initializer constant)
        {
        	location loc = constant.location;
            if (!ctn.is_value_type)
                AddError(loc, "RECORD_CONST_NOT_ALLOWED_{0}", ctn.name);
            if (ctn.fields.Count != constant.record_const_definition_list.Count)
                AddError(loc, "INVALID_RECORD_CONST_FIELD_COUNT");
            constant.type = ctn;
            constant.field_values.Clear();
            for (int i = 0; i < ctn.fields.Count; i++)
            {
                class_field cf = ctn.fields[i];
                if (cf.name.ToLower() != constant.record_const_definition_list[i].name.name.ToLower())
                    AddError(get_location(constant.record_const_definition_list[i].name), "FIELD_NAME_SHOULD_BE_EQUAL_RECORD_FIELD_NAME", cf.name);
                expression_node en = convert_strong_to_constant_or_function_call_for_varinit(convert_strong(constant.record_const_definition_list[i].val),cf.type);
                en.type = cf.type;
                constant.field_values.Add(en);
            }
            return constant;
        }
        
        private array_initializer ConvertNDimArrayInitializer(type_node tn, int cur_rank, type_node element_type, array_initializer constant)
        {
        	array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
        	int rank = aii.rank;
        	for (int i=0; i<constant.element_values.Count; i++)
        	{
        		expression_node e = constant.element_values[i];
        		if (e is array_initializer)
        		{
        			if (cur_rank>=rank)
        				constant.element_values[i] = ConvertArrayInitializer(tn.element_type,e as array_initializer);
        				//AddError(new CanNotConvertTypes(e,e.type,tn.element_type,e.location));
        			else
        			constant.element_values[i] = ConvertNDimArrayInitializer(tn,cur_rank+1,element_type,e as array_initializer);
        		}
        		else if (e is record_initializer)
            	{
            		if (element_type is common_type_node)
            			constant.element_values[i] = ConvertRecordInitializer(element_type as common_type_node, e as record_initializer);
            		else throw new NotSupportedError(constant.element_values[i].location);
            	}
        		else
            	{
        			if (cur_rank != rank)
                        AddError(constant.location, "RANK_MISMATCH_IN_INITILIALIZER");
        			constant.element_values[i] = convertion_data_and_alghoritms.convert_type(constant.element_values[i], element_type);
            	}
        	}
        	constant.type = tn;
        	return constant;
        }
        
        private array_initializer ConvertArrayInitializer(type_node tn, array_initializer constant)
        {
        	type_node element_type = null;
            if (IsBoundedArray(tn))
            {
                bounded_array_interface bai = tn.get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
                element_type = bai.element_type;
                ordinal_type_interface oti_indexer = bai.ordinal_type_interface;
                int arr_length = oti_indexer.ordinal_type_to_int(oti_indexer.upper_value) - oti_indexer.ordinal_type_to_int(oti_indexer.lower_value) + 1;
                if (arr_length != constant.element_values.Count)
                    AddError(constant.location, "ARRAY_CONST_{0}_ELEMENTS_EXPECTED", arr_length);
            }
            else
                if (IsUnsizedArray(tn))
                {
                    array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                    element_type = aii.element_type;
                    if (aii.rank > 1)
                    {
                    	array_initializer cnst = ConvertNDimArrayInitializer(tn,1,element_type,constant);
                    	cnst.type = tn;
                    	return cnst;
                    }
                }
                else
                	AddError(new CanNotConvertTypes(constant,constant.type,tn,constant.location));//CompilerInternalError("Unexpected array type");
            for (int i = 0; i < constant.element_values.Count; i++)
            if (constant.element_values[i] is array_initializer)
            	constant.element_values[i] = ConvertArrayInitializer(element_type, constant.element_values[i] as array_initializer);
            else if (constant.element_values[i] is record_initializer)
            {
            	if (element_type is common_type_node)
            		constant.element_values[i] = ConvertRecordInitializer(element_type as common_type_node, constant.element_values[i] as record_initializer);
            	else throw new NotSupportedError(constant.element_values[i].location);
            }
            else
            {
            	constant.element_values[i] = convertion_data_and_alghoritms.convert_type(constant.element_values[i], element_type);
            }
            constant.type = tn;
            return constant;
        }
        
        private array_const ConvertNDimArrayConst(type_node tn,  int cur_rank, type_node element_type, array_const constant)
        {
        	array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
        	int rank = aii.rank;
        	for (int i=0; i<constant.element_values.Count; i++)
        	{
        		expression_node e = constant.element_values[i];
        		if (e is array_const)
        		{
        			if (cur_rank>=rank)
        				constant.element_values[i] = ConvertArrayConst(tn.element_type,e as array_const);
        				//AddError(new CanNotConvertTypes(e,e.type,tn.element_type,e.location));
        			else
        			constant.element_values[i] = ConvertNDimArrayConst(tn,cur_rank+1,element_type,e as array_const);
        		}
        		else if (e is record_constant)
            	{
            		if (element_type is common_type_node)
            			constant.element_values[i] = ConvertRecordConst(element_type as common_type_node, e as record_constant);
            		else AddError(new NotSupportedError(constant.element_values[i].location));
            	}
        		else
            	{
        			if (cur_rank != rank)
                        AddError(constant.location, "RANK_MISMATCH_IN_INITILIALIZER");
        			constant.element_values[i] = convert_strong_to_constant_node(constant.element_values[i], element_type);
            	}
        	}
        	constant.SetType(tn);
        	return constant;
        }
        
        private array_const ConvertArrayConst(type_node tn, array_const constant)
        {
            type_node element_type = null;
            if (IsBoundedArray(tn))
            {
                bounded_array_interface bai = tn.get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
                element_type = bai.element_type;
                ordinal_type_interface oti_indexer = bai.ordinal_type_interface;
                int arr_length = oti_indexer.ordinal_type_to_int(oti_indexer.upper_value) - oti_indexer.ordinal_type_to_int(oti_indexer.lower_value) + 1;
                if (arr_length != constant.element_values.Count)
                    AddError(constant.location, "ARRAY_CONST_{0}_ELEMENTS_EXPECTED", arr_length);
            }
            else
                if (IsUnsizedArray(tn))
                {
                    array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                    element_type = aii.element_type;
                    if (aii.rank > 1)
                    {
                    	array_const cnst = ConvertNDimArrayConst(tn,1,element_type,constant);
                    	cnst.SetType(tn);
                    	return cnst;
                    }
                }
                else
                    throw new CompilerInternalError("Unexpected array type");
            for (int i = 0; i < constant.element_values.Count; i++)
                constant.element_values[i] = convert_strong_to_constant_node(constant.element_values[i], element_type);
            constant.SetType(tn);
            return constant;
        }

        private record_constant ConvertRecordConst(common_type_node ctn, record_constant constant)
        {
            location loc = constant.location;
            if (!ctn.is_value_type)
                AddError(loc, "RECORD_CONST_NOT_ALLOWED_{0}", ctn.name);
            if (ctn.fields.Count != constant.record_const_definition_list.Count)
                AddError(loc, "INVALID_RECORD_CONST_FIELD_COUNT");
            constant.SetType(ctn);
            constant.field_values.Clear();
            bool tmp = this.is_typed_const_def;
            this.is_typed_const_def = true;
            for (int i = 0; i < ctn.fields.Count; i++)
            {
                class_field cf = ctn.fields[i];
                if (cf.name.ToLower() != constant.record_const_definition_list[i].name.name.ToLower())
                    AddError(get_location(constant.record_const_definition_list[i].name), "FIELD_NAME_SHOULD_BE_EQUAL_RECORD_FIELD_NAME", cf.name);
                constant.field_values.Add(convert_strong_to_constant_node(constant.record_const_definition_list[i].val, cf.type));
            }
            this.is_typed_const_def = tmp;
            return constant;
        }

        public override void visit(SyntaxTree.diapason _diapason)
        {
            constant_node cnleft = convert_strong_to_constant_node(_diapason.left);
            constant_node cnright = convert_strong_to_constant_node(_diapason.right);
            type_node tn = convertion_data_and_alghoritms.type_constructor.get_pascal_diap(cnleft, cnright, get_location(_diapason.left),
                context.converted_namespace, get_location(_diapason.right));
            return_value(tn);            
        }

        public override void visit(SyntaxTree.ref_type _ref_type)
        {
            if (_ref_type.pointed_to is SyntaxTree.named_type_reference &&
                ((_ref_type.pointed_to as SyntaxTree.named_type_reference).names.Count == 1) &&
                context.CurrentScope.Find((_ref_type.pointed_to as SyntaxTree.named_type_reference).names[0].name) == null)
            {
                //это указатель на тип который еще не описан
                if (!is_direct_type_decl) AddError(new UndefinedNameReference((_ref_type.pointed_to as SyntaxTree.named_type_reference).names[0].name,get_location(_ref_type.pointed_to)));
                return_value(GetWaitedRefType((_ref_type.pointed_to as SyntaxTree.named_type_reference).names[0].name, get_location(_ref_type.pointed_to)));
            }
            else
            {
                type_node tn = convert_strong(_ref_type.pointed_to);
                if (tn.is_generic_parameter)
                {
                    if (SemanticRules.AllowPointersForGenericParameters)
                    {
                        get_type_abilities(tn).useful_for_pointers = true;
                    }
                    else
                    {
                        AddError(get_location(_ref_type), "POINTERS_OF_GENERIC_PARAMETERS_NOT_ALLOWED");
                    }
                }
                else
                {
                    if (!tn.is_value /*&& tn.type_special_kind != SemanticTree.type_special_kind.array_kind && tn.type_special_kind != SemanticTree.type_special_kind.set_type && tn.type_special_kind != SemanticTree.type_special_kind.array_wrapper && tn.type_special_kind != SemanticTree.type_special_kind.binary_file
                        && tn.type_special_kind != SemanticTree.type_special_kind.binary_file*/ && !(tn is ref_type_node) && tn != SystemLibrary.SystemLibrary.pointer_type && tn.type_special_kind != SemanticTree.type_special_kind.short_string && tn.type_special_kind != SemanticTree.type_special_kind.diap_type && !tn.IsEnum &&
                        !(tn.semantic_node_type == semantic_node_type.indefinite_type))
                        AddError(get_location(_ref_type), "POINTERS_OF_REF_TYPES_NOT_ALLOWED");
                }
                ref_type_node rtn = tn.ref_type;
                if (rtn != null)
                {
                	rtn.loc = get_location(_ref_type);
                	if (SemanticRules.StrongPointersTypeCheckForDotNet)
                    //CheckPointersTypeForDotNetFramework(tn, get_location(_ref_type.pointed_to));
                    	RefTypesForCheckPointersTypeForDotNetFramework.Add(rtn);
                	return_value(rtn);
                }
                else
                {
                	return_value(SystemLibrary.SystemLibrary.pointer_type);
                }
            }
        }

        internal bool CheckPointersTypeForDotNetFramework(type_node tn, location loc)
        {
            type_node ttn = FindBadTypeNodeForPointersInDotNetFramework(tn);
            __findBadTypeNodeForPointersPreparedTypes.Clear();
            if (ttn != null)
            {
                AddErrorCheckPointersTypeForDotNetFramework(tn, ttn, loc);
                return false;
            }
            return true;
        }

        public void AddErrorCheckPointersTypeForDotNetFramework(type_node tn, type_node ttn, location loc)
        {
            /*if (IsBoudedArray(ttn) || IsUnsizedArray(ttn) || ttn == SystemLibrary.SystemLibrary.string_type)
            {
                if (tn == ttn)
                    AddError(new CannotDeclarePointerToStringOrArrayType(loc));
                else
                    AddError(new CannotDeclarePointerToRecordContainsStringOrArrayFields(loc));
            }*/
//            if (IsBoudedArray(ttn) || ttn.type_special_kind == SemanticTree.type_special_kind.array_kind || ttn.type_special_kind == SemanticTree.type_special_kind.set_type
//                || ttn.type_special_kind == SemanticTree.type_special_kind.short_string || ttn.type_special_kind == SemanticTree.type_special_kind.typed_file)
//            {
//            	
//            }
//            else
            {
                if (tn == ttn)
                    AddError(loc, "CANNOT_DECLARED_POINTER_TO_REFERENCE_TYPE");
                else
                    AddError(loc, "CANNOT_DECLARED_POINTER_TO_RECORD_CONTAINS_REFERENCE_FIELDS");
            }
        }
        private List<type_node> __findBadTypeNodeForPointersPreparedTypes = new List<type_node>();
        private type_node FindBadTypeNodeForPointersInDotNetFramework(type_node tn)
        {
            if (__findBadTypeNodeForPointersPreparedTypes.Contains(tn))
                return null;
            __findBadTypeNodeForPointersPreparedTypes.Add(tn);
            if (tn.semantic_node_type == semantic_node_type.indefinite_type)
            {
                return null;
            }
            if (tn.is_generic_parameter)
            {
                if (SemanticRules.AllowPointersForGenericParameters)
                {
                    get_type_abilities(tn).useful_for_pointers = true;
                    return null;
                }
                else
                {
                    return tn;
                }
            }
            type_node btn = null;
            if (tn is ref_type_node)
                return FindBadTypeNodeForPointersInDotNetFramework((tn as ref_type_node).pointed_type);
            if (tn == SystemLibrary.SystemLibrary.pointer_type || tn == SystemLibrary.SystemLibrary.void_type)
                return null;
            if (tn.is_value)
            {
                generic_instance_type_node gitn = tn as generic_instance_type_node;
                if (gitn != null)
                {
                    List<type_node> ftypes = gitn.all_field_types;
                    foreach (type_node ftype in ftypes)
                    {
                        if ((btn = FindBadTypeNodeForPointersInDotNetFramework(ftype)) != null)
                            return btn;
                    }
                    return null;
                }
                if (tn is common_type_node)
                {
                	if (tn.IsEnum || tn.type_special_kind == SemanticTree.type_special_kind.diap_type) return null;
                	common_type_node ctn = tn as common_type_node;
                    foreach (class_field cf in ctn.fields)
                        if ((btn = FindBadTypeNodeForPointersInDotNetFramework(cf.type)) != null)
                            return btn;
                    return null;
                }
                if (tn is compiled_type_node)
                {
                    compiled_type_node ctn = tn as compiled_type_node;
                    if (ctn.IsPrimitive)
                        return null;
                    System.Reflection.FieldInfo[] fields = ctn.compiled_type.GetFields();
                    foreach (System.Reflection.FieldInfo fi in fields)
                        if (!fi.IsStatic)
                            if ((btn = FindBadTypeNodeForPointersInDotNetFramework(compiled_type_node.get_type_node(fi.FieldType))) != null)
                                return btn;
                    return null;
                }
            }
            /*if (IsBoudedArray(el_type))
            {
                //это обычный массив
                bounded_array_interface bai = (bounded_array_interface)el_type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                return CanUseThisTypeForTypedFiles(bai.element_type);
            }*/
            if (tn == SystemLibrary.SystemLibrary.string_type || tn.type_special_kind == SemanticTree.type_special_kind.set_type || tn.type_special_kind == SemanticTree.type_special_kind.short_string || tn.type_special_kind == SemanticTree.type_special_kind.array_wrapper
               || tn.type_special_kind == SemanticTree.type_special_kind.typed_file || tn.type_special_kind == SemanticTree.type_special_kind.binary_file || tn.type_special_kind == SemanticTree.type_special_kind.text_file
              || tn.type_special_kind == SemanticTree.type_special_kind.array_kind) return null;
            return tn;
        }


        /*private void ConvertStatement(SyntaxTree.statement stmt)
        {
            while (stmt != null)
            {
                if (stmt is SyntaxTree.if_node)
                {
                    SyntaxTree.if_node _if_node = stmt as SyntaxTree.if_node;

                    expression_node condition = convert_strong(_if_node.condition);
                    condition = convertion_data_and_alghoritms.convert_type(condition, SystemLibrary.SystemLibrary.bool_type);

                    statements_list sl = new statements_list(get_location(_if_node.then_body));
                    convertion_data_and_alghoritms.statement_list_stack_push(sl);

                    context.enter_code_block_with_bind();
                    statement_node then_st = convert_strong(_if_node.then_body);
                    context.leave_code_block();

                    sl = convertion_data_and_alghoritms.statement_list_stack.pop();
                    if (sl.statements.Count > 0)
                    {
                        sl.statements.AddElement(then_st);
                        then_st = sl;
                    }

                    if (_if_node.else_body != null)
                    {
                        sl = new statements_list(get_location(_if_node.else_body));
                        convertion_data_and_alghoritms.statement_list_stack_push(sl);
                    }

                    context.enter_code_block_with_bind();
                    statement_node else_st = convert_weak(_if_node.else_body);
                    context.leave_code_block();

                    if (_if_node.else_body != null)
                    {
                        sl = convertion_data_and_alghoritms.statement_list_stack.pop();
                        if (sl.statements.Count > 0)
                        {
                            sl.statements.AddElement(else_st);
                            else_st = sl;
                        }
                    }

                    if_node if_node = new if_node(condition, then_st, else_st, get_location(_if_node));
                    return_value(if_node);
                    return;
                }
                visit(stmt);
            }
        }*/

        void CheckToEmbeddedStatementCannotBeADeclaration(SyntaxTree.statement st)
        {
            if (st != null && st is SyntaxTree.var_statement)
                AddError(get_location(st), "EMBEDDED_SATATEMENT_CANNOT_BE_A_DECLARATION");
        }

        public override void visit(SyntaxTree.if_node _if_node)
        {
            expression_node condition = convert_strong(_if_node.condition);
            condition = convertion_data_and_alghoritms.convert_type(condition, SystemLibrary.SystemLibrary.bool_type);

            CheckToEmbeddedStatementCannotBeADeclaration(_if_node.then_body);
            CheckToEmbeddedStatementCannotBeADeclaration(_if_node.else_body);

            statements_list sl = new statements_list(get_location(_if_node.then_body));
            convertion_data_and_alghoritms.statement_list_stack_push(sl);

            context.enter_code_block_with_bind();
            statement_node then_st = convert_strong(_if_node.then_body);
            context.leave_code_block();

            sl = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
            {
                sl.statements.AddElement(then_st);
                then_st = sl;
            }

            if (_if_node.else_body != null)
            {
                sl = new statements_list(get_location(_if_node.else_body));
                convertion_data_and_alghoritms.statement_list_stack_push(sl);
            }

            context.enter_code_block_with_bind();
            statement_node else_st = convert_weak(_if_node.else_body);
            context.leave_code_block();

            if (_if_node.else_body != null)
            {
                sl = convertion_data_and_alghoritms.statement_list_stack.pop();
                if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
                {
                    sl.statements.AddElement(else_st);
                    else_st = sl;
                }
            }

            if_node if_node = new if_node(condition, then_st, else_st, get_location(_if_node));
            return_value(if_node);
        }

        public override void visit(SyntaxTree.while_node _while_node)
        {
            expression_node expr = convert_strong(_while_node.expr);
            expr = convertion_data_and_alghoritms.convert_type(expr, SystemLibrary.SystemLibrary.bool_type);

            CheckToEmbeddedStatementCannotBeADeclaration(_while_node.statements);

            while_node wn = new while_node(expr, get_location(_while_node));
            context.cycle_stack.push(wn);

            statements_list sl = new statements_list(get_location(_while_node.statements));
            convertion_data_and_alghoritms.statement_list_stack_push(sl);

            context.enter_code_block_with_bind();
            statement_node st = convert_strong(_while_node.statements);
            context.leave_code_block();

            sl = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
            {
                sl.statements.AddElement(st);
                st = sl;
            }

            wn.body = st;
            context.cycle_stack.pop();
            return_value(wn);
        }

        public override void visit(SyntaxTree.repeat_node _repeat_node)
        {
            repeat_node rep = new repeat_node(get_location(_repeat_node));
            context.cycle_stack.push(rep);

            statements_list sl = new statements_list(get_location(_repeat_node.statements));
            convertion_data_and_alghoritms.statement_list_stack_push(sl);

            CheckToEmbeddedStatementCannotBeADeclaration(_repeat_node.statements);

            statement_node st = convert_strong(_repeat_node.statements);

            sl = convertion_data_and_alghoritms.statement_list_stack.pop();
            //if (!(st is statements_list))
            if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
            {
                sl.statements.AddElement(st);
                st = sl;
            }

            rep.body = st;
            expression_node expr = convert_strong(_repeat_node.expr);
            expr = convertion_data_and_alghoritms.convert_type(expr, SystemLibrary.SystemLibrary.bool_type);
            rep.condition = expr;
            context.cycle_stack.pop();
            return_value(rep);
        }

        public override void visit(SyntaxTree.for_node _for_node)
        {
            #region MikhailoMMX, обработка omp parallel for
            bool isGenerateParallel = false;
            bool isGenerateSequential = true;
            if (OpenMP.ForsFound)
            {
                OpenMP.LoopVariables.Push(_for_node.loop_variable.name.ToLower());
                //если в программе есть хоть одна директива parallel for - проверяем:
                if (DirectivesToNodesLinks.ContainsKey(_for_node) && OpenMP.IsParallelForDirective(DirectivesToNodesLinks[_for_node]))
                {
                    //перед этим узлом есть директива parallel for
                    if (CurrentParallelPosition == ParallelPosition.Outside)            //входим в самый внешний параллельный for
                    {
                        if (_for_node.create_loop_variable || (_for_node.type_name != null))
                        {
                            //сгенерировать сначала последовательную ветку, затем параллельную
                            //устанавливаем флаг и продолжаем конвертирование, считая, что конвертируем последовательную ветку
                            isGenerateParallel = true;
                            CurrentParallelPosition = ParallelPosition.InsideSequential;
                            //в конце за счет флага вернем состояние обратно и сгенерируем и параллельную ветку тоже
                        }
                        else
                            WarningsList.Add(new OMP_BuildigError(new Exception("Переменная параллельного цикла должна быть определена в заголовке цикла"), new location(_for_node.source_context.begin_position.line_num, _for_node.source_context.begin_position.column_num, _for_node.source_context.end_position.line_num, _for_node.source_context.end_position.column_num, new document(_for_node.source_context.FileName))));
                    }
                    else //уже генерируем одну из веток
                        //если это параллельная ветка - последовательную генерировать не будем
                        if (CurrentParallelPosition == ParallelPosition.InsideParallel)
                        {
                            isGenerateParallel = true;
                            isGenerateSequential = false;
                        }
                        //else
                        //а если последовательная - то флаг isGenerateParallel не установлен, сгенерируется только последовательная
                }
            }
            #endregion
 

            location loc1 = get_location(_for_node.loop_variable);
            var_definition_node vdn = null;
            expression_node left, right, res;
            expression_node initv = convert_strong(_for_node.initial_value);
            expression_node tmp = initv;
            if (initv is typed_expression) initv = convert_typed_expression_to_function_call(initv as typed_expression);
            if (initv.type == null)
            	initv = tmp;
            statements_list head_stmts = new statements_list(loc1);
            convertion_data_and_alghoritms.statement_list_stack_push(head_stmts);
            if (_for_node.type_name == null && !_for_node.create_loop_variable)
            {
                definition_node dn = context.check_name_node_type(_for_node.loop_variable.name, loc1,
                    general_node_type.variable_node);
                vdn = (var_definition_node)dn;
                if (context.is_loop_variable(vdn))
                    AddError(get_location(_for_node.loop_variable), "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
                if (!check_name_in_current_scope(_for_node.loop_variable.name))
                    AddError(new ForLoopControlMustBeSimpleLocalVariable(loc1));
            }
            else
            {
                //В разработке DS
                //throw new NotSupportedError(get_location(_for_node.type_name));
                type_node tn;
                if (_for_node.type_name != null)
                    tn = convert_strong(_for_node.type_name);
                else
                    tn = initv.type;
                //if (tn == SystemLibrary.SystemLibrary.void_type && _for_node.type_name != null)
                //	AddError(new VoidNotValid(get_location(_for_node.type_name)))
                if (_for_node.type_name != null)
                    check_for_type_allowed(tn,get_location(_for_node.type_name));
                vdn = context.add_var_definition(_for_node.loop_variable.name, get_location(_for_node.loop_variable), tn, SemanticTree.polymorphic_state.ps_common);
            }
            internal_interface ii = vdn.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                AddError(new OrdinalTypeExpected(loc1));
            }
            ordinal_type_interface oti = (ordinal_type_interface)ii;


            location loc2 = get_location(_for_node.finish_value);
            var_definition_node vdn_finish = context.create_for_temp_variable(vdn.type, loc2);
            //Это должно стаять первее!
            left = create_variable_reference(vdn_finish, loc1);
            expression_node finishValue = convert_strong(_for_node.finish_value);
            right = finishValue;
            if (right is typed_expression) right = convert_typed_expression_to_function_call(right as typed_expression);
            res = find_operator(compiler_string_consts.assign_name, left, right, loc2);
            head_stmts.statements.AddElement(res);

            left = create_variable_reference(vdn, loc1);
            right = initv;
            res = find_operator(compiler_string_consts.assign_name, left, right, loc1);
            head_stmts.statements.AddElement(res);


            //for_node fn=new for_node(sl,;
            //fn.initialization_statement=sl;

            location loc3 = get_location(_for_node.initial_value);

            statement_node sn_inc = null;
            expression_node sn_while = null;
            expression_node sn_init_while = null;
            left = create_variable_reference(vdn, loc3);
            right = create_variable_reference(vdn, loc2);
            expression_node right_border = create_variable_reference(vdn_finish, loc2);
            switch (_for_node.cycle_type)
            {
                case SyntaxTree.for_cycle_type.to:
                    {
                        sn_inc = convertion_data_and_alghoritms.create_simple_function_call(oti.inc_method, loc1, left);
                        //if (vdn.type != SystemLibrary.SystemLibrary.bool_type)
                        sn_while = convertion_data_and_alghoritms.create_simple_function_call(oti.lower_method, loc2, right, right_border);
                        sn_init_while = convertion_data_and_alghoritms.create_simple_function_call(oti.lower_eq_method, loc2, right, right_border);
//                        else
//                        	sn_while = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.bool_noteq, loc2, right, right_border);
                        break;
                    }
                case SyntaxTree.for_cycle_type.downto:
                    {
                        sn_inc = convertion_data_and_alghoritms.create_simple_function_call(oti.dec_method, loc1, left);
                        //if (vdn.type != SystemLibrary.SystemLibrary.bool_type)
                        sn_while = convertion_data_and_alghoritms.create_simple_function_call(oti.greater_method, loc2, right, right_border);
                        sn_init_while = convertion_data_and_alghoritms.create_simple_function_call(oti.greater_eq_method, loc2, right, right_border);
//                        else
//                        	sn_while = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.bool_noteq, loc2, right, right_border);
                        break;
                    }
            }
            //fn.increment_statement=sn_inc;
            //fn.while_expr=sn_while;

            CheckToEmbeddedStatementCannotBeADeclaration(_for_node.statements);

            //DarkStar Modifed
            //исправил ошибку:  не работали break в циклах
            for_node fn = new for_node(null, sn_while, sn_init_while, sn_inc, null, get_location(_for_node));
            if (vdn.type == SystemLibrary.SystemLibrary.bool_type)
            	fn.bool_cycle = true;
            context.cycle_stack.push(fn);
            context.loop_var_stack.Push(vdn);
            statements_list slst = new statements_list(get_location(_for_node.statements));
            convertion_data_and_alghoritms.statement_list_stack_push(slst);

            context.enter_code_block_with_bind();
            fn.body = convert_strong(_for_node.statements);
            context.leave_code_block();

            slst = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (slst.statements.Count > 0 || slst.local_variables.Count > 0)
            {
                slst.statements.AddElement(fn.body);
                fn.body = slst;
            }

            context.cycle_stack.pop();
            context.loop_var_stack.Pop();
            head_stmts = convertion_data_and_alghoritms.statement_list_stack.pop();
            head_stmts.statements.AddElement(fn);

            #region MikhailoMMX, обработка omp parallel for
            //флаг был установлен только если это самый внешний parallel for и нужно сгенерировать обе ветки
            //или если это вложенный parallel for, нужно сгенерировать обе ветки, но без проверки на OMP_Available
            //Последовательная ветка только что сгенерирована, теперь меняем состояние и генерируем параллельную
            if (isGenerateParallel)
            {
                CurrentParallelPosition = ParallelPosition.InsideParallel;
                statements_list stl = OpenMP.TryConvertFor(head_stmts, _for_node, fn, vdn, initv, finishValue, this);
                CurrentParallelPosition = ParallelPosition.Outside;
                if (stl != null)
                {
                    OpenMP.LoopVariables.Pop();
                    return_value(stl);
                    return;
                }
            }
            if (OpenMP.ForsFound)
            {
                OpenMP.LoopVariables.Pop();
            }
            #endregion

            return_value(head_stmts);
        }


        private expression_node additional_indexer_convertion(expression_node ind_expr, type_node array_type)
        {
            return convertion_data_and_alghoritms.convert_type(ind_expr, SystemLibrary.SystemLibrary.integer_type);
        }

        private void indexer_as_expression_index(expression_node expr, SyntaxTree.expression_list parameters,
            motivation mot, location loc)
        {
            try_convert_typed_expression_to_function_call(ref expr);
            if (expr.type.type_special_kind == SemanticTree.type_special_kind.array_kind)
            {
                internal_interface ii = expr.type.get_internal_interface(internal_interface_kind.unsized_array_interface);
                array_internal_interface aii = (array_internal_interface)ii;
                int rank = aii.rank;
                //TODO: Многомерные массивы.
                expression_node ind_expr = convert_strong(parameters.expressions[0]);
                ind_expr = additional_indexer_convertion(ind_expr, expr.type);
                simple_array_indexing sai = new simple_array_indexing(expr, ind_expr, aii.element_type, loc);
                if (rank == 1)
                for (int i = 1; i < parameters.expressions.Count; i++)
                {
                    ii = sai.type.get_internal_interface(internal_interface_kind.unsized_array_interface);
                    location lloc = get_location(parameters.expressions[i]);
                    if (ii == null)
                    {
                        AddError(lloc, "{0}_DIMENSIONAL_ARRAY_CAN_NOT_HAVE_{1}_AND_MORE_INDEXING", i, i+1);
                    }
                    aii = (array_internal_interface)ii;
                    ind_expr = convert_strong(parameters.expressions[i]);
                    ind_expr = additional_indexer_convertion(ind_expr, sai.type);
                    sai = new simple_array_indexing(sai, ind_expr, aii.element_type, lloc);
                }
                else
                {
                    if (rank != parameters.expressions.Count)
                        AddError(get_location(parameters), "{0}_DIMENSIONAL_ARRAY_CAN_NOT_HAVE_{1}_AND_MORE_INDEXING", rank, rank + 1);
                	List<expression_node> lst = new List<expression_node>();
                	for (int i = 0; i < parameters.expressions.Count; i++)
                	{
                    	location lloc = get_location(parameters.expressions[i]);
                    	ind_expr = convert_strong(parameters.expressions[i]);
                    	ind_expr = additional_indexer_convertion(ind_expr, sai.type);
                    	lst.Add(ind_expr);
                	}
                	sai = new simple_array_indexing(expr, lst[0], aii.element_type, get_location(parameters));
                	sai.expr_indices = lst.ToArray();
                }
                switch (mot)
                {
                    case motivation.address_reciving:
                        {
                            return_addressed_value(sai);
                            break;
                        }
                    case motivation.expression_evaluation:
                        {
                            return_value(sai);
                            break;
                        }
                    case motivation.semantic_node_reciving:
                        {
                            return_semantic_value(sai);
                            break;
                        }
                }
                return;
            }
            else if (expr.type.type_special_kind == SemanticTree.type_special_kind.short_string)
            {
            	if (parameters.expressions.Count != 1)
                    AddError(loc, "INVALID_PARAMETER_COUNT_IN_INDEXER");
            	expression_node ind_expr = convert_strong(parameters.expressions[0]);
            	ind_expr = additional_indexer_convertion(ind_expr, expr.type);
            	//expression_node en = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure.sym_info as function_node,loc,ind_expr,new int_const_node((expr.type as short_string_type_node).Length,null));
            	switch (mot)
                {
                    case motivation.address_reciving:
                        {
                            simple_array_indexing sai = new simple_array_indexing(expr, ind_expr, SystemLibrary.SystemLibrary.char_type, loc);
                            return_addressed_value(sai);
                            //return_addressed_value(convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure.sym_info as function_node,loc,expr,ind_expr) as common_namespace_function_call);
                            break;
                        }
                    case motivation.expression_evaluation:
                        {
            				return_value(convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure.sym_info as function_node,loc,expr,ind_expr,new int_const_node((expr.type as short_string_type_node).Length,null)));
                            break;
                        }
                    case motivation.semantic_node_reciving:
                        {
            				return_semantic_value(convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.GetCharInShortStringProcedure.sym_info as function_node,loc,expr,ind_expr,new int_const_node((expr.type as short_string_type_node).Length,null)));
                            break;
                        }
                }
                return;
            	
            }
            /*else if (expr.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            {
            	internal_interface ii = expr.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                array_internal_interface aii = (array_internal_interface)ii;
                //TODO: Многомерные массивы.
                expression_node ind_expr = convert_strong(parameters.expressions[0]);
                ind_expr = additional_indexer_convertion(ind_expr, expr.type);
                simple_array_indexing sai = new simple_array_indexing(expr, ind_expr, aii.element_type, loc);
                for (int i = 1; i < parameters.expressions.Count; i++)
                {
                    ii = sai.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                    location lloc = get_location(parameters.expressions[i]);
                    if (ii == null)
                    {
                        throw new NDimensionalArrayCanNotHaveNPlusOneIndexer(lloc, expr, i);
                    }
                    aii = (array_internal_interface)ii;
                    ind_expr = convert_strong(parameters.expressions[i]);
                    ind_expr = create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, ind_expr,
                                            new int_const_node(ii.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value),cmc.location));
                    ind_expr = additional_indexer_convertion(ind_expr, sai.type);
                    sai = new simple_array_indexing(sai, ind_expr, aii.element_type, lloc);
                }
                switch (mot)
                {
                    case motivation.address_reciving:
                        {
                            return_addressed_value(sai);
                            break;
                        }
                    case motivation.expression_evaluation:
                        {
                            return_value(sai);
                            break;
                        }
                    case motivation.semantic_node_reciving:
                        {
                            return_semantic_value(sai);
                            break;
                        }
                }
                return;
                
                 if (factparams[i].semantic_node_type == semantic_node_type.common_method_call)
                            {
                                common_method_call cmc = (common_method_call)factparams[i];
                                internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                                if (ii != null)
                                {
                                    if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                                    {
                                        bounded_array_interface bai = (bounded_array_interface)ii;
                                        class_field cf = bai.int_array;
                                        expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                                        expression_node right = cmc.parameters[0];
                                        //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                                        right = convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                                        right = create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                            new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value),cmc.location));
                                        factparams[i] = new simple_array_indexing(left, right, cmc.type, cmc.location);
                                        is_pascal_array_ref = true;
                                    }
                                }
                            }
            }*/
            if (expr.type.default_property_node == null)
            {
                if (expr.type.semantic_node_type != semantic_node_type.delegated_method)
                {
                    AddError(loc, "NO_DEFAULT_PROPERTY_TO_TYPE_{0}", expr.type.PrintableName);
                }
                else
                {
                    AddError(loc, "NO_DEFAULT_PROPERTY_TO_FUNCTION_TYPE");
                }
            }

            if (expr.type.default_property_node.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
            {
                AddError(new CanNotReferenceToStaticPropertyWithExpression(expr.type.default_property_node, loc, expr.type));
            }
            non_static_property_reference nspr = new non_static_property_reference(expr.type.default_property_node, expr, loc);
            indexer_as_property_indexes(nspr, parameters, mot, loc);
            //eto vrode rabotaet normalno. no imet v vidu
            if (expr.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            {
            	expression_node en = ret.get_expression();
                if (en is non_static_property_reference)
                {
                    non_static_property_reference cmc = (non_static_property_reference)en;
                    internal_interface ii = cmc.expression.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                    if (ii != null)
                    {
                        //if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                        {
                            bounded_array_interface bai = (bounded_array_interface)ii;
                            class_field cf = bai.int_array;
                            expression_node left = new class_field_reference(cf, cmc.expression, cmc.location);
                            expression_node right = cmc.fact_parametres[0];
                            //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                            right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                            right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                                new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                            en = new simple_array_indexing(left, right, cmc.type, cmc.location);
                            //is_pascal_array_ref = true;
                        }
                    }
                }
                else
                {
                    common_method_call cmc = (common_method_call)en;
                    internal_interface ii = cmc.obj.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                    if (ii != null)
                    {
                        //if (cmc.function_node.name == compiler_string_consts.get_val_pascal_array_name)
                        {
                            bounded_array_interface bai = (bounded_array_interface)ii;
                            class_field cf = bai.int_array;
                            expression_node left = new class_field_reference(cf, cmc.obj, cmc.location);
                            expression_node right = cmc.parameters[0];
                            //right = convert_type(right, SystemLibrary.SystemLibrary.integer_type);
                            right = convertion_data_and_alghoritms.convert_type(right, (ii as bounded_array_interface).ordinal_type_interface.elems_type);
                            right = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibrary.int_sub, cmc.location, right,
                                                new int_const_node(bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value), cmc.location));
                            en = new simple_array_indexing(left, right, cmc.type, cmc.location);
                            //is_pascal_array_ref = true;
                        }
                    }
                }
                return_value(en);
            }
            
            return;
        }

        private void indexer_as_type_indexes(type_node type, SyntaxTree.expression_list parameters, motivation mot,
            location loc)
        {
            if (type.default_property_node == null)
            {
                if (type.semantic_node_type != semantic_node_type.delegated_method)
                {
                    AddError(loc, "NO_DEFAULT_PROPERTY_TO_TYPE_{0}", type.PrintableName);
                }
                else
                {
                    AddError(loc, "NO_DEFAULT_PROPERTY_TO_FUNCTION_TYPE");
                }
            }
            if (type.default_property_node.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
            {
                AddError(new CanNotReferenceToNonStaticPropertyWithType(type.default_property_node, loc, type));
            }
            static_property_reference spr = new static_property_reference(type.default_property_node, loc);
            indexer_as_property_indexes(spr, parameters, mot, loc);
            return;
        }

        private void indexer_as_property_indexes(static_property_reference spr, SyntaxTree.expression_list parameters,
            motivation mot, location loc)
        {
            /*if (spr.property.parameters.Count==0)
            {
                switch(mot)
                {
                    case motivation.expression_evaluation:
                    case motivation.semantic_node_reciving:
                    {
                        function_node fn=spr.property.get_function;
                        base_function_call_with_method bfc=convertion_data_and_alghoritms.create_simple_function_call(fn,loc,new expression_node[0]);
                        return_value(bfc);
                        return;
                    }
                    case motivation.address_reciving:
                    {
                        return_addressed_value(spr);
                        return;
                    }
                }
                throw new CompilerInternalError("Unsupported motivation");
            }*/
            if (spr.property.parameters.Count != 0)
            {
                if (parameters.expressions.Count != spr.property.parameters.Count)
                {
                    AddError(loc, "PROPERTY_{0}_REFERENCE_WITH_INVALID_PARAMS_COUNT", spr.property.name);
                }
                for (int i = 0; i < parameters.expressions.Count; i++)
                {
                    expression_node exp = convert_strong(parameters.expressions[i]);
                    exp = convertion_data_and_alghoritms.convert_type(exp, spr.property.parameters[i].type);
                    spr.fact_parametres.AddElement(exp);
                }
            }
            switch (mot)
            {
                case motivation.expression_evaluation:
                case motivation.semantic_node_reciving:
                    {
                        function_node fn = spr.property.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(spr.property, loc));
                        }
                        //expression_node bfc=convertion_data_and_alghoritms.create_simple_function_call(fn,loc,spr.fact_params.ToArray());
                        base_function_call bfc = create_static_method_call(fn, loc, spr.property.comprehensive_type, false);
                        bfc.parameters.AddRange(spr.fact_parametres);
                        if (spr.property.parameters.Count != 0)
                        {
                            return_value(bfc);
                            return;
                        }
                        indexer_as_expression_index(bfc, parameters, mot, loc);
                        return;
                    }
                case motivation.address_reciving:
                    {
                        if (spr.property.parameters.Count != 0)
                        {
                            return_addressed_value(spr);
                            return;
                        }
                        function_node fn = spr.property.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(spr.property, loc));
                        }
                        base_function_call bfc = create_static_method_call(fn, loc, spr.property.comprehensive_type, false);
                        bfc.parameters.AddRange(spr.fact_parametres);
                        indexer_as_expression_index(bfc, parameters, mot, loc);
                        return;
                    }
            }
            throw new CompilerInternalError("Unsupported motivation");
        }

        private void indexer_as_property_indexes(non_static_property_reference nspr, SyntaxTree.expression_list parameters,
            motivation mot, location loc)
        {
            if (nspr.property.parameters.Count != 0)
            {
                if (parameters.expressions.Count != nspr.property.parameters.Count)
                {
                    internal_interface ii = nspr.property.comprehensive_type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                    if (ii != null)
                    {
                        //bounded_array_interface dii = (bounded_array_interface)ii;

                        expression_node exp = convert_strong(parameters.expressions[0]);
                        exp = convertion_data_and_alghoritms.convert_type(exp, nspr.property.parameters[0].type);
                        nspr.fact_parametres.AddElement(exp);

                        base_function_call bfc = null;

                        for (int i = 1; i < parameters.expressions.Count; i++)
                        {
                            bfc = create_not_static_method_call(nspr.property.get_function, nspr.expression, exp.location, false);
                            bfc.parameters.AddRange(nspr.fact_parametres);

                            location lloc = get_location(parameters.expressions[i]);
                            ii = bfc.type.get_internal_interface(internal_interface_kind.bounded_array_interface);
                            if (ii == null)
                            {
                                AddError(lloc, "{0}_DIMENSIONAL_ARRAY_CAN_NOT_HAVE_{1}_AND_MORE_INDEXING", i, i+1);
                            }
                            bounded_array_interface bii = (bounded_array_interface)ii;
                            exp = convert_strong(parameters.expressions[i]);
                            exp = convertion_data_and_alghoritms.convert_type(exp, bii.index_type);
                            nspr = new non_static_property_reference(bii.index_property, bfc, lloc);
                            nspr.fact_parametres.AddElement(exp);
                        }
                    }
                    else
                    {
                        AddError(loc, "PROPERTY_{0}_REFERENCE_WITH_INVALID_PARAMS_COUNT", nspr.property.name);
                    }
                }
                else
                {
                    //String 1 based
                    if (parameters.expressions.Count == 1 &&
                       nspr.property.comprehensive_type == SystemLibrary.SystemLibrary.string_type &&
                       !SemanticRules.NullBasedStrings)
                    {
                        nspr.fact_parametres.AddElement(
                            ConstructDecExpr(
                                convertion_data_and_alghoritms.convert_type(
                                    convert_strong(parameters.expressions[0]), nspr.property.parameters[0].type), null));
                    }
                    else
                        for (int i = 0; i < parameters.expressions.Count; i++)
                        {
                            expression_node exp = convert_strong(parameters.expressions[i]);
                            exp = convertion_data_and_alghoritms.convert_type(exp, nspr.property.parameters[i].type);
                            nspr.fact_parametres.AddElement(exp);
                        }
                }
            }
            switch (mot)
            {
                case motivation.expression_evaluation:
                case motivation.semantic_node_reciving:
                    {
                        function_node fn = nspr.property.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(nspr.property, loc));
                        }
                        base_function_call bfc = create_not_static_method_call(fn, nspr.expression, loc, false);
                        bfc.parameters.AddRange(nspr.fact_parametres);
                        if (nspr.property.parameters.Count != 0)
                        {
                            return_value(bfc);
                            return;
                        }
                        indexer_as_expression_index(bfc, parameters, mot, loc);
                        return;
                    }
                case motivation.address_reciving:
                    {
                        if (nspr.property.parameters.Count != 0)
                        {
                            return_addressed_value(nspr);
                            return;
                        }
                        function_node fn = nspr.property.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(nspr.property, loc));
                        }
                        base_function_call bfc = create_not_static_method_call(fn, nspr.expression, loc, false);
                        bfc.parameters.AddRange(nspr.fact_parametres);
                        indexer_as_expression_index(bfc, parameters, mot, loc);
                        return;
                    }
            }
            throw new CompilerInternalError("Unsupported motivation");
        }

        public override void visit(SyntaxTree.indexer _indexer)
        {
            //lroman
            if (_indexer.dereferencing_value is closure_substituting_node)
            {
                var nodeToVisit = new indexer(((closure_substituting_node) _indexer.dereferencing_value).substitution,
                                              _indexer.indexes);
                visit(nodeToVisit);
                return;
            }

            motivation mot = motivation_keeper.motivation;
            SyntaxTree.ident idi = _indexer.dereferencing_value as SyntaxTree.ident;
            if (idi != null)
            {
                SymbolInfo si = context.find(idi.name);
                if (si == null)
                {
                    AddError(new UndefinedNameReference(idi.name, get_location(idi)));
                }
                if (si.sym_info.general_node_type == general_node_type.type_node)
                {
                    indexer_as_type_indexes((type_node)si.sym_info, _indexer.indexes, mot, get_location(idi));
                    return;
                }
                if (si.sym_info.general_node_type == general_node_type.property_node)
                {
                    property_node pn = (property_node)si.sym_info;
                    if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                    {
                        static_property_reference spr = new static_property_reference(pn, get_location(idi));
                        indexer_as_property_indexes(spr, _indexer.indexes, mot, get_location(idi));
                        return;
                    }
                    else
                    {
                        this_node thisnode = new this_node(pn.comprehensive_type, get_location(_indexer));
                        location loc111 = get_location(idi);
                        non_static_property_reference nspr = new non_static_property_reference(pn, thisnode, loc111);
                        indexer_as_property_indexes(nspr, _indexer.indexes, mot, loc111);
                        return;
                    }
                }
                expression_node en = ident_value_reciving(si, idi);
              
                indexer_as_expression_index(en, _indexer.indexes, mot, get_location(idi));
                return;
            }
            else
            {
                SyntaxTree.dot_node dotnd = _indexer.dereferencing_value as SyntaxTree.dot_node;
                if (dotnd != null)
                {
                    SyntaxTree.ident id = dotnd.right as SyntaxTree.ident;
                    semantic_node sn = convert_semantic_strong(dotnd.left);
                    switch (sn.general_node_type)
                    {
                        case general_node_type.type_node:
                            {
                                type_node ttp = (type_node)sn;
                                SymbolInfo si = ttp.find_in_type(id.name, context.CurrentScope);
                                if (si == null)
                                {
                                    AddError(new UndefinedNameReference(id.name, get_location(id)));
                                }
                                if (si.sym_info.general_node_type == general_node_type.property_node)
                                {
                                    property_node pn = (property_node)si.sym_info;
                                    static_property_reference spr = new static_property_reference(pn, get_location(id));
                                    indexer_as_property_indexes(spr, _indexer.indexes, mot, get_location(dotnd));
                                    return;
                                }
                                expression_node exp1 = create_static_expression(ttp, id, si);
                                indexer_as_expression_index(exp1, _indexer.indexes, mot, get_location(id));
                                return;
                            }
                        case general_node_type.namespace_node:
                        case general_node_type.unit_node:
                            {
                                SymbolInfo si = null;
                                if (sn is namespace_node)
                                    si = ((namespace_node)sn).find(id.name);
                                else
                                    si = ((unit_node)sn).find_only_in_namespace(id.name);
                                if (si == null)
                                {
                                    AddError(new UndefinedNameReference(id.name, get_location(id)));
                                }
                                location lloc = get_location(id);
                                if (si.sym_info.general_node_type == general_node_type.type_node)
                                {
                                    type_node tn = (type_node)si.sym_info;
                                    indexer_as_type_indexes(tn, _indexer.indexes, mot, lloc);
                                    return;
                                }
                                expression_node exp2 = ident_value_reciving(si, id);
                                indexer_as_expression_index(exp2, _indexer.indexes, mot, get_location(id));
                                return;
                            }
                        case general_node_type.expression:
                            {
                                expression_node ex = (expression_node)sn;
                                SymbolInfo si = ex.type.find_in_type(id.name, context.CurrentScope);
                                if (si == null)
                                {
                                    AddError(new UndefinedNameReference(id.name, get_location(id)));
                                }
                                if (si.sym_info.general_node_type == general_node_type.property_node)
                                {
                                    property_node pn = (property_node)si.sym_info;
                                    if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                                    {
                                        AddError(new CanNotReferenceToStaticPropertyWithExpression(pn, get_location(dotnd), ex.type));
                                    }
                                    location lloc11 = get_location(dotnd);
                                    try_convert_typed_expression_to_function_call(ref ex);
                                    non_static_property_reference nspr = new non_static_property_reference(pn, ex, lloc11);
                                    indexer_as_property_indexes(nspr, _indexer.indexes, mot, lloc11);
                                    return;
                                }
                                expression_node en2 = expression_value_reciving(id, si, ex, false);
                                indexer_as_expression_index(en2, _indexer.indexes, mot, get_location(id));
                                return;
                            }
                    }
                }
                else
                {
                    expression_node expr = convert_strong(_indexer.dereferencing_value);
                    indexer_as_expression_index(expr, _indexer.indexes, mot, get_location(_indexer.dereferencing_value));
                    return;
                }
            }
            /*expression_node left=convert_strong(_indexer.dereferencing_value);
            foreach(SyntaxTree.expression exp in _indexer.indexes.expressions)
            {
                expression_node right=convert_strong(exp);
                SymbolInfo si=left.type.find_in_type(compiler_string_consts.indexer_name);
                definition_node dn=check_name_node_type(compiler_string_consts.indexer_name,si,get_location(exp),
                    general_node_type.function_node);
                expression_nodeArrayList exprs=new expression_nodeArrayList();
                exprs.Add(right);
                function_node fn=convertion_data_and_alghoritms.select_function(exprs,si,get_location(exp));
                base_function_call_with_method bfc=create_not_static_method_call(fn,left,get_location(exp),false);
                bfc.parameters.AddRange(exprs);
                left=bfc;
            }*/
            /*switch (mot)
            {
                case motivation.address_reciving:
                {
                    return_addressed_value(left);
                    return;
                }
                case motivation.expression_evaluation:
                {
                    return_value(left);
                    return;
                }
                case motivation.semantic_node_reciving:
                {
                    return_semantic_value(left);
                    return;
                }		
            }*/
        }

        public override void visit(SyntaxTree.roof_dereference _roof_dereference)
        {
            expression_node exp = null;
            motivation mot = motivation_keeper.motivation;
            /*
            if (mot == motivation.address_reciving)
            {
                tmp = motivation_keeper.motivation;
            }
            */
            exp = convert_strong(_roof_dereference.dereferencing_value);
            if (exp is typed_expression) exp = convert_typed_expression_to_function_call(exp as typed_expression);
            //Check this expression
            ref_type_node rtn = exp.type as ref_type_node;
            //TODO: Сделать нормальное сообщение об ошибке.
            if (rtn == null)
            {
                AddError(get_location(_roof_dereference.dereferencing_value), "CANNOT_DEREFER_THIS_EXPR_OF_NON_POINTER_TYPE");
            }
            if (rtn.pointed_type == null)
                AddError(get_location(_roof_dereference.dereferencing_value), "TYPE_{0}_IS_NOT_DETERMINED_COMPLETELY", rtn.PointedTypeName);
            dereference_node res = new dereference_node(exp, get_location(_roof_dereference));
            switch (mot)
            {
                case motivation.address_reciving: return_addressed_value(res); break;
                case motivation.expression_evaluation: return_value(res); break;
                case motivation.semantic_node_reciving: return_semantic_value(res); break;
            }
            return;
        }

        public override void visit(SyntaxTree.dereference _dereference)
        {
            throw new NotSupportedError(get_location(_dereference));
        }

        public override void visit(SyntaxTree.expression_list _expression_list)
        {
            //throw new NotSupportedError(get_location(_expression_list));
            int count = _expression_list.expressions.Count;
            statement_node_list snl = new statement_node_list();
            for (int i = 0; i < count - 1; i++)
            {
                statement_node sn = ret.visit(_expression_list.expressions[i]);
                snl.AddElement(sn);
            }
            expression_node ex = ret.visit(_expression_list.expressions[count - 1]);
            statements_expression_node sen = new statements_expression_node(
                snl, ex, get_location(_expression_list));
            return_value(sen);
        }

        public override void visit(SyntaxTree.string_const _string_const)
        {
            expression_node en = null;
            if (_string_const.Value.Length != 1)
            {
                en = new string_const_node(_string_const.Value, get_location(_string_const));
                if (SemanticRules.GenerateNativeCode && SemanticRules.StringType != null)
                    en.type = SemanticRules.StringType;
            }
            else
                en = new char_const_node(Convert.ToChar(_string_const.Value), get_location(_string_const));
            

            return_value(en);
        }

        public override void visit(SyntaxTree.program_name _program_name)
        {
            throw new NotSupportedError(get_location(_program_name));
        }

        public override void visit(SyntaxTree.program_tree _program_tree)
        {
            _program_tree.compilation_units[0].visit(this);
        }

        public override void visit(SyntaxTree.declarations _subprogram_definitions)
        {
        	if (SemanticRules.OrderIndependedTypeNames)
        	{
        		foreach (SyntaxTree.declaration sd in _subprogram_definitions.defs)
            	{
        			if (sd is SyntaxTree.type_declaration)
        			{
        				convertion_data_and_alghoritms.check_node_parser_error(sd);
                		sd.visit(this);
        			}
            	}
        	}
        	foreach (SyntaxTree.declaration sd in _subprogram_definitions.defs)
            {
                convertion_data_and_alghoritms.check_node_parser_error(sd);
                sd.visit(this);
            }
            if (SemanticRules.OrderIndependedFunctionNames)
            {
            	visit_function_realizations(_subprogram_definitions);
            	context.clear_member_bindings();
            }
        }

        public override void visit(SyntaxTree.declaration _subprogram_definition)
        {
            throw new NotSupportedError(get_location(_subprogram_definition));
        }

        private SyntaxTree.expression get_possible_array_const(SyntaxTree.expression expr, type_node tn, int rank)
        {
            try
            {
                if (tn == null)
                    return expr;
                if (rank == 0)
                    return get_possible_array_const(expr, tn.element_type);
                if (expr is SyntaxTree.bracket_expr)
                {
                    SyntaxTree.array_const arr = new SyntaxTree.array_const();
                    arr.source_context = expr.source_context;
                    arr.elements = new SyntaxTree.expression_list();
                    arr.elements.expressions.Add(get_possible_array_const((expr as SyntaxTree.bracket_expr).expr, tn.element_type, rank - 1));
                    return arr;
                }
                else if (expr is SyntaxTree.array_const)
                {
                    SyntaxTree.array_const arr = expr as SyntaxTree.array_const;
                    if (arr.elements != null)
                        for (int i = 0; i < arr.elements.expressions.Count; i++)
                            arr.elements.expressions[i] = get_possible_array_const(arr.elements.expressions[i], tn.element_type, rank - 1);
                    return arr;
                }
            }
            catch
            {

            }
            return expr;
        }

        private SyntaxTree.expression get_possible_array_const(SyntaxTree.expression expr, type_node tn)
        {
            try
            {
                if (tn == null)
                    return expr;
                if (expr is SyntaxTree.bracket_expr && (tn.type_special_kind == SemanticTree.type_special_kind.array_kind || tn.type_special_kind == SemanticTree.type_special_kind.array_wrapper))
                {
                    array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                    if (aii != null && aii.rank > 1)
                        return get_possible_array_const(expr, tn, aii.rank);
                    SyntaxTree.array_const arr = new SyntaxTree.array_const();
                    arr.source_context = expr.source_context;
                    arr.elements = new SyntaxTree.expression_list();
                    arr.elements.expressions.Add(get_possible_array_const((expr as SyntaxTree.bracket_expr).expr, tn.element_type));
                    return arr;
                }
                else if (expr is SyntaxTree.array_const && (tn.type_special_kind == SemanticTree.type_special_kind.array_kind || tn.type_special_kind == SemanticTree.type_special_kind.array_wrapper))
                {
                    array_internal_interface aii = tn.get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
                    if (aii != null && aii.rank > 1)
                        return get_possible_array_const(expr, tn, aii.rank);
                    SyntaxTree.array_const arr = expr as SyntaxTree.array_const;
                    if (arr.elements != null)
                        for (int i = 0; i < arr.elements.expressions.Count; i++)
                            arr.elements.expressions[i] = get_possible_array_const(arr.elements.expressions[i], tn.element_type);
                    return arr;
                }
                else if (expr is SyntaxTree.record_const && tn is common_type_node)
                {
                    common_type_node ctn = tn as common_type_node;
                    SyntaxTree.record_const rec = expr as SyntaxTree.record_const;
                    for (int i = 0; i < rec.rec_consts.Count; i++)
                    {
                        if (i < ctn.fields.Count)
                            rec.rec_consts[i].val = get_possible_array_const(rec.rec_consts[i].val, ctn.fields[i].type);
                    }
                    return rec;
                }
            }
            catch
            {

            }
            return expr;
        }
        
        public override void visit(SyntaxTree.var_def_statement _var_def_statement)
        {
            if (_var_def_statement.vars_type == null && _var_def_statement.inital_value is SyntaxTree.function_lambda_definition)
                AddError(get_location(_var_def_statement.inital_value), "IMPOSSIBLE_TO_INFER_TYPES_IN_LAMBDA");  //lroman//

			bool is_event = _var_def_statement.is_event;

            if (context.converting_block() == block_type.type_block &&
                context.converted_type.IsInterface && !is_event)
            {
                AddError(get_location(_var_def_statement), "INVALID_INTERFACE_MEMBER");
            }

            if (is_event && context.converting_block() == block_type.type_block && context.converted_type.IsInterface && (_var_def_statement.vars_type is function_header || _var_def_statement.vars_type is procedure_header))
                AddError(get_location(_var_def_statement.vars_type), "ANONYMOUS_DELEGATE_IN_INTERFACE_NOT_ALLOWED");

            type_node tn = null;
            expression_node inital_value = null;
            if (_var_def_statement.vars_type != null)
            {
                tn = convert_strong(_var_def_statement.vars_type);
                LambdaHelper.InferTypesFromVarStmt(tn, _var_def_statement.inital_value as SyntaxTree.function_lambda_definition, this);  //lroman//
                ref_type_node rt = tn as ref_type_node;
                if (rt != null)
                {
                    if (type_section_converting)
                    {
                        RefTypesForCheckPointersTypeForDotNetFramework.Add(rt);
                    }
                    else
                    {
                        CheckPointersTypeForDotNetFramework(rt.pointed_type, get_location(_var_def_statement.vars_type));
                    }
                }
                if (is_event && !tn.IsDelegate)
                    AddError(get_location(_var_def_statement.vars_type), "EVENT_TYPE_MUST_BE_DELEGATE");
                //if (tn == SystemLibrary.SystemLibrary.void_type)
                //	AddError(new VoidNotValid(get_location(_var_def_statement.vars_type)));
                check_for_type_allowed(tn,get_location(_var_def_statement.vars_type));
                if (context.converting_block() == block_type.type_block && context.converted_type.is_value_type)
                    CheckForCircuralInRecord(tn, get_location(_var_def_statement.vars_type));
                if (_var_def_statement.inital_value != null)
                	if (is_event) AddError(new NotSupportedError(get_location(_var_def_statement.inital_value)));
                else
                {
                	_var_def_statement.inital_value = get_possible_array_const(_var_def_statement.inital_value,tn);
                	inital_value = convert_strong_to_constant_or_function_call_for_varinit(convert_strong(_var_def_statement.inital_value), tn);
                }
            }
            else
            {
            	if (is_event)
                    AddError(get_location(_var_def_statement), "EVENT_MUST_HAVE_TYPE");
            	expression_node cn = convert_strong_to_constant_or_function_call_for_varinit(convert_strong(_var_def_statement.inital_value));
                if (cn is constant_node)
                    (cn as constant_node).SetType(DeduceType(cn.type, get_location(_var_def_statement.inital_value)));
                inital_value = cn;
                tn = inital_value.type;
                if (tn is null_type_node)
                	AddError(cn.location, "CAN_NOT_DEDUCE_TYPE_{0}", tn.name);
            }
            if (inital_value != null && inital_value is typed_expression)
            {
            	inital_value = convert_typed_expression_to_function_call(inital_value as typed_expression);
            	tn = inital_value.type;
            	if (inital_value is typed_expression)
            	{
            		base_function_call bfc = ((inital_value as typed_expression).type as delegated_methods).proper_methods[0];
                    if (bfc.function.is_generic_function && _var_def_statement.vars_type == null)
                    {
                        AddError(inital_value.location, "CAN_NOT_DEDUCE_TYPE_{0}", null);
                    }
                    common_type_node del =
            			convertion_data_and_alghoritms.type_constructor.create_delegate(context.get_delegate_type_name(), bfc.simple_function_node.return_value_type, bfc.simple_function_node.parameters, context.converted_namespace, null);
            		context.converted_namespace.types.AddElement(del);
            		tn = del;
            		inital_value = convertion_data_and_alghoritms.explicit_convert_type(inital_value, del);
            		inital_value.type = tn;
            	}
                else if (inital_value is base_function_call)
                {
                    base_function_call bfc = inital_value as base_function_call;
                    if (bfc.function.is_generic_function && _var_def_statement.vars_type == null)
                    {
                        AddError(inital_value.location, "CAN_NOT_DEDUCE_TYPE_{0}", null);
                    }
                }
            }
            foreach (SyntaxTree.ident id in _var_def_statement.vars.idents)
            {
                location lid = get_location(id);
                SemanticTree.polymorphic_state ps = SemanticTree.polymorphic_state.ps_common;
                if (_var_def_statement.var_attr == SyntaxTree.definition_attribute.Static)
                    ps = SemanticTree.polymorphic_state.ps_static;
                if (!is_event)
                {
                	var_definition_node vdn = context.add_var_definition(id.name, lid, ps);
                	assign_doc_info(vdn,_var_def_statement);
                	if (_var_def_statement.attributes != null)
            		{
            			make_attributes_for_declaration(_var_def_statement, vdn);
            		}
                }
				else
				{
                    event_node ce = null;
                    if (context.converted_type != null && context.converted_type.IsInterface)
                        ps = SemanticTree.polymorphic_state.ps_virtual_abstract;
                    if (context.converted_type != null)
                        ce = context.add_event_definition(id.name, lid, tn, ps, context.converted_type.IsInterface);
                    else if (context.top_function == null)
                        ce = context.add_namespace_event(id.name, lid, tn);
                    else
                        AddError(lid, "EVENT_MUST_BE_IN_CLASS_OR_NAMESPACE");
                    assign_doc_info(ce,_var_def_statement);
					if (_var_def_statement.attributes != null)
            		{
            			make_attributes_for_declaration(_var_def_statement, ce);
            		}
				}
            }
            if (is_event) return;
            context.save_var_definitions();

            context.restore_var_definitions();
            context.close_var_definition_list(tn, inital_value);
            if (!SemanticRules.ManyVariablesOneInitializator && _var_def_statement.inital_value != null && _var_def_statement.vars.idents.Count > 1)
                AddError(get_location(_var_def_statement.inital_value), "ONE_VARIABLE_ONE_INITIALIZATOR");
        }
		
        private void CheckForCircularityInPointers(ref_type_node left, type_node right, location loc)
        {
        	if (left == null || right == null) 
        		return;
        	if (left == right)
				// #ErrorRefactoringProblem
				// AddError(loc, "CIRCULARITY_IN_POINTER"); // Если заменить этой строкой следующую, то при запуске bin\TestRunner.exe это приложение зацикливается, что сопровождается утечкой памяти
        		AddError(new CircuralityInPointer(loc));
        	if (right is ref_type_node)
        		CheckForCircularityInPointers(left, (right as ref_type_node).pointed_type, loc);
        	else if (right.type_special_kind == SemanticTree.type_special_kind.array_kind || right.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
        		CheckForCircularityInPointers(left,right.element_type,loc);
        }
        
        private void CheckForCircuralInRecord(type_node tn, location loc)
        {
            if (tn == context.converted_type || tn.original_generic == context.converted_type)
            {
                AddError(loc, "CIRCULARITY_IN_RECORD");
            }
            if (tn is common_type_node)
            {
                common_type_node ctn = tn as common_type_node;
                if (ctn.is_value_type)
                {
                    foreach (class_field fld in ctn.fields)
                        CheckForCircuralInRecord(fld.type, loc);
                }
                else
                {
                    foreach (class_field fld in ctn.fields)
                        if (fld.type is simple_array) CheckForCircuralInRecord((fld.type as simple_array).element_type, loc);
                }
            }
        }

        //Пытается вывести тип
        private type_node DeduceType(type_node tn, location loc)
        {
            if (tn is undefined_type || tn is null_type_node)
            {
                if (tn is ArrayConstType)
                    return convertion_data_and_alghoritms.type_constructor.create_unsized_array((tn as ArrayConstType).element_type, context.converted_namespace, 1, loc);
                AddError(loc, "CAN_NOT_DEDUCE_TYPE_{0}", tn.name);
            }
            return tn;
        }

        public override void visit(SyntaxTree.ident_list _ident_list)
        {
            throw new NotSupportedError(get_location(_ident_list));
        }

        public override void visit(SyntaxTree.variable_definitions _variable_definitions)
        {
            bool is_event = false;
            foreach (SyntaxTree.var_def_statement vds in _variable_definitions.var_definitions)
            {
                convertion_data_and_alghoritms.check_node_parser_error(vds);
                if (vds.is_event && !is_event)
                    is_event = true;
                else if (is_event)
                    vds.is_event = true;
                vds.visit(this);
            }
        }

        public override void visit(SyntaxTree.named_type_reference _named_type_reference)
        {
            //unit_name
            //TODO: Здесь список идентефикаторов.
            /*type_node tn=find_type(_named_type_reference.names[0].name,get_location(_named_type_reference));
            return_value(tn);*/
            /*if (blocks.is_tempalte)
            {
                if (_named_type_reference.names.Count == 1)
                {
                    type_node ttn = blocks.try_get_template_arg(_named_type_reference.names[0].name);
                    if (ttn != null)
                    {
                        return_value(ttn);
                        return;
                    }
                }
            }*/
            type_node tn = find_type(_named_type_reference, get_location(_named_type_reference));
//            if (SystemUnitAssigned && SystemLibrary.SystemLibInitializer.TextFileType.Found && tn.name == compiler_string_consts.text_file_name_type_name)
//                if (tn == SystemLibrary.SystemLibInitializer.TextFileType.TypeNode)
//                    SystemLibrary.SystemLibInitializer.TextFileType.TypeNode.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.text_file;
            return_value(tn);
        }

        public override void visit(SyntaxTree.type_definition _type_definition)
        {
            throw new NotSupportedError(get_location(_type_definition));
        }

        public override void visit(SyntaxTree.addressed_value _addressed_value)
        {
            throw new NotSupportedError(get_location(_addressed_value));
        }

        private addressed_expression ident_address_reciving(SymbolInfo si, SyntaxTree.ident _ident)
        {
            location lloc = get_location(_ident);
            definition_node dn = null;
            if (!internal_is_assign)
                dn = context.check_name_node_type(_ident.name, si, lloc,
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node);
            else
                dn = context.check_name_node_type(_ident.name, si, lloc,
                general_node_type.variable_node, general_node_type.property_node, general_node_type.event_node, general_node_type.constant_definition);
            if (dn.general_node_type == general_node_type.constant_definition)
                AddError(get_location(_ident), "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
            switch (dn.general_node_type)
            {
                case general_node_type.variable_node:
                    {
                        local_variable lv = dn as local_variable;
                        if (lv != null && (lv.function is common_method_node))
                        {
                            //self variable
                            if ((lv.function as common_method_node).self_variable == lv)
                                AddError(lloc, "VARIABLE_{0}_READONLY", lv.name);
                        }
                        return create_variable_reference(dn, lloc);
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            static_property_reference spr = new static_property_reference(pn, lloc);
                            return spr;
                        }
                        //this_node thisnode=new this_node(context.converted_type,lloc);
                        if (pn.comprehensive_type.Scope == null && pn is compiled_property_node)
                        	(pn.comprehensive_type as compiled_type_node).init_scope();
                        non_static_property_reference nspr = new non_static_property_reference(pn, GetCurrentObjectReference(pn.comprehensive_type.Scope, pn, lloc), lloc);
                        return nspr;
                    }
                case general_node_type.event_node:
                    {
                        if (dn.semantic_node_type == semantic_node_type.compiled_event)
                        {
                            compiled_event ce = (compiled_event)dn;
                            if (ce.is_static)
                            {
                                static_event_reference ser = new static_event_reference(ce, get_location(_ident));
                                return ser;
                            }
                            nonstatic_event_reference nser = new nonstatic_event_reference(
                                GetCurrentObjectReference(ce.comprehensive_type.Scope, ce, lloc), ce, lloc);
                            return nser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_event)
                        {
                        	common_event ce = (common_event)dn;
                        	if (ce.is_static)
                            {
                                static_event_reference ser = new static_event_reference(ce, get_location(_ident));
                                return ser;
                            }
                            nonstatic_event_reference nser = new nonstatic_event_reference(
                                GetCurrentObjectReference(ce.comprehensive_type.Scope, ce, lloc), ce, lloc);
                            return nser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_namespace_event)
                        {
                            common_namespace_event cne = (common_namespace_event)dn;
                            static_event_reference ser = new static_event_reference(cne, get_location(_ident));
                            return ser;
                        }
                        break;
                    }
            }
            return null;
        }

        private expression_node ident_value_reciving(SymbolInfo si, SyntaxTree.ident _ident)
        {
            //SymbolInfo si=blocks.find(_ident.name);
            location lloc = get_location(_ident);
            definition_node dn = context.check_name_node_type(_ident.name, si, get_location(_ident), general_node_type.variable_node,
                general_node_type.function_node, general_node_type.property_node, general_node_type.constant_definition,
                general_node_type.event_node);
            switch (dn.general_node_type)
            {
                case general_node_type.constant_definition:
                    {
                        constant_definition_node cdn = (constant_definition_node)dn;
                        if (cdn.const_value is array_const || cdn.const_value is record_constant || cdn.const_value is common_namespace_function_call_as_constant)
                        {
                        	return create_constant_reference(cdn,lloc);
                        }
                        if (cdn.const_value == null)
                            AddError(new UndefinedNameReference(cdn.name,get_location(_ident)));
                        constant_node cn = cdn.const_value.get_constant_copy(get_location(_ident));
                        if (cn != null) return cn;
                        return cdn.const_value;
                    }
                case general_node_type.variable_node:
                    {
                        local_variable lv = dn as local_variable;
                        if (lv != null && (lv.function is common_method_node))
                        {
                            //self variable
                            if ((lv.function as common_method_node).self_variable == lv)
                                //return new this_node((lv.function as common_method_node).cont_type, lloc);
                                return new this_node(lv.type, lloc);
                            //return GetCurrentObjectReference((lv.function as common_method_node).cont_type.Scope, lloc);
                        }
                        return create_variable_reference(dn, lloc);
                    }
                case general_node_type.function_node:
                    {
                        //return convertion_data_and_alghoritms.create_full_function_call(new expressions_list(),
                        //	si,lloc,blocks.converted_type,blocks.top_function,false);
                        if (!(si.sym_info is common_in_function_function_node))
                            return make_delegate_wrapper(null, si, lloc, ((si.sym_info is common_method_node) && ((common_method_node)si.sym_info).IsStatic));
                        return convertion_data_and_alghoritms.create_full_function_call(new expressions_list(),
                        	si,lloc,context.converted_type,context.top_function,false);
                    }
                case general_node_type.property_node:
                    {
                        property_node pn = (property_node)dn;
                        function_node fn = pn.get_function;
                        if (fn == null)
                        {
                            AddError(new ThisPropertyCanNotBeReaded(pn, lloc));
                        }
                        if (pn.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            return create_static_method_call(fn, lloc, pn.comprehensive_type, false);
                        }
                        check_property_no_params(pn, lloc);
                        //this_node thisnode = new this_node(context.converted_type, lloc);
                        if (pn.comprehensive_type.Scope == null && pn is compiled_property_node)
                        	(pn.comprehensive_type as compiled_type_node).init_scope();
                        return create_not_static_method_call(fn, GetCurrentObjectReference(pn.comprehensive_type.Scope, fn, lloc), lloc, false);
                    }
                case general_node_type.event_node:
                    {
                        if (dn.semantic_node_type == semantic_node_type.compiled_event)
                        {
                            compiled_event ce = (compiled_event)dn;
                            if (ce.is_static)
                            {
                                static_event_reference ser = new static_event_reference(ce, get_location(_ident));
                                return ser;
                            }
                            nonstatic_event_reference nser = new nonstatic_event_reference(
                                new this_node(context.converted_type, lloc), ce, lloc);
                            return nser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_event)
                        {
                            common_event ce = (common_event)dn;
                            if (ce.is_static)
                            {
                                static_event_reference ser = new static_event_reference(ce, get_location(_ident));
                                return ser;
                            }
                            nonstatic_event_reference nser = new nonstatic_event_reference(
                                new this_node(context.converted_type, lloc), ce, lloc);
                            return nser;
                        }
                        else if (dn.semantic_node_type == semantic_node_type.common_namespace_event)
                        {
                            common_namespace_event cne = (common_namespace_event)dn;
                            if (_compiled_unit.namespaces.IndexOf(cne.namespace_node) != -1)
                            {
                                static_event_reference ser = new static_event_reference(cne, get_location(_ident));
                                return ser;
                            }
                            else
                                AddError(get_location(_ident), "EVENT_{0}_MUST_BE_IN_LEFT_PART", cne.name);
                        }
                        break;
                    }
            }
            return null;
        }

        private expression_node create_constant_reference(constant_definition_node cdn, location loc)
        {
        	if (cdn is namespace_constant_definition)
        		return new namespace_constant_reference(cdn as namespace_constant_definition,loc);
        	else if (cdn is function_constant_definition)
        		return new function_constant_reference(cdn as function_constant_definition,loc);
        	else 
        		return cdn.const_value;
        }
        
        private SymbolInfo find_in_base(SyntaxTree.inherited_message ident)
        {
        	if (context.converted_type == null)
            {
                AddError(get_location(ident), "INHERITED_EXPECTED_IN_CLASS");
            }
            if (context.converted_type.base_type == null)
            {
                AddError(get_location(ident), "NO_BASE_CLASS_DEFINED_BUT_INHERITED_MEET");
            }
            common_method_node cmn = context.converted_func_stack.first() as common_method_node;
            if (cmn == null)
            {
                AddError(get_location(ident), "NAME_IN_BASE_CLASS_MUST_BE_METHOD");
            }
           
            SymbolInfo si = null;
            if (cmn.is_constructor)
            	si = context.converted_type.base_type.find_in_type(compiler_string_consts.default_constructor_name, context.CurrentScope);
            else
            	si = context.converted_type.base_type.find_in_type(cmn.name, context.CurrentScope);
            while (si != null)
            {
            	if (!cmn.is_constructor)
            	{
            		if (si.sym_info is function_node && convertion_data_and_alghoritms.function_eq_params_and_result(si.sym_info as function_node,cmn))
            			return si;
            	}
            	else
            	{
            		if (si.sym_info is function_node && convertion_data_and_alghoritms.function_eq_params(si.sym_info as function_node,cmn))
            			return si;
            	}
            	si = si.Next;
            }
            if (cmn.is_constructor)
                AddError(get_location(ident), "NO_CONSTRUCTOR_IN_BASE_CLASS_WITH_SAME_PARAMETERS");
            else
                AddError(get_location(ident), "NAME_IN_BASE_CLASS_MUST_BE_METHOD");
            return si;
            /*if (si == null)
            {
                throw new UndefinedNameReference(name, get_location(ident));
            }
            if (!(si.sym_info is common_method_node) && !(si.sym_info is compiled_function_node))
            {
            	throw new NameInBaseClassMustBeMethod(name, get_location(ident));
            }
            common_function_node cfn = si.sym_info as common_function_node;*/
        }
        
        private SymbolInfo find_in_base(SyntaxTree.inherited_ident ident)
        {
            if (context.converted_type == null)
            {
                AddError(get_location(ident), "INHERITED_EXPECTED_IN_CLASS");
            }
            if (context.converted_type.base_type == null)
            {
                AddError(get_location(ident), "NO_BASE_CLASS_DEFINED_BUT_INHERITED_MEET");
            }
            SymbolInfo si = context.converted_type.base_type.find_in_type(ident.name, context.CurrentScope);
            if (si == null)
            {
            	si = context.converted_namespace.find(ident.name);
            	if (si != null)
            	    AddError(get_location(ident), "CLASS_MEMBER_{0}_EXPECTED", ident.name);
            	else
            	    AddError(new UndefinedNameReference(ident.name, get_location(ident)));
            }
            return si;
        }

        private addressed_expression inherited_ident_address_reciving(SyntaxTree.inherited_ident _ident)
        {
            SymbolInfo si = find_in_base(_ident);
            return ident_address_reciving(si, _ident);
        }

        private addressed_expression ident_address_reciving(SyntaxTree.ident _ident)
        {
            if ((context.converting_block() == block_type.function_block) && (context.top_function.return_variable != null))
            {
                SymbolInfo si = context.top_function.scope.FindOnlyInScope(_ident.name);//context.find_only_in_namespace(_ident.name);
                if (si == null)
                {
                    int comp = SystemLibrary.SystemLibrary.string_comparer.Compare(_ident.name, context.top_function.name);
                    if (comp == 0)
                    {
                        local_variable lv = context.top_function.return_variable;
                        return new local_variable_reference(lv, 0, get_location(_ident));
                    }
                }
            }
            SymbolInfo idsi = context.find(_ident.name);

            return ident_address_reciving(idsi, _ident);
        }

        internal addressed_expression create_variable_reference(definition_node dn, location lloc)
        {
            switch (dn.semantic_node_type)
            {
                case semantic_node_type.local_variable:
                    {
                        local_variable lv = (local_variable)dn;
                        if (context.converting_block() == block_type.function_block)
                        {
                            return new local_variable_reference(lv,
                                convertion_data_and_alghoritms.symbol_table.GetRelativeScopeDepth(lv.function.scope, context.top_function.scope), lloc);
                        }
                        else if (context.converting_block() == block_type.namespace_block)
                        {
                            return new local_variable_reference(lv, 0, lloc);
                        }
                        else
                        {
                            throw new CompilerInternalError("Invalid block type");
                        }
                    }
                case semantic_node_type.namespace_variable:
                    {
                        namespace_variable nv = (namespace_variable)dn;
                        return new namespace_variable_reference(nv, lloc);
                    }
                case semantic_node_type.class_field:
                    {
                        class_field cf = (class_field)dn;
                        if (cf.polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            return new static_class_field_reference(cf, lloc);
                        }
                        expression_node obj = GetCurrentObjectReference(cf.cont_type.Scope,cf, lloc); //new this_node(context.converted_type, lloc);
                        return new class_field_reference(cf, obj, lloc);
                    }
                case semantic_node_type.common_parameter:
                    {
                        if (context.in_parameters_block)
                            AddError(new ParameterReferenceInDefaultParameterNotAllowed(lloc));
                        common_parameter cpar = (common_parameter)dn;
                        return new common_parameter_reference(cpar,
                            convertion_data_and_alghoritms.symbol_table.GetRelativeScopeDepth(cpar.common_function.scope, context.top_function.scope), lloc);
                    }
                case semantic_node_type.local_block_variable:
                    {
                        return new local_block_variable_reference((local_block_variable)dn, lloc);
                    }
            	case semantic_node_type.compiled_variable_definition:
            		{
            			return new static_compiled_variable_reference((compiled_variable_definition)dn,lloc);
            		}
                default: throw new NotSupportedError(lloc);
            }
            return null;
        }

        private expression_node ident_value_reciving(SyntaxTree.ident _ident)
        {
            SymbolInfo si = context.find(_ident.name);
            return ident_value_reciving(si, _ident);
        }
		
        private expression_node inherited_message_value_reciving(SyntaxTree.inherited_message _ident)
        {
        	SymbolInfo si = find_in_base(_ident);
        	bool constr = false;
            if (si != null) 
            if (si.sym_info is common_method_node)
            {
            	if ((si.sym_info as common_method_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                    AddError(get_location(_ident), "CANNOT_CALL_ABSTRACT_METHOD");
            	constr = (si.sym_info as common_method_node).is_constructor;
            }
            else
            if (si.sym_info is compiled_function_node)
            {
            	if ((si.sym_info as compiled_function_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                    AddError(get_location(_ident), "CANNOT_CALL_ABSTRACT_METHOD");
            }
            else if (si.sym_info is compiled_constructor_node)
            {
            	constr = true;
            }
            location loc = get_location(_ident);
            function_node fn = si.sym_info as function_node;
            base_function_call bfc = null;
            common_function_node cfn = context.converted_func_stack.first();
            int depth = context.converted_func_stack.size-1;
            if (!constr)
            {
            	if (fn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
            	{
            		bfc = create_not_static_method_call(fn,new this_node(context.converted_type,loc),loc,true);
            		if (bfc is common_method_call)
            			(bfc as common_method_call).virtual_call = false;
            		else 
            			(bfc as compiled_function_call).virtual_call = false;
            	}
            	else if (fn is common_method_node)
            		bfc = create_static_method_call(fn,loc,(fn as common_method_node).cont_type,true);
            	else if (fn is compiled_function_node)
            		bfc = create_static_method_call(fn,loc,(fn as compiled_function_node).cont_type,true);
            	else
            		throw new NotSupportedError(loc);
            }
            else
            {
            	if (!context.allow_inherited_ctor_call)
                    AddError(loc, "INHERITED_CONSTRUCTOR_CALL_MUST_BE_FIRST");
            	expressions_list exprs = new expressions_list();
            	foreach (common_parameter cp in cfn.parameters)
            	{
            		exprs.AddElement(new common_parameter_reference(cp,depth,loc));
            	}
            	if (fn is common_method_node)
            	{
            		common_constructor_call ccc = create_constructor_call((fn as common_method_node).cont_type,exprs,loc) as common_constructor_call;
            		ccc._new_obj_awaited = false;
            		return ccc;
            	}
            	else if (fn is compiled_constructor_node)
            	{
            		compiled_constructor_call ccc = create_constructor_call((fn as compiled_constructor_node).compiled_type,exprs,loc) as compiled_constructor_call;
            		ccc._new_obj_awaited = false;
            		return ccc;
            	}
            	else
            	throw new NotSupportedError(loc);
            }
            
            foreach (common_parameter cp in cfn.parameters)
            {
            	bfc.parameters.AddElement(new common_parameter_reference(cp,depth,loc));
            }
            return bfc;
        }
        
        private expression_node inherited_ident_value_reciving(SyntaxTree.inherited_ident _ident)
        {
            SymbolInfo si = find_in_base(_ident);
            if (si != null) 
            if (si.sym_info is common_method_node)
            {
            	if ((si.sym_info as common_method_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                    AddError(get_location(_ident), "CANNOT_CALL_ABSTRACT_METHOD");
            }
            else
            if (si.sym_info is compiled_function_node)
            {
            	if ((si.sym_info as compiled_function_node).polymorphic_state == SemanticTree.polymorphic_state.ps_virtual_abstract)
                    AddError(get_location(_ident), "CANNOT_CALL_ABSTRACT_METHOD");
            }
            return ident_value_reciving(si, _ident);
        }


        private semantic_node ident_semantic_reciving(SyntaxTree.ident _ident)
        {
            SymbolInfo si_left = context.find(_ident.name);
            return ident_semantic_reciving(si_left, _ident);
        }

        private semantic_node inherited_ident_semantic_reciving(SyntaxTree.inherited_ident _ident)
        {
            SymbolInfo si = find_in_base(_ident);
            return ident_semantic_reciving(si, _ident);
        }

        private semantic_node ident_semantic_reciving(SymbolInfo si_left, SyntaxTree.ident _ident)
        {
            //SymbolInfo si_left=blocks.find(_ident.name);
            location lloc = get_location(_ident);
            if (si_left == null)
            {
                AddError(new UndefinedNameReference(_ident.name, lloc));
            }
            definition_node dn = context.check_name_node_type(_ident.name, si_left, get_location(_ident),
                general_node_type.constant_definition, general_node_type.function_node,
                general_node_type.namespace_node, general_node_type.property_node,
                general_node_type.type_node, general_node_type.variable_node,
                general_node_type.unit_node);
            switch (dn.general_node_type)
            {
                case general_node_type.constant_definition:
                case general_node_type.function_node:
                case general_node_type.property_node:
                case general_node_type.variable_node:
                    {
                        return ident_value_reciving(si_left, _ident);
                    }
                case general_node_type.namespace_node:
                    {
                        //throw new CompilerInternalError("Unsupported now.");
                        return dn;
                    }
                case general_node_type.type_node:
                    {
                        type_node tn = dn as type_node;
                        //dot_node_as_type_ident(tn,id_right,mot);
                        return tn;
                    }
                case general_node_type.unit_node:
                    {
                        //throw new CompilerInternalError("Unsupproted now.");
                        //throw new NotSupportedError(get_location(_ident));
                        return dn;
                    }
            }
            return null;
        }

        public override void visit(SyntaxTree.ident _ident)
        {
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: return_addressed_value(ident_address_reciving(_ident)); break;
                case motivation.expression_evaluation: return_value(ident_value_reciving(_ident)); break;
                //case motivation.symbol_info_reciving: return_symbol_value(blocks.find(_ident.name));break;
                case motivation.semantic_node_reciving: return_semantic_value(ident_semantic_reciving(_ident)); break;
                default: throw new NotSupportedError(get_location(_ident));
            }
            return;
        }

        public override void visit(SyntaxTree.subprogram_body _subprogram_body)
        {
            throw new NotSupportedError(get_location(_subprogram_body));
        }

        public override void visit(SyntaxTree.statement _statement)
        {
            throw new NotSupportedError(get_location(_statement));
        }

        public override void visit(SyntaxTree.double_const _double_const)
        {
            expression_node en = new double_const_node(_double_const.val, get_location(_double_const));
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: throw new CompilerInternalError("Addres reciving from constant"); break;
                case motivation.expression_evaluation: return_value(en); break;
                case motivation.semantic_node_reciving: return_semantic_value(en); break;
            }
            //return_value(en);
            return;
        }

        public override void visit(SyntaxTree.int32_const int32)
        {

            //таблица целых констант на уровне синтаксиса
            //                    - 0 +
            // 32--------16----8----|----8----16--------32----------------64(bits)
            // [  int64  )[       int32       ](  int64 ](      uint64     ]

            //таблица целых констант на уровне семантики(???)
            //                    - 0 +
            // 32--------16----8----|----8----16--------32----------------64(bits)
            // [  long   )[  int   )[byte](int](  long  ](      ulong      ]

            //Отлюченно временно (из-за ошибок временно алгоритмах выбора оперетора?)
            /*if(int32.val>=byte.MinValue && int32.val<=byte.MaxValue)
                return_value(new byte_const_node((byte)int32.val, get_location(int32)));
			else*/
            expression_node en = null;
            /*
            if ((int32.val >= byte.MinValue) && (int32.val <= byte.MaxValue))
            {
                en = new byte_const_node((byte)int32.val, get_location(int32));
            }
            else
            {
                en = new int_const_node(int32.val, get_location(int32));
            }
            */
            en = SystemLibrary.static_executors.make_int_const(int32.val, get_location(int32));
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: throw new CompilerInternalError("Addres reciving from constant"); break;
                case motivation.expression_evaluation: return_value(en); break;
                case motivation.semantic_node_reciving: return_semantic_value(en); break;
            }
            //return_value(en);
            return;
        }

        public override void visit(SyntaxTree.int64_const int64)
        {
            expression_node en = new long_const_node(int64.val, get_location(int64));
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: throw new CompilerInternalError("Addres reciving from constant"); break;
                case motivation.expression_evaluation: return_value(en); break;
                case motivation.semantic_node_reciving: return_semantic_value(en); break;
            }
            //return_value(en);
            return;
        }

        public override void visit(SyntaxTree.uint64_const uint64)
        {
            //TODO добавить обработку
            // :-)
            //throw new CompilerInternalError("Числа UInt64 доступны только в PascalABC.NET Professional Edition");
            expression_node en = new ulong_const_node(uint64.val, get_location(uint64));
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: throw new CompilerInternalError("Addres reciving from constant"); break;
                case motivation.expression_evaluation: return_value(en); break;
                case motivation.semantic_node_reciving: return_semantic_value(en); break;
            }
            return;
        }


        public override void visit(SyntaxTree.bool_const _bool_const)
        {
            expression_node en = new bool_const_node(_bool_const.val, get_location(_bool_const));
            switch (motivation_keeper.motivation)
            {
                case motivation.address_reciving: throw new CompilerInternalError("Address reciving from constant"); break;
                case motivation.expression_evaluation: return_value(en); break;
                case motivation.semantic_node_reciving: return_semantic_value(en); break;
            }
            //return_value(en);
            return;
        }

        public override void visit(SyntaxTree.const_node _const_node)
        {
            throw new NotSupportedError(get_location(_const_node));
        }

        public override void visit(SyntaxTree.un_expr _un_expr)
        {
            expression_node exp = convert_strong(_un_expr.subnode);
            expression_node res = find_operator(_un_expr.operation_type, exp, get_location(_un_expr));
            return_value(res);
        }

        public override void visit(SyntaxTree.bin_expr _bin_expr)
        {
            expression_node left = convert_strong(_bin_expr.left);
            expression_node right = convert_strong(_bin_expr.right);
            expression_node res = find_operator(_bin_expr.operation_type, left, right, get_location(_bin_expr));
            
            //ssyy, 15.05.2009
            switch (_bin_expr.operation_type)
            {
                case PascalABCCompiler.SyntaxTree.Operators.Equal:
                case PascalABCCompiler.SyntaxTree.Operators.NotEqual:
                    if (left.type.is_generic_parameter && right.type.is_generic_parameter)
                    {
                        compiled_static_method_call cfc = new compiled_static_method_call(
                            SystemLibrary.SystemLibrary.object_equals_method, get_location(_bin_expr));
                        cfc.parameters.AddElement(left);
                        cfc.parameters.AddElement(right);
                        if (_bin_expr.operation_type == PascalABCCompiler.SyntaxTree.Operators.NotEqual)
                        {
                            res = new basic_function_call(SystemLibrary.SystemLibrary.bool_not as basic_function_node, get_location(_bin_expr), cfc);
                        }
                        else
                        {
                            res = cfc;
                        }
                    }
                    else if (left is static_event_reference)
                    {
                        if ((left as static_event_reference).en is compiled_event)
                            AddError(left.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (left as static_event_reference).en.name);
                        if (context.converted_type != null && context.converted_type != ((left as static_event_reference).en as common_event).cont_type)
                            AddError(left.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (left as static_event_reference).en.name);
                    }
                    else if (right is static_event_reference)
                    {
                        if ((right as static_event_reference).en is compiled_event)
                            AddError(right.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (right as static_event_reference).en.name);
                        if (context.converted_type != null && context.converted_type != ((right as static_event_reference).en as common_event).cont_type)
                            AddError(right.location, "EVENT_{0}_MUST_BE_IN_LEFT_PART", (right as static_event_reference).en.name);
                    }
                    break;
            }
            return_value(res);
        }

        private void check_property_no_params(property_node pn, location loc)
        {
            if (pn.parameters.Count != 0)
            {
                AddError(loc, "PROPERTY_{0}_REFERENCE_WITH_INVALID_PARAMS_COUNT", pn.name);
            }
        }

        private void check_property_params(static_property_reference pr, location loc)
        {
            if (pr.property.parameters.Count != pr.fact_parametres.Count)
            {
                AddError(loc, "PROPERTY_{0}_REFERENCE_WITH_INVALID_PARAMS_COUNT", pr.property.name);
            }
        }

        private void check_property_params(non_static_property_reference pr, location loc)
        {
            if (pr.property.parameters.Count != pr.fact_parametres.Count)
            {
                AddError(loc, "PROPERTY_{0}_REFERENCE_WITH_INVALID_PARAMS_COUNT", pr.property.name);
            }
        }

        private SyntaxTree.procedure_call ConvertEventOperationToProcedureCall(SyntaxTree.assign _assign)
        {
            SyntaxTree.ident event_name = _assign.to as SyntaxTree.ident;
            SyntaxTree.dot_node dot = _assign.to as SyntaxTree.dot_node;
            if (dot != null)
                event_name = dot.right as SyntaxTree.ident;
            if (event_name == null)
                AddError(new EventNameExpected(get_location(_assign.to)));
            string format;
            if (_assign.operator_type == PascalABCCompiler.SyntaxTree.Operators.AssignmentSubtraction)
                format = compiler_string_consts.event_remove_method_nameformat;
            else
                format = compiler_string_consts.event_add_method_nameformat;
            SyntaxTree.ident add_name = new SyntaxTree.ident(string.Format(format, event_name.name));
            add_name.source_context = event_name.source_context;
            if (dot != null)
                dot.right = add_name;
            SyntaxTree.expression_list exprlist = new SyntaxTree.expression_list();
            exprlist.expressions.Add(_assign.from);
            exprlist.source_context = _assign.to.source_context;
            SyntaxTree.method_call add_methcall = new SyntaxTree.method_call(exprlist);
            if (dot != null)
                add_methcall.dereferencing_value = dot;
            else
                add_methcall.dereferencing_value = add_name;
            add_methcall.source_context = _assign.source_context;
            SyntaxTree.procedure_call add_proccall = new SyntaxTree.procedure_call(add_methcall);
            add_proccall.source_context = _assign.source_context;
            return add_proccall;
        }
        private SyntaxTree.procedure_call ConvertOperatorToProcedureCall(SyntaxTree.assign _assign)
        {
            SyntaxTree.ident operator_name = new PascalABCCompiler.SyntaxTree.ident(name_reflector.get_name(_assign.operator_type));
            operator_name.source_context = _assign.to.source_context;
            SyntaxTree.dot_node dot = new PascalABCCompiler.SyntaxTree.dot_node(_assign.to, operator_name);
            dot.source_context = _assign.to.source_context;
            SyntaxTree.expression_list exprlist = new SyntaxTree.expression_list();
            exprlist.expressions.Add(_assign.from);
            exprlist.source_context = _assign.to.source_context;
            SyntaxTree.method_call add_methcall = new SyntaxTree.method_call(exprlist);
            add_methcall.dereferencing_value = dot;
            add_methcall.source_context = _assign.source_context;
            SyntaxTree.procedure_call add_proccall = new SyntaxTree.procedure_call(add_methcall);
            add_proccall.source_context = _assign.source_context;
            return add_proccall;
        }

        private expression_node ConstructDecExpr(expression_node en, location loc)
        {
            int_const_node one = new int_const_node(1, loc);
            return find_operator(SyntaxTree.Operators.Minus, en, one, loc);
        }
		
        private void check_field_reference_for_assign(class_field_reference expr, location loc)
        {
        	if (expr.obj is namespace_constant_reference || expr.obj is function_constant_reference)
                AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
        	else if (expr.obj is class_field_reference) check_field_reference_for_assign(expr.obj as class_field_reference,loc);
        	else if (expr.obj is simple_array_indexing) check_expression_for_assign((expr.obj as simple_array_indexing).simple_arr_expr,loc);
        }
        
        private void check_expression_for_assign(expression_node expr, location loc)
        {
        	if (expr is namespace_constant_reference || expr is function_constant_reference)
                AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
        	else if (expr is class_field_reference) check_field_reference_for_assign(expr as class_field_reference,loc);
        	else if (expr is simple_array_indexing) check_expression_for_assign((expr as simple_array_indexing).simple_arr_expr,loc);
        }
        
        public bool internal_is_assign = false;
        
        public override void visit(SyntaxTree.assign _assign)
        {
        	internal_is_assign = true;
        	addressed_expression to = convert_address_strong(_assign.to);
        	internal_is_assign = false;
            if (to == null)
            	AddError(get_location(_assign.to), "CAN_NOT_ASSIGN_TO_LEFT_PART");


            #region Вывод параметров лямбда-выражения
            LambdaHelper.InferTypesFromVarStmt(to.type, _assign.from as SyntaxTree.function_lambda_definition, this);  //lroman//
            #endregion
			

            //(ssyy) Вставляю проверки прямо сюда, т.к. запарился вылавливать другие случаи.
            bool flag=false;
            general_node_type node_type = general_node_type.constant_definition;
            if (convertion_data_and_alghoritms.check_for_constant_or_readonly(to, out flag, out node_type))
            {
            	if (flag)
                    AddError(to.location, "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
            	else
            		AddError(new CanNotAssignToReadOnlyElement(to.location,node_type));
            }

            expression_node from = convert_strong(_assign.from);

            if (stflambda.Count>0) // мы находимся внутри лямбды - возможно, вложенной
            {
                var fld = stflambda.Peek();
                if (_assign.to is ident && (_assign.to as ident).name.ToLower()=="result" && fld.RealSemTypeOfResExpr == null) // если это - первое присваивание Result
                {
                    fld.RealSemTypeOfResExpr = from.type;
                    fld.RealSemTypeOfResult = to.type;
                }
                    
            }

            location loc = get_location(_assign);
			bool oper_ass_in_prop = false;
			
            //проверка на обращение к полю записи возвращенной из функции с целью присваивания
            //нужно чтобы пользователь не мог менять временный обьект
            if (to.semantic_node_type == semantic_node_type.static_property_reference ||
                to.semantic_node_type == semantic_node_type.non_static_property_reference)
            {
                property_node pn = null;
                base_function_call prop_expr = null;
                if (to.semantic_node_type == semantic_node_type.static_property_reference)
                	pn = (to as static_property_reference).property;
                else
                	pn = (to as non_static_property_reference).property;
               
                PascalABCCompiler.SyntaxTree.Operators ot = PascalABCCompiler.SyntaxTree.Operators.Undefined;
            	switch (_assign.operator_type)
                {
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentAddition:
                		ot = PascalABCCompiler.SyntaxTree.Operators.Plus;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseAND:
                		ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseAND;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseLeftShift:
                		ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseLeftShift;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseOR:
                		ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseOR;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseRightShift:
                		ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseRightShift;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentBitwiseXOR:
                		ot = PascalABCCompiler.SyntaxTree.Operators.BitwiseXOR;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentDivision:
                		ot = PascalABCCompiler.SyntaxTree.Operators.Division;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentModulus:
                		ot = PascalABCCompiler.SyntaxTree.Operators.ModulusRemainder;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentMultiplication:
                		ot = PascalABCCompiler.SyntaxTree.Operators.Multiplication;
                		oper_ass_in_prop = true;
                		break;
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentSubtraction:
                        ot = PascalABCCompiler.SyntaxTree.Operators.Minus;
                        oper_ass_in_prop = true;
                		break;
                }
            	if (oper_ass_in_prop)
            	{
            		if (pn.get_function == null)
                	AddError(new ThisPropertyCanNotBeReaded(pn,loc));
            		if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
            		{
                		prop_expr = create_not_static_method_call(pn.get_function,(to as non_static_property_reference).expression,loc,false);
                		prop_expr.parameters.AddRange((to as non_static_property_reference).fact_parametres);
            		}
            		else
            		{
            			prop_expr = create_static_method_call(pn.get_function,loc,pn.comprehensive_type,false);
            			prop_expr.parameters.AddRange((to as static_property_reference).fact_parametres);
            		}
                	from = find_operator(ot,prop_expr,from,loc);
            	}
            }
            else
            if (to is class_field_reference)
            {
                if ((to as class_field_reference).obj.type.type_special_kind == SemanticTree.type_special_kind.record &&
                    (to as class_field_reference).obj is base_function_call)
            	{
                    //исключим ситуацию обращения к массиву
                    if (!(((to as class_field_reference).obj is common_method_call) &&
                    ((to as class_field_reference).obj as common_method_call).obj.type.type_special_kind == SemanticTree.type_special_kind.array_wrapper))
                        AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
            	}
            	//else check_field_reference_for_assign(to as class_field_reference,loc);
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.namespace_variable_reference)
            {
            	if (context.is_loop_variable((to as namespace_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_variable_reference)
            {
            	if (context.is_loop_variable((to as local_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_block_variable_reference)
            {
            	if (context.is_loop_variable((to as local_block_variable_reference).var))
                    AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (to is simple_array_indexing)
            	if ((to as simple_array_indexing).simple_arr_expr is class_field_reference && ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj != null &&
            	   ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj is constant_node)
                    AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
            if ((to.semantic_node_type == semantic_node_type.static_event_reference)
                    || (to.semantic_node_type == semantic_node_type.nonstatic_event_reference))
            {
                statement_node event_assign = null;
                if (_assign.operator_type == PascalABCCompiler.SyntaxTree.Operators.Assignment)
                {
                    //throw new CanNotAssignToEvent();
                }
                static_event_reference ser = (static_event_reference)to;
                expression_node right_del = convertion_data_and_alghoritms.convert_type(from, ser.en.delegate_type);
                switch (_assign.operator_type)
                {
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentAddition:
                        {
                            if (to.semantic_node_type == semantic_node_type.static_event_reference)
                            {
                                event_assign = convertion_data_and_alghoritms.create_simple_function_call(
                                    ser.en.add_method, loc, right_del);
                            }
                            else
                            {
                                if (ser.en.semantic_node_type == semantic_node_type.compiled_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    compiled_function_node cfn = (compiled_function_node)ser.en.add_method;
                                    compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                                else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    common_method_node cfn = (common_method_node)ser.en.add_method;
                                    common_method_call tmp_event_assign = new common_method_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                            }
                            break;
                        }
                    case PascalABCCompiler.SyntaxTree.Operators.AssignmentSubtraction:
                        {
                            if (to.semantic_node_type == semantic_node_type.static_event_reference)
                            {
                                event_assign = convertion_data_and_alghoritms.create_simple_function_call(
                                    ser.en.remove_method, loc, right_del);
                            }
                            else
                            {
                                if (ser.en.semantic_node_type == semantic_node_type.compiled_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    compiled_function_node cfn = (compiled_function_node)ser.en.remove_method;
                                    compiled_function_call tmp_event_assign = new compiled_function_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                                else if (ser.en.semantic_node_type == semantic_node_type.common_event)
                                {
                                    nonstatic_event_reference nser = (nonstatic_event_reference)ser;
                                    common_method_node cfn = (common_method_node)ser.en.remove_method;
                                    common_method_call tmp_event_assign = new common_method_call(cfn, nser.obj, loc);
                                    tmp_event_assign.parameters.AddElement(right_del);
                                    event_assign = tmp_event_assign;
                                }
                            }
                            break;
                        }
                    default:
                        {
                            AddError(loc, "ASSIGN_TO_EVENT");
                            //throw new CanNotApplyThisOperationToEvent

                            break;
                        }
                }
                return_value(event_assign);
                return;
            }

            if (_assign.operator_type == PascalABCCompiler.SyntaxTree.Operators.Assignment || oper_ass_in_prop)
            {
                if (to.semantic_node_type == semantic_node_type.static_property_reference)
                {
                    static_property_reference spr = (static_property_reference)to;
                    if (spr.property.set_function == null)
                    {
                    	AddError(new ThisPropertyCanNotBeWrited(spr.property, loc));
                    }
                    check_property_params(spr, loc);
                    function_node set_func = spr.property.set_function;
                    from = convertion_data_and_alghoritms.convert_type(from, spr.property.property_type);
                    spr.fact_parametres.AddElement(from);
                    base_function_call bfc = create_static_method_call(set_func, loc, spr.property.comprehensive_type,
                        true);
                    bfc.parameters.AddRange(spr.fact_parametres);
                    return_value((statement_node)bfc);
                    return;
                }
                else if (to.semantic_node_type == semantic_node_type.non_static_property_reference)
                {
                    non_static_property_reference nspr = (non_static_property_reference)to;
                    check_property_params(nspr, loc);
                    from = convertion_data_and_alghoritms.convert_type(from, nspr.property.property_type);
                    nspr.fact_parametres.AddElement(from);

                    //Обработка s[i]:='c'
                    if (SystemUnitAssigned)
                        if (nspr.property.comprehensive_type == SystemLibrary.SystemLibrary.string_type)
                        {
                            if (nspr.property == SystemLibrary.SystemLibrary.string_type.default_property_node)
                            {
                                if (SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure != null)
                                {
                                    expressions_list exl = new expressions_list();
                                    exl.AddElement(nspr.expression);
                                    exl.AddElement(nspr.fact_parametres[0]);
                                    exl.AddElement(from);
                                    function_node fn = convertion_data_and_alghoritms.select_function(exl, SystemLibrary.SystemLibInitializer.StringDefaultPropertySetProcedure.SymbolInfo, loc);
                                    expression_node ret = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, exl.ToArray());
                                    return_value((statement_node)ret);
                                    return;
                                }
                            }
                        }

                    if (nspr.property.set_function == null)
                    {
                    	AddError(new ThisPropertyCanNotBeWrited(nspr.property, loc));
                    }
                    function_node set_func = nspr.property.set_function;
                    base_function_call bfc = create_not_static_method_call(set_func, nspr.expression, loc,
                        true);
                    bfc.parameters.AddRange(nspr.fact_parametres);
                    return_value((statement_node)bfc);
                    return;
                }
                else if (to is simple_array_indexing && (to as simple_array_indexing).simple_arr_expr.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                	expression_node expr = (to as simple_array_indexing).simple_arr_expr;
                	expression_node ind_expr = (to as simple_array_indexing).ind_expr;
                	from = convertion_data_and_alghoritms.convert_type(from,SystemLibrary.SystemLibrary.char_type);
                	ind_expr = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.SetCharInShortStringProcedure.sym_info as function_node,loc,expr,ind_expr,new int_const_node((expr.type as short_string_type_node).Length,null),from);
                	return_value(find_operator(compiler_string_consts.assign_name, expr, ind_expr, get_location(_assign)));
                	return;
                }
                else if (to.type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                    if (from.type is null_type_node)
                        AddError(get_location(_assign), "NIL_WITH_VALUE_TYPES_NOT_ALLOWED");
                    expression_node clip_expr = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node, loc, convertion_data_and_alghoritms.convert_type(from, SystemLibrary.SystemLibrary.string_type), new int_const_node((to.type as short_string_type_node).Length, null));
                    statement_node en = find_operator(compiler_string_consts.assign_name, to, clip_expr, get_location(_assign));
                    return_value(en);
                    return;
                }
                else
                {

                    assign_is_converting = true;
                    statement_node en = find_operator(compiler_string_consts.assign_name, to, from, get_location(_assign));
                    assign_is_converting = false;
                    return_value(en);
                    return;
                }
            }
            else
            {
                assign_is_converting = true;
                statement_node en = find_operator(_assign.operator_type, to, from, get_location(_assign));
                assign_is_converting = false;
                return_value(en);
                return;
            }
            //throw new CompilerInternalError("Undefined assign to type");
        }

        public override void visit(SyntaxTree.expression _expression)
        {
            throw new NotSupportedError(get_location(_expression));
        }

        public override void visit(SyntaxTree.statement_list _statement_list)
        {
            #region MikhailoMMX, обработка omp parallel section
            bool isGenerateParallel = false;
            bool isGenerateSequential = true;
            if (OpenMP.SectionsFound)                               //если в программе есть хоть одна директива parallel sections - проверяем:
                if (DirectivesToNodesLinks.ContainsKey(_statement_list) && OpenMP.IsParallelSectionsDirective(DirectivesToNodesLinks[_statement_list]))
                {
                    //перед этим узлом есть директива parallel sections
                    if (CurrentParallelPosition == ParallelPosition.Outside)            //входим в самый внешний параллельный sections
                    {
                        //сгенерировать сначала последовательную ветку, затем параллельную
                        //устанавливаем флаг и продолжаем конвертирование, считая, что конвертируем последовательную ветку
                        isGenerateParallel = true;
                        CurrentParallelPosition = ParallelPosition.InsideSequential;
                        //в конце за счет флага вернем состояние обратно и сгенерируем и параллельную ветку тоже
                    }
                    else //уже генерируем одну из веток
                        if (CurrentParallelPosition == ParallelPosition.InsideParallel)
                        {
                            isGenerateParallel = true;
                            isGenerateSequential = false;
                        }
                        //else
                        //флаг isGenerateParallel не установлен, параллельная ветка не сгенерируется
                }
            #endregion


            statements_list stl = new statements_list(get_location(_statement_list), get_location_with_check(_statement_list.left_logical_bracket), get_location_with_check(_statement_list.right_logical_bracket));
            convertion_data_and_alghoritms.statement_list_stack_push(stl);
            foreach (SyntaxTree.statement syntax_statement in _statement_list.subnodes)
            {
                try
                {
                    if (syntax_statement is SyntaxTree.var_statement)
                        visit(syntax_statement as SyntaxTree.var_statement);
                    else if (MustVisitBody)
                    {
                        //(ssyy) TODO Сделать по-другому!!!
                        statement_node semantic_statement = convert_strong(syntax_statement);
                        //(ssyy) Проверка для C
                        if (semantic_statement != null)
                        {
                            //Обработать по другому - комментарий не мой - SSM
                            //if (context.CurrentScope.AddStatementsToFront)
                            //    stl.statements.AddElementFirst(semantic_statement);
                            //else
                            stl.statements.AddElement(semantic_statement);
                        }

                        context.allow_inherited_ctor_call = false;
                    }
                }
                catch (Errors.Error ex)
                {
                    if (ThrowCompilationError)
                        throw ex;
                    else
                        ErrorsList.Add(ex);
                }
            }
            convertion_data_and_alghoritms.statement_list_stack.pop();

            #region MikhailoMMX, обработка omp parallel sections
            //флаг был установлен только если это самый внешний parallel sections и нужно сгенерировать обе ветки
            //или если это вложенный parallel sections, нужно сгенерировать обе ветки, но без проверки на OMP_Available
            //Последовательная ветка только что сгенерирована, теперь меняем состояние и генерируем параллельную
            if (isGenerateParallel)
            {
                CurrentParallelPosition = ParallelPosition.InsideParallel;
                statements_list omp_stl = OpenMP.TryConvertSections(stl, _statement_list, this);
                CurrentParallelPosition = ParallelPosition.Outside;
                if (omp_stl != null)
                {
                    return_value(omp_stl);
                    return;
                }
            }
            #endregion

            return_value(stl);
        }

        public override void visit(SyntaxTree.syntax_tree_node _tree_node)
        {
            throw new NotSupportedError(get_location(_tree_node));
        }
        
        private bool has_dll_import_attribute(common_function_node cfn)
        {
        	foreach (attribute_node attr in cfn.attributes)
        	{
        		if (attr.attribute_type == SystemLibrary.SystemLibrary.dllimport_type)
        			return true;
        	}
        	return false;
        }
        
        private bool is_pinvoke(SyntaxTree.proc_block bl)
        {
        	if (bl is SyntaxTree.external_directive)
        	{
        		return true;
        	}
        	return false;
        }
        
        private attribute_node get_dll_import_attribute(common_function_node cfn)
        {
        	foreach (attribute_node attr in cfn.attributes)
        	{
        		if (attr.attribute_type == SystemLibrary.SystemLibrary.dllimport_type)
        			return attr;
        	}
        	return null;
        }
        
        public override void visit(SyntaxTree.external_directive _external_directive)
        {
            string module_name = "";
            string name = "";
            if (_external_directive.modulename == null)
            {
            	if (!has_dll_import_attribute(context.top_function))
            	{
                    AddError(context.top_function.loc, "FUNCTION_MUST_HAVE_DLLIMPORT_ATTRIBUTE");
            	}
            	else
            	{
            		location loc2 = get_location(_external_directive);
            		pinvoke_statement pinv_stmt = new pinvoke_statement(loc2);
            		//statement_node_list sal = new statement_node_list(loc);
            		//sal.AddElement(ext_stmt);
            		statements_list sl2 = new statements_list(loc2);
            		sl2.statements.AddElement(pinv_stmt);
            		//return_value(sl);
            		context.code = sl2;
            		return;
            	}
            }
            if (has_dll_import_attribute(context.top_function))
            {
                AddError(get_location(_external_directive.modulename), "FUNCTION_MUST_HAVE_ONLY_EXTERNAL_STATEMENT");
            }
            if (_external_directive.modulename is SyntaxTree.string_const)
            {
                module_name = ((SyntaxTree.string_const)_external_directive.modulename).Value;
            }
            else if (_external_directive.modulename is SyntaxTree.ident)
            {
                //throw new CompilerInternalError("Unsupported now");
                //throw new NotSupportedError(get_location(_external_directive));
                expression_node en = convert_strong(_external_directive.modulename as SyntaxTree.ident);
                if (en is string_const_node)
                	module_name = (en as string_const_node).constant_value;
                else
                    AddError(get_location(_external_directive.modulename), "STRING_CONSTANT_EXPECTED");
            }
            else
                AddError(get_location(_external_directive.modulename), "STRING_CONSTANT_OR_IDENTIFIER_EXPECTED");
            if (_external_directive.name == null)
            {
            	name = context.converted_func_stack.top().name;
            }
            else
            if (_external_directive.name is SyntaxTree.string_const)
            {
                name = ((SyntaxTree.string_const)_external_directive.name).Value;
            }
            else if (_external_directive.name is SyntaxTree.ident)
            {
            	expression_node en = convert_strong(_external_directive.name as SyntaxTree.ident);
                if (en is string_const_node)
                	name = (en as string_const_node).constant_value;
                else
                    AddError(get_location(_external_directive.name), "STRING_CONSTANT_EXPECTED");
            }
            else
            {
                //throw new CompilerInternalError("Unsupported now");
                AddError(get_location(_external_directive.name), "STRING_CONSTANT_OR_IDENTIFIER_EXPECTED");
            }
            location loc = get_location(_external_directive);
            external_statement ext_stmt = new external_statement(module_name, name, loc);
            //statement_node_list sal = new statement_node_list(loc);
            //sal.AddElement(ext_stmt);
            statements_list sl = new statements_list(loc);
            sl.statements.AddElement(ext_stmt);
            //return_value(sl);
            context.code = sl;
        }

        public override void visit(SyntaxTree.unit_or_namespace _name_space)
        {
            throw new NotSupportedError(get_location(_name_space));
        }

        public override void visit(SyntaxTree.using_list _using_list)
        {
            throw new NotSupportedError(get_location(_using_list));
        }

        public override void visit(SyntaxTree.jump_stmt _return_stmt)
        {
            if (_return_stmt.JumpType != PascalABCCompiler.SyntaxTree.JumpStmtType.Return)
            {
                throw new NotSupportedError(get_location(_return_stmt));
            }
            if (_return_stmt.expr == null)
            {
                if (context.top_function.return_value_type != null && context.top_function.return_value_type != SystemLibrary.SystemLibrary.void_type)
                {
                    AddError(get_location(_return_stmt), "FUNCTION_SHOULD_RETURN_VALUE");
                    ret.return_value(new empty_statement(null));
                    return;
                }
                ret.return_value(new return_node(null, null));
            }
            else
            {
                type_node rettype = context.top_function.return_value_type;
                if (rettype == null || rettype == SystemLibrary.SystemLibrary.void_type)
                {
                    AddError(get_location(_return_stmt.expr), "FUNCTION_SHOULD_NOT_RETURN_VALUE");
                    ret.return_value(new empty_statement(null));
                    return;
                }
                expression_node en = convert_strong(_return_stmt.expr);
                en = convertion_data_and_alghoritms.convert_type(en, rettype);
                ret.return_value(new return_node(en, get_location(_return_stmt.expr)));
            }
        }

        public override void visit(SyntaxTree.loop_stmt _loop_stmt)
        {
            throw new NotSupportedError(get_location(_loop_stmt));
        }

        private bool IsGetEnumerator(type_node tn, ref type_node elem_type)
        {
            if (tn == null || tn is null_type_node || tn.ImplementingInterfaces == null) return false;
            compiled_type_node ctn = compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(compiler_string_consts.IEnumerableInterfaceName));
            if (tn == ctn)
                return true;
            foreach (SemanticTree.ITypeNode itn in tn.ImplementingInterfaces)
            {
                if (itn == ctn)
                {
                    elem_type = tn.element_type;
                    if (elem_type != null) return true;
                    function_node get_enumerator_func = null;
                    SymbolInfo si = ctn.find_in_type(compiler_string_consts.GetEnumeratorMethodName);
                    while (get_enumerator_func == null && si != null)
                    {
                        get_enumerator_func = si.sym_info as function_node;
                        if (get_enumerator_func.parameters.Count == 0)
                            break;
                        get_enumerator_func = null;
                        si = si.Next;
                    }
                    elem_type = (get_enumerator_func.return_value_type.find_in_type(compiler_string_consts.CurrentPropertyName).sym_info as property_node).get_function.return_value_type;
                    return true;
                }
            }
            return false;
        }

        public bool FindIEnumerableElementType(SyntaxTree.foreach_stmt _foreach_stmt, type_node tn, ref type_node elem_type)
        {
            var IEnstring = "System.Collections.IEnumerable";
            compiled_type_node ctn = compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(IEnstring));
            if (tn is compiled_type_node || tn is compiled_generic_instance_type_node) // Если этот тип зашит в .NET
            // IEnumerable<integer>, Range(1,10), Dictionary<string,integer>: tn = compiled_type_node
            // IEnumerable<T>: tn = compiled_generic_instance_type_node
            // FibGen = class(IEnumerable,IEnumerator): tn = common_type_node, en = compiled_type_node
            {
                System.Type ct;
                if (tn is compiled_type_node)
                    ct = (tn as compiled_type_node).compiled_type;
                else
                {
                    var orig = (tn as compiled_generic_instance_type_node).original_generic as compiled_type_node;
                    var pars = tn.instance_params;
                    ct = orig.compiled_type;
                }
                    
                Type r;
                var IEnTstring = "System.Collections.Generic.IEnumerable`1";
                if (ct.ToString().StartsWith(IEnTstring))
                    r = ct;
                else
                    r = ct.GetInterface(IEnTstring);
                if (r != null)
                {
                    Type arg1 = r.GetGenericArguments().First();
                    var str = arg1.GetGenericArguments().Count();
                    if (tn is compiled_type_node)
                    {
                        elem_type = compiled_type_node.get_type_node(arg1);
                    }
                    else
                    {
                        if (arg1.GetGenericArguments().Count()>0)
                        {
                            elem_type = compiled_type_node.get_type_node(arg1.GetGenericTypeDefinition());
                            elem_type = elem_type.get_instance(tn.instance_params); // SSM 19/07/15 - работает!!!
                        }
                        else
                        {
                            var ip = tn.instance_params;
                            //var Tname = ip[0].name;
                            //elem_type = convert_strong(new SyntaxTree.named_type_reference(Tname, _foreach_stmt.in_what.source_context));
                            elem_type = ip[0];
                        }
                            
                        //elem_type.instance_params = tn.instance_params;
                        //var ip = tn.instance_params;
                        //var Tname = new string(tn.name.SkipWhile(c => c != '<').Skip(1).TakeWhile(c => c != ',' && c != '>').ToArray());
                        //var Tname = ip[0].name;
                        //var Tname = "System.Collections.Generic.KeyValuePair'2"; // <integer,TClass>
                        //elem_type = convert_strong(new SyntaxTree.named_type_reference(Tname, _foreach_stmt.in_what.source_context));
                    }
                    return true;
                }
                else
                {
                    var ttt = tn.ImplementingInterfaces;
                    foreach (SemanticTree.ITypeNode itn in tn.ImplementingInterfaces)
                    {
                        if (itn == ctn)
                        {
                            elem_type = SystemLibrary.SystemLibrary.object_type;
                            return true;
                        }
                    }
                }
            }
            /*else if (tn is compiled_generic_instance_type_node)
            {
                var g = tn as compiled_generic_instance_type_node;
                var og = g.original_generic;
                var tt = og.ImplementingInterfaces;
                if (_foreach_stmt.type_name != null && _foreach_stmt.type_name.GetType()!=typeof(SyntaxTree.no_type_foreach))
                    elem_type = convert_strong(_foreach_stmt.type_name);
                else
                {
                    var fn = tn.full_name;
                }
                return true;
            }  */
            else // если мы самостоятельно определяем этот тип - можно реализовать в PascalABC.NET только IEnumerable. 
            // Попытка реализовать IEnumerable<T> натыкается на необходимость определять GetEnumerator, возвращающий IEnumerator и IEnumerator<T>
            {
                if (tn == null || tn is null_type_node || tn.ImplementingInterfaces == null)
                    return false;
                foreach (SemanticTree.ITypeNode itn in tn.ImplementingInterfaces)
                {
                    if (itn == ctn)
                    {
                        elem_type = SystemLibrary.SystemLibrary.object_type;
                        return true;
                    }
                }
            }
            return false;
        }

        public override void visit(SyntaxTree.foreach_stmt _foreach_stmt)
        {
            var lambdaSearcher = new LambdaSearcher(_foreach_stmt.in_what);
            if (lambdaSearcher.CheckIfContainsLambdas())
            {
                AddError(new LambdasNotAllowedInForeachInWhatSatetement(get_location(lambdaSearcher.FoundLambda)));
            }

            //throw new NotSupportedError(get_location(_foreach_stmt));
            definition_node dn = null;
            var_definition_node vdn = null;
            statements_list sl2 = new statements_list(get_location(_foreach_stmt));
            convertion_data_and_alghoritms.statement_list_stack_push(sl2);

            expression_node in_what = convert_strong(_foreach_stmt.in_what);
            expression_node tmp = in_what;
            if (in_what is typed_expression) in_what = convert_typed_expression_to_function_call(in_what as typed_expression);
            type_node elem_type = null;
            if (in_what.type == null)
                in_what = tmp;
            //if (in_what.type.find_in_type("GetEnumerator") == null)

            if (!FindIEnumerableElementType(_foreach_stmt, in_what.type, ref elem_type))
            //if (!IsGetEnumerator(in_what.type, ref elem_type))
                AddError(in_what.location, "CAN_NOT_EXECUTE_FOREACH_BY_EXPR_OF_TYPE_{0}", in_what.type);

            if (_foreach_stmt.type_name == null)
            {
                location loc1 = get_location(_foreach_stmt.identifier);
                dn = context.check_name_node_type(_foreach_stmt.identifier.name, loc1,
                    general_node_type.variable_node);
                vdn = (var_definition_node)dn;
                if (!check_name_in_current_scope(_foreach_stmt.identifier.name))
                    AddError(loc1, "FOREACH_LOOP_CONTROL_MUST_BE_SIMPLE_LOCAL_VARIABLE");
            }
            else
            {
                //AddError(new NotSupportedError(get_location(_foreach_stmt.type_name)));
                vdn = context.add_var_definition(_foreach_stmt.identifier.name, get_location(_foreach_stmt.identifier));

                type_node tn;
                if (_foreach_stmt.type_name is SyntaxTree.no_type_foreach)
                {
                    tn = elem_type;
                }
                else
                {
                    tn = convert_strong(_foreach_stmt.type_name);
                    //if (tn == SystemLibrary.SystemLibrary.void_type)
                    //	AddError(new VoidNotValid(get_location(_foreach_stmt.type_name)));
                    check_for_type_allowed(tn, get_location(_foreach_stmt.type_name));
                }

                context.close_var_definition_list(tn, null);
            }

            //elem_type = vdn.type;
            if (!(vdn.type is compiled_generic_instance_type_node))
                convertion_data_and_alghoritms.check_convert_type_with_inheritance(vdn.type, elem_type, get_location(_foreach_stmt.identifier));

            //if (!convertion_data_and_alghoritms.eq_type_nodes(elem_type, vdn.type))
                //AddError(new TypesOfVarAndElementsInForeachMustBeEqual(vdn.type.name,elem_type.name,get_location(_foreach_stmt.identifier)));
                //AddError(new SimpleSemanticError("Тип элемента контейнера: " + elem_type.ToString() + "  Тип переменной foreach: " + vdn.type.ToString(), get_location(_foreach_stmt.identifier)));

            //convertion_data_and_alghoritms.check_convert_type_with_inheritance(vdn.type, elem_type, get_location(_foreach_stmt.identifier));
            //if (in_what.type.type_special_kind == SemanticTree.type_special_kind.set_type)
            /*{
                if (!convertion_data_and_alghoritms.eq_type_nodes(elem_type, vdn.type))
                {
                    possible_type_convertions ptc = type_table.get_convertions(vdn.type,elem_type);
                    if (ptc.first == null || ptc.first.is_explicit)
                    	if (vdn.node_location_kind == SemanticTree.node_location_kind.in_namespace_location)
                            throw new CanNotConvertTypes(new namespace_variable_reference(vdn as namespace_variable, get_location(_foreach_stmt.identifier)), vdn.type, elem_type, get_location(_foreach_stmt.identifier));
                        	else if (vdn.node_location_kind == SemanticTree.node_location_kind.in_function_location) throw new CanNotConvertTypes(new local_variable_reference(vdn as local_variable, 0, get_location(_foreach_stmt.identifier)), vdn.type, elem_type, get_location(_foreach_stmt.identifier));
							else if (vdn.node_location_kind == SemanticTree.node_location_kind.in_block_location) throw new CanNotConvertTypes(new local_block_variable_reference(vdn as local_block_variable,get_location(_foreach_stmt.identifier)),vdn.type,elem_type, get_location(_foreach_stmt.identifier));
                        	else throw new ForeachLoopControlMustBeSimpleLocalVariable(get_location(_foreach_stmt.identifier));
                	
                }
            }*/
            statements_list sl = new statements_list(get_location(_foreach_stmt.stmt));
            convertion_data_and_alghoritms.statement_list_stack_push(sl);
            CheckToEmbeddedStatementCannotBeADeclaration(_foreach_stmt.stmt);
            foreach_node fn = new foreach_node(vdn, in_what, null, get_location(_foreach_stmt));
			context.cycle_stack.push(fn);
			context.loop_var_stack.Push(vdn);
            context.enter_code_block_with_bind();
            statement_node body = convert_strong(_foreach_stmt.stmt);
            context.leave_code_block();
            context.loop_var_stack.Pop();
            sl = convertion_data_and_alghoritms.statement_list_stack.pop();
            //if (!(st is statements_list))
            if (sl.statements.Count > 0 || sl.local_variables.Count > 0)
            {
                sl.statements.AddElement(body);
                body = sl;
            }
            //CheckToEmbeddedStatementCannotBeADeclaration(_foreach_stmt.stmt);
            //statement_node body = convert_strong(_foreach_stmt.stmt);
            //foreach_node fn = new foreach_node(vdn, in_what, body, get_location(_foreach_stmt));
            fn.what_do = body;
            //statements_list sl2 = new statements_list(get_location(_foreach_stmt));
            convertion_data_and_alghoritms.statement_list_stack.pop();
            sl2.statements.AddElement(fn);
            context.cycle_stack.pop();
            //if (_foreach_stmt.type_name != null)
            //sl2.local_variables.Add(vdn as local_block_variable);
            return_value(sl2);
        }

        public override void visit(SyntaxTree.addressed_value_funcname _addressed_value_funcname)
        {
            throw new NotSupportedError(get_location(_addressed_value_funcname));
        }

        public override void visit(SyntaxTree.named_type_reference_list _named_type_reference_list)
        {
            throw new NotSupportedError(get_location(_named_type_reference_list));
        }

        public override void visit(SyntaxTree.template_param_list _template_param_list)
        {
            throw new NotSupportedError(get_location(_template_param_list));
        }

        public override void visit(SyntaxTree.question_colon_expression node)
        {
            expression_node condition = convert_strong(node.condition);
            condition = convertion_data_and_alghoritms.convert_type(condition, SystemLibrary.SystemLibrary.bool_type);
            expression_node left = convert_strong(node.ret_if_true);
            expression_node right = convert_strong(node.ret_if_false);
            right = convertion_data_and_alghoritms.convert_type(right, left.type);
            return_value(new question_colon_expression(condition, left, right, get_location(node)));
        }

        public List<type_node> visit_type_list(List<SyntaxTree.type_definition> types)
        {
            List<type_node> tparams = new List<type_node>();
            foreach (SyntaxTree.named_type_reference id in types)
            {
                type_node tn = ret.visit(id);
                if (tn == null)
                {
                    AddError(get_location(id), "TYPE_NAME_EXPECTED");
                }
                tparams.Add(tn);
            }
            return tparams;
        }

        public List<type_node> visit_type_list_with_check(List<SyntaxTree.type_definition> types, List<generic_parameter_eliminations> gpe_list, bool where_checking)
        {
            List<type_node> tparams = new List<type_node>();
            foreach (SyntaxTree.named_type_reference id in types)
            {
                type_node tn = ret.visit(id);
                CompilationErrorWithLocation not_useful = generic_parameter_eliminations.check_type_generic_useful(tn, get_location(id));
                if (not_useful != null)
                {
                    AddError(not_useful);
                }
                tparams.Add(tn);
            }
            //Проверяем типы на соответствие ограничениям
            int i;
            if (where_checking)
            {
                CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(tparams, gpe_list, false, out i);
                if (err != null)
                {
                    err.loc = get_location(types[i]);
                    AddError(err);
                }
            }
            return tparams;
        }

        public type_node get_generic_instance(SymbolInfo si, List<SyntaxTree.type_definition> type_pars)
        {
            type_node node = si.sym_info as type_node;
            compiled_type_node comp_node = node as compiled_type_node;
            List<generic_parameter_eliminations> lgpe = null;
            if (!context.skip_check_where_sections)
            {
                lgpe = (comp_node == null) ?
                    (node as common_type_node).parameters_eliminations :
                    comp_node.parameters_eliminations;
            }
            List<type_node> tparams = visit_type_list_with_check(type_pars, lgpe, !context.skip_check_where_sections);
            return node.get_instance(tparams);
        }

        public SymbolInfo ConvertTypeToInstance(SymbolInfo si, List<SyntaxTree.type_definition> type_pars, location loc)
        {
            if (si == null)
                return null;
            template_class tclass = si.sym_info as template_class;
            if (tclass == null)
            {
                generic_indicator gi = si.sym_info as generic_indicator;
                if (gi != null)
                {
                    AddError(loc, "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", gi.generic.name, gi.generic.generic_params.Count);
                }
                return null;
            }
            //Формируем список параметров инстанцирования
            List<type_node> tparams = visit_type_list(type_pars);

            type_node t = instance_any(tclass, tparams, loc);
            return new SymbolInfo(t);
        }

        public override void visit(SyntaxTree.template_type_reference _template_type_reference)
        {
            int last_num = _template_type_reference.name.names.Count;
            string typename = _template_type_reference.name.names[last_num - 1].name;
            location loc = get_location(_template_type_reference);
            SymbolInfo si;
            List<type_node> tparams;

            //Обрабатываем generic-типы
            int tcount = _template_type_reference.params_list.params_list.Count;
            string temp_name = _template_type_reference.name.names[last_num - 1].name;
            if (!_template_type_reference.name.names[last_num - 1].name.Contains(compiler_string_consts.generic_params_infix)) // SSM 26.03.14
                _template_type_reference.name.names[last_num - 1].name += compiler_string_consts.generic_params_infix + tcount;
            si = context.find_definition_node(_template_type_reference.name, loc);
            if (si != null)
            {
                type_node rez_type = get_generic_instance(si, _template_type_reference.params_list.params_list);
                if (context.skip_check_where_sections)
                {
                    generic_instance_type_node gitn = rez_type as generic_instance_type_node;
                    if (gitn != null)
                    {
                        context.possible_incorrect_instances.Add(
                            new type_instance_and_location(gitn, loc));
                    }
                }
                return_value(rez_type);
                return;
            }
            _template_type_reference.name.names[last_num - 1].name = temp_name;
            si = context.find_definition_node(_template_type_reference.name, loc);
            if (si == null)
            {
                check_possible_generic_names(_template_type_reference.name, loc);
            	AddError(new UndefinedNameReference(typename, loc));
            }
            template_class tclass = si.sym_info as template_class;
            if (tclass == null)
            {
                generic_indicator gi = si.sym_info as generic_indicator;
                if (gi != null)
                {
                    AddError(get_location(_template_type_reference), "TYPE_{0}_HAS_{1}_GENERIC_PARAMETERS", typename, gi.generic.generic_params.Count);
                }
                AddError(get_location(_template_type_reference), "{0}_IS_NOT_TEMPLATE_CLASS", typename);
            }

            //Формируем список параметров инстанцирования
            tparams = visit_type_list(_template_type_reference.params_list.params_list);

            type_node t = instance_any(tclass, tparams, get_location(_template_type_reference));
            return_value(t);
        }

        public type_node instance_any(template_class tc, List<type_node> template_params, location loc)
        {
            common_type_node t = instance(tc, template_params, loc);
            if (tc.is_synonym)
            {
                return t.fields[0].type;
            }
            return t;
        }

        public common_type_node instance(template_class tc, List<type_node> template_params, location loc)
        {
            //Проверяем, что попытка инстанцирования корректна
            SyntaxTree.class_definition cl_def = tc.type_dec.type_def as SyntaxTree.class_definition;
            SyntaxTree.template_type_name ttn = tc.type_dec.type_name as SyntaxTree.template_type_name;
#if (DEBUG)
            if (!tc.is_synonym)
            {
                if (cl_def == null)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("No body definition in template class.");
                }
                if (cl_def.template_args == null || cl_def.template_args.idents == null)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("No template arguments in syntax tree.");
                }
            }
            else
            {
                if (ttn == null)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("No template name.");
                }
            }
#endif
            List<SyntaxTree.ident> template_formals = (tc.is_synonym) ?
                ttn.template_args.idents : cl_def.template_args.idents;
            if (template_formals.Count != ttn.template_args.idents.Count)
            {
                AddError(loc, "TEMPLATE_ARGUMENTS_COUNT_MISMATCH");
            }

            if (!tc.is_synonym)
            {
                foreach (type_node tnode in template_params)
                {
                    if (depended_from_generic_parameter(tnode))
                    {
                        AddError(loc, "TEMPLATES_AND_GENERICS_ARE_INCOMPATIBLE");
                    }
                }
            }

            //А вдруг такая инстанция уже есть - вернём её
            string inst_name = tc.CreateTemplateInstance(template_params, loc);
            SymbolInfo si = tc.cnn.scope.FindOnlyInScope(inst_name);
            if (si != null)
            {
                return (common_type_node)si.sym_info;
            }

            //Переходим в место описания шаблона
            common_namespace_node current_namespace = context.converted_namespace;
            context.converted_namespace = tc.cnn;
            common_type_node current_type = context.converted_type;
            context.converted_type = null;
            template_class current_template = context.converted_template_type;
            context.converted_template_type = null;
            common_function_node_stack current_funk_stack = context.converted_func_stack;
            context.converted_func_stack = new common_function_node_stack(); //Думаю, это будет работать

            //DarkStar
            statement_list_stack statement_list_stack = convertion_data_and_alghoritms.statement_list_stack;
            convertion_data_and_alghoritms.statement_list_stack = new statement_list_stack();

            bool current_cur_meth_not_in_class = current_converted_method_not_in_class_defined;
            current_converted_method_not_in_class_defined = false;
            SymbolInfo current_last_created_function = context.last_created_function;
            context.last_created_function = null;
            bool current_record_created = _record_created;
            _record_created = false;
            SemanticTree.field_access_level current_fal = context.get_field_access_level();
            document current_doc = current_document;
            current_document = tc.cur_document;
            bool current_body_exists = body_exists;
            body_exists = false;
            bool current_allow_inherited_ctor_call = context.allow_inherited_ctor_call;
            bool current_is_direct_type_decl = is_direct_type_decl;
            is_direct_type_decl = false;
            context.allow_inherited_ctor_call = false;
            bool current_type_section_converting = type_section_converting;
            type_section_converting = true;
            SemanticTree.field_access_level curr_fal = context.get_field_access_level();
            List<var_definition_node> current_var_defs = context.var_defs;
            context.var_defs = new List<var_definition_node>();
            Hashtable current_member_decls = context.member_decls;
            context.member_decls = new Hashtable();
            //подменяем using-список 
            using_namespace_list current_using_list = new using_namespace_list();
            foreach (using_namespace un in using_list)
            {
                current_using_list.AddElement(un);
            }
            using_list.clear();
            foreach (using_namespace un in tc.using_list)
            {
                using_list.AddElement(un);
            }
            context.enter_code_block_without_bind();

            common_type_node ctn;

            bool interface_creating = false;

            //Создаём инстанцию шаблона
            if (tc.is_synonym)
            {
                ctn = context.create_type(inst_name, get_location(ttn));
            }
            else
            {
                if (cl_def.keyword == SyntaxTree.class_keyword.Record)
                {
                    ctn = context.create_record_type(get_location(tc.type_dec.type_name), inst_name);
                }
                else
                {
                    interface_creating = (cl_def.keyword == PascalABCCompiler.SyntaxTree.class_keyword.Interface);
                    ctn = context.advanced_create_type(inst_name, get_location(tc.type_dec.type_name), interface_creating);
                }
            }
            ctn.original_template = tc;
            bool indefinite = false;
            foreach (type_node tn in template_params)
            {
                indefinite = indefinite || tn.depended_from_indefinite;
            }
            ctn.SetDependedFromIndefinite(indefinite);
            tc.instance_params.Add(ctn, template_params);

            //Ставим в соответствие формальным параметрам шаблона фактические
            for (int i = 0; i < template_params.Count; i++)
            {
                ctn.Scope.AddSymbol(template_formals[i].name, new SymbolInfo(template_params[i]));
            }

            //Разбор тела класса
            if (tc.is_synonym)
            {
                type_node synonym_value = convert_strong(tc.type_dec.type_def);
                ctn.fields.AddElement(new class_field(compiler_string_consts.synonym_value_name,
                    synonym_value, ctn, PascalABCCompiler.SemanticTree.polymorphic_state.ps_static,
                    PascalABCCompiler.SemanticTree.field_access_level.fal_public, null));
            }
            else
            {
                if (cl_def.keyword == SyntaxTree.class_keyword.Record)
                {
                    _record_created = true;
                }

                bool tmp_direct = is_direct_type_decl;
                is_direct_type_decl = true;
                hard_node_test_and_visit(cl_def);
                is_direct_type_decl = tmp_direct;

                //Разбор методов, описанных вне класса
                if (!interface_creating)
                {
                    type_section_converting = false;
                    foreach (procedure_definition_info pdi in tc.external_methods)
                    {
                        if (context.converted_namespace != pdi.nspace)
                        {
                            context.converted_namespace = pdi.nspace;
                            using_namespace_list ulist =
                                (pdi.nspace == tc.cnn) ? tc.using_list : tc.using_list2;
                            using_list.clear();
                            foreach (using_namespace un in ulist)
                            {
                                using_list.AddElement(un);
                            }
                            template_class.AddUsingListToScope(context.converted_namespace.scope, ulist);
                        }

                        //Заменяем в дереве метода имя шаблонного класса на имя инстанции
                        string tmp_name = pdi.proc.proc_header.name.class_name.name;
                        pdi.proc.proc_header.name.class_name.name = inst_name;

                        hard_node_test_and_visit(pdi.proc);

                        //Обратная замена
                        pdi.proc.proc_header.name.class_name.name = tmp_name;
                    }

                    foreach (common_method_node cmn in ctn.methods)
                    {
                        if (cmn.function_code == null)
                        {
                            AddError(cmn.loc, "FUNCTION_PREDEFINITION_WITHOUT_DEFINITION");
                        }
                    }
                }
            }

            //Прыжок обратно
            context.leave_code_block();
            context.converted_namespace = current_namespace;
            context.converted_type = current_type;
            context.converted_template_type = current_template;
            context.set_field_access_level(current_fal);
            context.converted_func_stack = current_funk_stack;
            convertion_data_and_alghoritms.statement_list_stack = statement_list_stack;
            current_document = current_doc;
            body_exists = current_body_exists;
            _record_created = current_record_created;
            context.last_created_function = current_last_created_function;
            is_direct_type_decl = current_is_direct_type_decl;
            context.allow_inherited_ctor_call = current_allow_inherited_ctor_call;
            current_converted_method_not_in_class_defined = current_cur_meth_not_in_class;
            context.set_field_access_level(curr_fal);
            type_section_converting = current_type_section_converting;
            context.member_decls = current_member_decls;
            context.var_defs = current_var_defs;
            using_list.clear();
            foreach (using_namespace un in current_using_list)
            {
                using_list.AddElement(un);
            }

            return ctn;
        }

        private SyntaxTree.addressed_value convert_named_type_reference_to_addressed_value(SyntaxTree.named_type_reference ntr)
        {
            SyntaxTree.addressed_value adrv = ntr.names[0];
            for (int i = 1; i < ntr.names.Count; i++)
                adrv = new SyntaxTree.dot_node(adrv, ntr.names[i]);
            return adrv;
        }

        private base_function_call create_constructor_call(type_node tn, expressions_list exprs, location loc, Tuple<bool, List<SyntaxTree.expression>> lambdas_info = null)
        {
            if (tn.IsInterface)
            {
            	AddError(loc, "INTERFACE_{0}_CONSTRUCTOR_CALL", tn.name);
            }
            if (tn.IsAbstract)
            {
            	AddError(loc, "ABSTRACT_CONSTRUCTOR_{0}_CALL", tn.name);
            }
            SymbolInfo si = tn.find_in_type(TreeConverter.compiler_string_consts.default_constructor_name, context.CurrentScope); //tn.Scope); 
            delete_inherited_constructors(ref si, tn);
            if (si == null)
                AddError(loc, "CONSTRUCTOR_NOT_FOUND");
            function_node fnn = null;
            try
            {
                #region Если встретились лямбды в фактических параметрах, то выбираем нужную функцию из перегруженных, выводим типы, отмечаем флаг в лямбдах, говорящий о том, что мы их реально обходим 
                //lroman//
                if (lambdas_info != null && lambdas_info.Item1)
                {
                    LambdaHelper.processingLambdaParametersForTypeInference++;
                    var syntax_nodes_parameters = lambdas_info.Item2;

                    // SSM 21.05.14 - попытка обработать перегруженные функции с параметрами-лямбдами с различными возвращаемыми значениями
                    function_node_list spf = null;
                    try
                    {
                        function_node fn = convertion_data_and_alghoritms.select_function(exprs, si, loc,
                                                                                          syntax_nodes_parameters);
                        int exprCounter = 0;
                        foreach (SyntaxTree.expression en in syntax_nodes_parameters)
                        {
                            if (!(en is SyntaxTree.function_lambda_definition))
                            {
                                exprCounter++;
                                continue;
                            }
                            else
                            {
                                var enLambda = (SyntaxTree.function_lambda_definition)en;
                                LambdaHelper.InferTypesFromVarStmt(fn.parameters[exprCounter].type, enLambda, this);
                                enLambda.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                                exprs[exprCounter] = convert_strong(en);
                                enLambda.lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                                exprCounter++;
                            }
                        }
                    }
                    catch (SeveralFunctionsCanBeCalled sf)
                    {
                        spf = sf.set_of_possible_functions;
                            // Возможны несколько перегруженных версий - надо выводить дальше в надежде что какие-то уйдут и останется одна
                    }

                    Exception lastmultex = null;
                    if (spf != null) // пытаемся инстанцировать одну за другой и ошибки гасим try
                    {
                        // exprs - глобальная, поэтому надо копировать
                        int spfnum = -1; // spfnum - первый номер правильно инстанцированной. Пока -1. Если потом встретился второй, то тоже ошибка
                        int GoodVersionsCount = 0;
                        int GoodVersionsCountWithSameResType = 0;
                        for (int i = 0; i < spf.Count; i++)
                        {
                            function_node fn = spf[i];
                            try
                            {
                                int exprCounter = 0;
                                expressions_list exprs1 = new expressions_list();
                                exprs1.AddRange(exprs); // сделали копию

                                foreach (SyntaxTree.expression en in syntax_nodes_parameters)
                                {
                                    if (!(en is SyntaxTree.function_lambda_definition))
                                    {
                                        exprCounter++;
                                        continue;
                                    }
                                    else
                                    {
                                        var fld = en as SyntaxTree.function_lambda_definition;

                                        var lambdaName = fld.lambda_name; //lroman Сохранять имя необходимо
                                        var fl = fld.lambda_visit_mode;

                                        // запомнили типы параметров лямбды - SSM
                                        object[] realparamstype = new object[fld.formal_parameters.params_list.Count];
                                            // здесь хранятся выведенные типы лямбд или null если типы явно заданы
                                        for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                        {
                                            var laminftypeK =
                                                fld.formal_parameters.params_list[k].vars_type as
                                                SyntaxTree.lambda_inferred_type;
                                            if (laminftypeK == null)
                                                realparamstype[k] = null;
                                            else realparamstype[k] = laminftypeK.real_type;
                                        }

                                        // запоминаем реальный тип возвращаемого значения если он не указан явно (это должен быть any_type или null если он указан явно) - он может измениться при следующем вызове, поэтому мы его восстановим
                                        var restype = fld.return_type as SyntaxTree.lambda_inferred_type;
                                        object realrestype = null;
                                        if (restype != null)
                                            realrestype = restype.real_type;

                                        LambdaHelper.InferTypesFromVarStmt(fn.parameters[exprCounter].type, fld, this);
                                        fld.lambda_visit_mode = LambdaVisitMode.VisitForAdvancedMethodCallProcessing;
                                            //lroman
                                        fld.lambda_name = LambdaHelper.GetAuxiliaryLambdaName(lambdaName);
                                            // поправляю имя. Думаю, назад возвращать не надо. ПРОВЕРИТЬ!

                                        //contextChanger.SaveContextAndUpToNearestDefSect();
                                        try
                                        {
                                            exprs1[exprCounter] = convert_strong(en);

                                            type_node resexprtype = fld.RealSemTypeOfResExpr as type_node;
                                            type_node resformaltype = fld.RealSemTypeOfResult as type_node;
                                            var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                            if (bbb)
                                            {
                                                GoodVersionsCountWithSameResType += 1;
                                                spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                            }

                                            /*var tt = fn.parameters[exprCounter].type as compiled_type_node;
                                            if (tt != null && tt.compiled_type.FullName.ToLower().StartsWith("system.func"))
                                            {
                                                resformaltype = tt.instance_params[tt.instance_params.Count - 1]; // Последний параметр в записи Func<T,T1,...TN> - тип возвращаемого значения
                                                var bbb = resexprtype == resformaltype; // только в одном случае должно быть true - эту версию и надо выбирать. Если в нескольких, то неоднозначность
                                                if (bbb)
                                                {
                                                    GoodVersionsCountWithSameResType += 1;
                                                    spfnum = i; // здесь запоминаем индекс потому что он точно подойдет. Тогда ниже он запоминаться не будет. 
                                                }
                                            }*/
                                        }
                                        catch
                                        {
                                            throw;
                                        }
                                        finally
                                        {
                                            LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, en as function_lambda_definition);
                                            // восстанавливаем сохраненный тип возвращаемого значения
                                            if (restype != null)
                                                restype.real_type = realrestype;
                                            // восстанавливаем сохраненные типы параметров лямбды, которые не были заданы явно
                                            for (var k = 0; k < fld.formal_parameters.params_list.Count; k++)
                                            {
                                                var laminftypeK =
                                                    fld.formal_parameters.params_list[k].vars_type as
                                                    SyntaxTree.lambda_inferred_type;
                                                if (laminftypeK != null)
                                                    laminftypeK.real_type = realparamstype[k];
                                            }

                                            fld.lambda_name = lambdaName; //lroman Восстанавливаем имена
                                            fld.lambda_visit_mode = fl;
                                        }

                                        //contextChanger.RestoreCurrentContext();
                                        exprCounter++;
                                    }
                                }
                                /*if (spfnum >= 0) // два удачных инстанцирования - плохо. Может, одно - с более близким типом возвращаемого значения, тогда это плохо - надо доделать, но пока так
                                {
                                    spfnum = -2;
                                    break;
                                }*/

                                if (GoodVersionsCountWithSameResType == 0)
                                    spfnum = i; // здесь запоминаем индекс только если нет подошедших, совпадающих по типу возвращаемого значения
                                GoodVersionsCount += 1;
                                for (int j = 0; j < exprs.Count; j++) // копируем назад если всё хорошо
                                    exprs[j] = exprs1[j];
                            }
                            catch (Exception e)
                            {
                                // если сюда попали, значит, не вывели типы в лямбде и надо эту инстанцию пропускать
                                //contextChanger.RestoreCurrentContext();
                                lastmultex = e;
                            }
                        }
                        if (GoodVersionsCount > 1 && GoodVersionsCountWithSameResType != 1) // подошло много, но не было ровно одной с совпадающим типом возвращаемого значения
                            throw new SeveralFunctionsCanBeCalled(loc, spf);
                        if (GoodVersionsCount == 0) // было много, но ни одна не подошла из-за лямбд
                        {
                            throw lastmultex;
                            //throw new NoFunctionWithSameArguments(subloc2, false);
                        }

                        var kk = 0;
                        foreach (SyntaxTree.expression en in syntax_nodes_parameters)
                            //lroman окончательно подставить типы в лямбды
                        {
                            if (!(en is SyntaxTree.function_lambda_definition))
                            {
                                kk++;
                                continue;
                            }
                            else
                            {
                                LambdaHelper.InferTypesFromVarStmt(spf[spfnum].parameters[kk].type,
                                                                   en as SyntaxTree.function_lambda_definition, this);
                                exprs[kk] = convert_strong(en);
                                kk++;
                            }
                        }
                    }
                    // SSM 21.05.14 end
                    LambdaHelper.processingLambdaParametersForTypeInference--;

                    fnn = convertion_data_and_alghoritms.select_function(exprs, si, loc);
                }
                    //lroman//
                    #endregion

                else
                {
                    fnn = convertion_data_and_alghoritms.select_function(exprs, si, loc);
                }
            }
            catch (NoFunctionWithSameParametresNum e)
            {
                e.is_constructor = true;
                throw;
            }
            return create_static_method_call_with_params(fnn, loc, tn, false, exprs);
        }
		
        private expression_node clip_expression_if_need(expression_node expr, type_node tn)
        {
        	if (!(tn is short_string_type_node) && tn.type_special_kind != SemanticTree.type_special_kind.set_type)
        		return expr;
        	if (tn is short_string_type_node)
        	{
        		return convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringProcedure.sym_info as function_node,null,convertion_data_and_alghoritms.convert_type(expr,SystemLibrary.SystemLibrary.string_type),new int_const_node((tn as short_string_type_node).Length,null));
        	}
            else if (tn.type_special_kind == SemanticTree.type_special_kind.set_type && tn.element_type != null)
            {
                ordinal_type_interface oti = tn.element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
                if (oti != null)
                {
                    base_function_call cmc = null;
                    if (SystemLibrary.SystemLibInitializer.ClipFunction.sym_info is common_namespace_function_node)
                        cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as common_namespace_function_node, null);
                    else
                        cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipFunction.sym_info as compiled_function_node, null);
                    cmc.parameters.AddElement(expr);
                    cmc.parameters.AddElement(oti.lower_value.get_constant_copy(null));
                    cmc.parameters.AddElement(oti.upper_value.get_constant_copy(null));
                    cmc.ret_type = tn;
                    return cmc;
                }
                else if (tn.element_type.type_special_kind == SemanticTree.type_special_kind.short_string)
                {
                    base_function_call cmc = null;
                    if (SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info is common_namespace_function_node)
                        cmc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as common_namespace_function_node, null);
                    else
                        cmc = new compiled_static_method_call(SystemLibrary.SystemLibInitializer.ClipShortStringInSetFunction.sym_info as compiled_function_node, null);
                    cmc.parameters.AddElement(expr);
                    cmc.parameters.AddElement(new int_const_node((tn.element_type as short_string_type_node).Length, null));
                    cmc.ret_type = tn;
                    return cmc;
                }
            }
        	return expr;
        }
        
        public override void visit(SyntaxTree.new_expr _new_expr)
        {
            type_node tn = ret.visit(_new_expr.type);
            //if (tn == SystemLibrary.SystemLibrary.void_type)
            //	AddError(new VoidNotValid(get_location(_new_expr.type)));
            check_for_type_allowed(tn,get_location(_new_expr.type));
            location loc = get_location(_new_expr);
            expressions_list exprs = null;
            var lambdas_are_in_parameters = false;
            var syntax_nodes_parameters = _new_expr.params_list == null
                                              ? new List<expression>()
                                              : _new_expr.params_list.expressions;

            if (_new_expr.params_list != null)
            {
                exprs = new expressions_list();
                foreach (SyntaxTree.expression en in _new_expr.params_list.expressions)
                {
                    if (en is SyntaxTree.function_lambda_definition)
                    {
                        lambdas_are_in_parameters = true;
                        ((SyntaxTree.function_lambda_definition)en).lambda_visit_mode = LambdaVisitMode.VisitForInitialMethodCallProcessing;
                    }
                    exprs.AddElement(convert_strong(en));
                }
            }
            else
                exprs = new expressions_list();
            if (_new_expr.new_array)
            {
                //if (exprs.Count == 1)
                {
                    //new typename[size]
                    type_node atn = convertion_data_and_alghoritms.type_constructor.create_unsized_array(tn, context.converted_namespace, exprs.Count, loc);
                    //тип элементов
                    typeof_operator to = new typeof_operator(tn, loc);
                    List<expression_node> lst = new List<expression_node>();
                    //размер
                    for (int i=0; i<exprs.Count; i++)
                    {
                    	expression_node expr = exprs[i];
                    	expr = convertion_data_and_alghoritms.convert_type(expr, SystemLibrary.SystemLibrary.integer_type);
                    	if (expr is int_const_node && (expr as int_const_node).constant_value < 0)
                    		AddError(expr.location, "NEGATIVE_ARRAY_LENGTH_({0})_NOT_ALLOWED", (expr as int_const_node).constant_value);
                    	lst.Add(expr);
                    }
                    //это вызов спецфункции
                    expression_node retv = convertion_data_and_alghoritms.create_simple_function_call(SystemLibrary.SystemLibInitializer.NewArrayProcedureDecl, loc, to, new int_const_node(exprs.Count,loc));
                    base_function_call cnfc = retv as base_function_call;
                    foreach (expression_node e in lst)
                    	cnfc.parameters.AddElement(e);
                    if (_new_expr.array_init_expr != null)
                    {
                    	if (exprs.Count > 1)
                    	{
                    		foreach (expression_node expr in lst)
                    		if (!(expr is int_const_node))
                                AddError(expr.location, "CONSTANT_EXPRESSION_EXPECTED");
                    		if (_new_expr.array_init_expr.elements == null)
                                AddError(get_location(_new_expr.array_init_expr), "ARRAY_CONST_EXPECTED");
                    		_new_expr.array_init_expr = get_possible_array_const(_new_expr.array_init_expr,atn) as SyntaxTree.array_const;
                    		expression_node e = convert_strong(_new_expr.array_init_expr);
                    		if (e is array_initializer)
                    		{
                    			array_initializer tmp = e as array_initializer;
                    			for (int i=0; i<exprs.Count; i++)
                    			{
                    				int len = (exprs[i] as int_const_node).constant_value;
                    				if (!(e is array_initializer) || (e as array_initializer).element_values.Count != len)
                                        AddError(e.location, "ARRAY_CONST_{0}_ELEMENTS_EXPECTED", len);
                    				e = (e as array_initializer).element_values[0];
                    			}
                    			cnfc.parameters.AddElement(ConvertArrayInitializer(atn,tmp));
                    		}
                    	}
                    	else
                    	{
                    		expression_node expr = lst[0];
                    		if (!(expr is int_const_node))
                                AddError(expr.location, "CONSTANT_EXPRESSION_EXPECTED");
                    		int len = (expr as int_const_node).constant_value;
                            if (_new_expr.array_init_expr.elements == null)
                            {
                                if (len != 0)
                                    AddError(get_location(_new_expr.array_init_expr), "ARRAY_CONST_{0}_ELEMENTS_EXPECTED", len);
                            }
                    		else if (_new_expr.array_init_expr.elements.expressions.Count != len)
                                AddError(get_location(_new_expr.array_init_expr), "ARRAY_CONST_{0}_ELEMENTS_EXPECTED", len);
                    		_new_expr.array_init_expr = get_possible_array_const(_new_expr.array_init_expr,atn) as SyntaxTree.array_const;
                    		if (_new_expr.array_init_expr.elements != null)
                            foreach (SyntaxTree.expression e in _new_expr.array_init_expr.elements.expressions)
                    			cnfc.parameters.AddElement(convertion_data_and_alghoritms.convert_type(clip_expression_if_need(convert_strong(e),tn),tn));
                    	}
                    }
                    //явное преобразования для того чтобы тип задать
                    function_node fn = convertion_data_and_alghoritms.get_empty_conversion(retv.type, atn, false);
                    retv = convertion_data_and_alghoritms.create_simple_function_call(fn, loc, retv);
                    return_value(retv);
                    return;
                }
                
            }
            return_value(create_constructor_call(tn, exprs, get_location(_new_expr.type), new Tuple<bool, List<expression>>(lambdas_are_in_parameters, syntax_nodes_parameters)));
        }

        public override void visit(SyntaxTree.where_type_specificator_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.where_definition node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.where_definition_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
		
        private bool can_evaluate_size(type_node tn)
        {
            if (tn is compiled_type_node)
            {
                if (tn.type_special_kind == SemanticTree.type_special_kind.array_wrapper || tn.type_special_kind == SemanticTree.type_special_kind.set_type
                    || tn.type_special_kind == SemanticTree.type_special_kind.short_string || tn.type_special_kind == SemanticTree.type_special_kind.typed_file || tn.type_special_kind == SemanticTree.type_special_kind.text_file
                   || tn.type_special_kind == SemanticTree.type_special_kind.binary_file || tn.type_special_kind == SemanticTree.type_special_kind.array_kind)
                    return true;
                if (tn.type_special_kind == SemanticTree.type_special_kind.diap_type || tn.type_special_kind == SemanticTree.type_special_kind.enum_kind)
                    return true;
                if (tn.IsPointer)
                    return true;
                /*if (!tn.is_value_type)
                    return false;
                if (!tn.IsPointer)
                    return (tn as compiled_type_node).compiled_type.IsLayoutSequential;
                else return true;*/
                if (tn.is_generic_parameter || tn.is_generic_type_definition || tn.is_generic_type_instance)
                    return false;
            }
        	if (tn is common_type_node)
        	{
        		common_type_node ctn = tn as common_type_node;
        		
        		if (ctn.type_special_kind == SemanticTree.type_special_kind.diap_type || ctn.type_special_kind == SemanticTree.type_special_kind.enum_kind)
        			return true;
                if (tn.type_special_kind == SemanticTree.type_special_kind.array_wrapper || tn.type_special_kind == SemanticTree.type_special_kind.set_type || tn.type_special_kind == SemanticTree.type_special_kind.short_string 
                    || tn.type_special_kind == SemanticTree.type_special_kind.typed_file || tn.type_special_kind == SemanticTree.type_special_kind.text_file
                    || tn.type_special_kind == SemanticTree.type_special_kind.binary_file || tn.type_special_kind == SemanticTree.type_special_kind.array_kind)
                    return true;
                if (tn.IsPointer)
                    return true;
                
                if (tn.is_generic_parameter || tn.is_generic_type_definition || tn.is_generic_type_instance)
                    return false;
        		foreach (class_field cf in ctn.fields)
        			if (!cf.IsStatic)
        			if (!can_evaluate_size(cf.type)) return false;

        	}
        	return true;
        }
        
        private expression_node make_sizeof_operator(type_node tn, location loc)
        {
        	return new sizeof_operator(tn,loc);
        }
        
        public override void visit(SyntaxTree.sizeof_operator node)
        {
            //if (SystemLibrary.SystemLibInitializer.GetRuntimeSizeFunction != null)
            //{
            //    SystemLibrary.SystemLibInitializer.GetRuntimeSizeFunction.Restore();
            //}
            if (node.type_def != null && node.type_def is SyntaxTree.named_type_reference)
            {
                SyntaxTree.named_type_reference ntr = node.type_def as SyntaxTree.named_type_reference;
                type_node tn = find_type(ntr, get_location(ntr));
                switch (tn.type_special_kind)
                {
                	//case SemanticTree.type_special_kind.short_string:
                	//	return_value(new int_const_node((tn as short_string_type_node).Length+1,get_location(ntr))); break;
                	case SemanticTree.type_special_kind.enum_kind:
                		return_value(new int_const_node(sizeof(int),get_location(ntr))); break;
                default:
                	//if (!tn.is_value_type && tn != SystemLibrary.SystemLibrary.pointer_type && !tn.is_generic_parameter && tn.type_special_kind != SemanticTree.type_special_kind.diap_type)
                	//	AddError(new OperatorSizeOfMustBeUsedWithAValueType(tn.name, get_location(ntr)));
                	if (tn.is_generic_parameter || tn.is_generic_type_definition || tn.is_generic_type_instance)
                	{
                        AddError(get_location(ntr), "OPERATOR_SIZEOF_CAN_NOT_BE_USED_WITH_GENERIC_TYPE");
                	}
                	if (!can_evaluate_size(tn))
                		AddError(get_location(ntr), "CANNOT_EVALUATE_SIZE", tn.name);
                	return_value(make_sizeof_operator(tn, get_location(node)));
                	break;
                }
            }
            else
                throw new NotSupportedError(get_location(node));

        }

        public override void visit(SyntaxTree.typeof_operator node)
        {
            return_value(new typeof_operator(find_type(node.type_name, get_location(node.type_name)), get_location(node)));
        }

        public override void visit(SyntaxTree.compiler_directive node)
        {
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_faststrings)
            {
                SemanticRules.FastStrings = true;
                return;
            }
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_nullbasedstrings)
            {
                SemanticRules.NullBasedStrings = node.Directive.text.ToLower() == compiler_string_consts.true_const_name;
                return;
            }
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_nullbasedstrings_ON)
            {
                SemanticRules.NullBasedStrings = true;
                return;
            }
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_nullbasedstrings_OFF)
            {
                SemanticRules.NullBasedStrings = false;
                return;
            }
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_initstring_as_empty_ON)
            {
                SemanticRules.InitStringAsEmptyString = true;
                return;
            }
            if (node.Name.text.ToLower() == compiler_string_consts.compiler_directive_initstring_as_empty_OFF)
            {
                SemanticRules.InitStringAsEmptyString = false;
                return;
            }
            if (node.Name.text == "platform" && node.Directive.text.ToLower() == "native")
            {
                SemanticRules.GenerateNativeCode = true;
                
                
                return;
            }
        }
        
        public override void visit(SyntaxTree.operator_name_ident node)
        {
            throw new NotSupportedError(get_location(node));
        }
        
        public override void visit(SyntaxTree.var_statement node)
        {
            //throw new NotSupportedError(get_location(node));
            //надо сделать чтобы у begin ... end была своя scope
            visit(node.var_def);
            //foreach (SyntaxTree.ident id in node.var_def.vars.idents)
            //    blocks.add_special_local_var(new local_variable(id.name,convert_strong(node.var_def.vars_type),null,get_location(node.var_def)));

        }

        public override void visit(SyntaxTree.expression_as_statement node)
        {
            return_value((statement_node)convert_strong(node.expr));
        }

        //ssyy
        public void generate_inherit_constructors()
        {
            common_type_node _ctn = context.converted_type;
            if (_ctn == null)
            {
                throw new CompilerInternalError("Can generate inherited constructors only in class.");
            }
            if (_ctn.has_user_defined_constructor)
            {
                //Пользователь определил хотя бы один конструктор, никакие конструкторы не наследуем.
                return;
            }
            //Получили список процедур предка, имеющих имя Create
            SymbolInfo si = _ctn.base_type.find_in_type(compiler_string_consts.default_constructor_name, _ctn.base_type.Scope);
            delete_inherited_constructors(ref si, _ctn.base_type);
            while (si != null)
            {
                function_node fn = si.sym_info as function_node;
                compiled_constructor_node pconstr = fn as compiled_constructor_node;
                common_method_node mconstr = fn as common_method_node;
                //Если это конструктор...
                if (pconstr != null ||
                    mconstr != null && mconstr.is_constructor)
                {
                    //Генерируем унаследованный конструктор
                    location loc = null;
                    SemanticTree.polymorphic_state ps;
                    if (mconstr != null)
                    {
                        loc = mconstr.loc;
                        ps = mconstr.polymorphic_state;
                    }
                    else //значит (pconstr != null)
                    {
                        ps = pconstr.polymorphic_state;
                    }
                    if (pconstr != null)
                        context.set_field_access_level(pconstr.field_access_level);
                    else
                        context.set_field_access_level(mconstr.field_access_level);
                    common_method_node gen_constr = context.create_function(compiler_string_consts.default_constructor_name, loc) as common_method_node;
                    gen_constr.polymorphic_state = ps;
                    gen_constr.is_overload = true;
                    gen_constr.is_constructor = true;
                    gen_constr.field_access_level = fn.field_access_level;
                    gen_constr.return_value_type = _ctn;

                    foreach (parameter par in fn.parameters)
                    {
                        //(ssyy) Интересно, зачем это.
                        concrete_parameter_type cpt =
                            (par.parameter_type == SemanticTree.parameter_type.var) ?
                            concrete_parameter_type.cpt_var :
                            concrete_parameter_type.cpt_none;
                        common_parameter c_p = new common_parameter(par.name,
                            par.parameter_type, gen_constr, cpt, null);
                        c_p.type = par.type;
                        c_p.set_param_is_params(par.is_params);
                        c_p.inital_value = par.inital_value;
                        gen_constr.parameters.AddElement(c_p);
                        c_p.default_value = par.default_value;
                    }

                    base_function_call bfc;

                    if (mconstr != null)
                    {
                        common_constructor_call c1 = new common_constructor_call(mconstr, null);
                        c1._new_obj_awaited = false;
                        bfc = c1;
                    }
                    else
                    {
                        compiled_constructor_call c2 = new compiled_constructor_call(pconstr, null);
                        c2._new_obj_awaited = false;
                        bfc = c2;
                    }
                    foreach (parameter p in gen_constr.parameters)
                    {
                        bfc.parameters.AddElement(
                            create_variable_reference(p, null));
                    }
                    statements_list snlist = new statements_list(null);
                    snlist.statements.AddElement(bfc);
                    snlist.statements.AddElement(new empty_statement(null));
                    gen_constr.function_code = snlist;
                    context.leave_block();
                    if (fn.parameters.Count == 0 || fn.parameters[0].default_value != null)
                    {
                        _ctn.has_default_constructor = true;
                    }
                }
                si = si.Next;
            }
        }
        //\ssyy

        //ssyy
        public void generate_inherited_from_base_and_interface_function(common_type_node ctype, function_node func)
        {
            common_method_node gen_func = context.create_function(func.name, null) as common_method_node;
            gen_func.polymorphic_state = SemanticTree.polymorphic_state.ps_common;
            gen_func.newslot_awaited = true;
            gen_func.is_final = true;
            gen_func.is_overload = true;
            gen_func.field_access_level = SemanticTree.field_access_level.fal_public;
            gen_func.return_value_type = func.return_value_type;
            //gen_func.return_variable = func.retu

            foreach (parameter par in func.parameters)
            {
                concrete_parameter_type cpt =
                    (par.parameter_type == SemanticTree.parameter_type.value) ?
                    concrete_parameter_type.cpt_const :
                    concrete_parameter_type.cpt_var;
                common_parameter c_p = new common_parameter(par.name,
                    par.parameter_type, gen_func, cpt, null);
                c_p.type = par.type;
                c_p.set_param_is_params(par.is_params);
                c_p.inital_value = par.inital_value;
                gen_func.parameters.AddElement(c_p);
            }

            local_variable lv = new local_variable(compiler_string_consts.self_word, gen_func.cont_type, gen_func, null);
            gen_func.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(lv));
            gen_func.self_variable = lv;

            base_function_call bfc;
            this_node tn = null;

            common_method_node commn = func as common_method_node;
            if (commn != null)
            {
                tn = new this_node(commn.comperehensive_type as type_node, null);
                bfc = new common_method_call(commn, tn, null);
            }
            else
            {
                compiled_function_node compn = func as compiled_function_node;
                tn = new this_node(compn.comperehensive_type as type_node, null);
                bfc = new compiled_function_call(compn, tn, null);
            }

            foreach (parameter p in gen_func.parameters)
            {
                bfc.parameters.AddElement(
                    create_variable_reference(p, null));
            }

            //Это запретит чистку стека
            bfc.last_result_function_call = true;

            statements_list snlist = new statements_list(null);
            snlist.statements.AddElement(bfc);
            snlist.statements.AddElement(new empty_statement(null));
            gen_func.function_code = snlist;
            context.pop_top_function();
            //context.leave_block();
        }
        //\ssyy

        private Dictionary<string, ref_type_node> WaitedRefTypes = new Dictionary<string, ref_type_node>();
        internal ref_type_node GetWaitedRefType(string name, location loc)
        {
            ref_type_node rtn = null;
            if (WaitedRefTypes.TryGetValue(name, out rtn))
                return rtn;
            rtn = new ref_type_node(null);
            rtn.PointedTypeName = name;
            rtn.loc = loc;
            WaitedRefTypes.Add(name, rtn);
            return rtn;
        }

        internal List<ref_type_node> RefTypesForCheckPointersTypeForDotNetFramework = new List<ref_type_node>();
        void ProcessRefTypesForCheckPointersTypeForDotNetFramework()
        {
            foreach (ref_type_node rtn in RefTypesForCheckPointersTypeForDotNetFramework)
                if (SemanticRules.StrongPointersTypeCheckForDotNet)
                    CheckPointersTypeForDotNetFramework(rtn.pointed_type, rtn.loc);
            RefTypesForCheckPointersTypeForDotNetFramework.Clear();
        }
        internal void CheckWaitedRefTypes(type_node tn)
        {
            ref_type_node rtn = null;
            if (WaitedRefTypes.TryGetValue(tn.name, out rtn))
            {
                if (SemanticRules.StrongPointersTypeCheckForDotNet)
                    RefTypesForCheckPointersTypeForDotNetFramework.Add(rtn);
                rtn.SetPointedType(tn);
                WaitedRefTypes.Remove(tn.name);
            }
        }

        internal void EnterTypeDeclarationsSection()
        {
            type_section_converting = true;
        }

        internal void LeaveTypeDeclarationsSection()
        {
            type_section_converting = false;
            if (context.types_predefined.Count > 0)
            {
            	AddError(context.types_predefined[0].loc, "NO_TYPE_{0}_DEFINITION", context.types_predefined[0].name);
            }
            foreach (string Name in WaitedRefTypes.Keys)
                WaitedRefTypes[Name].SetPointedType(find_type(Name, WaitedRefTypes[Name].loc));
            foreach (ref_type_node rtn in WaitedRefTypes.Values)
            	CheckForCircularityInPointers(rtn, rtn.pointed_type,rtn.loc);
            ProcessRefTypesForCheckPointersTypeForDotNetFramework();
        }

        internal void ProcessCheckPointersInRecord()
        {
        	ProcessRefTypesForCheckPointersTypeForDotNetFramework();
        }

        public override void visit(SyntaxTree.c_scalar_type node)
        {
            Type t = null;
            switch (node.scalar_name)
            {
                case SyntaxTree.c_scalar_type_name.tn_void:
                    t = typeof(void);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_char:
                    t = typeof(char);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_double:
                    t = typeof(double);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_float:
                    t = typeof(float);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_int:
                    t = (node.sign == SyntaxTree.c_scalar_sign.unsigned) ?
                        typeof(UInt32) :
                        typeof(Int32);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_long:
                case SyntaxTree.c_scalar_type_name.tn_long_int:
                    t = (node.sign == SyntaxTree.c_scalar_sign.unsigned) ?
                        typeof(UInt64) :
                        typeof(Int64);
                    break;
                case SyntaxTree.c_scalar_type_name.tn_short:
                case SyntaxTree.c_scalar_type_name.tn_short_int:
                    t = typeof(Int16);
                    break;
            }
            ret.return_value(compiled_type_node.get_type_node(t));
        }

        public override void visit(SyntaxTree.c_module node)
        {
            string namespace_name = "";
            location loc = null;
            //            if (_program_module.program_name != null)
            //            {
            //                namespace_name = _program_module.program_name.prog_name.name;
            //                loc = get_location(_program_module.program_name.prog_name);
            //                if (namespace_name.ToLower() != System.IO.Path.GetFileNameWithoutExtension(_program_module.file_name).ToLower())
            //                    throw new ProgramNameMustBeEqualProgramFileName(loc);
            //            }


            _compiled_unit = new common_unit_node();

            _compiled_unit.compiler_directives = ConvertDirectives(node);
            //PrepareDirectives(_compiled_unit.compiler_directives);

            //using_list.AddRange(interface_using_list);
            //weak_node_test_and_visit(_program_module.using_namespaces);
            //weak_node_test_and_visit(node.used_units);

            //compiled_main_unit=new unit_node();
            //SymbolTable.Scope[] used_units=new SymbolTable.Scope[used_assemblyes.Count+1];

            SymbolTable.Scope[] used_units = build_referenced_units(referenced_units,true);

            _compiled_unit.scope = convertion_data_and_alghoritms.symbol_table.CreateUnitInterfaceScope(used_units);

            common_namespace_node cnsn = context.create_namespace(namespace_name, _compiled_unit, _compiled_unit.scope, loc);
            cnsn.is_main = true;
            //compiled_program=new program_node();
            //compiled_program.units.Add(compiled_main_unit);

            reset_for_interface();

            UpdateUnitDefinitionItemForUnit(_compiled_unit);

            //_program_module.program_block.visit(this);
            //hard_node_test_and_visit(_program_module.program_block);
            //context.check_labels(context.converted_namespace.labels);

            //TODO: Доделать.
            //common_namespace_function_node main_function = new common_namespace_function_node(compiler_string_consts.temp_main_function_name,
            //    null, null, cnsn, null);
            //main_function.function_code = context.code;
            //cnsn.functions.AddElement(main_function);

            weak_node_test_and_visit(node.defs);

            bool main_not_found = true;
            foreach (function_node fn in cnsn.functions)
            {
                if (fn.name == compiler_string_consts.c_main_function_name)
                {
                    _compiled_unit.main_function = fn as common_namespace_function_node;
                    //context.apply_special_local_vars(_compiled_unit.main_function);
                    main_not_found = false;
                    break;
                }
            }
            if (main_not_found)
            {
                AddError(get_location(node), "NO_MAIN_FUNCTION");
            }

            context.leave_block();

            //_compiled_unit.main_function = main_function;

        }

        public override void visit(SyntaxTree.declarations_as_statement node)
        {
            //throw new NotSupportedError(get_location(node));
            node.defs.visit(this);
            return_value(new empty_statement(null));
        }
        public override void visit(SyntaxTree.array_size node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.enumerator node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.enumerator_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.c_for_cycle node)
        {
            //throw new NotSupportedError(get_location(node));
            location loc1 = get_location(node.expr1);
            statements_list head_stmts = new statements_list(loc1);
            convertion_data_and_alghoritms.statement_list_stack_push(head_stmts);
            //statement_node sn_init = convert_weak((SyntaxTree.statement)node.expr1); // SSM 12/06/15 - всё равно c-узлы никому не нужны
            statement_node sn_init = convert_weak(node.expr1); // SSM 12/06/15
            expression_node sn_cond = convert_weak(node.expr2);
            //statement_node sn_next = convert_weak((SyntaxTree.statement)node.expr3); // SSM 12/06/15
            statement_node sn_next = convert_weak(node.expr3); // SSM 12/06/15

            CheckToEmbeddedStatementCannotBeADeclaration(node.stmt);

            for_node fn = new for_node(sn_init, sn_cond, null, sn_next, null, get_location(node));
            context.cycle_stack.push(fn);

            statements_list slst = new statements_list(get_location(node.stmt));
            convertion_data_and_alghoritms.statement_list_stack_push(slst);

            context.enter_code_block_with_bind();
            fn.body = convert_strong(node.stmt);
            context.leave_code_block();

            slst = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (slst.statements.Count > 0 || slst.local_variables.Count > 0)
            {
                slst.statements.AddElement(fn.body);
                fn.body = slst;
            }

            context.cycle_stack.pop();

            head_stmts = convertion_data_and_alghoritms.statement_list_stack.pop();
            head_stmts.statements.AddElement(fn);
            return_value(head_stmts);

            /*slst = convertion_data_and_alghoritms.statement_list_stack.pop();
            if (slst.statements.Count > 0 || slst.local_variables.Count > 0)
            {
                slst.statements.AddElement(fn.body);
                fn.body = slst;
            }

            context.cycle_stack.pop();*/
            //ret.return_value(slst);
        }
        public override void visit(SyntaxTree.switch_stmt node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.type_definition_attr node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.type_definition_attr_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.lock_stmt node)
        {
            expression_node lock_object = convert_strong(node.lock_object);
            //Нужно ли это???
            if (lock_object.type.semantic_node_type == semantic_node_type.delegated_method)
                try_convert_typed_expression_to_function_call(ref lock_object);
            if (lock_object.type == null || lock_object.type.is_value_type)
                AddError(get_location(node.lock_object), "EXPRESSION_IN_LOCK_STATEMENT_RETURNED_NOT_A_REFERENCE_TYPE", lock_object.type);
            CheckToEmbeddedStatementCannotBeADeclaration(node.stmt);
            statement_node stmt = convert_strong(node.stmt);
            return_value(new lock_statement(lock_object, stmt, get_location(node)));
        }
        public override void visit(SyntaxTree.compiler_directive_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.compiler_directive_if node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.documentation_comment_list node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.documentation_comment_section node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.documentation_comment_tag node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.documentation_comment_tag_param node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.token_taginfo node)
        {
            throw new NotSupportedError(get_location(node));
        }
        public override void visit(SyntaxTree.declaration_specificator node)
        {
            throw new NotSupportedError(get_location(node));
        }

        public override void visit(SyntaxTree.ident_with_templateparams _ident_with_templateparams)
        {
            SyntaxTree.expression ex = _ident_with_templateparams.name;
            SyntaxTree.ident id_ex = _ident_with_templateparams.name as SyntaxTree.ident;
            int par_count = _ident_with_templateparams.template_params.params_list.Count;
            if (id_ex != null)
            {
                SymbolInfo type_si = context.find(id_ex.name + compiler_string_consts.generic_params_infix + par_count.ToString());
                if (type_si != null)
                {
                    return_value(get_generic_instance(type_si, _ident_with_templateparams.template_params.params_list));
                    return;
                }
                type_si = context.find(id_ex.name);
                if (type_si != null && type_si.sym_info.general_node_type == general_node_type.template_type)
                {
                    template_class tc = type_si.sym_info as template_class;
                    List<type_node> tpars = visit_type_list(_ident_with_templateparams.template_params.params_list);
                    return_value(instance_any(tc, tpars, get_location(_ident_with_templateparams)));
                    return;
                }
            }
            expression_node adr = ret.visit(ex);
            typed_expression te = adr as typed_expression;
            if (te != null)
            {
                delegated_methods dm = te.type as delegated_methods;
                if (dm != null)
                {
                    List<type_node> ptypes = visit_type_list(_ident_with_templateparams.template_params.params_list);
                    base_function_call_list bfcl = new base_function_call_list();
                    common_method_node cnode;
                    compiled_function_node comp_node;
                    function_node fnode;
                    int generic_count = 0;
                    foreach (base_function_call bfc in dm.proper_methods)
                    {
                        if (bfc.simple_function_node.is_generic_function)
                        {
                            generic_count++;
                        }
                    }
                    bool one_function = false;
                    switch (generic_count)
                    {
                        case 0:
                            AddError(get_location(_ident_with_templateparams), "TRIANGLE_BRACKETS_NOT_ALLOWED_WITH_COMMON_FUNCTIONS"); break;
                        case 1:
                            one_function = true;
                            break;
                        default:
                            one_function = false;
                            break;
                    }
                    foreach (base_function_call bfc in dm.proper_methods)
                    {
                        if (!bfc.simple_function_node.is_generic_function)
                        {
                            continue;
                        }
                        switch (bfc.semantic_node_type)
                        {
                            case semantic_node_type.common_namespace_function_call:
                                common_namespace_function_node cnfn = bfc.simple_function_node.get_instance(ptypes, one_function, get_location(_ident_with_templateparams)) as common_namespace_function_node;
                                if (cnfn != null)
                                {
                                    bfcl.AddElement(new common_namespace_function_call(cnfn, bfc.location));
                                }
                                break;
                            case semantic_node_type.common_method_call:
                                cnode = bfc.simple_function_node.get_instance(ptypes, one_function, get_location(_ident_with_templateparams)) as common_method_node;
                                if (cnode != null)
                                {
                                    bfcl.AddElement(new common_method_call(cnode, (bfc as common_method_call).obj, bfc.location));
                                }
                                break;
                            case semantic_node_type.compiled_function_call:
                                fnode = bfc.simple_function_node.get_instance(ptypes, one_function, get_location(_ident_with_templateparams));
                                cnode = fnode as common_method_node;
                                if (cnode != null)
                                {
                                    bfcl.AddElement(new common_method_call(cnode, (bfc as compiled_function_call).obj, bfc.location));
                                }
                                else
                                {
                                    comp_node = fnode as compiled_function_node;
                                    if (comp_node != null)
                                    {
                                        bfcl.AddElement(new compiled_function_call(comp_node, (bfc as compiled_function_call).obj, bfc.location));
                                    }
                                }
                                break;
                            case semantic_node_type.common_static_method_call:
                            case semantic_node_type.compiled_static_method_call:
                                fnode = bfc.simple_function_node.get_instance(ptypes, one_function, get_location(_ident_with_templateparams));
                                cnode = fnode as common_method_node;
                                if (cnode != null)
                                {
                                    bfcl.AddElement(new common_static_method_call(cnode, bfc.location));
                                }
                                else
                                {
                                    comp_node = fnode as compiled_function_node;
                                    if (comp_node != null)
                                    {
                                        bfcl.AddElement(new compiled_static_method_call(comp_node, bfc.location));
                                    }
                                }
                                break;
                        }
                    }
                    if (bfcl.Count == 0)
                    {
                        AddError(get_location(_ident_with_templateparams.template_params), "NO_FUNCTIONS_{0}_CAN_BE_USED_WITH_THIS_SPECIFICATION", dm.proper_methods[0].function.name);
                    }
                    delegated_methods dm1 = new delegated_methods();
                    dm1.proper_methods.AddRange(bfcl);
                    typed_expression te1 = new typed_expression(dm1, te.location);
                    return_value(te1);
                    return;
                }
            }
            AddError(get_location(_ident_with_templateparams.name), "TRIANGLE_BRACKETS_NOT_AWAITED");
         }

        public override void visit(SyntaxTree.template_type_name node)
        {
            throw new NotSupportedError(get_location(node));
        }

        public override void visit(SyntaxTree.default_operator _default_operator)
        {
            type_node tn = ret.visit(_default_operator.type_name);
            if (tn == null)
            {
                AddError(get_location(_default_operator), "TYPE_REFERENCE_EXPECTED");
            }
            if (tn == SystemLibrary.SystemLibrary.void_type)
            {
                AddError(get_location(_default_operator.type_name), "INAVLID_TYPE");
            }
            return_value(new default_operator_node(tn, get_location(_default_operator)));
            //throw new NotSupportedError(get_location(_default_operator));
        }
        
        public override void visit(SyntaxTree.bracket_expr _bracket_expr)
        {
        	_bracket_expr.expr.visit(this);
        }
        
        public override void visit(SyntaxTree.attribute _attribute)
        {
        	
        }
        
        public override void visit(SyntaxTree.attribute_list _attribute_list)
        {
        	
        }
        
        public override void visit(SyntaxTree.simple_attribute_list _simple_attribute_list)
        {
        	
        }

        /* Этот код мешает. Может, его убрать?
         
        public override void visit(SyntaxTree.function_lambda_definition _function_lambda_definition)
        {
        SyntaxTree.procedure_definition _func_def = new PascalABCCompiler.SyntaxTree.procedure_definition();
        SyntaxTree.method_name _method_name = new SyntaxTree.method_name(null, new SyntaxTree.ident("$a"), null);
        SyntaxTree.function_header _function_header = new SyntaxTree.function_header();

        object rt = new object();
        _function_header.name = _method_name;
        SyntaxTree.formal_parameters fps=new PascalABCCompiler.SyntaxTree.formal_parameters();
        for (int i = 0; i < _function_lambda_definition.formal_parameters.params_list.Count; i++)
        {
            SyntaxTree.ident_list _ident_list = new SyntaxTree.ident_list();
            SyntaxTree.ident id =_function_lambda_definition.formal_parameters.params_list[i].idents.idents[0];
            _ident_list.idents.Add(id);
            SyntaxTree.named_type_reference _named_type_reference1 = new SyntaxTree.named_type_reference();
            SyntaxTree.ident idtype1 = new SyntaxTree.ident("datatype");
            _named_type_reference1.names.Add(idtype1);
            SyntaxTree.typed_parameters _typed_parametres = new SyntaxTree.typed_parameters(_ident_list, (SyntaxTree.type_definition)_named_type_reference1, SyntaxTree.parametr_kind.none, null);
            fps.params_list.Add(_typed_parametres);
        }
        _function_header.parameters = fps;
        fps.visit(this);
        SyntaxTree.named_type_reference _named_type_reference = new SyntaxTree.named_type_reference();
        SyntaxTree.ident idtype = new SyntaxTree.ident("datatype");
        _named_type_reference.source_context = idtype.source_context;
        _named_type_reference.names.Add(idtype);

        rt = _named_type_reference;
        _function_header.return_type = (SyntaxTree.type_definition)_named_type_reference;

        _function_header.of_object = false;
        _function_header.class_keyword = false;
        SyntaxTree.block _block = new SyntaxTree.block(null, null);
        SyntaxTree.statement_list sl = new SyntaxTree.statement_list();
        sl.subnodes.Add(_function_lambda_definition.proc_body);
        _block.program_code = sl;
        _func_def.proc_header = _function_header;
        _func_def.proc_body = (SyntaxTree.proc_block)_block;
        _func_def.visit(this);
        expressions_list el = new expressions_list();
        foreach (SyntaxTree.expression en in _function_lambda_definition.parameters.expressions)
        {
            el.AddElement(convert_strong(en));
        }
        function_node fn = find_function("$a", get_location(_function_header), el);*/
        ///////////////////////////////////////////////////////////////////////////


            /*hard_node_test_and_visit(_function_lambda_definition.formal_parameters);
            hard_node_test_and_visit(_function_lambda_definition.proc_body);

            CheckToEmbeddedStatementCannotBeADeclaration(_function_lambda_definition.proc_body);
            statement_node body = convert_strong(_function_lambda_definition.proc_body);
            //context.leave_code_block();
            type_node tp = convert_strong(_function_lambda_definition.return_type);
            parameter_list pl = new parameter_list();
            for (int i = 0; i < _function_lambda_definition.formal_parameters.params_list.Count; i++)
            {
                common_parameter cp = new common_parameter(_function_lambda_definition.formal_parameters.params_list[i].idents.idents[0].name,
                            SemanticTree.parameter_type.value, null,
                            concrete_parameter_type.cpt_none, get_location(_function_lambda_definition.formal_parameters.params_list[i].idents.idents[0]));
                pl.AddElement(cp);
            }

            function_lambda_node fln = new function_lambda_node(pl, tp, body);
            fln.type = tp;
            //fln.function = fn;
            return_value(fln);*/


            //lroman// 
            /*LambdaHelper.captureCheck = true; //захват пока не делаем
            var block = LambdaHelper.CreateFictiveBlockForLambda(_function_lambda_definition);
            context.create_lambda();
            hard_node_test_and_visit(block.program_code);
            context.remove_lambda();
            LambdaHelper.captureCheck = false;*/

            //if (LambdaHelper.capturedVariables.Count == 0)


        // SSM - 15.1.2014
        //public void proba_after_visit_function_lambda_definition(SyntaxTree.function_lambda_definition _function_lambda_definition, SyntaxTree.procedure_definition pd)
        //{
        //    var fvisbody = new FindMainIdentsVisitor();
        //    fvisbody.ProcessNode(_function_lambda_definition.proc_body);

        //    var fvisparams = new FindMainIdentsVisitor();
        //    fvisparams.ProcessNode(_function_lambda_definition.formal_parameters);

        //    var fvislocaldefs = new FindLocalDefsVisitor();
        //    fvislocaldefs.ProcessNode(_function_lambda_definition.proc_body);
        //    fvislocaldefs.vars.Add("result");

        //    var candidates = fvisbody.vars.Except(fvisparams.vars, StringComparer.OrdinalIgnoreCase).Except(fvislocaldefs.vars, StringComparer.OrdinalIgnoreCase).ToArray();

        //    List<SyntaxTree.ident> li = new List<SyntaxTree.ident>();
        //    List<SyntaxTree.type_definition> lt = new List<SyntaxTree.type_definition>();
        //    SymbolInfo si;
        //    foreach (var x in candidates)
        //    {
        //        si = context.find(x);
        //        var tp = si.sym_info.GetType();
        //        if (tp == typeof(TreeRealization.local_block_variable) || tp == typeof(TreeRealization.local_variable))
        //        {
        //            var id = new SyntaxTree.ident(x);
        //            li.Add(id);
        //            var typ = ident_value_reciving(id).type;
        //            lt.Add(SyntaxTreeBuilder.BuildSemanticType(typ));
        //            // Попытаемся определить их типы
        //        }
        //        else if (tp == typeof(TreeRealization.class_field))
        //        {
        //            // Поля класса тоже захватываем! 
        //            var id = new SyntaxTree.ident(x);
        //            li.Add(id);
        //            var typ = ident_value_reciving(id).type;
        //            lt.Add(SyntaxTreeBuilder.BuildSemanticType(typ));
        //        }
        //        else if (tp == typeof(TreeRealization.common_parameter))
        //        {
        //            // И параметры метода/подпрограммы тоже захватываем! 
        //            var id = new SyntaxTree.ident(x);
        //            li.Add(id);
        //            var typ = ident_value_reciving(id).type;
        //            lt.Add(SyntaxTreeBuilder.BuildSemanticType(typ));
        //        }
        //    }

        //    var UNS = UniqueNumStr();
        //    var LambdaClassName = "#LambdaClassName" + UNS;
            
        //    //if (li.Count > 0)
        //    {
        //        contextChanger.SaveContextAndUpToGlobalLevel();
        //        var bt = SyntaxTreeBuilder.BuildClassWithOneMethod(LambdaClassName, li, lt, pd);
        //        visit(bt); // добавили класс __A в раздел описаний
        //        contextChanger.RestoreCurrentContext();
        //    }

        //    /* другая версия кода внизу
        //     * var ttp = new SyntaxTree.named_type_reference("__A", null);
        //    type_node semttp = convert_strong(ttp);

        //    common_function_node topfun = context.func_stack.top();
        //    local_variable lv = new local_variable("a1", semttp, topfun, null);

        //    topfun.var_definition_nodes_list.AddElement(lv);
        //    context.CurrentScope.AddSymbol("a1", new SymbolInfo(lv));*/

        //    // теперь сформируем семантический узел вида a1 := new __A()
        //    var ttp = new SyntaxTree.named_type_reference(LambdaClassName, null);
        //    type_node semttp = convert_strong(ttp);

        //    var LambdaVarName = "#LambdaVarName" + UNS;
        //    context.add_var_definition(LambdaVarName, null, semttp, SemanticTree.polymorphic_state.ps_common); // добавление описания локальной переменной. Все такие описания в .NET-коде делаются перед любыми операторами

        //    SyntaxTree.expression_list el = new SyntaxTree.expression_list();
        //    for (int i = 0; i < li.Count; i++)
        //        el.Add(li[i]);
        //    var nex = new SyntaxTree.new_expr(ttp, el, null);
        //    var ass = new SyntaxTree.assign(LambdaVarName, nex);
        //    var sass = convert_strong(ass);

        //    // теперь добавим этот узел в конец самого верхнего statement_list текущей функции
        //    var sf = convertion_data_and_alghoritms.statement_list_stack.first();
        //    sf.statements.AddElement(sass);

        //    var dn = new SyntaxTree.dot_node(new SyntaxTree.ident(LambdaVarName), pd.proc_header.name.meth_name);
        //    visit(dn);
        //}

        public override void visit(closure_substituting_node _closure_substituting_node)
        {
            visit(_closure_substituting_node.substitution);
        }

        public override void visit(SyntaxTree.function_lambda_definition _function_lambda_definition)
        {
            _function_lambda_definition.RealSemTypeOfResExpr = null; // После первого присваивания Result она будет содержать тип type_node в правой части Result
            _function_lambda_definition.RealSemTypeOfResult = null;
            try
            {                
                stflambda.Push(_function_lambda_definition);
                if (_function_lambda_definition.substituting_node != null)
                {
                    if (_function_lambda_definition.substituting_node is dot_node)
                    {
                        visit((dot_node)_function_lambda_definition.substituting_node);
                    }
                    else
                    {
                        if (_function_lambda_definition.substituting_node is ident)
                        {
                            visit((ident)_function_lambda_definition.substituting_node);
                        }
                        else
                        {
                            if (_function_lambda_definition.substituting_node is ident_with_templateparams)
                            {
                                visit((ident_with_templateparams)_function_lambda_definition.substituting_node);
                            }
                        }
                    }
                    return;
                }

                Func<function_lambda_definition, ident_list, where_definition_list, expression> makeProcedureForLambdaAndVisit =
                    (lambdaDefinition, tempParsList, whereSection) =>
                    {
                        var procDecl = LambdaHelper.ConvertLambdaNodeToProcDefNode(lambdaDefinition);

                        if (tempParsList != null)
                        {
                            procDecl.proc_header.template_args = tempParsList;
                        }

                        if (whereSection != null && whereSection.defs != null && whereSection.defs.Count != 0)
                        {
                            procDecl.proc_header.where_defs = whereSection;
                        }

                        if (!context.func_stack.Empty && context.func_stack.top().polymorphic_state == SemanticTree.polymorphic_state.ps_static)
                        {
                            procDecl.proc_header.class_keyword = true;
                            if (procDecl.proc_header.proc_attributes == null)
                                procDecl.proc_header.proc_attributes = new SyntaxTree.procedure_attributes_list();
                            procDecl.proc_header.proc_attributes.proc_attributes.Add(new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static));
                        }

                        try
                        {
                            visit(procDecl);
                        }
                        catch
                        {
                            context.remove_lambda_function(procDecl.proc_header.name.meth_name.name, true);
                            throw;
                        }

                        context.remove_lambda_function(procDecl.proc_header.name.meth_name.name, false);

                        return tempParsList == null ?
                            (expression)procDecl.proc_header.name.meth_name :
                            (expression)new ident_with_templateparams(procDecl.proc_header.name.meth_name, new template_param_list(tempParsList.idents.Select(l => SyntaxTreeBuilder.BuildSimpleType(l.name)).ToList()));
                    };


                switch (lambdaProcessingState)
                {
                    case LambdaProcessingState.FinishPhase:
                        {
                            if (context.top_function != null && context.top_function.generic_params != null)
                            {
                                var pars = context.top_function.generic_params.Select(par => new ident(par.name)).ToList();

                                var whereSection =
                                    CapturedVariablesSubstitutor.GetWhereSection(
                                        generic_parameter_eliminations.make_eliminations_common(
                                            context.top_function.generic_params), pars);

                                if (_function_lambda_definition.formal_parameters != null && _function_lambda_definition.formal_parameters.params_list.Count > 0)
                                {
                                    for (var j = 0; j < _function_lambda_definition.formal_parameters.params_list.Count; j++)
                                    {
                                        if (_function_lambda_definition.formal_parameters.params_list[j].vars_type is lambda_inferred_type)
                                        {
                                            if ((_function_lambda_definition.formal_parameters.params_list[j].vars_type as lambda_inferred_type).real_type is type_node)
                                            {
                                                _function_lambda_definition.formal_parameters.params_list[j].vars_type =
                                                    LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)(_function_lambda_definition.formal_parameters.params_list[j].vars_type as lambda_inferred_type).real_type);
                                            }
                                        }
                                    }
                                }

                                if (_function_lambda_definition.return_type is lambda_inferred_type)
                                {
                                    if ((_function_lambda_definition.return_type as lambda_inferred_type).real_type is type_node)
                                    {
                                        _function_lambda_definition.return_type = LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)(_function_lambda_definition.return_type as lambda_inferred_type).real_type);
                                    }
                                }

                                var methodNameToVisit = (ident_with_templateparams)makeProcedureForLambdaAndVisit(_function_lambda_definition, new ident_list(pars), whereSection);
                                _function_lambda_definition.substituting_node = methodNameToVisit;
                                visit(methodNameToVisit);
                            }
                            else
                            {
                                var methodNameToVisit = (ident)makeProcedureForLambdaAndVisit(_function_lambda_definition, null, null);
                                _function_lambda_definition.substituting_node = methodNameToVisit;
                                visit(methodNameToVisit);
                            }
                            break;
                        }
                    case LambdaProcessingState.TypeInferencePhase:
                        {
                            if (_function_lambda_definition.lambda_visit_mode ==
                                LambdaVisitMode.VisitForAdvancedMethodCallProcessing)
                            {
                                makeProcedureForLambdaAndVisit(_function_lambda_definition, null, null);
                                if (!LambdaHelper.IsAuxiliaryLambdaName(_function_lambda_definition.lambda_name))
                                {
                                    LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, _function_lambda_definition);
                                }
                            }

                            ret.return_value((semantic_node)LambdaHelper.GetTempFunctionNodeForTypeInference(_function_lambda_definition, this));

                            break;
                        }
                    case LambdaProcessingState.ClosuresProcessingPhase:
                        {
                            makeProcedureForLambdaAndVisit(_function_lambda_definition, null, null);
                            LambdaHelper.RemoveLambdaInfoFromCompilationContext(context, _function_lambda_definition);
                            ret.return_value((semantic_node)LambdaHelper.GetTempFunctionNodeForTypeInference(_function_lambda_definition, this));
                            break;
                        }
                    case LambdaProcessingState.ClosuresProcessingVisitGeneratedClassesPhase:
                        {
                            makeProcedureForLambdaAndVisit(_function_lambda_definition, null, null);
                            break;
                        }
                    case LambdaProcessingState.None:
                        {
                            if (context.converting_block() == block_type.namespace_block)
                            {
                                var methodNameToVisit = (ident)makeProcedureForLambdaAndVisit(_function_lambda_definition, null, null);
                                _function_lambda_definition.substituting_node = methodNameToVisit;
                                visit(methodNameToVisit);
                            }
                            break;
                        }
                }
            }
            finally
            {
                stflambda.Pop();
            }
        }

        public override void visit(SyntaxTree.function_lambda_call _function_lambda_call)
        {
            SyntaxTree.op_type_node _op_type_node = new SyntaxTree.op_type_node(SyntaxTree.Operators.Assignment);
            for (int i = 0; i < _function_lambda_call.parameters.expressions.Count; i++)
            {
                SyntaxTree.assign _assign = new SyntaxTree.assign(_function_lambda_call.f_lambda_def.parameters.expressions[i] as SyntaxTree.addressed_value, _function_lambda_call.parameters.expressions[i] as SyntaxTree.expression, _op_type_node.type);
                ((SyntaxTree.statement_list)_function_lambda_call.f_lambda_def.proc_body).subnodes.Insert(0, _assign);
            }
            SyntaxTree.expression_list el = new SyntaxTree.expression_list();
            for (int i = 0; i < _function_lambda_call.f_lambda_def.formal_parameters.params_list.Count; i++)
                if (i < _function_lambda_call.parameters.expressions.Count)
                    el.expressions.Add(_function_lambda_call.parameters.expressions[i]);
                else
                {
                    el.expressions.Add(new SyntaxTree.ident(_function_lambda_call.f_lambda_def.formal_parameters.params_list[i].idents.idents[0].name));
                }
            SyntaxTree.method_call _method_call = new SyntaxTree.method_call(el);
            if (_method_call is SyntaxTree.dereference)
            {
                ((SyntaxTree.dereference)_method_call).dereferencing_value = (SyntaxTree.addressed_value)(new SyntaxTree.ident(_function_lambda_call.f_lambda_def.lambda_name));
            }
            _method_call.visit(this);
        }

        public void lambda_header_visit(SymbolInfo si, SyntaxTree.function_header _function_header, type_node tn1)
        {
            type_node tn = null;
            if (_function_header.return_type == null)
            {
                if (context.top_function.IsOperator)
                    AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            }
            if (_function_header.return_type != null)
            {
                check_parameter_on_complex_type(_function_header.return_type);
                if (tn1 != null)
                    tn = tn1;
                else
                    tn = convert_strong(_function_header.return_type);
                //if (tn == SystemLibrary.SystemLibrary.void_type)
                //    AddError(new VoidNotValid(get_location(_function_header.return_type)));
                check_for_type_allowed(tn, get_location(_function_header.return_type));
            }
            //(ssyy) moved up, так как при проверке аттрибута override надо знать тип возвращаемого значения
            context.top_function.return_value_type = tn;
            assign_doc_info(context.top_function, _function_header);
            if (_function_header.attributes != null)
            {
                make_attributes_for_declaration(_function_header, context.top_function);
            }
            if (context.converted_type != null && has_dll_import_attribute(context.top_function))
                AddError(get_dll_import_attribute(context.top_function).location, "DLLIMPORT_ATTRIBUTE_CANNOT_BE_APPLIED_TO_METHOD");
            if (_function_header.name.class_name != null)
                with_class_name = true;
            if (_function_header.class_keyword && !has_static_attr(_function_header.proc_attributes.proc_attributes))
            {
                SyntaxTree.procedure_attribute pa = new SyntaxTree.procedure_attribute(PascalABCCompiler.SyntaxTree.proc_attribute.attr_static);
                pa.source_context = _function_header.source_context;
                _function_header.proc_attributes.proc_attributes.Add(pa);
            }
            weak_node_test_and_visit(_function_header.proc_attributes);
            if (context.top_function.IsOperator)
            {
                common_method_node cmmn = context.top_function as common_method_node;
                //if (cmmn == null)
                //{
                //    throw new OverloadOperatorMustBeStaticFunction(get_location(_function_header), context.top_function);
                //}
                if (cmmn != null && cmmn.polymorphic_state != SemanticTree.polymorphic_state.ps_static)
                {
                    AddError(get_location(_function_header), "OVERLOADED_OPERATOR_MUST_BE_STATIC_FUNCTION");
                }
                if (cmmn != null && (cmmn.name == compiler_string_consts.implicit_operator_name || cmmn.name == compiler_string_consts.explicit_operator_name))
                    if (!convertion_data_and_alghoritms.eq_type_nodes(tn, cmmn.comperehensive_type as type_node) && !convertion_data_and_alghoritms.eq_type_nodes(cmmn.comperehensive_type as type_node, cmmn.parameters[0].type))
                    {
                        AddError(get_location(_function_header.return_type), "RETURN_VALUE_SHOULD_HAVE_TYPE_{0}", (cmmn.comperehensive_type as type_node).PrintableName);
                    }
                    else if (convertion_data_and_alghoritms.eq_type_nodes(tn, cmmn.parameters[0].type))
                    {
                        AddError(get_location(_function_header), "CIRCURAL_TYPE_CONVERSION_DEFINITION");
                    }
            }
            with_class_name = false;
            if (context.top_function != null && context.top_function is common_namespace_function_node && (context.top_function as common_namespace_function_node).ConnectedToType != null && !context.top_function.IsOperator)
            {
                concrete_parameter_type cpt = concrete_parameter_type.cpt_none;
                SemanticTree.parameter_type pt = PascalABCCompiler.SemanticTree.parameter_type.value;
                if ((context.top_function as common_namespace_function_node).ConnectedToType.is_value_type)
                {
                    cpt = concrete_parameter_type.cpt_var;
                    pt = PascalABCCompiler.SemanticTree.parameter_type.var;
                }
                common_parameter cp = new common_parameter(compiler_string_consts.self_word, (context.top_function as common_namespace_function_node).ConnectedToType, pt,
                                                                                context.top_function, cpt, null, null);
                context.top_function.parameters.AddElementFirst(cp);
                context.top_function.scope.AddSymbol(compiler_string_consts.self_word, new SymbolInfo(cp));
            }
            CheckOverrideOrReintroduceExpectedWarning(get_location(_function_header));

            bool unique = context.close_function_params(body_exists);
            if (context.top_function.return_value_type == null)
                AddError(get_location(_function_header), "FUNCTION_NEED_RETURN_TYPE");
            if (_function_header.where_defs != null)
            {
                if (unique)
                {
                    visit_where_list(_function_header.where_defs);
                }
                else
                {
                    AddError(get_location(_function_header.where_defs), "WHERE_SECTION_MUST_BE_ONLY_IN_FIRST_DECLARATION");
                }
            }
            convertion_data_and_alghoritms.create_function_return_variable(context.top_function, si);

            /*if (_function_header.name != null && context.converted_compiled_type != null && context.top_function is common_namespace_function_node)
            {
                if (context.FindMethodToOverride(context.top_function as common_namespace_function_node) != null)
                    AddError(new CanNotDeclareExtensionMethodAsOverrided(get_location(_function_header)));
            }*/
            //TODO: Разобрать подробнее.
            if (!body_exists)
            {
                if ((context.top_function.semantic_node_type == semantic_node_type.common_method_node)
                    || ((context.func_stack_size_is_one()) && (_is_interface_part)))
                {
                    context.leave_block();
                }
            }
            body_exists = false;
        }
        public void lambda_body_visit(SyntaxTree.block _block)
        {
            //weak_node_test_and_visit(_block.defs);

            //ssyy добавил генерацию вызова конструктора предка без параметров
            if (context.converting_block() == block_type.function_block)
            {
                common_method_node cmn = context.top_function as common_method_node;
                if (cmn != null && cmn.is_constructor && !(cmn.polymorphic_state == SemanticTree.polymorphic_state.ps_static))
                {
                    context.allow_inherited_ctor_call = true;
                    if (_block.program_code != null && _block.program_code.subnodes != null)
                    {
                        //(ssyy) Для записей конструктор предка не вызываем.
                        bool should_ctor_add = context.converted_type.is_class;
                        if (should_ctor_add)
                        {
                            if (_block.program_code.subnodes.Count > 0)
                            {
                                SyntaxTree.procedure_call pc = _block.program_code.subnodes[0] as SyntaxTree.procedure_call;
                                if (pc != null || _block.program_code.subnodes[0] is SyntaxTree.inherited_message)
                                {
                                    //SyntaxTree.inherited_ident ii = pc.func_name as SyntaxTree.inherited_ident;
                                    //SyntaxTree.method_call mc = pc.func_name as SyntaxTree.method_call;
                                    //SyntaxTree.ident id = pc.func_name as SyntaxTree.ident;
                                    //if (/*ii != null*/id != null || mc != null /*&& mc.dereferencing_value is SyntaxTree.inherited_ident*/)
                                    {
                                        //(ssyy) Не уверен, что следующий оператор необходим.
                                        convertion_data_and_alghoritms.check_node_parser_error(_block.program_code);

                                        statement_node inh = convert_strong(_block.program_code.subnodes[0]);

                                        compiled_constructor_call c1 = inh as compiled_constructor_call;
                                        if (c1 != null && !c1._new_obj_awaited)
                                        {
                                            should_ctor_add = false;
                                        }
                                        else
                                        {
                                            common_constructor_call c2 = inh as common_constructor_call;
                                            if (c2 != null && !c2._new_obj_awaited)
                                            {
                                                should_ctor_add = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (should_ctor_add)
                        {
                            //Пытаемся добавить вызов .ctor() предка...
                            //Для начала проверим, есть ли у предка таковой.
                            bool not_found = true;
                            SymbolInfo sym = context.converted_type.base_type.find_in_type(compiler_string_consts.default_constructor_name, context.CurrentScope);
                            while (not_found && sym != null)
                            {
                                compiled_constructor_node ccn = sym.sym_info as compiled_constructor_node;
                                if (ccn != null && ccn.parameters.Count == 0)
                                {
                                    not_found = false;
                                }
                                else
                                {
                                    common_method_node cnode = sym.sym_info as common_method_node;
                                    if (cnode != null && cnode.is_constructor && (cnode.parameters.Count == 0 || cnode.parameters[0].default_value != null))
                                    {
                                        not_found = false;
                                    }
                                }
                                sym = sym.Next;
                            }
                            if (not_found)
                            {
                                //У предка нет конструктора по умолчанию
                                AddError(get_location(_block.program_code), "INHERITED_CONSTRUCTOR_CALL_EXPECTED");
                            }
                            else
                            {
                                //Генерируем вызов .ctor() предка
                                SyntaxTree.inherited_ident ii = new SyntaxTree.inherited_ident();
                                ii.name = compiler_string_consts.default_constructor_name;
                                _block.program_code.subnodes.Insert(0, new SyntaxTree.procedure_call(ii));
                                //context.allow_inherited_ctor_call = false;
                            }
                        }
                    }
                }
            }
            //\ssyy

            //ssyy
            if (context.converting_block() == block_type.namespace_block)
            {
                context.check_predefinition_defined();
            }

            context.enter_code_block_with_bind();
            statement_node sn = convert_strong(_block.program_code);
            context.leave_code_block();

            context.code = sn;
        }
		
		public override void visit(SyntaxTree.lambda_inferred_type lit) //lroman//
        {
            return_value(lit.real_type as type_node);
        }
		
        public override void visit(SyntaxTree.semantic_check sem)
        {
            if (sem == null)
                return;
            switch (sem.CheckName)
            {
                case "ExprIsInteger":
                    var ex = (SyntaxTree.expression)sem.param[0];
                    expression_node en = convert_strong(ex);
                    en.location = get_location(ex);
                    if (en.type != SystemLibrary.SystemLibrary.integer_type)
                        AddError(get_location(ex), "STRING_CONSTANT_OR_IDENTIFIER_EXPECTED");
                    break;
                case " ":
                    break;
                case "  ":
                    break;
            }
        }

        public override void visit(SyntaxTree.same_type_node st) // SSM 22/06/13
        {
            expression_node en = convert_strong(st.ex);
            return_value(en.type);
        }

        public type_node try_calc_expr_type(SyntaxTree.expression ex) // SSM 10.07.13
        {
            try
            {
                expression_node en = convert_strong(ex);
                return en.type;
            }
            catch (Exception e)
            {
                return new undefined_type("#UndefType", null);
            }
        }

        public override void visit(SyntaxTree.semantic_type_node stn)
        {
            return_value(stn.type as type_node);
        }

        public bool SameTypeAutoClasses(List<string> names, List<type_node> types, common_type_node t)
        {
            if (t.properties.Count() != names.Count())
                return false;

            var tnames = t.properties.Select(tt => tt.name);

            return types.SequenceEqual(t.fields.Select(tt => tt.type)) && names.SequenceEqual(tnames, StringComparer.OrdinalIgnoreCase); 
            //return true;
        }

        public override void visit(SyntaxTree.unnamed_type_object unn) // SSM 27.06.13
        {
            var l = unn.ne_list;
            var names = l.name_expr.Select(x=>x.name).ToList();
            var strnames = names.Select(id=>id.name).ToList(); // преобразование idents в строки
            var semantic_types = l.name_expr.Select(x => convert_strong(x.expr).type).ToList();
            var types = semantic_types.Select(x => SyntaxTreeBuilder.BuildSemanticType(x)).ToList();

            contextChanger.SaveContextAndUpToGlobalLevel();

            var tt = context._cmn.types.Where(t => t.name.StartsWith("AnonymousType#")); // найти все анонимные типы, полученные в текущей компиляции

            var typ = tt.FirstOrDefault(t => SameTypeAutoClasses(strnames, semantic_types, t)); // найти структурно совпадающий уже существующий анонимный тип

            // Генерация глобальных описаний - перенесена с синтаксического уровня
            if (typ != null)
                unn.set_name(typ.name); // если нашли, то просто меняем имя на имя этого класса
            else
            {
                var td = SyntaxTreeBuilder.BuildAutoClass(unn.name(), names, types, unn.is_class);
                visit(td);
            }

            contextChanger.RestoreCurrentContext();

            // Сахарный узел. Переадресация дальнейшей генерации узлу new_ex
            visit(unn.new_ex);
        }

        public override void visit(SyntaxTree.short_func_definition def) // SSM 20.07.13
        {
            // Сахарный узел. Переадресация дальнейшей генерации узлу procedure_definition
            visit(def.procdef);
        }

        public override void visit(matching_expression _matching_expression)
        {
            SyntaxTree.procedure_call pc = new SyntaxTree.procedure_call();
            SyntaxTree.method_call mc = new SyntaxTree.method_call();
            dot_node dot = new dot_node(new ident("PABCSystem"), new ident("KV"));
            mc.dereferencing_value = dot;
            pc.func_name = mc;
            pc.source_context = _matching_expression.source_context;
            SyntaxTree.expression_list exl = new PascalABCCompiler.SyntaxTree.expression_list();
            exl.Add(_matching_expression.left);
            exl.Add(_matching_expression.right);
            mc.parameters = exl;
            visit(pc);
        }
        public override void visit(SyntaxTree.sequence_type _sequence_type)
        {
            // SSM 11/05/15 sugared node
            var l = new List<ident>();
            l.Add(new ident("System"));
            l.Add(new ident("Collections"));
            l.Add(new ident("Generic"));
            l.Add(new ident("IEnumerable"));
            var tr = new template_type_reference(new named_type_reference(l), new template_param_list(_sequence_type.elements_type, _sequence_type.elements_type.source_context), _sequence_type.source_context);
            visit(tr);
        }
        public override void visit(SyntaxTree.modern_proc_type _modern_proc_type)
        {
            if (_modern_proc_type.res != null)
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Func"));
                var t = new template_param_list();
                if (_modern_proc_type.aloneparam != null)
                    t.Add(_modern_proc_type.aloneparam);
                if (_modern_proc_type.el != null)
                {
                    var en = _modern_proc_type.el;
                    if (en.enumerators.Count == 1)
                        AddError(get_location(en.enumerators[0].name), "ONE_TYPE_PARAMETER_MUSTBE_WITHOUT_PARENTHESES");
                    for (int i = 0; i < en.enumerators.Count; i++)
                    {
                        if (en.enumerators[i].value != null)
                            AddError(get_location(en.enumerators[i].name), "ONE_TKIDENTIFIER");
                        t.Add(new named_type_reference(en.enumerators[i].name, en.enumerators[i].name.source_context));
                    }
                }
                t.Add(_modern_proc_type.res, _modern_proc_type.res.source_context);
                t.source_context = _modern_proc_type.source_context;
                var ttr = new template_type_reference(new named_type_reference(l), t, _modern_proc_type.source_context);
                visit(ttr);
            }
            else if (_modern_proc_type.aloneparam != null || _modern_proc_type.el != null)
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Action"));
                var t = new template_param_list();
                if (_modern_proc_type.aloneparam != null)
                    t.Add(_modern_proc_type.aloneparam);
                if (_modern_proc_type.el != null)
                {
                    var en = _modern_proc_type.el;
                    if (en.enumerators.Count == 1)
                        AddError(get_location(en.enumerators[0].name), "ONE_TYPE_PARAMETER_MUSTBE_WITHOUT_PARENTHESES");
                    for (int i = 0; i < en.enumerators.Count; i++)
                    {
                        if (en.enumerators[i].value != null)
                            AddError(get_location(en.enumerators[i].name), "ONE_TKIDENTIFIER");
                        t.Add(new named_type_reference(en.enumerators[i].name, en.enumerators[i].name.source_context));
                    }
                }
                t.source_context = _modern_proc_type.source_context;
                var ttr = new template_type_reference(new named_type_reference(l), t, _modern_proc_type.source_context);
                visit(ttr);
            }
            else
            {
                var l = new List<ident>();
                l.Add(new ident("System"));
                l.Add(new ident("Action"));
                var ntr = new named_type_reference(l);
                visit(ntr);
            }
        }
    }

}
