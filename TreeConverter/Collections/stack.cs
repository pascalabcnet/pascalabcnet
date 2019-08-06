// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;

namespace PascalABCCompiler.Collections
{
    [Serializable]
    public class stack<T>
    {
        protected System.Collections.Generic.Stack<T> internal_stack = new System.Collections.Generic.Stack<T>();

        public void push(T element)
        {
            internal_stack.Push(element);
        }

        public T pop()
        {
            return internal_stack.Pop();
        }

        public T top()
        {
            if (internal_stack.Count == 0)
                return default(T);
            //T t = internal_stack.Pop();
            //internal_stack.Push(t);
            return internal_stack.Peek();
        }
		
        public T first()
        {
        	if (internal_stack.Count == 0)
                return default(T);
        	return internal_stack.ToArray()[internal_stack.Count-1];
        }
        public int size
        {
            get
            {
                return internal_stack.Count;
            }
        }

        public bool Empty
        {
            get
            {
                return internal_stack.Count == 0;
            }
        }

        public void clear()
        {
            internal_stack.Clear();
        }

        public System.Collections.Generic.Stack<T> CloneInternalStack()
        {
            return new System.Collections.Generic.Stack<T>(internal_stack.Reverse());
        }
    }
}
