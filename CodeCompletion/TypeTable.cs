// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PascalABCCompiler.SyntaxTree;
using System.Reflection;
using PascalABCCompiler;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
	public static class TypeTable
	{
		public static CompiledScope int_type;
        public static CompiledScope real_type;
        public static CompiledScope string_type;
        public static CompiledScope bool_type;
        public static CompiledScope char_type;
        public static CompiledScope byte_type;
        public static CompiledScope float_type;
        public static CompiledScope sbyte_type;
        public static CompiledScope int16_type;
        public static CompiledScope int64_type;
        public static CompiledScope uint16_type;
        public static CompiledScope uint32_type;
        public static CompiledScope uint64_type;
        public static CompiledScope ptr_type;
        public static CompiledScope obj_type;
        public static CompiledScope void_type;
        private static Dictionary<Type, CompiledScope> type_cache = new Dictionary<Type, CompiledScope>();
        
        private static ProcScope int_plus;
        private static ProcScope int_minus;
        private static ProcScope int_mul;
        private static ProcScope int_div;
        private static ProcScope int_mod;
        private static ProcScope int_shl;
        private static ProcScope int_shr;
        private static ProcScope int_and;
        private static ProcScope int_or;
        private static ProcScope int_xor;
        private static ProcScope int_eq;
        private static ProcScope int_noteq;
        private static ProcScope int_sm;
        private static ProcScope int_smeq;
        private static ProcScope int_gr;
        private static ProcScope int_greq;
        
        private static ProcScope byte_and;
        private static ProcScope byte_or;
        private static ProcScope byte_xor;
        
        private static ProcScope sbyte_and;
        private static ProcScope sbyte_or;
        private static ProcScope sbyte_xor;
        
        private static ProcScope uint16_and;
        private static ProcScope uint16_or;
        private static ProcScope uint16_xor;
        
        private static ProcScope int16_and;
        private static ProcScope int16_or;
        private static ProcScope int16_xor;
        
        private static ProcScope uint32_and;
        private static ProcScope uint32_or;
        private static ProcScope uint32_xor;
        
        private static ProcScope int64_plus;
        private static ProcScope int64_minus;
        private static ProcScope int64_mul;
        private static ProcScope int64_div;
        private static ProcScope int64_mod;
        private static ProcScope int64_shl;
        private static ProcScope int64_shr;
        private static ProcScope int64_and;
        private static ProcScope int64_or;
        private static ProcScope int64_xor;
        private static ProcScope int64_eq;
        private static ProcScope int64_noteq;
        private static ProcScope int64_sm;
        private static ProcScope int64_smeq;
        private static ProcScope int64_gr;
        private static ProcScope int64_greq;
        
        private static ProcScope uint64_and;
        private static ProcScope uint64_or;
        private static ProcScope uint64_xor;
        
        private static ProcScope real_plus;
        private static ProcScope real_minus;
        private static ProcScope real_mul;
        private static ProcScope real_div;
        private static ProcScope real_eq;
        private static ProcScope real_noteq;
        private static ProcScope real_sm;
        private static ProcScope real_smeq;
        private static ProcScope real_gr;
        private static ProcScope real_greq;
        
        private static ProcScope bool_and;
        private static ProcScope bool_or;
        private static ProcScope bool_xor;
        private static ProcScope bool_eq;
        private static ProcScope bool_noteq;
        private static ProcScope bool_sm;
        private static ProcScope bool_smeq;
        private static ProcScope bool_gr;
        private static ProcScope bool_greq;
        
        private static ProcScope char_eq;
        private static ProcScope char_noteq;
        private static ProcScope char_sm;
        private static ProcScope char_smeq;
        private static ProcScope char_gr;
        private static ProcScope char_greq;
        private static ProcScope char_plus;

        private static ProcScope string_plus;
        private static ProcScope string_eq;
        private static ProcScope string_noteq;
        private static ProcScope string_sm;
        private static ProcScope string_smeq;
        private static ProcScope string_gr;
        private static ProcScope string_greq;
        
        public static void Clear()
        {
        	type_cache.Clear();
        }

        static TypeTable()
        {
            obj_type = get_compiled_type(new SymInfo(StringConstants.object_type_name, SymbolKind.Type, StringConstants.object_type_name), typeof(object));

            int_type = get_compiled_type(new SymInfo(StringConstants.integer_type_name, SymbolKind.Type, StringConstants.integer_type_name), typeof(int));


            real_type = get_compiled_type(new SymInfo(StringConstants.real_type_name, SymbolKind.Type, StringConstants.real_type_name), typeof(double));

            string_type = get_compiled_type(new SymInfo(StringConstants.string_type_name, SymbolKind.Class, StringConstants.string_type_name), typeof(string));

            char_type = get_compiled_type(new SymInfo(StringConstants.char_type_name, SymbolKind.Type, StringConstants.char_type_name), typeof(char));

            bool_type = get_compiled_type(new SymInfo(StringConstants.bool_type_name, SymbolKind.Type, StringConstants.bool_type_name), typeof(bool));

            byte_type = get_compiled_type(new SymInfo(StringConstants.byte_type_name, SymbolKind.Type, StringConstants.byte_type_name), typeof(byte));

            int16_type = get_compiled_type(new SymInfo(StringConstants.short_type_name, SymbolKind.Type, StringConstants.short_type_name), typeof(short));

            sbyte_type = get_compiled_type(new SymInfo(StringConstants.sbyte_type_name, SymbolKind.Type, StringConstants.sbyte_type_name), typeof(sbyte));

            uint16_type = get_compiled_type(new SymInfo(StringConstants.ushort_type_name, SymbolKind.Type, StringConstants.ushort_type_name), typeof(ushort));

            uint32_type = get_compiled_type(new SymInfo(StringConstants.uint_type_name, SymbolKind.Type, StringConstants.uint_type_name), typeof(uint));

            int64_type = get_compiled_type(new SymInfo(StringConstants.long_type_name, SymbolKind.Type, StringConstants.long_type_name), typeof(long));

            uint64_type = get_compiled_type(new SymInfo(StringConstants.ulong_type_name, SymbolKind.Type, StringConstants.ulong_type_name), typeof(ulong));

            float_type = get_compiled_type(new SymInfo(StringConstants.float_type_name, SymbolKind.Type, StringConstants.float_type_name), typeof(float));

            ptr_type = get_compiled_type(new SymInfo(StringConstants.pointer_type_name, SymbolKind.Type, StringConstants.pointer_type_name), Type.GetType("System.Void*"));

            void_type = get_compiled_type(new SymInfo("void", SymbolKind.Type, "void"), typeof(void));

            ProcScope ps = new ProcScope(StringConstants.plus_name, int_type);
            int_plus = ps;
            ElementScope left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ElementScope right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.plus_name, ps);

            ps = new ProcScope(StringConstants.minus_name, int_type);
            int_minus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.minus_name, ps);

            ps = new ProcScope(StringConstants.mul_name, int_type);
            int_mul = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.mul_name, ps);

            ps = new ProcScope(StringConstants.idiv_name, int_type);
            int_div = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.idiv_name, ps);

            ps = new ProcScope(StringConstants.mod_name, int_type);
            int_mod = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.mod_name, ps);

            ps = new ProcScope(StringConstants.shl_name, int_type);
            int_shl = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.shl_name, ps);

            ps = new ProcScope(StringConstants.shr_name, int_type);
            int_shr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.shr_name, ps);

            ps = new ProcScope(StringConstants.and_name, int_type);
            int_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, int_type);
            int_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, int_type);
            int_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = int_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.xor_name, ps);

            ps = new ProcScope(StringConstants.eq_name, int_type);
            int_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, int_type);
            int_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, int_type);
            int_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, int_type);
            int_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, int_type);
            int_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.greq_name, int_type);
            int_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int_type, int_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int_type.AddName(StringConstants.greq_name, ps);

            //byte type
            byte_type.AddName(StringConstants.plus_name, int_plus);
            byte_type.AddName(StringConstants.minus_name, int_minus);
            byte_type.AddName(StringConstants.mul_name, int_mul);
            byte_type.AddName(StringConstants.idiv_name, int_div);
            byte_type.AddName(StringConstants.mod_name, int_mod);
            byte_type.AddName(StringConstants.shl_name, int_shl);
            byte_type.AddName(StringConstants.shr_name, int_shr);
            byte_type.AddName(StringConstants.eq_name, int_eq);
            byte_type.AddName(StringConstants.noteq_name, int_noteq);
            byte_type.AddName(StringConstants.sm_name, int_sm);
            byte_type.AddName(StringConstants.smeq_name, int_smeq);
            byte_type.AddName(StringConstants.gr_name, int_gr);
            byte_type.AddName(StringConstants.greq_name, int_greq);
            ps = new ProcScope(StringConstants.and_name, byte_type);
            byte_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            ps.return_type = byte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            byte_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, byte_type);
            byte_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            ps.return_type = byte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            byte_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, byte_type);
            byte_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), byte_type, byte_type);
            ps.return_type = byte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            byte_type.AddName(StringConstants.xor_name, ps);

            //sbyte
            sbyte_type.AddName(StringConstants.plus_name, int_plus);
            sbyte_type.AddName(StringConstants.minus_name, int_minus);
            sbyte_type.AddName(StringConstants.mul_name, int_mul);
            sbyte_type.AddName(StringConstants.idiv_name, int_div);
            sbyte_type.AddName(StringConstants.mod_name, int_mod);
            sbyte_type.AddName(StringConstants.shl_name, int_shl);
            sbyte_type.AddName(StringConstants.shr_name, int_shr);
            sbyte_type.AddName(StringConstants.eq_name, int_eq);
            sbyte_type.AddName(StringConstants.noteq_name, int_noteq);
            sbyte_type.AddName(StringConstants.sm_name, int_sm);
            sbyte_type.AddName(StringConstants.smeq_name, int_smeq);
            sbyte_type.AddName(StringConstants.gr_name, int_gr);
            sbyte_type.AddName(StringConstants.greq_name, int_greq);
            ps = new ProcScope(StringConstants.and_name, sbyte_type);
            sbyte_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            ps.return_type = sbyte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            sbyte_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, sbyte_type);
            sbyte_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            ps.return_type = sbyte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            sbyte_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, sbyte_type);
            sbyte_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), sbyte_type, sbyte_type);
            ps.return_type = sbyte_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            sbyte_type.AddName(StringConstants.xor_name, ps);

            //ushort
            uint16_type.AddName(StringConstants.plus_name, int_plus);
            uint16_type.AddName(StringConstants.minus_name, int_minus);
            uint16_type.AddName(StringConstants.mul_name, int_mul);
            uint16_type.AddName(StringConstants.idiv_name, int_div);
            uint16_type.AddName(StringConstants.mod_name, int_mod);
            uint16_type.AddName(StringConstants.shl_name, int_shl);
            uint16_type.AddName(StringConstants.shr_name, int_shr);
            uint16_type.AddName(StringConstants.eq_name, int_eq);
            uint16_type.AddName(StringConstants.noteq_name, int_noteq);
            uint16_type.AddName(StringConstants.sm_name, int_sm);
            uint16_type.AddName(StringConstants.smeq_name, int_smeq);
            uint16_type.AddName(StringConstants.gr_name, int_gr);
            uint16_type.AddName(StringConstants.greq_name, int_greq);
            ps = new ProcScope(StringConstants.and_name, uint16_type);
            uint16_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            ps.return_type = uint16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint16_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, uint16_type);
            uint16_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            ps.return_type = uint16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint16_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, uint16_type);
            uint16_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint16_type, uint16_type);
            ps.return_type = uint16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint16_type.AddName(StringConstants.xor_name, ps);

            //int16
            int16_type.AddName(StringConstants.plus_name, int_plus);
            int16_type.AddName(StringConstants.minus_name, int_minus);
            int16_type.AddName(StringConstants.mul_name, int_mul);
            int16_type.AddName(StringConstants.idiv_name, int_div);
            int16_type.AddName(StringConstants.mod_name, int_mod);
            int16_type.AddName(StringConstants.shl_name, int_shl);
            int16_type.AddName(StringConstants.shr_name, int_shr);
            int16_type.AddName(StringConstants.eq_name, int_eq);
            int16_type.AddName(StringConstants.noteq_name, int_noteq);
            int16_type.AddName(StringConstants.sm_name, int_sm);
            int16_type.AddName(StringConstants.smeq_name, int_smeq);
            int16_type.AddName(StringConstants.gr_name, int_gr);
            int16_type.AddName(StringConstants.greq_name, int_greq);
            ps = new ProcScope(StringConstants.and_name, uint16_type);
            int16_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            ps.return_type = int16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int16_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, int16_type);
            int16_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            ps.return_type = int16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int16_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, int16_type);
            int16_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int16_type, int16_type);
            ps.return_type = int16_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int16_type.AddName(StringConstants.xor_name, ps);

            //int64
            ps = new ProcScope(StringConstants.plus_name, int64_type);
            int64_plus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.plus_name, ps);

            ps = new ProcScope(StringConstants.minus_name, int64_type);
            int64_minus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.minus_name, ps);

            ps = new ProcScope(StringConstants.mul_name, int64_type);
            int64_mul = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.mul_name, ps);

            ps = new ProcScope(StringConstants.idiv_name, int64_type);
            int64_div = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.idiv_name, ps);

            ps = new ProcScope(StringConstants.mod_name, int64_type);
            int64_mod = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.mod_name, ps);

            ps = new ProcScope(StringConstants.shl_name, int64_type);
            int64_shl = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.shl_name, ps);

            ps = new ProcScope(StringConstants.shr_name, int64_type);
            int64_shr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.shr_name, ps);

            ps = new ProcScope(StringConstants.and_name, int64_type);
            int64_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, int64_type);
            int64_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, int64_type);
            int64_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = int64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.xor_name, ps);

            ps = new ProcScope(StringConstants.eq_name, int64_type);
            int64_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, int64_type);
            int64_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, int64_type);
            int64_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, int64_type);
            int64_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, int64_type);
            int64_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.greq_name, int64_type);
            int64_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            int64_type.AddName(StringConstants.greq_name, ps);

            //uint
            //int16
            uint32_type.AddName(StringConstants.plus_name, int64_plus);
            uint32_type.AddName(StringConstants.minus_name, int64_minus);
            uint32_type.AddName(StringConstants.mul_name, int64_mul);
            uint32_type.AddName(StringConstants.idiv_name, int64_div);
            uint32_type.AddName(StringConstants.mod_name, int64_mod);
            uint32_type.AddName(StringConstants.shl_name, int64_shl);
            uint32_type.AddName(StringConstants.shr_name, int64_shr);
            uint32_type.AddName(StringConstants.eq_name, int64_eq);
            uint32_type.AddName(StringConstants.noteq_name, int64_noteq);
            uint32_type.AddName(StringConstants.sm_name, int64_sm);
            uint32_type.AddName(StringConstants.smeq_name, int64_smeq);
            uint32_type.AddName(StringConstants.gr_name, int64_gr);
            uint32_type.AddName(StringConstants.greq_name, int64_greq);
            ps = new ProcScope(StringConstants.and_name, uint32_type);
            uint32_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            ps.return_type = uint32_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint32_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, uint32_type);
            uint32_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            ps.return_type = uint32_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint32_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, uint32_type);
            uint32_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint32_type, uint32_type);
            ps.return_type = uint32_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint32_type.AddName(StringConstants.xor_name, ps);

            //uint64 type
            uint64_type.AddName(StringConstants.plus_name, int64_plus);
            uint64_type.AddName(StringConstants.minus_name, int64_minus);
            uint64_type.AddName(StringConstants.mul_name, int64_mul);
            uint64_type.AddName(StringConstants.idiv_name, int64_div);
            uint64_type.AddName(StringConstants.mod_name, int64_mod);
            uint64_type.AddName(StringConstants.shl_name, int64_shl);
            uint64_type.AddName(StringConstants.shr_name, int64_shr);
            uint64_type.AddName(StringConstants.eq_name, int64_eq);
            uint64_type.AddName(StringConstants.noteq_name, int64_noteq);
            uint64_type.AddName(StringConstants.sm_name, int64_sm);
            uint64_type.AddName(StringConstants.smeq_name, int64_smeq);
            uint64_type.AddName(StringConstants.gr_name, int64_gr);
            uint64_type.AddName(StringConstants.greq_name, int64_greq);
            ps = new ProcScope(StringConstants.and_name, uint64_type);
            uint64_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            ps.return_type = uint64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint64_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, uint64_type);
            uint64_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            ps.return_type = uint64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint64_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, uint64_type);
            uint64_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), uint64_type, uint64_type);
            ps.return_type = uint64_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            uint64_type.AddName(StringConstants.xor_name, ps);

            //real_type
            ps = new ProcScope(StringConstants.plus_name, real_type);
            real_plus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = real_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.plus_name, ps);

            ps = new ProcScope(StringConstants.minus_name, real_type);
            real_minus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = real_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.minus_name, ps);

            ps = new ProcScope(StringConstants.mul_name, real_type);
            real_mul = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = real_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.mul_name, ps);

            ps = new ProcScope(StringConstants.div_name, real_type);
            real_div = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = real_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.div_name, ps);

            ps = new ProcScope(StringConstants.eq_name, real_type);
            real_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = real_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, real_type);
            real_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, real_type);
            real_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, real_type);
            real_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, real_type);
            real_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), real_type, real_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.greq_name, int64_type);
            real_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), int64_type, int64_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            real_type.AddName(StringConstants.greq_name, ps);

            int_type.AddName(StringConstants.plus_name, int64_plus);
            int_type.AddName(StringConstants.minus_name, int64_minus);
            int_type.AddName(StringConstants.mul_name, int64_mul);
            int_type.AddName(StringConstants.idiv_name, int64_div);
            int_type.AddName(StringConstants.mod_name, int64_mod);
            int_type.AddName(StringConstants.shl_name, int64_shl);
            int_type.AddName(StringConstants.shr_name, int64_shr);
            int_type.AddName(StringConstants.eq_name, int64_eq);
            int_type.AddName(StringConstants.noteq_name, int64_noteq);
            int_type.AddName(StringConstants.sm_name, int64_sm);
            int_type.AddName(StringConstants.smeq_name, int64_smeq);
            int_type.AddName(StringConstants.gr_name, int64_gr);
            int_type.AddName(StringConstants.greq_name, int64_greq);

            int_type.AddName(StringConstants.plus_name, real_plus);
            int_type.AddName(StringConstants.minus_name, real_minus);
            int_type.AddName(StringConstants.mul_name, real_mul);
            int_type.AddName(StringConstants.div_name, real_div);
            int_type.AddName(StringConstants.eq_name, real_eq);
            int_type.AddName(StringConstants.noteq_name, real_noteq);
            int_type.AddName(StringConstants.sm_name, real_sm);
            int_type.AddName(StringConstants.smeq_name, real_smeq);
            int_type.AddName(StringConstants.gr_name, real_gr);
            int_type.AddName(StringConstants.greq_name, real_greq);

            int64_type.AddName(StringConstants.plus_name, real_plus);
            int64_type.AddName(StringConstants.minus_name, real_minus);
            int64_type.AddName(StringConstants.mul_name, real_mul);
            int64_type.AddName(StringConstants.div_name, real_div);
            int64_type.AddName(StringConstants.eq_name, real_eq);
            int64_type.AddName(StringConstants.noteq_name, real_noteq);
            int64_type.AddName(StringConstants.sm_name, real_sm);
            int64_type.AddName(StringConstants.smeq_name, real_smeq);
            int64_type.AddName(StringConstants.gr_name, real_gr);
            int64_type.AddName(StringConstants.greq_name, real_greq);

            //boolean type
            ps = new ProcScope(StringConstants.and_name, bool_type);
            bool_and = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.and_name, ps);

            ps = new ProcScope(StringConstants.or_name, bool_type);
            bool_or = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.or_name, ps);

            ps = new ProcScope(StringConstants.xor_name, bool_type);
            bool_xor = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.xor_name, ps);

            ps = new ProcScope(StringConstants.eq_name, bool_type);
            bool_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, bool_type);
            bool_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, bool_type);
            bool_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, bool_type);
            bool_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, bool_type);
            bool_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.greq_name, bool_type);
            bool_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), bool_type, bool_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            bool_type.AddName(StringConstants.greq_name, ps);

            //char type
            ps = new ProcScope(StringConstants.eq_name, char_type);
            char_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, char_type);
            char_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, char_type);
            char_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, char_type);
            char_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.greq_name, char_type);
            char_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.greq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, char_type);
            char_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.plus_name, char_type);
            char_plus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            ps.return_type = string_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.plus_name, ps);

            ps = new ProcScope(StringConstants.plus_name, char_type);
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), char_type, char_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, char_type);
            ps.return_type = string_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            char_type.AddName(StringConstants.plus_name, ps);

            //string type
            ps = new ProcScope(StringConstants.plus_name, string_type);
            string_plus = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = string_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.plus_name, ps);

            ps = new ProcScope(StringConstants.eq_name, string_type);
            string_eq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.eq_name, ps);

            ps = new ProcScope(StringConstants.noteq_name, string_type);
            string_noteq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.noteq_name, ps);

            ps = new ProcScope(StringConstants.sm_name, string_type);
            string_sm = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.sm_name, ps);

            ps = new ProcScope(StringConstants.smeq_name, string_type);
            string_smeq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.smeq_name, ps);

            ps = new ProcScope(StringConstants.gr_name, string_type);
            string_gr = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.gr_name, ps);

            ps = new ProcScope(StringConstants.greq_name, string_type);
            string_greq = ps;
            left = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            right = new ElementScope(new SymInfo("", SymbolKind.Parameter, ""), string_type, string_type);
            ps.return_type = bool_type;
            ps.AddParameter(left);
            ps.AddParameter(right);
            string_type.AddName(StringConstants.greq_name, ps);
        }

        public static CompiledScope get_compiled_type(Type t)
        {
            return get_compiled_type(null, t);
        }

        public static CompiledScope get_compiled_type(SymInfo si, Type t)
        {
            CompiledScope sc = null;
            if (type_cache.TryGetValue(t, out sc))
                return sc;
            if (si == null)
                si = new SymInfo(t.Name, SymbolKind.Type, t.FullName);
            sc = new CompiledScope(si, t);
        	type_cache[t] = sc;
        	return sc;
        }
        
       	public static TypeScope get_type(object o)
        {
        	if (o == null) return null;
        	switch (Type.GetTypeCode(o.GetType()))
        	{
        		case TypeCode.Boolean : return bool_type;
        		case TypeCode.Byte : return byte_type;
        		case TypeCode.Char : return char_type;
        		case TypeCode.Double : return real_type;
        		case TypeCode.Int32 : return int_type;
        		case TypeCode.String : return string_type;
        		case TypeCode.Int16 : return int16_type;
        		case TypeCode.Int64 : return int64_type;
        		case TypeCode.SByte : return sbyte_type;
        		case TypeCode.UInt16 : return uint16_type;
        		case TypeCode.UInt32 : return uint32_type;
        		case TypeCode.UInt64 : return uint64_type;
        		case TypeCode.Single : return float_type;
        	}
        	return null;
        }
	}
}
