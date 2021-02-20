﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.Core;
using ICSharpCode.NRefactory;
using ICSharpCode.SharpDevelop.Project;

namespace ICSharpCode.SharpDevelop.Editor.Search
{
	public class SearchResultMatch
	{
		FileName fileName;
		int offset;
		int length;
		Location startLocation;
		Location endLocation;
		HighlightedInlineBuilder builder;
		
		public FileName FileName {
			get { return fileName; }
		}
		
		public Location StartLocation {
			get { return startLocation; }
		}
		
		public Location EndLocation {
			get { return endLocation; }
		}
		
		public HighlightedInlineBuilder Builder {
			get { return builder; }
		}
		
		public int StartOffset {
			get { return offset; }
		}
		
		public int Length {
			get { return length; }
		}
		
		public int EndOffset {
			get { return offset + length; }
		}
		
		public virtual string TransformReplacePattern(string pattern)
		{
			return pattern;
		}
		
		public SearchResultMatch(FileName fileName, Location startLocation, Location endLocation, int offset, int length, HighlightedInlineBuilder builder)
		{
			this.fileName = fileName;
			this.startLocation = startLocation;
			this.endLocation = endLocation;
			this.offset = offset;
			this.length = length;
			this.builder = builder;
		}
		
		/// <summary>
		/// Gets a special text to display, or null to display the line's content.
		/// </summary>
		public virtual string DisplayText {
			get {
				return null;
			}
		}
		
		public override string ToString()
		{
			return String.Format("[{3}: FileName={0}, StartLocation={1}, EndLocation={2}]",
			                     fileName, startLocation, endLocation,
			                     GetType().Name);
		}
	}
	
	public class SimpleSearchResultMatch : SearchResultMatch
	{
		string displayText;
		
		public override string DisplayText {
			get {
				return displayText;
			}
		}
		
		public SimpleSearchResultMatch(FileName fileName, Location position, int offset, string displayText)
			: base(fileName, position, position, offset, 0, null)
		{
			this.displayText = displayText;
		}
	}
	
	public class AvalonEditSearchResultMatch : SearchResultMatch
	{
		ICSharpCode.AvalonEdit.Search.ISearchResult match;
		
		public AvalonEditSearchResultMatch(FileName fileName, Location startLocation, Location endLocation, int offset, int length, HighlightedInlineBuilder builder, ICSharpCode.AvalonEdit.Search.ISearchResult match)
			: base(fileName, startLocation, endLocation, offset, length, builder)
		{
			this.match = match;
		}
		
		public override string TransformReplacePattern(string pattern)
		{
			return match.ReplaceWith(pattern);
		}
	}
	
	public class SearchedFile
	{
		public FileName FileName { get; private set; }
		
		public IList<SearchResultMatch> Matches { get; private set; }
		
		public SearchedFile(FileName fileName, IList<SearchResultMatch> matches)
		{
			if (fileName == null)
				throw new ArgumentNullException("fileName");
			if (matches == null)
				throw new ArgumentNullException("matches");
			this.FileName = fileName;
			this.Matches = matches;
		}
	}
}
