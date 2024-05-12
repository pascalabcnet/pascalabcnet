// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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
			ht.Add(((int)(SyntaxTree.Operators.LogicalAND)),StringConstants.and_name);
			ht.Add(((int)(SyntaxTree.Operators.Division)),StringConstants.div_name);
			ht.Add(((int)(SyntaxTree.Operators.IntegerDivision)),StringConstants.idiv_name);
			ht.Add(((int)(SyntaxTree.Operators.Equal)),StringConstants.eq_name);
			ht.Add(((int)(SyntaxTree.Operators.Greater)),StringConstants.gr_name);
			ht.Add(((int)(SyntaxTree.Operators.GreaterEqual)),StringConstants.greq_name);
			ht.Add(((int)(SyntaxTree.Operators.ModulusRemainder)),StringConstants.mod_name);
			ht.Add(((int)(SyntaxTree.Operators.Multiplication)),StringConstants.mul_name);
			ht.Add(((int)(SyntaxTree.Operators.LogicalNOT)), StringConstants.not_name);
			ht.Add(((int)(SyntaxTree.Operators.NotEqual)),StringConstants.noteq_name);
			ht.Add(((int)(SyntaxTree.Operators.LogicalOR)),StringConstants.or_name);
			ht.Add(((int)(SyntaxTree.Operators.Plus)),StringConstants.plus_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseLeftShift)),StringConstants.shl_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseRightShift)),StringConstants.shr_name);
			ht.Add(((int)(SyntaxTree.Operators.Less)),StringConstants.sm_name);
			ht.Add(((int)(SyntaxTree.Operators.LessEqual)),StringConstants.smeq_name);
			ht.Add(((int)(SyntaxTree.Operators.Minus)),StringConstants.minus_name);
			ht.Add(((int)(SyntaxTree.Operators.BitwiseXOR)),StringConstants.xor_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentAddition)), StringConstants.plusassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentSubtraction)), StringConstants.minusassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentMultiplication)), StringConstants.multassign_name);
            ht.Add(((int)(SyntaxTree.Operators.AssignmentDivision)), StringConstants.divassign_name);
            ht.Add(((int)(SyntaxTree.Operators.Assignment)), StringConstants.assign_name);
            ht.Add(((int)(SyntaxTree.Operators.In)), StringConstants.in_name);
			ht.Add(((int)(SyntaxTree.Operators.Implicit)), StringConstants.implicit_operator_name);
			ht.Add(((int)(SyntaxTree.Operators.Explicit)), StringConstants.explicit_operator_name);
            ht.Add(((int)(SyntaxTree.Operators.Power)), StringConstants.power_name);

            pc.Add(StringConstants.and_name,         2);
            pc.Add(StringConstants.div_name,         2);
            pc.Add(StringConstants.idiv_name,        2);
            pc.Add(StringConstants.eq_name,          2);
            pc.Add(StringConstants.gr_name,          2);
            pc.Add(StringConstants.greq_name,        2);
            pc.Add(StringConstants.mod_name,         2);
            pc.Add(StringConstants.mul_name,         2);
            pc.Add(StringConstants.not_name,         1);
            pc.Add(StringConstants.noteq_name,       2);
            pc.Add(StringConstants.or_name,          2);
            pc.Add(StringConstants.plus_name,        2);
            pc.Add(StringConstants.shl_name,         2);
            pc.Add(StringConstants.shr_name,         2);
            pc.Add(StringConstants.sm_name,          2);
            pc.Add(StringConstants.smeq_name,        2);
            pc.Add(StringConstants.minus_name,       2);
            pc.Add(StringConstants.xor_name,         2);
            pc.Add(StringConstants.plusassign_name,  2);
            pc.Add(StringConstants.minusassign_name, 2);
            pc.Add(StringConstants.multassign_name,  2);
            pc.Add(StringConstants.divassign_name,   2);
            pc.Add(StringConstants.assign_name,      2);
            pc.Add(StringConstants.in_name,          2);
            pc.Add(StringConstants.power_name, 2);
            pc.Add(StringConstants.implicit_operator_name, 1);
            pc.Add(StringConstants.explicit_operator_name, 1);
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
