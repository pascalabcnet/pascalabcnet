// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace ICSharpCode.TextEditor.Document
{
    /// <summary>
    /// A simple folding strategy which calculates the folding level
    /// using the indent level of the line.
    /// </summary>
   /* public class TestFoldingStrategy : IFoldingStrategy
    {
        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string file_name, object parseInformation)
        {
            List<FoldMarker> l = new List<FoldMarker>();
            Stack<int> offsetStack = new Stack<int>();
            Stack<string> textStack = new Stack<string>();
            int level = 0;
            	l.Add(new FoldMarker(document,1,0,10,0));
//            foreach (LineSegment segment in document.LineSegmentCollection) {
  //          }
            return l;
        }

        int GetLevel(IDocument document, int offset)
        {
            int level = 0;
            int spaces = 0;
            for (int i = offset; i < document.TextLength; ++i)
            {
                char c = document.GetCharAt(i);
                if (c == '\t' || (c == ' ' && ++spaces == 4))
                {
                    spaces = 0;
                    ++level;
                }
                else
                {
                    break;
                }
            }
            return level;
        }
    }*/
}