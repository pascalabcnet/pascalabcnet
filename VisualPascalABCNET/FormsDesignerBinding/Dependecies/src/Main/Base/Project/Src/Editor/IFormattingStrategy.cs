﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using ICSharpCode.NRefactory;
using System.Collections.Generic;

namespace ICSharpCode.SharpDevelop.Editor
{
	/// <summary>
	/// Indentation and formatting strategy.
	/// </summary>
	public interface IFormattingStrategy
	{
		/// <summary>
		/// This function formats a specific line after <code>charTyped</code> is pressed.
		/// </summary>
		void FormatLine(ITextEditor editor, char charTyped);
		
		/// <summary>
		/// This function sets the indentation level in a specific line
		/// </summary>
		void IndentLine(ITextEditor editor, IDocumentLine line);
		
		/// <summary>
		/// This function sets the indentation in a range of lines.
		/// </summary>
		void IndentLines(ITextEditor editor, int beginLine, int endLine);
		
		/// <summary>
		/// This function surrounds the selected text with a comment.
		/// </summary>
		void SurroundSelectionWithComment(ITextEditor editor);
	}
	
	public class DefaultFormattingStrategy : IFormattingStrategy
	{
		internal static readonly DefaultFormattingStrategy DefaultInstance = new DefaultFormattingStrategy();
		
		public virtual void FormatLine(ITextEditor editor, char charTyped)
		{
		}
		
		public virtual void IndentLine(ITextEditor editor, IDocumentLine line)
		{
			IDocument document = editor.Document;
			int lineNumber = line.LineNumber;
			if (lineNumber > 1) {
				IDocumentLine previousLine = document.GetLine(lineNumber - 1);
				string indentation = DocumentUtilitites.GetWhitespaceAfter(document, previousLine.Offset);
				// copy indentation to line
				string newIndentation = DocumentUtilitites.GetWhitespaceAfter(document, line.Offset);
				document.Replace(line.Offset, newIndentation.Length, indentation);
			}
		}
		
		public virtual void IndentLines(ITextEditor editor, int begin, int end)
		{
			using (editor.Document.OpenUndoGroup()) {
				for (int i = begin; i <= end; i++) {
					IndentLine(editor, editor.Document.GetLine(i));
				}
			}
		}
		
		public virtual void SurroundSelectionWithComment(ITextEditor editor)
		{
		}
		
		/// <summary>
		/// Default implementation for single line comments.
		/// </summary>
		protected void SurroundSelectionWithSingleLineComment(ITextEditor editor, string comment)
		{
			using (editor.Document.OpenUndoGroup()) {
				Location startPosition = editor.Document.OffsetToPosition(editor.SelectionStart);
				Location endPosition = editor.Document.OffsetToPosition(editor.SelectionStart + editor.SelectionLength);
				
				// endLine is one above endPosition if no characters are selected on the last line (e.g. line selection from the margin)
				int endLine = (endPosition.Column == 1 && endPosition.Line > startPosition.Line) ? endPosition.Line - 1 : endPosition.Line;
				
				List<IDocumentLine> lines = new List<IDocumentLine>();
				bool removeComment = true;
				
				for (int i = startPosition.Line; i <= endLine; i++) {
					lines.Add(editor.Document.GetLine(i));
					if (!lines[i - startPosition.Line].Text.Trim().StartsWith(comment, StringComparison.Ordinal))
						removeComment = false;
				}
				
				foreach (IDocumentLine line in lines) {
					if (removeComment) {
						editor.Document.Remove(line.Offset + line.Text.IndexOf(comment, StringComparison.Ordinal), comment.Length);
					} else {
						editor.Document.Insert(line.Offset, comment, AnchorMovementType.BeforeInsertion);
					}
				}
			}
		}
		
		/// <summary>
		/// Default implementation for multiline comments.
		/// </summary>
		protected void SurroundSelectionWithBlockComment(ITextEditor editor, string blockStart, string blockEnd)
		{
			using (editor.Document.OpenUndoGroup()) {
				int startOffset = editor.SelectionStart;
				int endOffset = editor.SelectionStart + editor.SelectionLength;
				
				if (editor.SelectionLength == 0) {
					IDocumentLine line = editor.Document.GetLineForOffset(editor.SelectionStart);
					startOffset = line.Offset;
					endOffset = line.Offset + line.Length;
				}
				
				BlockCommentRegion region = FindSelectedCommentRegion(editor, blockStart, blockEnd);
				
				if (region != null) {
					editor.Document.Remove(region.EndOffset, region.CommentEnd.Length);
					editor.Document.Remove(region.StartOffset, region.CommentStart.Length);
				} else {
					editor.Document.Insert(endOffset, blockEnd);
					editor.Document.Insert(startOffset, blockStart);
				}
			}
		}
		
		public static BlockCommentRegion FindSelectedCommentRegion(ITextEditor editor, string commentStart, string commentEnd)
		{
			IDocument document = editor.Document;
			
			if (document.TextLength == 0) {
				return null;
			}
			
			// Find start of comment in selected text.
			
			int commentEndOffset = -1;
			string selectedText = editor.SelectedText;
			
			int commentStartOffset = selectedText.IndexOf(commentStart);
			if (commentStartOffset >= 0) {
				commentStartOffset += editor.SelectionStart;
			}

			// Find end of comment in selected text.
			
			if (commentStartOffset >= 0) {
				commentEndOffset = selectedText.IndexOf(commentEnd, commentStartOffset + commentStart.Length - editor.SelectionStart);
			} else {
				commentEndOffset = selectedText.IndexOf(commentEnd);
			}
			
			if (commentEndOffset >= 0) {
				commentEndOffset += editor.SelectionStart;
			}
			
			// Find start of comment before or partially inside the
			// selected text.
			
			int commentEndBeforeStartOffset = -1;
			if (commentStartOffset == -1) {
				int offset = editor.SelectionStart + editor.SelectionLength + commentStart.Length - 1;
				if (offset > document.TextLength) {
					offset = document.TextLength;
				}
				string text = document.GetText(0, offset);
				commentStartOffset = text.LastIndexOf(commentStart);
				if (commentStartOffset >= 0) {
					// Find end of comment before comment start.
					commentEndBeforeStartOffset = text.IndexOf(commentEnd, commentStartOffset, editor.SelectionStart - commentStartOffset);
					if (commentEndBeforeStartOffset > commentStartOffset) {
						commentStartOffset = -1;
					}
				}
			}
			
			// Find end of comment after or partially after the
			// selected text.
			
			if (commentEndOffset == -1) {
				int offset = editor.SelectionStart + 1 - commentEnd.Length;
				if (offset < 0) {
					offset = editor.SelectionStart;
				}
				string text = document.GetText(offset, document.TextLength - offset);
				commentEndOffset = text.IndexOf(commentEnd);
				if (commentEndOffset >= 0) {
					commentEndOffset += offset;
				}
			}
			
			if (commentStartOffset != -1 && commentEndOffset != -1) {
				return new BlockCommentRegion(commentStart, commentEnd, commentStartOffset, commentEndOffset);
			}
			
			return null;
		}
	}
	
	public class BlockCommentRegion
	{
		public string CommentStart { get; private set; }
		public string CommentEnd { get; private set; }
		public int StartOffset { get; private set; }
		public int EndOffset { get; private set; }
		
		/// <summary>
		/// The end offset is the offset where the comment end string starts from.
		/// </summary>
		public BlockCommentRegion(string commentStart, string commentEnd, int startOffset, int endOffset)
		{
			this.CommentStart = commentStart;
			this.CommentEnd = commentEnd;
			this.StartOffset = startOffset;
			this.EndOffset = endOffset;
		}
		
		public override int GetHashCode()
		{
			int hashCode = 0;
			unchecked {
				if (CommentStart != null) hashCode += 1000000007 * CommentStart.GetHashCode();
				if (CommentEnd != null) hashCode += 1000000009 * CommentEnd.GetHashCode();
				hashCode += 1000000021 * StartOffset.GetHashCode();
				hashCode += 1000000033 * EndOffset.GetHashCode();
			}
			return hashCode;
		}
		
		public override bool Equals(object obj)
		{
			BlockCommentRegion other = obj as BlockCommentRegion;
			if (other == null) return false;
			return this.CommentStart == other.CommentStart &&
				this.CommentEnd == other.CommentEnd &&
				this.StartOffset == other.StartOffset &&
				this.EndOffset == other.EndOffset;
		}
	}
}
