// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.SyntaxTree;
namespace PascalABCCompiler.ParserTools
{

    public class SourceContextComparer : IComparer<SourceContext>
    {
        public int Compare(SourceContext x, SourceContext y)
        {
            return x.Position - y.Position;
        }
    }

    public class SourceContextPair
    {
        SourceContext NewContext;
        SourceContext RealContext;

        public SourceContextPair(SourceContext x, SourceContext y)
        {
            NewContext = x;
            RealContext = y;
        }

        public SourceContext GetNew()
        {
            return NewContext;
        }

        public SourceContext GetReal()
        {
            return RealContext;
        }
    }

    public class SourceContextMap
    {
        SourceContext CurrentSourceContext = null;
        //SortedList<SourceContext, SourceContext> AreaList = new SortedList<SourceContext, SourceContext>(new SourceContextComparer());
        List<SourceContextPair> AreaList = new List<SourceContextPair>();
        
        SourceContext Find(SourceContext tf)
        {
            //TODO Надо сделать половинным делением
            //foreach (SourceContext t in AreaList.Keys)
            //    if (tf.In(t))
            //        return t;
            //return null;
            if (AreaList.Count == 0)
                return null;
            if ( tf.begin_position.line_num < 1 || tf.begin_position.line_num > AreaList.Count)
                return AreaList[AreaList.Count-1].GetNew();
            else
                return AreaList[tf.begin_position.line_num - 1].GetNew();
                //return AreaList.Keys[tf.begin_position.line_num - 1];

        }
        public void AddArea(SourceContext tf, SourceContext real_context)
        {
            AreaList.Add(new SourceContextPair(tf, real_context));
            //AreaList.Add(tf, real_context);
        }
        public SourceContext GetSourceContext(SourceContext scv)
        {
            //Это надо считать только по мере надобности!!!
            SourceContext scvf;
            if (CurrentSourceContext != null && scv.In(CurrentSourceContext))
                scvf = CurrentSourceContext;
            else
            {
                scvf = Find(scv);
                CurrentSourceContext = scvf;
            }

            //scvf = Find(scv);
            if (scvf == null)
                return scv;
            //SourceContext sco = AreaList[scvf];
            if (scvf.begin_position.line_num < 1 || scvf.begin_position.line_num > AreaList.Count)
                return null;               
            SourceContext sco = AreaList[scvf.begin_position.line_num - 1].GetReal();
            SourceContext scn = new SourceContext(
                sco.begin_position.line_num + scv.begin_position.line_num - scvf.begin_position.line_num,
                sco.begin_position.column_num + scv.begin_position.column_num - scvf.begin_position.column_num,
                //sco.begin_position.column_num == -1 ? 1 : scv.begin_position.column_num,
                sco.end_position.line_num + scv.end_position.line_num - scvf.end_position.line_num,
                sco.begin_position.column_num + scv.end_position.column_num - 1,
                //sco.end_position.column_num == -1 ? 1 : scv.end_position.column_num,
                sco.Position + scv.Position - scvf.Position,
                sco.Position + scv.Position - scvf.Position + scv.Length);
            scn.FileName = sco.FileName;
            return scn;
        }
    }
}
