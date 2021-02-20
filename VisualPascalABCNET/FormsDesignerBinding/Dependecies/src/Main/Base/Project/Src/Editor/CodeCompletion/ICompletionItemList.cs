﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;

namespace ICSharpCode.SharpDevelop.Editor.CodeCompletion
{
	public interface ICompletionItemList
	{
		/// <summary>
		/// Gets the items in the list.
		/// </summary>
		IEnumerable<ICompletionItem> Items { get; }
		
		/// <summary>
		/// Gets the suggested item.
		/// This item will be pre-selected in the completion list.
		/// </summary>
		ICompletionItem SuggestedItem { get; }
		
		/// <summary>
		/// Gets the length of the preselection (text in front of the completion list that
		/// should be included as completed expression).
		/// </summary>
		int PreselectionLength { get; }
		
		/// <summary>
		/// Processes the specified key press.
		/// </summary>
		CompletionItemListKeyResult ProcessInput(char key);
		
		/// <summary>
		/// True if this list contains all items that were available.
		/// False if this list could contain even more items 
		/// (e.g. by including items from all referenced projects, regardless of imports).
		/// </summary>
		bool ContainsAllAvailableItems { get; }
		
		/// <summary>
		/// Performs code completion for the selected item.
		/// </summary>
		void Complete(CompletionContext context, ICompletionItem item);
	}
	

	
	public enum CompletionItemListKeyResult
	{
		/// <summary>
		/// Normal key, used to choose an entry from the completion list
		/// </summary>
		NormalKey,
		/// <summary>
		/// This key triggers insertion of the completed expression
		/// </summary>
		InsertionKey,
		/// <summary>
		/// Increment both start and end offset of completion region when inserting this
		/// key. Can be used to insert whitespace (or other characters) in front of the expression
		/// while the completion window is open.
		/// </summary>
		BeforeStartKey,
		/// <summary>
		/// This key triggers cancellation of completion. The completion window will be closed.
		/// </summary>
		Cancel
	}
	
	public class DefaultCompletionItemList : ICompletionItemList
	{
		List<ICompletionItem> items = new List<ICompletionItem>();
		
		public List<ICompletionItem> Items {
			get { return items; }
		}
		
		/// <inheritdoc />
		public virtual bool ContainsAllAvailableItems {
			get { return true; }
		}
		
		/// <summary>
		/// Sorts the items by their text.
		/// </summary>
		public void SortItems()	// PERF this is called twice
		{
			// the user might use method names is his language, so sort using CurrentCulture
			items.Sort((a,b) => {
			           	int r = string.Compare(a.Text, b.Text, StringComparison.CurrentCultureIgnoreCase);
			           	if (r != 0)
			           		return r;
			           	else
			           		return string.Compare(a.Text, b.Text, StringComparison.CurrentCulture);
			           });
		}
		
		/// <inheritdoc/>
		public int PreselectionLength { get; set; }
		
		/// <inheritdoc/>
		public ICompletionItem SuggestedItem { get; set; }
		
		IEnumerable<ICompletionItem> ICompletionItemList.Items {
			get { return items; }
		}
		
		/// <summary>
		/// Allows the insertion of a single space in front of the completed text.
		/// </summary>
		public bool InsertSpace { get; set; }
		
		/// <inheritdoc/>
		public virtual CompletionItemListKeyResult ProcessInput(char key)
		{
			if (key == ' ' && this.InsertSpace) {
				this.InsertSpace = false; // insert space only once
				return CompletionItemListKeyResult.BeforeStartKey;
			} else if (char.IsLetterOrDigit(key) || key == '_') {
				this.InsertSpace = false; // don't insert space if user types normally
				return CompletionItemListKeyResult.NormalKey;
			} else {
				// do not reset insertSpace when doing an insertion!
				return CompletionItemListKeyResult.InsertionKey;
			}
		}
		
		/// <inheritdoc/>
		public virtual void Complete(CompletionContext context, ICompletionItem item)
		{
			if (context == null)
				throw new ArgumentNullException("context");
			if (item == null)
				throw new ArgumentNullException("item");
			if (InsertSpace) {
				InsertSpace = false;
				context.Editor.Document.Insert(context.StartOffset, " ");
				context.StartOffset++;
				context.EndOffset++;
			}
			item.Complete(context);
		}
	}
}
