﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.ComponentModel;
using System.Windows.Forms;

using ICSharpCode.Core.Services;

namespace ICSharpCode.Core.WinForms
{
	/// <summary>
	/// Class with static methods to show message boxes.
	/// All text displayed using the MessageService is passed to the
	/// <see cref="StringParser"/> to replace ${res} markers.
	/// </summary>
	public class WinFormsMessageService : IDialogMessageService
	{
		/// <summary>
		/// Gets/Sets the form used as owner for shown message boxes.
		/// </summary>
		public IWin32Window DialogOwner { get; set; }
		
		/// <summary>
		/// Gets/Sets the object used to synchronize message boxes shown on other threads.
		/// </summary>
		public ISynchronizeInvoke DialogSynchronizeInvoke { get; set; }
		
		void BeginInvoke(MethodInvoker method)
		{
			ISynchronizeInvoke si = DialogSynchronizeInvoke;
			if (si == null || !si.InvokeRequired)
				method();
			else
				si.BeginInvoke(method, null);
		}
		
		void Invoke(MethodInvoker method)
		{
			ISynchronizeInvoke si = DialogSynchronizeInvoke;
			if (si == null || !si.InvokeRequired)
				method();
			else
				si.Invoke(method, null);
		}
		
		public virtual void ShowException(Exception ex, string message)
		{
			if (ex != null) {
				message += "\n\nException occurred: " + ex.ToString();
			}
			ShowError(message);
		}
		
		public void ShowError(string message)
		{
			message = StringParser.Parse(message);
			
			string caption = StringParser.Parse("${res:Global.ErrorText}");
			BeginInvoke(
				delegate {
					MessageBox.Show(DialogOwner,
					                message, caption,
					                MessageBoxButtons.OK, MessageBoxIcon.Warning,
					                MessageBoxDefaultButton.Button1, GetOptions(message, caption));
				});
		}
		
		public void ShowWarning(string message)
		{
			message = StringParser.Parse(message);
			
			string caption = StringParser.Parse("${res:Global.WarningText}");
			BeginInvoke(
				delegate {
					MessageBox.Show(DialogOwner,
					                message, caption,
					                MessageBoxButtons.OK, MessageBoxIcon.Warning,
					                MessageBoxDefaultButton.Button1, GetOptions(message, caption));
				});
		}
		
		public bool AskQuestion(string question, string caption)
		{
			DialogResult result = DialogResult.None;
			Invoke(
				delegate {
					result = MessageBox.Show(DialogOwner,
					                         StringParser.Parse(question),
					                         StringParser.Parse(caption),
					                         MessageBoxButtons.YesNo,
					                         MessageBoxIcon.Question,
					                         MessageBoxDefaultButton.Button1,
					                         GetOptions(question, caption));
				});
			return result == DialogResult.Yes;
		}
		
		static MessageBoxOptions GetOptions(string text, string caption)
		{
			return IsRtlText(text) ? MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign : 0;
		}
		
		static bool IsRtlText(string text)
		{
			if (!RightToLeftConverter.IsRightToLeft)
				return false;
			foreach (char c in StringParser.Parse(text)) {
				if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
					return true;
			}
			return false;
		}
		
		public int ShowCustomDialog(string caption, string dialogText, int acceptButtonIndex, int cancelButtonIndex, params string[] buttontexts)
		{
			int result = 0;
			Invoke(
				delegate {
					using (CustomDialog messageBox = new CustomDialog(caption, dialogText, acceptButtonIndex, cancelButtonIndex, buttontexts)) {
						messageBox.ShowDialog(DialogOwner);
						result = messageBox.Result;
					}
				});
			return result;
		}
		
		public string ShowInputBox(string caption, string dialogText, string defaultValue)
		{
			string result = null;
			Invoke(
				delegate {
					using (InputBox inputBox = new InputBox(dialogText, caption, defaultValue)) {
						inputBox.ShowDialog(DialogOwner);
						result = inputBox.Result;
					}
				});
			return result;
		}
		
		public void ShowMessage(string message, string caption)
		{
			message = StringParser.Parse(message);
			BeginInvoke(
				delegate {
					MessageBox.Show(DialogOwner,
					                message,
					                StringParser.Parse(caption),
					                MessageBoxButtons.OK,
					                MessageBoxIcon.Information,
					                MessageBoxDefaultButton.Button1,
					                GetOptions(message, caption));
				});
		}
		
		public void InformSaveError(string fileName, string message, string dialogName, Exception exceptionGot)
		{
			BeginInvoke(
				delegate {
					using (SaveErrorInformDialog dlg = new SaveErrorInformDialog(fileName, message, dialogName, exceptionGot)) {
						dlg.ShowDialog(DialogOwner);
					}
				});
		}
		
		public ChooseSaveErrorResult ChooseSaveError(string fileName, string message, string dialogName, Exception exceptionGot, bool chooseLocationEnabled)
		{
			ChooseSaveErrorResult r = ChooseSaveErrorResult.Ignore;
			Invoke(
				delegate {
				restartlabel:
					using (SaveErrorChooseDialog dlg = new SaveErrorChooseDialog(fileName, message, dialogName, exceptionGot, chooseLocationEnabled)) {
						switch (dlg.ShowDialog(DialogOwner)) {
							case DialogResult.OK:
								// choose location:
								using (SaveFileDialog fdiag = new SaveFileDialog()) {
									fdiag.OverwritePrompt = true;
									fdiag.AddExtension    = true;
									fdiag.CheckFileExists = false;
									fdiag.CheckPathExists = true;
									fdiag.Title           = "Choose alternate file name";
									fdiag.FileName        = fileName;
									if (fdiag.ShowDialog() == DialogResult.OK) {
										r = ChooseSaveErrorResult.SaveAlternative(fdiag.FileName);
										break;
									} else {
										goto restartlabel;
									}
								}
							case DialogResult.Retry:
								r = ChooseSaveErrorResult.Retry;
								break;
							default:
								r = ChooseSaveErrorResult.Ignore;
								break;
						}
					}
				});
			return r;
		}
	}
}
