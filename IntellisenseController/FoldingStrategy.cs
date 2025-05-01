// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using CodeCompletion;
using PascalABCCompiler.Parsers;

namespace VisualPascalABC
{
    public class RegionFoldMarker : FoldMarker
    {
        public RegionFoldMarker(IDocument document, int startLine, int startColumn, int endLine, int endColumn, FoldType foldType, string foldText):
            base(document, startLine, startColumn, endLine, endColumn, foldType, foldText)
        {

        }   
    }

	public class ParserFoldingStrategy : IFoldingStrategy
	{
        private List<FoldMarker> GetFoldMarkers(IDocument doc, IBaseScope root, string fileName)
        {
            List<FoldMarker> foldMarkers = new List<FoldMarker>();
            if (!(root is ITypeScope))
            {
                Position pos = root.GetPosition();
                if (pos.file_name != null)
                {
                    Position head_pos = root.GetHeaderPosition();
                    FoldMarker newFoldMarker = null;
                    if (head_pos.file_name == null)
                    {
                        if (root is ImplementationUnitScope)
                        {
                            Position unit_pos = root.TopScope.GetPosition();
                            newFoldMarker = new FoldMarker(doc, pos.line - 1, pos.column - 1, unit_pos.end_line - 1, unit_pos.end_column, FoldType.MemberBody);
                        }
                        else
                            newFoldMarker = new FoldMarker(doc, pos.line - 1, pos.column - 1, pos.end_line - 1, pos.end_column, FoldType.MemberBody);
                    }
                    else
                        newFoldMarker = new FoldMarker(doc, head_pos.end_line - 1, head_pos.end_column - 1, pos.end_line - 1, pos.end_column, FoldType.MemberBody);
                    if (newFoldMarker.Length > 0)
                    {
                        foldMarkers.Add(newFoldMarker);
                    }
                }
                if (root.Regions != null)
                {
                    foreach (Position p in root.Regions)
                    {
                        foldMarkers.Add(new RegionFoldMarker(doc, p.line - 1, p.column, p.end_line - 1, p.end_column, FoldType.MemberBody, p.fold_text));
                    }
                }
                Position body_pos = root.GetBodyPosition();
                if (body_pos.file_name != null)
                {
                    FoldMarker newFoldMarker = null;
                    if (body_pos.line < pos.line && foldMarkers.Count > 0)
                        foldMarkers.RemoveAt(foldMarkers.Count - 1);
                    newFoldMarker = new FoldMarker(doc, body_pos.line - 1, body_pos.column - 1, body_pos.end_line - 1, body_pos.end_column, FoldType.MemberBody);
                    if (newFoldMarker.Length > 0)
                    {
                        foldMarkers.Add(newFoldMarker);
                    }
                }

            }
            foreach (IBaseScope ss in root.Members)
            {
                if (ss is ITypeScope)
                {
                    Position body_pos = ss.GetBodyPosition();
                    Position head_pos = ss.GetHeaderPosition();
                    if (head_pos.file_name == null || body_pos.file_name == null)
                        continue;
                    FoldMarker newFoldMarker = new FoldMarker(doc, head_pos.end_line - 1, head_pos.end_column - 1, body_pos.end_line - 1, body_pos.end_column, FoldType.TypeBody);
                    if (newFoldMarker.Length > 0)
                    {
                        foldMarkers.Add(newFoldMarker);
                    }
                    foldMarkers.AddRange(GetFoldMarkers(doc, ss, fileName));
                }
                else if (ss is IProcScope)
                {
                    FoldType ft = FoldType.MemberBody;
                    Position head_pos = ss.GetHeaderPosition();
                    Position body_pos = ss.GetBodyPosition();
                    if (head_pos.file_name == null || body_pos.file_name == null)
                        continue;
                    FoldMarker newFoldMarker = new FoldMarker(doc, head_pos.end_line - 1, head_pos.end_column, body_pos.end_line - 1, body_pos.end_column, ft);
                    if (newFoldMarker.Length > 0)
                    {
                        foldMarkers.Add(newFoldMarker);
                    }
                }
                else if (ss is IImplementationUnitScope)
                    foldMarkers.AddRange(GetFoldMarkers(doc, ss, fileName));
            }
            
            return foldMarkers;
        }
		
		public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInfo)
		{
			List<FoldMarker> foldMarkers = GetFoldMarkers(document, parseInfo as IBaseScope, fileName);
			return foldMarkers;
		}
		
	}
}
