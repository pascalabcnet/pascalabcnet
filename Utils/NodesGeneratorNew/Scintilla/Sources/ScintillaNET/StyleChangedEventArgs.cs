#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the StyleChanged event
    /// </summary>
    /// <remarks>
    ///     StyleChangedEventHandler is used for the StyleChanged Event which is also used as 
    ///     a more specific abstraction around the SCN_MODIFIED notification message.
    /// </remarks>
    public class StyleChangedEventArgs : ModifiedEventArgs
    {
        #region Fields

        private int _length;
        private int _position;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns how many characters have changed
        /// </summary>
        public int Length
        {
            get
            {
                return _length;
            }
        }


        /// <summary>
        ///     Returns the starting document position where the style has been changed
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
        }

        #endregion Properties


        #region Constructors

        internal StyleChangedEventArgs(int position, int length, int modificationType) : base(modificationType)
        {
            _position = position;
            _length = length;
        }

        #endregion Constructors
    }
}
