// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Отображение типов операторов узлов синтаксического дерева в имена методов.
using System;

namespace PascalABCCompiler.TreeConverter
{

	public class name_reflector
	{

        private static System.Collections.Generic.Dictionary<int, string> ht = new System.Collections.Generic.Dictionary<int, string>();
        private static System.Collections.Generic.Dictionary<string, int> pc = new System.Collections.Generic.Dictionary<string, int>();

		static name_reflector()
		{		
			ht.Add(((int)(SyntaxTree.Operators.LogicalAND)),compiler_string_consts.and_name);
			ht.Add(((int)(SyntaxTree.Operators.Division)),compiler_string_consts.div_name);
			ht.Add(((int)(SyntaxTree.Operators.IntegerDivision)),compiler_string_consts.idiv_name);
			ht.Add(((int)(SyntaxTree.Operators.Equal)),compiler_string_consts.eq_name);
			ht.Add(((int)(SyntaxTree.Operators.Greater)),compiler_string_consts.gr_name);
			ht.Add(((int)(SyntaxTree.Operators.GreaterEqual)),compiler_string_consts.greq_name);
			ht.Add(((int)(SyntaxTree.Operators.ModulusRemainder)),compiler_string_consts.mod_name);
			ht.Add(((int)(SyntaxTree.Operators.Multiplication)),compiler_string_consts.mul_name);
			ht.Add(((int)(SyntaxTree.Operators.LogicalNOT)),compiler_string_consts.not_name);
			ht.Add(((int)(SyntaxTree.Operators.NotEqual)),compiler_string_consts.noteq_name);
			ht.Add(((int)(SyntaxTree.Operators.LogicalOR)),compiler_string_consts.or_name);
			ht.Add(((int)(SyntaxTree.Operators.Plus)),compiler_string_consts.plus_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseLeftShift)),compiler_string_consts.shl_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseRightShift)),compiler_string_consts.shr_name);
			ht.Add(((int)(SyntaxTree.Operators.Less)),compiler_string_consts.sm_name);
			ht.Add(((int)(SyntaxTree.Operators.LessEqual)),compiler_string_consts.smeq_name);
			ht.Add(((int)(SyntaxTree.Operators.Minus)),compiler_string_consts.minus_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseXOR)),compiler_string_consts.xor_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentAddition)), compiler_string_consts.plusassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentSubtraction)), compiler_string_consts.minusassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentMultiplication)), compiler_string_consts.multassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentDivision)), compiler_string_consts.divassign_name);
            ht.Add(((int)(SyntaxTree.Operators.Assignment)), compiler_string_consts.assign_name);
            ht.Add(((int)(SyntaxTree.Operators.In)), compiler_string_consts.in_name);
			ht.Add(((int)(SyntaxTree.Operators.Implicit)), compiler_string_consts.implicit_operator_name);
			ht.Add(((int)(SyntaxTree.Operators.Explicit)), compiler_string_consts.explicit_operator_name);
			
            pc.Add(compiler_string_consts.and_name,         2);
            pc.Add(compiler_string_consts.div_name,         2);
            pc.Add(compiler_string_consts.idiv_name,        2);
            pc.Add(compiler_string_consts.eq_name,          2);
            pc.Add(compiler_string_consts.gr_name,          2);
            pc.Add(compiler_string_consts.greq_name,        2);
            pc.Add(compiler_string_consts.mod_name,         2);
            pc.Add(compiler_string_consts.mul_name,         2);
            pc.Add(compiler_string_consts.not_name,         1);
            pc.Add(compiler_string_consts.noteq_name,       2);
            pc.Add(compiler_string_consts.or_name,          2);
            pc.Add(compiler_string_consts.plus_name,        2);
            pc.Add(compiler_string_consts.shl_name,         2);
            pc.Add(compiler_string_consts.shr_name,         2);
            pc.Add(compiler_string_consts.sm_name,          2);
            pc.Add(compiler_string_consts.smeq_name,        2);
            pc.Add(compiler_string_consts.minus_name,       2);
            pc.Add(compiler_string_consts.xor_name,         2);
            pc.Add(compiler_string_consts.plusassign_name,  2);
            pc.Add(compiler_string_consts.minusassign_name, 2);
            pc.Add(compiler_string_consts.multassign_name,  2);
            pc.Add(compiler_string_consts.divassign_name,   2);
            pc.Add(compiler_string_consts.assign_name,      2);
            pc.Add(compiler_string_consts.in_name,          2);
            pc.Add(compiler_string_consts.implicit_operator_name, 1);
            pc.Add(compiler_string_consts.explicit_operator_name, 1);
        }

		public static string get_name(SyntaxTree.Operators ot)
		{
            string str;
            if (ht.TryGetValue((int)ot, out str))
            {
                return str;
            }
            return null;
		}

        public static int get_params_count(string operator_name)
        {
            int count;
            if (pc.TryGetValue(operator_name, out count))
            {
                return count;
            }
            return -1;
        }


	}

}
