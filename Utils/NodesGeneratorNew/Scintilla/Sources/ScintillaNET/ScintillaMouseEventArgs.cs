#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for Scintilla mouse events
    /// </summary>
    public class ScintillaMouseEventArgs : EventArgs
    {
        #region Fields

        private int _position;
        private int _x;
        private int _y;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns the Document position
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }


        /// <summary>
        ///     Returns the X (left) position of mouse in pixels
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }


        /// <summary>
        ///     Returns the Y (top) position of mouse in pixels
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the ScintillaMouseEventArgs class.
        /// </summary>
        /// <param name="x">X (left) position of mouse in pixels</param>
        /// <param name="y">Y (top) position of mouse in pixels</param>
        /// <param name="position"> Document position</param>
        public ScintillaMouseEventArgs(int x, int y, int position)
        {
            _x = x;
            _y = y;
            _position = position;
        }

        #endregion Constructors
    }
}
