// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Причина обхода узла синтаксического дерева - получение выражения, адреса, типа узла.
using System;

namespace PascalABCCompiler.TreeConverter
{
    //TODO: Переименовать в request.
    /// <summary>
    /// Будущее название класса - request.
    /// </summary>
	public enum motivation {none,expression_evaluation,address_reciving,semantic_node_reciving};

    //TODO: Избавиться от этого.
	public class motivation_keeper
	{

		private motivation _mot=motivation.expression_evaluation;
		//private bool motivation_set=false;

        public void reset()
        {
            _mot = motivation.expression_evaluation;
        }

		public void set_motivation_to_expect_address()
		{
			_mot=motivation.address_reciving;
		}

		public void set_motivation_to_except_semantic_node()
		{
			_mot=motivation.semantic_node_reciving;
		}

		public motivation motivation
		{
            //(ssyy) Darkstar!!! Данный код провоцирует ложные переходы в режиме трассировки!
			get
			{
				motivation temp=_mot;
				_mot=motivation.expression_evaluation;
				return temp;
			}
		}

	}

}