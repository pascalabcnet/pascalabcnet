﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.Core;

namespace ICSharpCode.SharpDevelop.Gui
{
	public delegate void SaveEventHandler(object sender, SaveEventArgs e);
	
	public class SaveEventArgs : System.EventArgs
	{
		bool successful;
		
		public bool Successful {
			get {
				return successful;
			}
		}
		
		public SaveEventArgs(bool successful)
		{
			this.successful = successful;
		}
	}
	
	/// <summary>
	/// IViewContent is the base interface for "windows" in the document area of SharpDevelop.
	/// A view content is a view onto multiple files, or other content that opens like a document
	/// (e.g. the start page).
	/// </summary>
	public interface IViewContent : IDisposable, ICanBeDirty, IServiceProvider
	{
		/// <summary>
		/// This is the UI element for the view.
		/// You can use both Windows.Forms and WPF controls.
		/// </summary>
		object Control {
			get;
		}
		
		/// <summary>
		/// Gets the control which has focus initially.
		/// </summary>
		object InitiallyFocusedControl {
			get;
		}
		
		/// <summary>
		/// The workbench window in which this view is displayed.
		/// </summary>
		IWorkbenchWindow WorkbenchWindow {
			get;
			set;
		}
		
		/// <summary>
		/// Is raised when the value of the TabPageText property changes.
		/// </summary>
		event EventHandler TabPageTextChanged;
		
		/// <summary>
		/// The text on the tab page when more than one view content
		/// is attached to a single window.
		/// </summary>
		string TabPageText {
			get;
		}
		
		/// <summary>
		/// The title of the view content. This normally is the title of the primary file being edited.
		/// </summary>
		string TitleName {
			get;
		}
		
		/// <summary>
		/// Is called each time the name for the content has changed.
		/// </summary>
		event EventHandler TitleNameChanged;
		
		/// <summary>
		/// The tooltip that will be shown when you hover the mouse over the title
		/// </summary>
		string InfoTip {
			get;
		}

		/// <summary>
		/// Is called each time the info tip for the content has changed.
		/// </summary>
		event EventHandler InfoTipChanged;

		/// <summary>
		/// Saves the content to the location <code>fileName</code>
		/// </summary>
		/// <remarks>
		/// When the user switches between multiple views editing the same file, a view
		/// change will trigger one view content to save that file into a memory stream
		/// and the other view content will load the file from that memory stream.
		/// </remarks>
		void Save(OpenedFile file, Stream stream);
		
		/// <summary>
		/// Load or reload the content of the specified file from the stream.
		/// </summary>
		/// <remarks>
		/// When the user switches between multiple views editing the same file, a view
		/// change will trigger one view content to save that file into a memory stream
		/// and the other view content will load the file from that memory stream.
		/// </remarks>
		void Load(OpenedFile file, Stream stream);
		
		/// <summary>
		/// Gets the list of files that are being edited using this view content.
		/// The returned collection usually is read-only.
		/// </summary>
		IList<OpenedFile> Files { get; }
		
		/// <summary>
		/// Gets the primary file being edited. Might return null if no file is edited.
		/// </summary>
		OpenedFile PrimaryFile { get; }
		
		/// <summary>
		/// Gets the name of the primary file being edited. Might return null if no file is edited.
		/// </summary>
		FileName PrimaryFileName { get; }
		
		/// <summary>
		/// Builds an <see cref="INavigationPoint"/> for the current position.
		/// </summary>
		INavigationPoint BuildNavPoint();
		
		bool IsDisposed { get; }
		
		event EventHandler Disposed;
		
		/// <summary>
		/// Gets if the view content is read-only (can be saved only when choosing another file name).
		/// </summary>
		bool IsReadOnly { get; }
		
		/// <summary>
		/// Gets if the view content is view-only (cannot be saved at all).
		/// </summary>
		bool IsViewOnly { get; }
		
		/// <summary>
		/// Gets whether this view content should be closed when the solution is closed.
		/// </summary>
		bool CloseWithSolution { get; }
		
		#region Secondary view content support
		/// <summary>
		/// Gets the collection that stores the secondary view contents.
		/// </summary>
		ICollection<IViewContent> SecondaryViewContents { get; }
		
		
		/// <summary>
		/// Gets switching without a Save/Load cycle for <paramref name="file"/> is supported
		/// when switching from this view content to <paramref name="newView"/>.
		/// </summary>
		bool SupportsSwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView);
		
		/// <summary>
		/// Gets switching without a Save/Load cycle for <paramref name="file"/> is supported
		/// when switching from <paramref name="oldView"/> to this view content.
		/// </summary>
		bool SupportsSwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView);
		
		/// <summary>
		/// Executes an action before switching from this view content to the new view content.
		/// </summary>
		void SwitchFromThisWithoutSaveLoad(OpenedFile file, IViewContent newView);
		
		/// <summary>
		/// Executes an action before switching from the old view content to this view content.
		/// </summary>
		void SwitchToThisWithoutSaveLoad(OpenedFile file, IViewContent oldView);
		#endregion
	}
}
