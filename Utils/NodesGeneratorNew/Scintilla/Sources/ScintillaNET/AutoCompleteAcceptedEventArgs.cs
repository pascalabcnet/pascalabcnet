#region Using Directives

using System;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the AutoCompleteAccepted event
    /// </summary>
    public class AutoCompleteAcceptedEventArgs : EventArgs
    {
        #region Fields

        private string _text;
        private int _wordStartPosition;
        private bool _cancel = false;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Gets/Sets if the autocomplete action should be cancelled
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }


        /// <summary>
        ///     Text of the selected autocomplete entry selected
        /// </summary>
        public string Text
        {
            get { return _text; }
        }


        /// <summary>
        ///     Returns the _start position of the current word in the document.
        /// </summary>
        /// <remarks>
        ///     This controls how many characters of the selected autocomplete entry
        ///     is actually inserted into the document
        /// </remarks>
        public int WordStartPosition
        {
            get
            {
                return _wordStartPosition;
            }
        }

        #endregion Properties


        #region Constructors

        internal AutoCompleteAcceptedEventArgs(SCNotification eventSource, Encoding encoding)
        {
            _wordStartPosition = (int)eventSource.lParam;
            _text = Utilities.IntPtrToString(encoding, eventSource.text);
        }


        /// <summary>
        ///     Initializes a new instance of the AutoCompleteAcceptedEventArgs class.
        /// </summary>
        /// <param name="text">Text of the selected autocomplete entry selected</param>
        public AutoCompleteAcceptedEventArgs(string text)
        {
            _text = text;
        }

        #endregion Constructors
    }
}
