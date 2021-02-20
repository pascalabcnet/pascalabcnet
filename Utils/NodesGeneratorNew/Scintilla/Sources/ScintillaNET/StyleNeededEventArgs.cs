#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    /// Provides data for the StyleNeeded event
    /// </summary>
    public class StyleNeededEventArgs : EventArgs
    {
        #region Fields

        private Range _range;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns the document range that needs styling
        /// </summary>
        public Range Range
        {
            get { return _range; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the StyleNeededEventArgs class.
        /// </summary>
        /// <param name="range">the document range that needs styling</param>
        public StyleNeededEventArgs(Range range)
        {
            _range = range;
        }

        #endregion Constructors
    }
}
