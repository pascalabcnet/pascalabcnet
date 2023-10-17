// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.Collections
{
    /// <summary>
    /// Расширяемая коллекция. Элементы из нее нельзя удалять.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class extendable_collection<T> : System.Collections.Generic.IEnumerable<T>
    {
        /// <summary>
        /// Enumerator для списка элементов.
        /// </summary>
        public class enumerator : System.Collections.Generic.IEnumerator<T>
        {
            private int i = -1;

            private readonly extendable_collection<T> enumerable_list;

            public enumerator(extendable_collection<T> expr_list)
            {
                enumerable_list = expr_list;
            }

            public T Current
            {
                get
                {
                    return enumerable_list[i];
                }
            }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                i++;
                if (i >= enumerable_list._elements.Count)
                {
                    return false;
                }
                return true;
            }

            public void Reset()
            {
                i = -1;
            }
        }

        protected readonly System.Collections.Generic.List<T> _elements =
            new System.Collections.Generic.List<T>();
        protected T[] _elements_as_arr = null;

        /// <summary>
        /// Добавить элемент к списку.
        /// </summary>
        /// <param name="expression">Добавляемый элемент.</param>
        public void AddElement(T element)
        {
            _elements.Add(element);
            _elements_as_arr = null;
        }
        
        public void RemoveElement(T element)
        {
            _elements.Remove(element);
            _elements_as_arr = null;
        }
        
        /// <summary>
        /// Добавляет список элементов к списку.
        /// </summary>
        /// <param name="elements">Список добавляемых элементов.</param>
        public void AddRange(extendable_collection<T> elements)
        {
            _elements.AddRange(elements._elements);
            _elements_as_arr = null;
        }

        public void AddRange(T[] elements)
        {
            _elements.AddRange(elements);
            _elements_as_arr = null;
        }

        //ssyy
        public int IndexOf(T element)
        {
            return _elements.IndexOf(element);
        }
        //\ssyy

        public void AddElementFirst(T element)
        {
            _elements.Insert(0, element);
            _elements_as_arr = null;
        }

        public void AddRangeFirst(extendable_collection<T> elements)
        {
            _elements.InsertRange(0, elements._elements);
            _elements_as_arr = null;
        }

        public void AddRangeFirst(T[] elements)
        {
            _elements.InsertRange(0, elements);
            _elements_as_arr = null;
        }

        /// <summary>
        /// Возвращает массив элементов, хранящихся в коллкции.
        /// </summary>
        /// <returns>Массив элементов</returns>
        public T[] ToArray()
        {
            if (_elements_as_arr == null)
                _elements_as_arr = _elements.ToArray();
            return _elements_as_arr;
            //return _elements.ToArray();
        }

        /// <summary>
        /// Индексирует элементы коллекции.
        /// </summary>
        /// <param name="num">Номер элемента.</param>
        /// <returns>Элемент с указанным номером.</returns>
        public T this[int num]
        {
            get
            {
                return _elements[num];
            }
        }

        /// <summary>
        /// Число элементов в коллекции.
        /// </summary>
        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return new enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    [Serializable]
    public class extended_collection<T> : extendable_collection<T>
    {
        public void remove(T element)
        {
            _elements.Remove(element);
            _elements_as_arr = null;
        }

        public void remove_at(int index)
        {
            _elements.RemoveAt(index);
            _elements_as_arr = null;
        }
		
        public bool Contains(T elem)
        {
        	return _elements.Contains(elem);
        }
        
        public T this[int i]
        {
            get
            {
                return _elements[i];
            }
            set
            {
                _elements[i] = value;
                _elements_as_arr = null;
            }
        }

        public void remove_range(int index,int count)
        {
            _elements.RemoveRange(index, count);
            _elements_as_arr = null;
        }

        public void Clear()
        {
            _elements.Clear();
            _elements_as_arr = null;

        }
    }

}
