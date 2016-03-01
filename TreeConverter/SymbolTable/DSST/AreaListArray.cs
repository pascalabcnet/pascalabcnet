// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace SymbolTable
{
	public class AreaNodesList
	{
		public AreaListNode[] data;
		int count;

		public int Count
		{
			get
			{
				return count;
			}
		}

		public AreaNodesList(int start_size)
		{
			data=new AreaListNode[start_size];
			count=0;
			for (int i=0;i<data.Length;i++) data[i]=null;
		}

		public void Add(AreaListNode node)
		{
			if (count>=data.Length) Resize(data.Length*SymbolTableConstants.AreaList_ResizeParam);
			if (count==0) 
			{
				count++;
				data[0]=node;
				return;
			}
			if (data[count-1].Area<node.Area)
			{
				data[count]=node;
				count++;
				return;
			}
			int i=0;
			while (data[i].Area<node.Area) i++;
			if (data[i].Area==node.Area) 
			{
				data[i].InfoList.Add(node.InfoList[0]);
                //data[i].InfoList[0].Next = node.InfoList[0];
				return;
				//throw new Exception("Ошибка при добавлении области видимости к идентефикатору: такая область уже существует");
			}
			int k=i;
			for (i=count;i>=k+1;i--) data[i]=data[i-1];
			count++;
            data[i]=node;
		}

		private void Resize(int new_size)
		{
			AreaListNode[] d=new AreaListNode[new_size];
			data.CopyTo(d,0);
			data=d;
		}

		public int IndexOf(int Area)
		{
            if (count == 0) 
                return -1;
            int l = 0, r = count - 1, t = (r - l) / 2;
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
		public AreaListNode this [int index]
		{
			get
			{
				return data[index];
			}
			set
			{
				data[index]=value;
			}
		}

	}
	
}