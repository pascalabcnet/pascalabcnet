﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ICSharpCode.Core.WinForms
{
	/// <summary>
	/// Helper class to access the clipboard without worrying about ExternalExceptions
	/// </summary>
	public static class ClipboardWrapper
	{
		[Obsolete("Avoid using this property: it is problematic because it requires exclusive clipboard access. " +
		          "The Clipboard.ContainsText() implementation in WPF is much better than the one in WinForms.")]
		public static bool ContainsText {
			get {
				try {
					return Clipboard.ContainsText();
				} catch (ExternalException) {
					return false;
				}
			}
		}
		
		public static string GetText()
		{
			// retry 2 times should be enough for read access
			try {
				return Clipboard.GetText();
			} catch (ExternalException) {
				return Clipboard.GetText();
			}
		}
		
		public static void SetText(string text)
		{
			DataObject data = new DataObject();
			data.SetData(DataFormats.UnicodeText, true, text);
			SetDataObject(data);
		}
		
		/// <summary>
		/// Gets the current clipboard content.
		/// Can return null!
		/// </summary>
		public static IDataObject GetDataObject()
		{
			// retry 2 times should be enough for read access
			try {
				return Clipboard.GetDataObject();
			} catch (ExternalException) {
				try {
					return Clipboard.GetDataObject();
				} catch (ExternalException) {
					return null;
				}
			}
		}
		
		public static void SetDataObject(object data)
		{
			SafeSetClipboard(data);
		}
		
		// Code duplication: TextAreaClipboardHandler.cs also has SafeSetClipboard
		[ThreadStatic] static int SafeSetClipboardDataVersion;
		
		static void SafeSetClipboard(object dataObject)
		{
			// Work around ExternalException bug. (SD2-426)
			// Best reproducable inside Virtual PC.
			int version = unchecked(++SafeSetClipboardDataVersion);
			try {
				Clipboard.SetDataObject(dataObject, true);
			} catch (ExternalException) {
				Timer timer = new Timer();
				timer.Interval = 100;
				timer.Tick += delegate {
					timer.Stop();
					timer.Dispose();
					if (SafeSetClipboardDataVersion == version) {
						try {
							Clipboard.SetDataObject(dataObject, true, 10, 50);
						} catch (ExternalException) { }
					}
				};
				timer.Start();
			}
		}
	}
}
