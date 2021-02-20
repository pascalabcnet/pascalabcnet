#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the LinesNeedShown event
    /// </summary>
    public class LinesNeedShownEventArgs : EventArgs
    {
        #region Fields

        private int _firstLine;
        private int _lastLine;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns the first (top) line that needs to be shown
        /// </summary>
        public int FirstLine
        {
            get { return _firstLine; }
        }


        /// <summary>
        ///     Returns the last (bottom) line that needs to be shown
        /// </summary>
        public int LastLine
        {
            get { return _lastLine; }
            set { _lastLine = value; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the LinesNeedShownEventArgs class.
        /// </summary>
        /// <param name="startLine">the first (top) line that needs to be shown</param>
        /// <param name="endLine">the last (bottom) line that needs to be shown</param>
        public LinesNeedShownEventArgs(int startLine, int endLine)
        {
            _firstLine = startLine;
            _lastLine = endLine;
        }

        #endregion Constructors
    }
}
