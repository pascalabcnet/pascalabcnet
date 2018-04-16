// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;

namespace System.Linq
{
    public static class ExtSeqMy
    {
        public static string JoinIntoString<T>(this IEnumerable<T> seq, string delim = " ")
        {
            var sb = new System.Text.StringBuilder();
            foreach (var x in seq)
                sb.Append(x.ToString() + delim);
            return sb.ToString();
        }
    }
}

namespace SymbolTable
{
	/*public class AreaNodesList
	{
        public override string ToString() => data.Take(Count).JoinIntoString();

        //public AreaListNode data[];
        public List<AreaListNode> data;

		public int Count
		{ 
			get{return data.Count;}
		}

		public AreaNodesList(int start_size)
		{
			data=new List<AreaListNode>(start_size);
		}

		public void Add(AreaListNode node)
		{
            if(Count == 0 || data[Count - 1].Area < node.Area)
            {
                data.Add(node);
                return;
            }

			int i=0;
			while (data[i].Area<node.Area) i++;
			if (data[i].Area==node.Area) 
			{
				data[i].InfoList.Add(node.InfoList[0]);
				return;
				//throw new Exception("Ошибка при добавлении области видимости к идентефикатору: такая область уже существует");
			}
            data.Insert(i, node);
		}

		public int IndexOf(int Area)
		{
            if (Count == 0) 
                return -1;
            int l = 0, r = Count - 1, t = (r - l) / 2;
            while ((data[l + t].Area != Area) && (r - l > 0))
            {
                if (Area > data[l + t].Area)
                    l = l + t + 1;
                else
                    r = r - t - 1;
                t = (r - l) / 2;
            }
            if (data[l + t].Area == Area) 
                return l + t;
            return -1;
		}

        public void Remove(int Area)
        {
            data.RemoveAt(IndexOf(Area));
        }

		public AreaListNode this [int index]
		{
			get{return data[index];	}
			set{data[index]=value;	}
		}

	}*/
	
}