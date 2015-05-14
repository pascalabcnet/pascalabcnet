#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the MacroRecorded event
    /// </summary>
    public class MacroRecordEventArgs : EventArgs
    {
        #region Fields

        private Message _recordedMessage;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns the recorded window message that can be sent back to the native Scintilla window
        /// </summary>
        public Message RecordedMessage
        {
            get
            {
                return _recordedMessage;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the MacroRecordEventArgs class.
        /// </summary>
        /// <param name="recordedMessage">the recorded window message that can be sent back to the native Scintilla window</param>
        public MacroRecordEventArgs(Message recordedMessage)
        {
            _recordedMessage = recordedMessage;
        }


        /// <summary>
        ///     Initializes a new instance of the MacroRecordEventArgs class.
        /// </summary>
        /// <param name="ea">NativeScintillaEventArgs object containing the message data</param>
        public MacroRecordEventArgs(NativeScintillaEventArgs ea)
        {
            _recordedMessage = ea.Msg;
            _recordedMessage.LParam = ea.SCNotification.lParam;
            _recordedMessage.WParam = ea.SCNotification.wParam;
        }

        #endregion Constructors
    } 
}
