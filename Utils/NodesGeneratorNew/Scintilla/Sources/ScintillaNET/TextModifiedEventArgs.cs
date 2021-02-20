#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provices data for the TextModified event
    /// </summary>
    /// <remarks>
    ///     TextModifiedEventHandler is used as an abstracted subset of the
    ///     SCN_MODIFIED notification message. It's used whenever the SCNotification's
    ///     modificationType flags are SC_MOD_INSERTTEXT ,SC_MOD_DELETETEXT, 
    ///     SC_MOD_BEFOREINSERT and SC_MOD_BEFORE_DELETE. They all use a 
    ///     TextModifiedEventArgs which corresponds to a subset of the 
    ///     SCNotification struct having to do with these modification types.
    /// </remarks>
    public class TextModifiedEventArgs : ModifiedEventArgs
    {
        #region Constants

        private const string STRING_FORMAT = "ModificationTypeFlags\t:{0}\r\nPosition\t\t\t:{1}\r\nLength\t\t\t\t:{2}\r\nLinesAddedCount\t\t:{3}\r\nText\t\t\t\t:{4}\r\nIsUserChange\t\t\t:{5}\r\nMarkerChangeLine\t\t:{6}";

        #endregion Constants


        #region Fields

        private bool _isUserChange;
        private int _length;
        private int _linesAddedCount;
        private int _markerChangedLine;
        private int _position;
        private string _text;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Overridden.
        /// </summary>
        public override string ToString()
        {
            return string.Format(STRING_FORMAT, ModificationType, _position, _length, _linesAddedCount, _text, _isUserChange, _markerChangedLine) + Environment.NewLine + UndoRedoFlags.ToString();
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Returns true if the change was a direct result of user interaction
        /// </summary>
        public bool IsUserChange
        {
            get
            {
                return _isUserChange;
            }
        }


        /// <summary>
        ///     Returns the length of the change occured.
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
        }


        /// <summary>
        ///     Returns the # of lines added or removed as a result of the change
        /// </summary>
        public int LinesAddedCount
        {
            get
            {
                return _linesAddedCount;
            }
        }


        /// <summary>
        ///     Returns the line # of where the marker change occured (if applicable)
        /// </summary>
        public int MarkerChangedLine
        {
            get
            {
                return _markerChangedLine;
            }
        }


        /// <summary>
        ///     Returns the document position where the change occured
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
        }


        /// <summary>
        ///     The affected text of the change
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the TextModifiedEventArgs class.
        /// </summary>
        /// <param name="position">document position where the change occured</param>
        /// <param name="_length">_length of the change occured</param>
        /// <param name="linesAddedCount">the # of lines added or removed as a result of the change</param>
        /// <param name="text">affected text of the change</param>
        /// <param name="isUserChange">true if the change was a direct result of user interaction</param>
        /// <param name="markerChangedLine"> the line # of where the marker change occured (if applicable)</param>
        public TextModifiedEventArgs(int modificationType, bool isUserChange, int markerChangedLine, int position, int length, int linesAddedCount, string text) : base(modificationType)
        {
            _isUserChange = isUserChange;
            _markerChangedLine = markerChangedLine;
            _position = position;
            _length = length;
            _linesAddedCount = linesAddedCount;
            _text = text;
        }

        #endregion Constructors
    }
}
