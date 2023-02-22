﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 2643 $</version>
// </file>

using System;
using System.Text;

namespace ICSharpCode.TextEditor.Document
{
	/// <summary>
	/// This class handles the auto and smart indenting in the textbuffer while
	/// you type.
	/// </summary>
	public class DefaultFormattingStrategy : IFormattingStrategy
	{
		/// <summary>
		/// Creates a new instance off <see cref="DefaultFormattingStrategy"/>
		/// </summary>
		public DefaultFormattingStrategy()
		{
		}
		
		/// <summary>
		/// returns the whitespaces which are before a non white space character in the line line
		/// as a string.
		/// </summary>
		protected string GetIndentation(TextArea textArea, int lineNumber)
		{
			if (lineNumber < 0 || lineNumber > textArea.Document.TotalNumberOfLines) {
				throw new ArgumentOutOfRangeException("lineNumber");
			}
			
			string lineText = TextUtilities.GetLineAsString(textArea.Document, lineNumber);
			StringBuilder whitespaces = new StringBuilder();
			
			foreach (char ch in lineText) {
				if (Char.IsWhiteSpace(ch)) {
					whitespaces.Append(ch);
				} else {
					break;
				}
			}
			return whitespaces.ToString();
		}
		
		/// <summary>
		/// Could be overwritten to define more complex indenting.
		/// </summary>
		protected virtual int AutoIndentLine(TextArea textArea, int lineNumber)
		{
			string indentation = lineNumber != 0 ? GetIndentation(textArea, lineNumber - 1) : "";
			if(indentation.Length > 0) {
				string newLineText = indentation + TextUtilities.GetLineAsString(textArea.Document, lineNumber).Trim();
				LineSegment oldLine  = textArea.Document.GetLineSegment(lineNumber);
				textArea.Document.Replace(oldLine.Offset, oldLine.Length, newLineText);
			}
			return indentation.Length;
		}
		
		/// <summary>
		/// Could be overwritten to define more complex indenting.
		/// </summary>
		protected virtual int SmartIndentLine(TextArea textArea, int line)
		{
			return AutoIndentLine(textArea, line); // smart = autoindent in normal texts
		}
		
		/// <summary>
		/// This function formats a specific line after <code>ch</code> is pressed.
		/// </summary>
		/// <returns>
		/// the caret delta position the caret will be moved this number
		/// of bytes (e.g. the number of bytes inserted before the caret, or
		/// removed, if this number is negative)
		/// </returns>
		public virtual void FormatLine(TextArea textArea, int line, int cursorOffset, char ch)
		{
			if (ch == '\n') {
				textArea.Caret.Column = IndentLine(textArea, line);
			}
		}
		
		/// <summary>
		/// This function sets the indentation level in a specific line
		/// </summary>
		/// <returns>
		/// the number of inserted characters.
		/// </returns>
		public int IndentLine(TextArea textArea, int line)
		{
			textArea.Document.UndoStack.StartUndoGroup();
			int result;
			switch (textArea.Document.TextEditorProperties.IndentStyle) {
				case IndentStyle.None:
					result = 0;
					break;
				case IndentStyle.Auto:
					result = AutoIndentLine(textArea, line);
					break;
				case IndentStyle.Smart:
					result = SmartIndentLine(textArea, line);
					break;
				default:
					throw new NotSupportedException("Unsupported value for IndentStyle: " + textArea.Document.TextEditorProperties.IndentStyle);
			}
			textArea.Document.UndoStack.EndUndoGroup();
			return result;
		}
		
		/// <summary>
		/// This function sets the indentlevel in a range of lines.
		/// </summary>
		public virtual void IndentLines(TextArea textArea, int begin, int end)
		{
			textArea.Document.UndoStack.StartUndoGroup();
			for (int i = begin; i <= end; ++i) {
				IndentLine(textArea, i);
			}
			textArea.Document.UndoStack.EndUndoGroup();
		}
		
		public virtual int SearchBracketBackward(IDocument document, int offset, char openBracket, char closingBracket)
		{
			int brackets = -1;
			// first try "quick find" - find the matching bracket if there is no string/comment in the way
            bool kavs = false;
			for (int i = offset; i >= 0; --i) {
				char ch = document.GetCharAt(i);
				if (ch == openBracket) {
                    if (!kavs || ch == '{')
					++brackets;
					if (brackets == 0) return i;
				} else if (ch == closingBracket) {
                    if (!kavs || ch == '}')
                        --brackets;
                }
                else if (ch == '"')
                {
					break;
				} else if (ch == '\'') {
					kavs = !kavs;
				} else if (ch == '/' && i > 0) {
					if (document.GetCharAt(i - 1) == '/') break;
					if (document.GetCharAt(i - 1) == '*') break;
				}
			}
			return -1;
		}
		
		public virtual int SearchBracketForward(IDocument document, int offset, char openBracket, char closingBracket)
		{
			int brackets = 1;
            bool kavs = false;
			// try "quick find" - find the matching bracket if there is no string/comment in the way
			for (int i = offset; i < document.TextLength; ++i) {
				char ch = document.GetCharAt(i);
				if (ch == openBracket) {
                    if (!kavs || ch == '{')
                        ++brackets;
				} else if (ch == closingBracket) {
                    if (!kavs || ch == '}')
                        --brackets;
					if (brackets == 0) return i;
				} else if (ch == '"') {
					break;
				} else if (ch == '\'') {
					kavs = !kavs;
				} else if (ch == '/' && i > 0) {
					if (document.GetCharAt(i - 1) == '/') break;
				} else if (ch == '*' && i > 0) {
					if (document.GetCharAt(i - 1) == '/') break;
				}
			}
			return -1;
		}
	}
}
