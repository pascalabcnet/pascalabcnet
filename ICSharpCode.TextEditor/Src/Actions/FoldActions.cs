﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 1965 $</version>
// </file>

using System;
using System.Collections.Generic;
using ICSharpCode.TextEditor.Document;

namespace ICSharpCode.TextEditor.Actions 
{
	public class ToggleFolding : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			List<FoldMarker> foldMarkers = textArea.Document.FoldingManager.GetFoldingsWithStart(textArea.Caret.Line);
			foreach (FoldMarker fm in foldMarkers) {
				fm.IsFolded = !fm.IsFolded;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
	
	public class ToggleAllFoldings : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			bool doFold = true;
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				if (fm.IsFolded) {
					doFold = false;
					break;
				}
			}
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				fm.IsFolded = doFold;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
	
	public class ShowDefinitionsOnly : AbstractEditAction
	{
		public override void Execute(TextArea textArea)
		{
			foreach (FoldMarker fm in  textArea.Document.FoldingManager.FoldMarker) {
				fm.IsFolded = fm.FoldType == FoldType.MemberBody || fm.FoldType == FoldType.Region;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
	}
}
