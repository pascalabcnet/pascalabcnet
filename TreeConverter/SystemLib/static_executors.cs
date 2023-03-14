// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Интерпретатор константных выражений
using System;

using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.TreeConverter;

namespace PascalABCCompiler.SystemLibrary
{

    public static class static_executors
    {
        public static void init_compiled_type_executors()
        {
            //byte
            SystemLibrary.byte_to_sbyte.compile_time_executor = byte_to_sbyte_executor;
            SystemLibrary.byte_to_short.compile_time_executor = byte_to_short_executor;
            SystemLibrary.byte_to_ushort.compile_time_executor = byte_to_ushort_executor;
            SystemLibrary.byte_to_int_bfn.compile_time_executor = byte_to_int_executor;
            SystemLibrary.byte_to_uint.compile_time_executor = byte_to_uint_executor;
            SystemLibrary.byte_to_long.compile_time_executor = byte_to_long_executor;
            SystemLibrary.byte_to_ulong.compile_time_executor = byte_to_ulong_executor;
            SystemLibrary.byte_to_char.compile_time_executor = byte_to_char_executor;
            SystemLibrary.byte_to_float.compile_time_executor = byte_to_float_executor;
            SystemLibrary.byte_to_double.compile_time_executor = byte_to_double_executor;
            SystemLibrary.byte_unmin.compile_time_executor = byte_unmin_executor;
            SystemLibrary.byte_not.compile_time_executor = byte_not_executor;
            //SystemLibrary.byte_add.compile_time_executor = byte_add_executor;
            //SystemLibrary.byte_sub.compile_time_executor = byte_sub_executor;
            //SystemLibrary.byte_mul.compile_time_executor = byte_mul_executor;
            SystemLibrary.byte_div.compile_time_executor = byte_div_executor;
            //SystemLibrary.byte_mod.compile_time_executor = byte_mod_executor;
            //SystemLibrary.byte_idiv.compile_time_executor = byte_idiv_executor;
            //SystemLibrary.byte_gr.compile_time_executor = byte_gr_executor;
            //SystemLibrary.byte_greq.compile_time_executor = byte_greq_executor;
            //SystemLibrary.byte_sm.compile_time_executor = byte_sm_executor;
            //SystemLibrary.byte_smeq.compile_time_executor = byte_smeq_executor;
            //SystemLibrary.byte_eq.compile_time_executor = byte_eq_executor;
            //SystemLibrary.byte_noteq.compile_time_executor = byte_noteq_executor;
            //SystemLibrary.byte_and.compile_time_executor = byte_and_executor;
            //SystemLibrary.byte_or.compile_time_executor = byte_or_executor;
            //SystemLibrary.byte_xor.compile_time_executor = byte_xor_executor;
            //SystemLibrary.byte_shl.compile_time_executor = byte_shl_executor;
            //SystemLibrary.byte_shr.compile_time_executor = byte_shr_executor; 
            //sbyte
            SystemLibrary.sbyte_to_byte.compile_time_executor = sbyte_to_sbyte_executor;
            SystemLibrary.sbyte_to_short.compile_time_executor = sbyte_to_short_executor;
            SystemLibrary.sbyte_to_ushort.compile_time_executor = sbyte_to_ushort_executor;
            SystemLibrary.sbyte_to_int_bfn.compile_time_executor = sbyte_to_int_executor;
            SystemLibrary.sbyte_to_uint.compile_time_executor = sbyte_to_uint_executor;
            SystemLibrary.sbyte_to_long.compile_time_executor = sbyte_to_long_executor;
            SystemLibrary.sbyte_to_ulong.compile_time_executor = sbyte_to_ulong_executor;
            SystemLibrary.sbyte_to_char.compile_time_executor = sbyte_to_char_executor;
            SystemLibrary.sbyte_to_float.compile_time_executor = sbyte_to_float_executor;
            SystemLibrary.sbyte_to_double.compile_time_executor = sbyte_to_double_executor;
            SystemLibrary.sbyte_unmin.compile_time_executor = sbyte_unmin_executor;
            SystemLibrary.sbyte_not.compile_time_executor = sbyte_not_executor;
            /*
            SystemLibrary.sbyte_add.compile_time_executor = sbyte_add_executor;
            SystemLibrary.sbyte_sub.compile_time_executor = sbyte_sub_executor;
            SystemLibrary.sbyte_mul.compile_time_executor = sbyte_mul_executor;
            */
            SystemLibrary.sbyte_div.compile_time_executor = sbyte_div_executor;
            //SystemLibrary.sbyte_mod.compile_time_executor = sbyte_mod_executor;
            
            //SystemLibrary.sbyte_idiv.compile_time_executor = sbyte_idiv_executor;
            /*
            SystemLibrary.sbyte_gr.compile_time_executor = sbyte_gr_executor;
            SystemLibrary.sbyte_greq.compile_time_executor = sbyte_greq_executor;
            SystemLibrary.sbyte_sm.compile_time_executor = sbyte_sm_executor;
            SystemLibrary.sbyte_smeq.compile_time_executor = sbyte_smeq_executor;
            SystemLibrary.sbyte_eq.compile_time_executor = sbyte_eq_executor;
            SystemLibrary.sbyte_noteq.compile_time_executor = sbyte_noteq_executor;
            SystemLibrary.sbyte_and.compile_time_executor = sbyte_and_executor;
            SystemLibrary.sbyte_or.compile_time_executor = sbyte_or_executor;
            SystemLibrary.sbyte_xor.compile_time_executor = sbyte_xor_executor;
            */
            //SystemLibrary.sbyte_shl.compile_time_executor = sbyte_shl_executor;
            //SystemLibrary.sbyte_shr.compile_time_executor = sbyte_shr_executor;
            //short
            SystemLibrary.short_to_byte.compile_time_executor = short_to_byte_executor;
	        SystemLibrary.short_to_sbyte.compile_time_executor = short_to_sbyte_executor;
            SystemLibrary.short_to_ushort.compile_time_executor = short_to_ushort_executor;
            SystemLibrary.short_to_int_bfn.compile_time_executor = short_to_int_executor;
            SystemLibrary.short_to_uint.compile_time_executor = short_to_uint_executor;
            SystemLibrary.short_to_long.compile_time_executor = short_to_long_executor;
            SystemLibrary.short_to_ulong.compile_time_executor = short_to_ulong_executor;
            SystemLibrary.short_to_char.compile_time_executor = short_to_char_executor;
            SystemLibrary.short_to_float.compile_time_executor = short_to_float_executor;
            SystemLibrary.short_to_double.compile_time_executor = short_to_double_executor;
            SystemLibrary.short_unmin.compile_time_executor = short_unmin_executor;
            SystemLibrary.short_not.compile_time_executor = short_not_executor;
            /*
            SystemLibrary.short_add.compile_time_executor = short_add_executor;
            SystemLibrary.short_sub.compile_time_executor = short_sub_executor;
            SystemLibrary.short_mul.compile_time_executor = short_mul_executor;
            */
            SystemLibrary.short_div.compile_time_executor = short_div_executor;
            //SystemLibrary.short_mod.compile_time_executor = short_mod_executor;
            //SystemLibrary.short_idiv.compile_time_executor = short_idiv_executor;
            /*
            SystemLibrary.short_gr.compile_time_executor = short_gr_executor;
            SystemLibrary.short_greq.compile_time_executor = short_greq_executor;
            SystemLibrary.short_sm.compile_time_executor = short_sm_executor;
            SystemLibrary.short_smeq.compile_time_executor = short_smeq_executor;
            SystemLibrary.short_eq.compile_time_executor = short_eq_executor;
            SystemLibrary.short_noteq.compile_time_executor = short_noteq_executor;
            SystemLibrary.short_and.compile_time_executor = short_and_executor;
            SystemLibrary.short_or.compile_time_executor = short_or_executor;
            SystemLibrary.short_xor.compile_time_executor = short_xor_executor;
            */
            //SystemLibrary.short_shl.compile_time_executor = short_shl_executor;
            //SystemLibrary.short_shr.compile_time_executor = short_shr_executor;
            //ushort
            SystemLibrary.ushort_to_byte.compile_time_executor = ushort_to_byte_executor;
	        SystemLibrary.ushort_to_sbyte.compile_time_executor = ushort_to_sbyte_executor;
            SystemLibrary.ushort_to_short.compile_time_executor = ushort_to_short_executor;
            SystemLibrary.ushort_to_int_bfn.compile_time_executor = ushort_to_int_executor;
            SystemLibrary.ushort_to_uint.compile_time_executor = ushort_to_uint_executor;
            SystemLibrary.ushort_to_long.compile_time_executor = ushort_to_long_executor;
            SystemLibrary.ushort_to_ulong.compile_time_executor = ushort_to_ulong_executor;
            SystemLibrary.ushort_to_char.compile_time_executor = ushort_to_char_executor;
            SystemLibrary.ushort_to_float.compile_time_executor = ushort_to_float_executor;
            SystemLibrary.ushort_to_double.compile_time_executor = ushort_to_double_executor;
            SystemLibrary.ushort_unmin.compile_time_executor = ushort_unmin_executor;
            SystemLibrary.ushort_not.compile_time_executor = ushort_not_executor;
            /*
            SystemLibrary.ushort_add.compile_time_executor = ushort_add_executor;
            SystemLibrary.ushort_sub.compile_time_executor = ushort_sub_executor;
            SystemLibrary.ushort_mul.compile_time_executor = ushort_mul_executor;
            */
            SystemLibrary.ushort_div.compile_time_executor = ushort_div_executor;
            //SystemLibrary.ushort_mod.compile_time_executor = ushort_mod_executor;
            //SystemLibrary.ushort_idiv.compile_time_executor = ushort_idiv_executor;
            /*
            SystemLibrary.ushort_gr.compile_time_executor = ushort_gr_executor;
            SystemLibrary.ushort_greq.compile_time_executor = ushort_greq_executor;
            SystemLibrary.ushort_sm.compile_time_executor = ushort_sm_executor;
            SystemLibrary.ushort_smeq.compile_time_executor = ushort_smeq_executor;
            SystemLibrary.ushort_eq.compile_time_executor = ushort_eq_executor;
            SystemLibrary.ushort_noteq.compile_time_executor = ushort_noteq_executor;
            SystemLibrary.ushort_and.compile_time_executor = ushort_and_executor;
            SystemLibrary.ushort_or.compile_time_executor = ushort_or_executor;
            SystemLibrary.ushort_xor.compile_time_executor = ushort_xor_executor;
            */
            //SystemLibrary.ushort_shl.compile_time_executor = ushort_shl_executor;
            //SystemLibrary.ushort_shr.compile_time_executor = ushort_shr_executor;
            //int
            SystemLibrary.int_to_byte.compile_time_executor = int_to_byte_executor;
	        SystemLibrary.int_to_sbyte.compile_time_executor = int_to_sbyte_executor;
            SystemLibrary.int_to_short.compile_time_executor = int_to_short_executor;
            SystemLibrary.int_to_ushort.compile_time_executor = int_to_ushort_executor;
            SystemLibrary.int_to_uint.compile_time_executor = int_to_uint_executor;
            SystemLibrary.int_to_long.compile_time_executor = int_to_long_executor;
            SystemLibrary.int_to_ulong.compile_time_executor = int_to_ulong_executor;
            SystemLibrary.int_to_char.compile_time_executor = int_to_char_executor;
            SystemLibrary.int_to_float.compile_time_executor = int_to_float_executor;
            SystemLibrary.int_to_double.compile_time_executor = int_to_double_executor;
            SystemLibrary.int_unmin.compile_time_executor = int_unmin_executor;
            SystemLibrary.int_not.compile_time_executor = int_not_executor;
            SystemLibrary.int_add.compile_time_executor = int_add_executor;
            SystemLibrary.int_sub.compile_time_executor = int_sub_executor;
            SystemLibrary.int_mul.compile_time_executor = int_mul_executor;
            SystemLibrary.int_div.compile_time_executor = int_div_executor;
            SystemLibrary.int_mod.compile_time_executor = int_mod_executor;
            SystemLibrary.int_idiv.compile_time_executor = int_idiv_executor;
            SystemLibrary.int_gr.compile_time_executor = int_gr_executor;
            SystemLibrary.int_greq.compile_time_executor = int_greq_executor;
            SystemLibrary.int_sm.compile_time_executor = int_sm_executor;
            SystemLibrary.int_smeq.compile_time_executor = int_smeq_executor;
            SystemLibrary.int_eq.compile_time_executor = int_eq_executor;
            SystemLibrary.int_noteq.compile_time_executor = int_noteq_executor;
            SystemLibrary.int_and.compile_time_executor = int_and_executor;
            SystemLibrary.int_or.compile_time_executor = int_or_executor;
            SystemLibrary.int_xor.compile_time_executor = int_xor_executor;
            SystemLibrary.int_shl.compile_time_executor = int_shl_executor;
            SystemLibrary.int_shr.compile_time_executor = int_shr_executor;
            //uint
            SystemLibrary.uint_to_byte.compile_time_executor = uint_to_byte_executor;
            SystemLibrary.uint_to_sbyte.compile_time_executor = uint_to_sbyte_executor;
            SystemLibrary.uint_to_short.compile_time_executor = uint_to_short_executor;
            SystemLibrary.uint_to_ushort.compile_time_executor = uint_to_ushort_executor;
            SystemLibrary.uint_to_int_bfn.compile_time_executor = uint_to_int_executor;
            SystemLibrary.uint_to_long.compile_time_executor = uint_to_long_executor;
            SystemLibrary.uint_to_ulong.compile_time_executor = uint_to_ulong_executor;
            SystemLibrary.uint_to_char.compile_time_executor = uint_to_char_executor;
            SystemLibrary.uint_to_float.compile_time_executor = uint_to_float_executor;
            SystemLibrary.uint_to_double.compile_time_executor = uint_to_double_executor;
            SystemLibrary.uint_unmin.compile_time_executor = uint_unmin_executor;
            SystemLibrary.uint_not.compile_time_executor = uint_not_executor;
            SystemLibrary.uint_add.compile_time_executor = uint_add_executor;
            SystemLibrary.uint_sub.compile_time_executor = uint_sub_executor;
            SystemLibrary.uint_mul.compile_time_executor = uint_mul_executor;
            SystemLibrary.uint_div.compile_time_executor = uint_div_executor;
            SystemLibrary.uint_mod.compile_time_executor = uint_mod_executor;
            SystemLibrary.uint_idiv.compile_time_executor = uint_idiv_executor;
            SystemLibrary.uint_gr.compile_time_executor = uint_gr_executor;
            SystemLibrary.uint_greq.compile_time_executor = uint_greq_executor;
            SystemLibrary.uint_sm.compile_time_executor = uint_sm_executor;
            SystemLibrary.uint_smeq.compile_time_executor = uint_smeq_executor;
            SystemLibrary.uint_eq.compile_time_executor = uint_eq_executor;
            SystemLibrary.uint_noteq.compile_time_executor = uint_noteq_executor;
            SystemLibrary.uint_and.compile_time_executor = uint_and_executor;
            SystemLibrary.uint_or.compile_time_executor = uint_or_executor;
            SystemLibrary.uint_xor.compile_time_executor = uint_xor_executor;
            SystemLibrary.uint_shl.compile_time_executor = uint_shl_executor;
            SystemLibrary.uint_shr.compile_time_executor = uint_shr_executor;
            //long
            SystemLibrary.long_to_byte.compile_time_executor = long_to_byte_executor;
            SystemLibrary.long_to_sbyte.compile_time_executor = long_to_sbyte_executor;
            SystemLibrary.long_to_short.compile_time_executor = long_to_short_executor;
            SystemLibrary.long_to_ushort.compile_time_executor = long_to_ushort_executor;
            SystemLibrary.long_to_int_bfn.compile_time_executor = long_to_int_executor;
            SystemLibrary.long_to_uint.compile_time_executor = long_to_uint_executor;
            SystemLibrary.long_to_ulong.compile_time_executor = long_to_ulong_executor;
            SystemLibrary.long_to_char.compile_time_executor = long_to_char_executor;
            SystemLibrary.long_to_float.compile_time_executor = long_to_float_executor;
            SystemLibrary.long_to_double.compile_time_executor = long_to_double_executor;
            SystemLibrary.long_unmin.compile_time_executor = long_unmin_executor;
            SystemLibrary.long_not.compile_time_executor = long_not_executor;
            SystemLibrary.long_add.compile_time_executor = long_add_executor;
            SystemLibrary.long_sub.compile_time_executor = long_sub_executor;
            SystemLibrary.long_mul.compile_time_executor = long_mul_executor;
            SystemLibrary.long_div.compile_time_executor = long_div_executor;
            SystemLibrary.long_mod.compile_time_executor = long_mod_executor;
            SystemLibrary.long_idiv.compile_time_executor = long_idiv_executor;
            SystemLibrary.long_gr.compile_time_executor = long_gr_executor;
            SystemLibrary.long_greq.compile_time_executor = long_greq_executor;
            SystemLibrary.long_sm.compile_time_executor = long_sm_executor;
            SystemLibrary.long_smeq.compile_time_executor = long_smeq_executor;
            SystemLibrary.long_eq.compile_time_executor = long_eq_executor;
            SystemLibrary.long_noteq.compile_time_executor = long_noteq_executor;
            SystemLibrary.long_and.compile_time_executor = long_and_executor;
            SystemLibrary.long_or.compile_time_executor = long_or_executor;
            SystemLibrary.long_xor.compile_time_executor = long_xor_executor;
            SystemLibrary.long_shl.compile_time_executor = long_shl_executor;
            SystemLibrary.long_shr.compile_time_executor = long_shr_executor;
            //ulong
            SystemLibrary.ulong_to_byte.compile_time_executor = ulong_to_byte_executor;
            SystemLibrary.ulong_to_sbyte.compile_time_executor = ulong_to_sbyte_executor;
            SystemLibrary.ulong_to_short.compile_time_executor = ulong_to_short_executor;
            SystemLibrary.ulong_to_ushort.compile_time_executor = ulong_to_ushort_executor;
            SystemLibrary.ulong_to_int_bfn.compile_time_executor = ulong_to_int_executor;
            SystemLibrary.ulong_to_uint.compile_time_executor = ulong_to_uint_executor;
            SystemLibrary.ulong_to_long.compile_time_executor = ulong_to_long_executor;
            SystemLibrary.ulong_to_char.compile_time_executor = ulong_to_char_executor;
            SystemLibrary.ulong_to_float.compile_time_executor = ulong_to_float_executor;
            SystemLibrary.ulong_to_double.compile_time_executor = ulong_to_double_executor;
            SystemLibrary.ulong_unmin.compile_time_executor = ulong_unmin_executor;
            SystemLibrary.ulong_not.compile_time_executor = ulong_not_executor;
            SystemLibrary.ulong_add.compile_time_executor = ulong_add_executor;
            SystemLibrary.ulong_sub.compile_time_executor = ulong_sub_executor;
            SystemLibrary.ulong_mul.compile_time_executor = ulong_mul_executor;
            SystemLibrary.ulong_div.compile_time_executor = ulong_div_executor;
            SystemLibrary.ulong_mod.compile_time_executor = ulong_mod_executor;
            SystemLibrary.ulong_idiv.compile_time_executor = ulong_idiv_executor;
            SystemLibrary.ulong_gr.compile_time_executor = ulong_gr_executor;
            SystemLibrary.ulong_greq.compile_time_executor = ulong_greq_executor;
            SystemLibrary.ulong_sm.compile_time_executor = ulong_sm_executor;
            SystemLibrary.ulong_smeq.compile_time_executor = ulong_smeq_executor;
            SystemLibrary.ulong_eq.compile_time_executor = ulong_eq_executor;
            SystemLibrary.ulong_noteq.compile_time_executor = ulong_noteq_executor;
            SystemLibrary.ulong_and.compile_time_executor = ulong_and_executor;
            SystemLibrary.ulong_or.compile_time_executor = ulong_or_executor;
            SystemLibrary.ulong_xor.compile_time_executor = ulong_xor_executor;
            SystemLibrary.ulong_shl.compile_time_executor = ulong_shl_executor;
            SystemLibrary.ulong_shr.compile_time_executor = ulong_shr_executor;
            //real (double)
            //SystemLibrary.double_to_byte.compile_time_executor = double_to_byte_executor;
            //SystemLibrary.double_to_sbyte.compile_time_executor = double_to_sbyte_executor;
            //SystemLibrary.double_to_short.compile_time_executor = double_to_short_executor;
            //SystemLibrary.double_to_ushort.compile_time_executor = double_to_ushort_executor;
            //SystemLibrary.double_to_int_bfn.compile_time_executor = double_to_int_executor;
            //SystemLibrary.double_to_uint.compile_time_executor = double_to_uint_executor;
            //SystemLibrary.double_to_long.compile_time_executor = double_to_long_executor;
            //SystemLibrary.double_to_ulong.compile_time_executor = double_to_ulong_executor;
            //SystemLibrary.double_to_char.compile_time_executor = double_to_char_executor;
            SystemLibrary.double_to_float.compile_time_executor = double_to_float_executor;
            SystemLibrary.real_unmin.compile_time_executor = real_unmin_executor;
            SystemLibrary.real_add.compile_time_executor = real_add_executor;
            SystemLibrary.real_sub.compile_time_executor = real_sub_executor;
            SystemLibrary.real_mul.compile_time_executor = real_mul_executor;
            SystemLibrary.real_div.compile_time_executor = real_div_executor;
            SystemLibrary.real_gr.compile_time_executor = real_gr_executor;
            SystemLibrary.real_greq.compile_time_executor = real_greq_executor;
            SystemLibrary.real_sm.compile_time_executor = real_sm_executor;
            SystemLibrary.real_smeq.compile_time_executor = real_smeq_executor;
            SystemLibrary.real_eq.compile_time_executor = real_eq_executor;
            SystemLibrary.real_noteq.compile_time_executor = real_noteq_executor;
            //float
            //SystemLibrary.float_to_byte.compile_time_executor = float_to_byte_executor;
            //SystemLibrary.float_to_sbyte.compile_time_executor = float_to_sbyte_executor;
            //SystemLibrary.float_to_short.compile_time_executor = float_to_short_executor;
            //SystemLibrary.float_to_ushort.compile_time_executor = float_to_ushort_executor;
            //SystemLibrary.float_to_int_bfn.compile_time_executor = float_to_int_executor;
            //SystemLibrary.float_to_uint.compile_time_executor = float_to_uint_executor;
            //SystemLibrary.float_to_long.compile_time_executor = float_to_long_executor;
            //SystemLibrary.float_to_ulong.compile_time_executor = float_to_ulong_executor;
            //SystemLibrary.float_to_char.compile_time_executor = float_to_char_executor;
            SystemLibrary.float_to_double.compile_time_executor = float_to_double_executor;
            SystemLibrary.float_unmin.compile_time_executor = float_unmin_executor;
            SystemLibrary.float_add.compile_time_executor = float_add_executor;
            SystemLibrary.float_sub.compile_time_executor = float_sub_executor;
            SystemLibrary.float_mul.compile_time_executor = float_mul_executor;
            SystemLibrary.float_div.compile_time_executor = float_div_executor;
            SystemLibrary.float_gr.compile_time_executor = float_gr_executor;
            SystemLibrary.float_greq.compile_time_executor = float_greq_executor;
            SystemLibrary.float_sm.compile_time_executor = float_sm_executor;
            SystemLibrary.float_smeq.compile_time_executor = float_smeq_executor;
            SystemLibrary.float_eq.compile_time_executor = float_eq_executor;
            SystemLibrary.float_noteq.compile_time_executor = float_noteq_executor;
            //char
            SystemLibrary.char_to_string.compile_time_executor = char_to_string_executor;
            SystemLibrary.char_to_byte.compile_time_executor = char_to_byte_executor;
            SystemLibrary.char_to_sbyte.compile_time_executor = char_to_sbyte_executor;
            SystemLibrary.char_to_short.compile_time_executor = char_to_short_executor;
            SystemLibrary.char_to_ushort.compile_time_executor = char_to_ushort_executor;
            SystemLibrary.char_to_int_bfn.compile_time_executor = char_to_int_executor;
            SystemLibrary.char_to_uint.compile_time_executor = char_to_uint_executor;
            SystemLibrary.char_to_long.compile_time_executor = char_to_long_executor;
            SystemLibrary.char_to_ulong.compile_time_executor = char_to_ulong_executor;
            SystemLibrary.char_to_float.compile_time_executor = char_to_float_executor;
            SystemLibrary.char_to_double.compile_time_executor = char_to_double_executor;
            SystemLibrary.char_gr.compile_time_executor = char_gr_executor;
            SystemLibrary.char_greq.compile_time_executor = char_greq_executor;
            SystemLibrary.char_sm.compile_time_executor = char_sm_executor;
            SystemLibrary.char_smeq.compile_time_executor = char_smeq_executor;
            SystemLibrary.char_eq.compile_time_executor = char_eq_executor;
            SystemLibrary.char_noteq.compile_time_executor = char_noteq_executor;
            //bool
            SystemLibrary.bool_not.compile_time_executor = bool_not_executor;
            SystemLibrary.bool_and.compile_time_executor = bool_and_executor;
            SystemLibrary.bool_or.compile_time_executor = bool_or_executor;
            SystemLibrary.bool_xor.compile_time_executor = bool_xor_executor;
            SystemLibrary.bool_gr.compile_time_executor = bool_gr_executor;
            SystemLibrary.bool_greq.compile_time_executor = bool_greq_executor;
            SystemLibrary.bool_sm.compile_time_executor = bool_sm_executor;
            SystemLibrary.bool_smeq.compile_time_executor = bool_smeq_executor;
            SystemLibrary.bool_eq.compile_time_executor = bool_eq_executor;
            SystemLibrary.bool_noteq.compile_time_executor = bool_noteq_executor;

            //string
            SystemLibrary.string_add.compile_time_executor = string_add_executor;
            SystemLibrary.char_add.compile_time_executor = char_add_executor;

            //+= -= *= /=
            SystemLibrary._byte_plusassign.compile_time_executor = byte_plusassign_executor;
            SystemLibrary._byte_minusassign.compile_time_executor = byte_minusassign_executor;
            SystemLibrary._byte_multassign.compile_time_executor = byte_multassign_executor;
            SystemLibrary._byte_divassign.compile_time_executor = byte_divassign_executor;

            SystemLibrary._sbyte_plusassign.compile_time_executor = sbyte_plusassign_executor;
            SystemLibrary._sbyte_minusassign.compile_time_executor = sbyte_minusassign_executor;
            SystemLibrary._sbyte_multassign.compile_time_executor = sbyte_multassign_executor;
            SystemLibrary._sbyte_divassign.compile_time_executor = sbyte_divassign_executor;

            SystemLibrary._short_plusassign.compile_time_executor = short_plusassign_executor;
            SystemLibrary._short_minusassign.compile_time_executor = short_minusassign_executor;
            SystemLibrary._short_multassign.compile_time_executor = short_multassign_executor;
            SystemLibrary._short_divassign.compile_time_executor = short_divassign_executor;

            SystemLibrary._ushort_plusassign.compile_time_executor = ushort_plusassign_executor;
            SystemLibrary._ushort_minusassign.compile_time_executor = ushort_minusassign_executor;
            SystemLibrary._ushort_multassign.compile_time_executor = ushort_multassign_executor;
            SystemLibrary._ushort_divassign.compile_time_executor = ushort_divassign_executor;

            SystemLibrary._int_plusassign.compile_time_executor = int_plusassign_executor;
            SystemLibrary._int_minusassign.compile_time_executor = int_minusassign_executor;
            SystemLibrary._int_multassign.compile_time_executor = int_multassign_executor;
            SystemLibrary._int_divassign.compile_time_executor = int_divassign_executor;
            
            SystemLibrary._uint_plusassign.compile_time_executor = uint_plusassign_executor;
            SystemLibrary._uint_minusassign.compile_time_executor = uint_minusassign_executor;
            SystemLibrary._uint_multassign.compile_time_executor = uint_multassign_executor;
            SystemLibrary._uint_divassign.compile_time_executor = uint_divassign_executor;

            SystemLibrary._long_plusassign.compile_time_executor = long_plusassign_executor;
            SystemLibrary._long_minusassign.compile_time_executor = long_minusassign_executor;
            SystemLibrary._long_multassign.compile_time_executor = long_multassign_executor;
            SystemLibrary._long_divassign.compile_time_executor = long_divassign_executor;
            
            SystemLibrary._ulong_plusassign.compile_time_executor = ulong_plusassign_executor;
            SystemLibrary._ulong_minusassign.compile_time_executor = ulong_minusassign_executor;
            SystemLibrary._ulong_multassign.compile_time_executor = ulong_multassign_executor;
            SystemLibrary._ulong_divassign.compile_time_executor = ulong_divassign_executor;

            SystemLibrary._float_plusassign.compile_time_executor = float_plusassign_executor;
            SystemLibrary._float_minusassign.compile_time_executor = float_minusassign_executor;
            SystemLibrary._float_multassign.compile_time_executor = float_multassign_executor;
            SystemLibrary._float_divassign.compile_time_executor = float_divassign_executor;

            SystemLibrary._double_plusassign.compile_time_executor = double_plusassign_executor;
            SystemLibrary._double_minusassign.compile_time_executor = double_minusassign_executor;
            SystemLibrary._double_multassign.compile_time_executor = double_multassign_executor;
            SystemLibrary._double_divassign.compile_time_executor = double_divassign_executor;
        }

        private static expression_node inline_assign_operator(function_node assign_operator,function_node assign, function_node operation, location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseExtendedAssignmentOperatorsForPrimitiveTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(assign_operator.name, parameters[0]));
            basic_function_call operationc = new basic_function_call((basic_function_node)operation, call_location);
            operationc.parameters.AddElement(parameters[0]);
            operationc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(parameters[1], parameters[0].type));
            basic_function_call assignc = new basic_function_call((basic_function_node)assign, call_location);
            assignc.parameters.AddElement(parameters[0]);
            assignc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(operationc, parameters[0].type));
            return assignc;
        }
        
        public static expression_node set_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            //return inline_assign_operator(SystemLibrary._byte_plusassign, SystemLibrary.byte_assign, SystemLibrary.int_add, call_location, parameters);
        	if (!SemanticRules.UseExtendedAssignmentOperatorsForPrimitiveTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(compiler_string_consts.plusassign_name, parameters[0]));
        	//basic_function_call operationc = new basic_function_call((basic_function_node)operation, call_location);
            base_function_call cnfc = null;
            if (SystemLibInitializer.SetUnionProcedure.sym_info is common_namespace_function_node)
                cnfc = new common_namespace_function_call(SystemLibInitializer.SetUnionProcedure.sym_info as common_namespace_function_node, call_location);
        	else
                cnfc = new compiled_static_method_call(SystemLibInitializer.SetUnionProcedure.sym_info as compiled_function_node, call_location);
            cnfc.parameters.AddElement(parameters[0]);
        	cnfc.parameters.AddElement(parameters[1]);
            //operationc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(parameters[1], parameters[0].type));
            basic_function_call assignc = new basic_function_call(parameters[0].type.find_first_in_type(compiler_string_consts.assign_name).sym_info as basic_function_node, call_location);
            assignc.parameters.AddElement(parameters[0]);
            assignc.parameters.AddElement(cnfc);
            //assignc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(operationc, parameters[0].type));
            return assignc;
        }
        
        public static expression_node short_string_addassign_executor(location call_location, params expression_node[] parameters)
        {
        	if (!SemanticRules.UseExtendedAssignmentOperatorsForPrimitiveTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(compiler_string_consts.plusassign_name, parameters[0]));
        	compiled_static_method_call csmc = new compiled_static_method_call(SystemLibrary.string_add as compiled_function_node, call_location);
        	csmc.parameters.AddElement(parameters[0]);
        	csmc.parameters.AddElement(parameters[1]);
        	basic_function_call assignc = new basic_function_call(parameters[0].type.find_first_in_type(compiler_string_consts.assign_name).sym_info as basic_function_node, call_location);
        	assignc.parameters.AddElement(parameters[0]);
            base_function_call cnfc = null;
            if (SystemLibInitializer.ClipShortStringProcedure.sym_info is common_namespace_function_node)
                cnfc = new common_namespace_function_call(SystemLibInitializer.ClipShortStringProcedure.sym_info as common_namespace_function_node, call_location);
        	else
                cnfc = new compiled_static_method_call(SystemLibInitializer.ClipShortStringProcedure.sym_info as compiled_function_node, call_location);
            cnfc.parameters.AddElement(csmc);
        	cnfc.parameters.AddElement(new int_const_node((parameters[0].type as short_string_type_node).Length,call_location));
        	assignc.parameters.AddElement(cnfc);
            return assignc;
        }
        
        public static expression_node set_subassign_executor(location call_location, params expression_node[] parameters)
        {
            //return inline_assign_operator(SystemLibrary._byte_plusassign, SystemLibrary.byte_assign, SystemLibrary.int_add, call_location, parameters);
        	if (!SemanticRules.UseExtendedAssignmentOperatorsForPrimitiveTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(compiler_string_consts.plusassign_name, parameters[0]));
        	//basic_function_call operationc = new basic_function_call((basic_function_node)operation, call_location);
            base_function_call cnfc = null;
            if (SystemLibInitializer.SetSubtractProcedure.sym_info is common_namespace_function_node)
                cnfc = new common_namespace_function_call(SystemLibInitializer.SetSubtractProcedure.sym_info as common_namespace_function_node, call_location);
        	else
                cnfc = new compiled_static_method_call(SystemLibInitializer.SetSubtractProcedure.sym_info as compiled_function_node, call_location);
            cnfc.parameters.AddElement(parameters[0]);
        	cnfc.parameters.AddElement(parameters[1]);
            //operationc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(parameters[1], parameters[0].type));
            basic_function_call assignc = new basic_function_call(parameters[0].type.find_first_in_type(compiler_string_consts.assign_name).sym_info as basic_function_node, call_location);
            assignc.parameters.AddElement(parameters[0]);
            assignc.parameters.AddElement(cnfc);
            //assignc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(operationc, parameters[0].type));
            return assignc;
        }
        
        public static expression_node set_multassign_executor(location call_location, params expression_node[] parameters)
        {
            //return inline_assign_operator(SystemLibrary._byte_plusassign, SystemLibrary.byte_assign, SystemLibrary.int_add, call_location, parameters);
        	if (!SemanticRules.UseExtendedAssignmentOperatorsForPrimitiveTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(compiler_string_consts.plusassign_name, parameters[0]));
        	//basic_function_call operationc = new basic_function_call((basic_function_node)operation, call_location);
            base_function_call cnfc = null;
            if (SystemLibInitializer.SetIntersectProcedure.sym_info is common_namespace_function_node)
                cnfc = new common_namespace_function_call(SystemLibInitializer.SetIntersectProcedure.sym_info as common_namespace_function_node, call_location);
        	else
                cnfc = new compiled_static_method_call(SystemLibInitializer.SetIntersectProcedure.sym_info as compiled_function_node, call_location);
            cnfc.parameters.AddElement(parameters[0]);
        	cnfc.parameters.AddElement(parameters[1]);
            //operationc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(parameters[1], parameters[0].type));
            basic_function_call assignc = new basic_function_call(parameters[0].type.find_first_in_type(compiler_string_consts.assign_name).sym_info as basic_function_node, call_location);
            assignc.parameters.AddElement(parameters[0]);
            assignc.parameters.AddElement(cnfc);
            //assignc.parameters.AddElement(SystemLibrary.syn_visitor.convertion_data_and_alghoritms.convert_type(operationc, parameters[0].type));
            return assignc;
        }
        
        //byte
        private static expression_node byte_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._byte_plusassign, SystemLibrary.byte_assign, SystemLibrary.int_add, call_location, parameters);
        }
        private static expression_node byte_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._byte_minusassign, SystemLibrary.byte_assign, SystemLibrary.int_sub, call_location, parameters);
        }
        private static expression_node byte_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._byte_multassign, SystemLibrary.byte_assign, SystemLibrary.int_mul, call_location, parameters);
        }
        private static expression_node byte_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._byte_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._byte_divassign, SystemLibrary.byte_assign, SystemLibrary.int_idiv, call_location, parameters);
        }
        //sbyte
        private static expression_node sbyte_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._sbyte_plusassign, SystemLibrary.sbyte_assign, SystemLibrary.int_add, call_location, parameters);
        }
        private static expression_node sbyte_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._sbyte_minusassign, SystemLibrary.sbyte_assign, SystemLibrary.int_sub, call_location, parameters);
        }
        private static expression_node sbyte_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._sbyte_multassign, SystemLibrary.sbyte_assign, SystemLibrary.int_mul, call_location, parameters);
        }
        private static expression_node sbyte_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._sbyte_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._sbyte_divassign, SystemLibrary.sbyte_assign, SystemLibrary.int_idiv, call_location, parameters);
        }
        //short
        private static expression_node short_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._short_plusassign, SystemLibrary.short_assign, SystemLibrary.int_add, call_location, parameters);
        }
        private static expression_node short_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._short_minusassign, SystemLibrary.short_assign, SystemLibrary.int_sub, call_location, parameters);
        }
        private static expression_node short_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._short_multassign, SystemLibrary.short_assign, SystemLibrary.int_mul, call_location, parameters);
        }
        private static expression_node short_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._short_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._short_divassign, SystemLibrary.short_assign, SystemLibrary.int_idiv, call_location, parameters);
        }
        //ushort
        private static expression_node ushort_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ushort_plusassign, SystemLibrary.ushort_assign, SystemLibrary.int_add, call_location, parameters);
        }
        private static expression_node ushort_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ushort_minusassign, SystemLibrary.ushort_assign, SystemLibrary.int_sub, call_location, parameters);
        }
        private static expression_node ushort_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ushort_multassign, SystemLibrary.ushort_assign, SystemLibrary.int_mul, call_location, parameters);
        }
        private static expression_node ushort_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._ushort_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._ushort_divassign, SystemLibrary.ushort_assign, SystemLibrary.int_idiv, call_location, parameters);
        }
        //int
        private static expression_node int_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._int_plusassign, SystemLibrary.int_assign, SystemLibrary.int_add, call_location, parameters);
        }
        private static expression_node int_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._int_minusassign, SystemLibrary.int_assign, SystemLibrary.int_sub, call_location, parameters);
        }
        private static expression_node int_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._int_multassign, SystemLibrary.int_assign, SystemLibrary.int_mul, call_location, parameters);
        }
        private static expression_node int_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._int_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._int_divassign, SystemLibrary.int_assign, SystemLibrary.int_idiv, call_location, parameters);
        }
        //uint
        private static expression_node uint_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._uint_plusassign, SystemLibrary.uint_assign, SystemLibrary.uint_add, call_location, parameters);
        }
        private static expression_node uint_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._uint_minusassign, SystemLibrary.uint_assign, SystemLibrary.uint_sub, call_location, parameters);
        }
        private static expression_node uint_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._uint_multassign, SystemLibrary.uint_assign, SystemLibrary.uint_mul, call_location, parameters);
        }
        private static expression_node uint_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._uint_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._uint_divassign, SystemLibrary.uint_assign, SystemLibrary.uint_idiv, call_location, parameters);
        }
        //long
        private static expression_node long_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._long_plusassign, SystemLibrary.long_assign, SystemLibrary.long_add, call_location, parameters);
        }
        private static expression_node long_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._long_minusassign, SystemLibrary.long_assign, SystemLibrary.long_sub, call_location, parameters);
        }
        private static expression_node long_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._long_multassign, SystemLibrary.long_assign, SystemLibrary.long_mul, call_location, parameters);
        }
        private static expression_node long_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._long_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._long_divassign, SystemLibrary.long_assign, SystemLibrary.long_idiv, call_location, parameters);
        }
        //ulong
        private static expression_node ulong_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ulong_plusassign, SystemLibrary.ulong_assign, SystemLibrary.ulong_add, call_location, parameters);
        }
        private static expression_node ulong_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ulong_minusassign, SystemLibrary.ulong_assign, SystemLibrary.ulong_sub, call_location, parameters);
        }
        private static expression_node ulong_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._ulong_multassign, SystemLibrary.ulong_assign, SystemLibrary.ulong_mul, call_location, parameters);
        }
        private static expression_node ulong_divassign_executor(location call_location, params expression_node[] parameters)
        {
            if (!SemanticRules.UseDivisionAssignmentOperatorsForIntegerTypes)
                SystemLibrary.syn_visitor.AddError(new OperatorCanNotBeAppliedToThisType(SystemLibrary._ulong_divassign.name, parameters[0]));
            return inline_assign_operator(SystemLibrary._ulong_divassign, SystemLibrary.ulong_assign, SystemLibrary.ulong_idiv, call_location, parameters);
        }
        //float
        private static expression_node float_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._float_plusassign, SystemLibrary.float_assign, SystemLibrary.float_add, call_location, parameters);
        }
        private static expression_node float_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._float_minusassign, SystemLibrary.float_assign, SystemLibrary.float_sub, call_location, parameters);
        }
        private static expression_node float_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._float_multassign, SystemLibrary.float_assign, SystemLibrary.float_mul, call_location, parameters);
        }
        private static expression_node float_divassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._float_divassign, SystemLibrary.float_assign, SystemLibrary.float_div, call_location, parameters);
        }
        //double
        private static expression_node double_plusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._double_plusassign, SystemLibrary.real_assign, SystemLibrary.real_add, call_location, parameters);
        }
        private static expression_node double_minusassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._double_minusassign, SystemLibrary.real_assign, SystemLibrary.real_sub, call_location, parameters);
        }
        private static expression_node double_multassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._double_multassign, SystemLibrary.real_assign, SystemLibrary.real_mul, call_location, parameters);
        }
        private static expression_node double_divassign_executor(location call_location, params expression_node[] parameters)
        {
            return inline_assign_operator(SystemLibrary._double_divassign, SystemLibrary.real_assign, SystemLibrary.real_div, call_location, parameters);
        }



        private static byte check_byte_overflow(location loc, int value)
        {
            if (value >= byte.MinValue && value <= byte.MaxValue)
                return (byte)value;
            else
                throw new SimpleSemanticError(loc, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
        }

        private static int check_int_overflow(location loc,int value)
        {
            if (value >= int.MinValue && value <= int.MaxValue)
                return (int)value;
            else
                throw new SimpleSemanticError(loc, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
        }

        private static long check_long_overflow(location loc,long value)
        {
            if (value >= long.MinValue && value <= long.MaxValue)
                return (long)value;
            else
                throw new SimpleSemanticError(loc, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
        }

        public static expression_node make_int_const(int value,location loc)
        {
            /*
            if ((value <= byte.MaxValue) && (value >= byte.MinValue))
            {
                return new byte_const_node((byte)value, loc);
            }
            if ((value <= sbyte.MaxValue) && (value >= sbyte.MinValue))
            {
                return new sbyte_const_node((sbyte)value, loc);
            }
            if ((value <= short.MaxValue) && (value >= short.MinValue))
            {
                return new short_const_node((short)value, loc);
            }
            if ((value <= ushort.MaxValue) && (value >= ushort.MinValue))
            {
                return new ushort_const_node((ushort)value, loc);
            }
            */
            return new int_const_node(value, loc);
        }

        private static expression_node byte_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value,call_location);
        }

        private static expression_node byte_to_short_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_int_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_long_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_char_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_float_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node byte_to_double_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node bcn = parameters[0] as byte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        //byte
        private static expression_node byte_add_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value + right.constant_value), call_location);
        }

        private static expression_node byte_sub_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value - right.constant_value), call_location);
        }

        private static expression_node byte_mul_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value * right.constant_value), call_location);
        }

        private static expression_node byte_mod_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value % right.constant_value), call_location);
        }

        private static expression_node byte_idiv_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((left.constant_value / right.constant_value), call_location);
        }

        private static expression_node byte_div_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node( ((double)left.constant_value / (double)right.constant_value), call_location));
        }

        private static expression_node byte_unmin_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((-(left.constant_value)), call_location);
        }

        private static expression_node byte_not_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            //TODO: Проверить опреатор ~. Это опреатор двоичного дополнения. 
            //Его оставлять в byte с проверкой или уводить в int????
            return make_int_const((~(left.constant_value)), call_location);
        }

        private static expression_node byte_gr_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node byte_greq_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node byte_sm_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node byte_smeq_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node byte_eq_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node byte_noteq_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node byte_and_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) & (right.constant_value), call_location);
        }

        private static expression_node byte_or_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) | (right.constant_value), call_location);
        }

        private static expression_node byte_xor_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            byte_const_node right = parameters[1] as byte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const(left.constant_value ^ right.constant_value, call_location);
        }
        /*
        private static expression_node byte_shl_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value << right.constant_value), call_location);
        }

        private static expression_node byte_shr_executor(location call_location, params expression_node[] parameters)
        {
            byte_const_node left = parameters[0] as byte_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value >> right.constant_value), call_location);
        }
        */



        //sbyte
        private static expression_node sbyte_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value,call_location);
        }

	private static expression_node sbyte_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value,call_location);
        }

        private static expression_node sbyte_to_short_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_int_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_long_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_char_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_float_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_to_double_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node bcn = parameters[0] as sbyte_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node sbyte_add_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value + right.constant_value), call_location);
        }

        private static expression_node sbyte_sub_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value - right.constant_value), call_location);
        }

        private static expression_node sbyte_mul_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value * right.constant_value), call_location);
        }

        private static expression_node sbyte_mod_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value % right.constant_value), call_location);
        }

        private static expression_node sbyte_idiv_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((left.constant_value / right.constant_value), call_location);
        }

        private static expression_node sbyte_div_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node( ((double)left.constant_value / (double)right.constant_value), call_location));
        }

        private static expression_node sbyte_unmin_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((-(left.constant_value)), call_location);
        }

        private static expression_node sbyte_not_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((~(left.constant_value)), call_location);
        }

        private static expression_node sbyte_gr_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node sbyte_greq_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node sbyte_sm_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node sbyte_smeq_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node sbyte_eq_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node sbyte_noteq_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node sbyte_and_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) & (right.constant_value), call_location);
        }

        private static expression_node sbyte_or_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) | (right.constant_value), call_location);
        }

        private static expression_node sbyte_xor_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            sbyte_const_node right = parameters[1] as sbyte_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const(left.constant_value ^ right.constant_value, call_location);
        }
        /*
        private static expression_node sbyte_shl_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value << right.constant_value), call_location);
        }

        private static expression_node sbyte_shr_executor(location call_location, params expression_node[] parameters)
        {
            sbyte_const_node left = parameters[0] as sbyte_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value >> right.constant_value), call_location);
        }
        */


        //short
        private static expression_node short_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value,call_location);
        }

	    private static expression_node short_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value,call_location);
        }

        private static expression_node short_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node short_to_int_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node short_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node short_to_long_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node short_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node short_to_char_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node short_to_float_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node short_to_double_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node bcn = parameters[0] as short_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node short_add_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value + right.constant_value), call_location);
        }

        private static expression_node short_sub_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value - right.constant_value), call_location);
        }

        private static expression_node short_mul_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value * right.constant_value), call_location);
        }

        private static expression_node short_mod_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value % right.constant_value), call_location);
        }

        private static expression_node short_idiv_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((left.constant_value / right.constant_value), call_location);
        }

        private static expression_node short_div_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node( ((double)left.constant_value / (double)right.constant_value), call_location));
        }

        private static expression_node short_unmin_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((-(left.constant_value)), call_location);
        }

        private static expression_node short_not_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((~(left.constant_value)), call_location);
        }

        private static expression_node short_gr_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node short_greq_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node short_sm_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node short_smeq_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node short_eq_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node short_noteq_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node short_and_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) & (right.constant_value), call_location);
        }

        private static expression_node short_or_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) | (right.constant_value), call_location);
        }

        private static expression_node short_xor_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const(left.constant_value ^ right.constant_value, call_location);
        }

        /*
        private static expression_node short_shl_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value << right.constant_value), call_location);
        }

        private static expression_node short_shr_executor(location call_location, params expression_node[] parameters)
        {
            short_const_node left = parameters[0] as short_const_node;
            if (left == null)
            {
                return null;
            }
            short_const_node right = parameters[1] as short_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value >> right.constant_value), call_location);
        }
        */


        //ushort
        	private static expression_node ushort_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value,call_location);
        }

	private static expression_node ushort_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value,call_location);
        }

        private static expression_node ushort_to_short_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_int_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_long_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_char_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_float_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node ushort_to_double_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node bcn = parameters[0] as ushort_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node ushort_add_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value + right.constant_value), call_location);
        }

        private static expression_node ushort_sub_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value - right.constant_value), call_location);
        }

        private static expression_node ushort_mul_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value * right.constant_value), call_location);
        }

        private static expression_node ushort_mod_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value % right.constant_value), call_location);
        }

        private static expression_node ushort_idiv_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((left.constant_value / right.constant_value), call_location);
        }

        private static expression_node ushort_div_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node( ((double)left.constant_value / (double)right.constant_value), call_location));
        }

        private static expression_node ushort_unmin_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((-(left.constant_value)), call_location);
        }

        private static expression_node ushort_not_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            return make_int_const((~(left.constant_value)), call_location);
        }

        private static expression_node ushort_gr_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node ushort_greq_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node ushort_sm_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node ushort_smeq_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node ushort_eq_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node ushort_noteq_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node ushort_and_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) & (right.constant_value), call_location);
        }

        private static expression_node ushort_or_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value) | (right.constant_value), call_location);
        }

        private static expression_node ushort_xor_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const(left.constant_value ^ right.constant_value, call_location);
        }
        /*
        private static expression_node ushort_shl_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value << right.constant_value), call_location);
        }

        private static expression_node ushort_shr_executor(location call_location, params expression_node[] parameters)
        {
            ushort_const_node left = parameters[0] as ushort_const_node;
            if (left == null)
            {
                return null;
            }
            ushort_const_node right = parameters[1] as ushort_const_node;
            if (right == null)
            {
                return null;
            }
            return make_int_const((left.constant_value >> right.constant_value), call_location);
        }
        */



        //int
        private static expression_node int_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node int_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node int_to_short_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node int_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node int_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node int_to_long_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node int_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node int_to_char_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node int_to_float_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node int_to_double_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node bcn = parameters[0] as int_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node int_add_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            int result=0;
            try
            {
                checked
                {
                    result = (left.constant_value + right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_sub_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            int result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value - right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_mul_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            int result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value * right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_mod_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            int result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value % right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            catch (System.DivideByZeroException)
            {
                throw new SimpleSemanticError(right.location, "DIVIDE_BY_ZERO_EXCEPTION");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_idiv_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value / right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_div_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((left.constant_value / right.constant_value), call_location));
        }

        private static expression_node int_unmin_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int result = 0;
            try
            {
                checked
                {
                    result = -left.constant_value;
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (make_int_const(result, call_location));
        }

        private static expression_node int_not_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            //TODO: Проверить опреатор ~. Это опреатор двоичного дополнения.
            return (make_int_const((~(left.constant_value)), call_location));
        }

        private static expression_node int_gr_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node int_greq_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node int_sm_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node int_smeq_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node int_eq_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node int_noteq_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node int_and_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (make_int_const(((left.constant_value) & (right.constant_value)), call_location));
        }

        private static expression_node int_or_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (make_int_const(((left.constant_value) | (right.constant_value)), call_location));
        }

        private static expression_node int_xor_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (make_int_const((left.constant_value ^ right.constant_value), call_location));
        }

        private static expression_node int_shl_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (make_int_const((left.constant_value << right.constant_value), call_location));
        }

        private static expression_node int_shr_executor(location call_location, params expression_node[] parameters)
        {
            int_const_node left = parameters[0] as int_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (make_int_const((left.constant_value >> right.constant_value), call_location));
        }




        //uint
        private static expression_node uint_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_short_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_int_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_long_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_char_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_float_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node uint_to_double_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node bcn = parameters[0] as uint_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node uint_add_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            uint result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value + right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new uint_const_node(result, call_location));
        }

        private static expression_node uint_sub_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            uint result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value - right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new uint_const_node(result, call_location));
        }

        private static expression_node uint_mul_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            uint result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value * right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new uint_const_node(result, call_location));

        }

        private static expression_node uint_mod_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            uint result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value % right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new uint_const_node(result, call_location));
        }

        private static expression_node uint_idiv_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value / right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new uint_const_node(result, call_location));
        }

        private static expression_node uint_div_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((((double)left.constant_value) / ((double)right.constant_value)), call_location));
        }

        private static expression_node uint_unmin_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            return (new long_const_node((-(left.constant_value)), call_location));
        }

        private static expression_node uint_not_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            //TODO: Проверить опреатор ~. Это опреатор двоичного дополнения.
            return (new uint_const_node((~(left.constant_value)), call_location));
        }

        private static expression_node uint_gr_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node uint_greq_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node uint_sm_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node uint_smeq_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node uint_eq_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node uint_noteq_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node uint_and_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new uint_const_node(((left.constant_value) & (right.constant_value)), call_location));
        }

        private static expression_node uint_or_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new uint_const_node(((left.constant_value) | (right.constant_value)), call_location));
        }

        private static expression_node uint_xor_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            uint_const_node right = parameters[1] as uint_const_node;
            if (right == null)
            {
                return null;
            }
            return (new uint_const_node((left.constant_value ^ right.constant_value), call_location));
        }

        private static expression_node uint_shl_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new uint_const_node((left.constant_value << right.constant_value), call_location));
        }

        private static expression_node uint_shr_executor(location call_location, params expression_node[] parameters)
        {
            uint_const_node left = parameters[0] as uint_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new uint_const_node((left.constant_value >> right.constant_value), call_location));
        }





        //long
        private static expression_node long_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node long_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node long_to_short_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node long_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node long_to_int_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node long_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node long_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node long_to_char_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node long_to_float_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node long_to_double_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node bcn = parameters[0] as long_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node long_add_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value + right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_sub_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value - right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_mul_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value * right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_mod_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value % right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_idiv_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value / right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_div_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((((double)left.constant_value) / ((double)right.constant_value)), call_location));
        }

        private static expression_node long_unmin_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long result = 0;
            try
            {
                checked
                {
                    result = -(left.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new long_const_node(result, call_location));
        }

        private static expression_node long_not_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            //TODO: Проверить опреатор ~. Это опреатор двоичного дополнения.
            return (new long_const_node((~(left.constant_value)), call_location));
        }

        private static expression_node long_gr_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node long_greq_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node long_sm_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node long_smeq_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node long_eq_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node long_noteq_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node long_and_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new long_const_node(((left.constant_value) & (right.constant_value)), call_location));
        }

        private static expression_node long_or_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new long_const_node(((left.constant_value) | (right.constant_value)), call_location));
        }

        private static expression_node long_xor_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            long_const_node right = parameters[1] as long_const_node;
            if (right == null)
            {
                return null;
            }
            return (new long_const_node((left.constant_value ^ right.constant_value), call_location));
        }

        private static expression_node long_shl_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            //ТУТ Int!!!!!!!!
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new long_const_node((left.constant_value << right.constant_value), call_location));
        }

        private static expression_node long_shr_executor(location call_location, params expression_node[] parameters)
        {
            long_const_node left = parameters[0] as long_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new long_const_node((left.constant_value >> right.constant_value), call_location));
        }


        
        
        
        //ulong
        private static expression_node ulong_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_short_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_int_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            // SSM 26.02.20 #2208 fix
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_long_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_char_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_float_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node ulong_to_double_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node ulong_unmin_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node bcn = parameters[0] as ulong_const_node;
            if (bcn == null)
            {
                return null;
            }
            //TODO: Здесь, в некоторых ситуациях, программа должна падать. Потом сделать корректную обработку.
            return new long_const_node((-((long)bcn.constant_value)), call_location);
        }

        private static expression_node ulong_add_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            ulong result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value + right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new ulong_const_node(result, call_location));
        }

        private static expression_node ulong_sub_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            ulong result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value - right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new ulong_const_node(result, call_location));
        }

        private static expression_node ulong_mul_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            ulong result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value * right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new ulong_const_node(result, call_location));

        }

        private static expression_node ulong_mod_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            ulong result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value % right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new ulong_const_node(result, call_location));
        }

        private static expression_node ulong_idiv_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong result = 0;
            try
            {
                checked
                {
                    result = (left.constant_value / right.constant_value);
                }
            }
            catch (System.OverflowException)
            {
                throw new SimpleSemanticError(call_location, "ENUMENTAR_TYPE_VALUE_OVERFLOW");
            }
            return (new ulong_const_node(result, call_location));
        }

        private static expression_node ulong_div_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((((double)left.constant_value) / ((double)right.constant_value)), call_location));
        }
        /*
        private static expression_node ulong_unmin_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            return (new ulong_const_node((-(left.constant_value)), call_location));
        }
        */
        private static expression_node ulong_not_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            //TODO: Проверить опреатор ~. Это опреатор двоичного дополнения.
            return (new ulong_const_node((~(left.constant_value)), call_location));
        }

        private static expression_node ulong_gr_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node ulong_greq_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node ulong_sm_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node ulong_smeq_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node ulong_eq_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node ulong_noteq_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node ulong_and_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new ulong_const_node(((left.constant_value) & (right.constant_value)), call_location));
        }

        private static expression_node ulong_or_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new ulong_const_node(((left.constant_value) | (right.constant_value)), call_location));
        }

        private static expression_node ulong_xor_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            ulong_const_node right = parameters[1] as ulong_const_node;
            if (right == null)
            {
                return null;
            }
            return (new ulong_const_node((left.constant_value ^ right.constant_value), call_location));
        }

        private static expression_node ulong_shl_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new ulong_const_node((left.constant_value << right.constant_value), call_location));
        }

        private static expression_node ulong_shr_executor(location call_location, params expression_node[] parameters)
        {
            ulong_const_node left = parameters[0] as ulong_const_node;
            if (left == null)
            {
                return null;
            }
            int_const_node right = parameters[1] as int_const_node;
            if (right == null)
            {
                return null;
            }
            return (new ulong_const_node((left.constant_value >> right.constant_value), call_location));
        }
        
        


        //float
        private static expression_node float_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node float_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node float_to_short_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node float_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node float_to_int_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node float_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node float_to_long_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node float_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node float_to_char_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node float_to_double_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node bcn = parameters[0] as float_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node float_unmin_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            return (new float_const_node((-(left.constant_value)), call_location));
        }

        private static expression_node float_add_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new float_const_node((left.constant_value + right.constant_value), call_location));
        }

        private static expression_node float_sub_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new float_const_node((left.constant_value - right.constant_value), call_location));
        }

        private static expression_node float_mul_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new float_const_node((left.constant_value * right.constant_value), call_location));
        }

        private static expression_node float_div_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            /*
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            */
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            return (new float_const_node((left.constant_value / right.constant_value), call_location));
        }

        private static expression_node float_gr_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node float_greq_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node float_sm_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node float_smeq_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node float_eq_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node float_noteq_executor(location call_location, params expression_node[] parameters)
        {
            float_const_node left = parameters[0] as float_const_node;
            if (left == null)
            {
                return null;
            }
            float_const_node right = parameters[1] as float_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        
        
        //real (double)
        private static expression_node double_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node double_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node double_to_short_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node double_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node double_to_int_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node double_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node double_to_long_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node double_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node double_to_char_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new char_const_node((char)bcn.constant_value, call_location);
        }

        private static expression_node double_to_float_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node bcn = parameters[0] as double_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node real_unmin_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            return (new double_const_node((-(left.constant_value)), call_location));
        }

        private static expression_node real_add_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((left.constant_value + right.constant_value), call_location));
        }

        private static expression_node real_sub_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((left.constant_value - right.constant_value), call_location));
        }

        private static expression_node real_mul_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new double_const_node((left.constant_value * right.constant_value), call_location));
        }

        private static expression_node real_div_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            /*
            if (right.constant_value == 0)
            {
                throw new SimpleSemanticError(parameters[1].location, "DIVISION_BY_ZERO_CONSTANT");
            }
            */
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            return (new double_const_node((left.constant_value / right.constant_value), call_location));
        }

        private static expression_node real_gr_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node real_greq_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node real_sm_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node real_smeq_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node real_eq_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node real_noteq_executor(location call_location, params expression_node[] parameters)
        {
            double_const_node left = parameters[0] as double_const_node;
            if (left == null)
            {
                return null;
            }
            double_const_node right = parameters[1] as double_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }





        //char
        private static expression_node char_to_byte_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new byte_const_node((byte)bcn.constant_value, call_location);
        }

        private static expression_node char_to_sbyte_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new sbyte_const_node((sbyte)bcn.constant_value, call_location);
        }

        private static expression_node char_to_short_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new short_const_node((short)bcn.constant_value, call_location);
        }

        private static expression_node char_to_ushort_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ushort_const_node((ushort)bcn.constant_value, call_location);
        }

        private static expression_node char_to_int_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new int_const_node((int)bcn.constant_value, call_location);
        }

        private static expression_node char_to_uint_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new uint_const_node((uint)bcn.constant_value, call_location);
        }

        private static expression_node char_to_long_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new long_const_node((long)bcn.constant_value, call_location);
        }

        private static expression_node char_to_ulong_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new ulong_const_node((ulong)bcn.constant_value, call_location);
        }

        private static expression_node char_to_float_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new float_const_node((float)bcn.constant_value, call_location);
        }

        private static expression_node char_to_double_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node bcn = parameters[0] as char_const_node;
            if (bcn == null)
            {
                return null;
            }
            return new double_const_node((double)bcn.constant_value, call_location);
        }

        private static expression_node char_gr_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value > right.constant_value), call_location));
        }

        private static expression_node char_greq_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value >= right.constant_value), call_location));
        }

        private static expression_node char_sm_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value < right.constant_value), call_location));
        }

        private static expression_node char_smeq_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value <= right.constant_value), call_location));
        }

        private static expression_node char_eq_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node char_noteq_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
            {
                return null;
            }
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node bool_not_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            return (new bool_const_node((!(left.constant_value)), call_location));
        }

        private static expression_node bool_and_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value && right.constant_value), call_location));
        }

        private static expression_node bool_or_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value || right.constant_value), call_location));
        }

        private static expression_node bool_xor_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value ^ right.constant_value), call_location));
        }

        private static expression_node bool_gr_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value & (!(right.constant_value))), call_location));
        }

        private static expression_node bool_greq_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value || (!(right.constant_value))), call_location));
        }

        private static expression_node bool_sm_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node(((!(left.constant_value)) & right.constant_value), call_location));
        }

        private static expression_node bool_smeq_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node(((!(left.constant_value)) || right.constant_value), call_location));
        }

        private static expression_node bool_eq_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value == right.constant_value), call_location));
        }

        private static expression_node bool_noteq_executor(location call_location, params expression_node[] parameters)
        {
            bool_const_node left = parameters[0] as bool_const_node;
            if (left == null)
            {
                return null;
            }
            bool_const_node right = parameters[1] as bool_const_node;
            if (right == null)
            {
                return null;
            }
            return (new bool_const_node((left.constant_value != right.constant_value), call_location));
        }

        private static expression_node string_add_executor(location call_location, params expression_node[] parameters)
        {
            string_const_node left = parameters[0] as string_const_node;
            if (left == null)
                return null;
            string_const_node right = parameters[1] as string_const_node;
            if (right == null)
                return null;
            return new string_const_node(left.constant_value + right.constant_value, call_location);
        }

        private static expression_node char_to_string_executor(location call_location, params expression_node[] parameters)
        {
            if (parameters.Length > 1)
                return null;
            if (parameters[0] is char_const_node)
                return new string_const_node((parameters[0] as char_const_node).constant_value.ToString(),call_location);
            return null;
        }

        private static expression_node char_add_executor(location call_location, params expression_node[] parameters)
        {
            char_const_node left = parameters[0] as char_const_node;
            if (left == null)
                return null;
            char_const_node right = parameters[1] as char_const_node;
            if (right == null)
                return null;
            return new string_const_node(left.constant_value.ToString() + right.constant_value.ToString(), call_location);
        }

        public static expression_node enum_or_executor(location call_location, params expression_node[] parameters)
        {
            enum_const_node left = parameters[0] as enum_const_node;
            if (left == null && parameters[0] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[0] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    left = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[0].type, parameters[0].location);
            }
            if (left == null)
                return null;
            enum_const_node right = parameters[1] as enum_const_node;
            if (right == null && parameters[1] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[1] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    right = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[1].type, parameters[1].location);
            }
            if (right == null)
                return null;

            return new enum_const_node(left.constant_value | right.constant_value, left.type, call_location);
        }

        public static expression_node enum_and_executor(location call_location, params expression_node[] parameters)
        {
            enum_const_node left = parameters[0] as enum_const_node;
            if (left == null && parameters[0] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[0] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    left = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[0].type, parameters[0].location);
            }
            if (left == null)
                return null;
            enum_const_node right = parameters[1] as enum_const_node;
            if (right == null && parameters[1] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[1] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    right = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[1].type, parameters[1].location);
            }
            if (right == null)
                return null;
            return new enum_const_node(left.constant_value & right.constant_value, left.type, call_location);
        }

        public static expression_node enum_xor_executor(location call_location, params expression_node[] parameters)
        {
            enum_const_node left = parameters[0] as enum_const_node;
            if (left == null && parameters[0] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[0] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    left = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[0].type, parameters[0].location);
            }
            if (left == null)
                return null;
            enum_const_node right = parameters[1] as enum_const_node;
            if (right == null && parameters[1] is static_compiled_variable_reference)
            {
                static_compiled_variable_reference cvr = parameters[1] as static_compiled_variable_reference;
                if (cvr.var.IsLiteral)
                    right = new enum_const_node((int)cvr.var.compiled_field.GetRawConstantValue(), parameters[1].type, parameters[1].location);
            }
            if (right == null)
                return null;
            return new enum_const_node(left.constant_value ^ right.constant_value, left.type, call_location);
        }
    }

}
