//------------------------------------------------------------------------------
/// <copyright from='1997' to='2002' company='Microsoft Corporation'>
///    Copyright (c) Microsoft Corporation. All Rights Reserved.
///
///    This source code is intended only as a supplement to Microsoft
///    Development Tools and/or on-line documentation.  See these other
///    materials for detailed information regarding Microsoft code samples.
///
/// </copyright>
//------------------------------------------------------------------------------
namespace SampleDesignerApplication
{
	using System;
	using System.Windows.Forms;
	using System.ComponentModel.Design;
	using System.Windows.Forms.Design;
	using SampleDesignerHost;

	/// This filter is used to catch keyboard input that is meant for the designer.
	/// It does not prevent the message from continuing, but instead merely
	/// deciphers the keystroke and performs the appropriate MenuCommand.
	public class KeystrokeMessageFilter : System.Windows.Forms.IMessageFilter
	{
		private IDesignerHost host;

		public KeystrokeMessageFilter(IDesignerHost host)
		{
			this.host = host;
		}
		#region Implementation of IMessageFilter

		public bool PreFilterMessage(ref Message m)
		{
			// Catch WM_KEYCHAR if the designerView has focus
			if ((m.Msg == 0x0100) && (((SampleDesignerHost)host).View.Focused))
			{
				IMenuCommandService mcs = host.GetService(typeof(IMenuCommandService)) as IMenuCommandService;

				// WM_KEYCHAR only tells us the last key pressed. Thus we check
				// Control for modifier keys (Control, Shift, etc.)
				//
				switch (((int)m.WParam) | ((int)Control.ModifierKeys))
				{
					case (int)Keys.Up: mcs.GlobalInvoke(MenuCommands.KeyMoveUp);
						break;
					case (int)Keys.Down: mcs.GlobalInvoke(MenuCommands.KeyMoveDown);
						break;
					case (int)Keys.Right: mcs.GlobalInvoke(MenuCommands.KeyMoveRight);
						break;
					case (int)Keys.Left: mcs.GlobalInvoke(MenuCommands.KeyMoveLeft);
						break;
					case (int)(Keys.Control | Keys.Up): mcs.GlobalInvoke(MenuCommands.KeyNudgeUp);
						break;
					case (int)(Keys.Control | Keys.Down): mcs.GlobalInvoke(MenuCommands.KeyNudgeDown);
						break;
					case (int)(Keys.Control | Keys.Right): mcs.GlobalInvoke(MenuCommands.KeyNudgeRight);
						break;
					case (int)(Keys.Control | Keys.Left): mcs.GlobalInvoke(MenuCommands.KeyNudgeLeft);
						break;
					case (int)(Keys.Shift | Keys.Up): mcs.GlobalInvoke(MenuCommands.KeySizeHeightIncrease);
						break;
					case (int)(Keys.Shift | Keys.Down): mcs.GlobalInvoke(MenuCommands.KeySizeHeightDecrease);
						break;
					case (int)(Keys.Shift | Keys.Right): mcs.GlobalInvoke(MenuCommands.KeySizeWidthIncrease);
						break;
					case (int)(Keys.Shift | Keys.Left): mcs.GlobalInvoke(MenuCommands.KeySizeWidthDecrease);
						break;
					case (int)(Keys.Control | Keys.Shift | Keys.Up): mcs.GlobalInvoke(MenuCommands.KeyNudgeHeightIncrease);
						break;
					case (int)(Keys.Control | Keys.Shift | Keys.Down): mcs.GlobalInvoke(MenuCommands.KeyNudgeHeightDecrease);
						break;
					case (int)(Keys.Control | Keys.Shift | Keys.Right): mcs.GlobalInvoke(MenuCommands.KeyNudgeWidthIncrease);
						break;
					case (int)(Keys.ControlKey | Keys.Shift | Keys.Left): mcs.GlobalInvoke(MenuCommands.KeyNudgeWidthDecrease);
						break;
					case (int)(Keys.Escape): mcs.GlobalInvoke(MenuCommands.KeyCancel);
						break;
					case (int)(Keys.Shift | Keys.Escape): mcs.GlobalInvoke(MenuCommands.KeyReverseCancel);
						break;
					case (int)(Keys.Enter): mcs.GlobalInvoke(MenuCommands.KeyDefaultAction);
						break;
				}
			}
			// Never filter the message
			return false;
		}

		#endregion
	}
}
