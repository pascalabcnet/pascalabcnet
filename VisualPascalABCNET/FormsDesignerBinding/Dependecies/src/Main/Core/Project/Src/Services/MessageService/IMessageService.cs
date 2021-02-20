﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;

namespace ICSharpCode.Core.Services
{
	/// <summary>
	/// Interface for the MessageService.
	/// </summary>
	public interface IMessageService
	{
		/// <summary>
		/// Shows an error.
		/// </summary>
		void ShowError(string message);
		
		/// <summary>
		/// Shows an exception.
		/// </summary>
		void ShowException(Exception ex, string message);
		
		/// <summary>
		/// Shows a warning message.
		/// </summary>
		void ShowWarning(string message);
		
		/// <summary>
		/// Asks the user a Yes/No question, using "Yes" as the default button.
		/// Returns <c>true</c> if yes was clicked, <c>false</c> if no was clicked.
		/// </summary>
		bool AskQuestion(string question, string caption);
		
		/// <summary>
		/// Shows a custom dialog.
		/// </summary>
		/// <param name="caption">The title of the dialog.</param>
		/// <param name="dialogText">The description shown in the dialog.</param>
		/// <param name="acceptButtonIndex">
		/// The number of the button that is the default accept button.
		/// Use -1 if you don't want to have an accept button.
		/// </param>
		/// <param name="cancelButtonIndex">
		/// The number of the button that is the cancel button.
		/// Use -1 if you don't want to have a cancel button.
		/// </param>
		/// <param name="buttontexts">The captions of the buttons.</param>
		/// <returns>The number of the button that was clicked, or -1 if the dialog was closed  without clicking a button.</returns>
		int ShowCustomDialog(string caption, string dialogText, int acceptButtonIndex, int cancelButtonIndex, params string[] buttontexts);
		string ShowInputBox(string caption, string dialogText, string defaultValue);
		void ShowMessage(string message, string caption);
		
		/// <summary>
		/// Show a message informing the user about a save error.
		/// </summary>
		void InformSaveError(string fileName, string message, string dialogName, Exception exceptionGot);
		
		/// <summary>
		/// Show a message informing the user about a save error,
		/// and allow him to retry/save under alternative name.
		/// </summary>
		ChooseSaveErrorResult ChooseSaveError(string fileName, string message, string dialogName, Exception exceptionGot, bool chooseLocationEnabled);
	}
	
	public sealed class ChooseSaveErrorResult
	{
		public bool IsRetry { get; private set; }
		public bool IsIgnore { get; private set; }
		public bool IsSaveAlternative { get { return AlternativeFileName != null; } }
		public string AlternativeFileName { get; private set; }
		
		private ChooseSaveErrorResult() {}
		
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification="ChooseSaveErrorResult is immutable")]
		public readonly static ChooseSaveErrorResult Retry = new ChooseSaveErrorResult { IsRetry = true };
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification="ChooseSaveErrorResult is immutable")]
		public readonly static ChooseSaveErrorResult Ignore = new ChooseSaveErrorResult { IsIgnore = true };
		
		public static ChooseSaveErrorResult SaveAlternative(string alternativeFileName)
		{
			if (alternativeFileName == null)
				throw new ArgumentNullException("alternativeFileName");
			return new ChooseSaveErrorResult { AlternativeFileName = alternativeFileName };
		}
	}
}
