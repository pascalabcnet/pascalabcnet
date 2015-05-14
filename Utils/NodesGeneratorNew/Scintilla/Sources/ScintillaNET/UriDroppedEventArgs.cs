#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    /// Provides data for the UriDropped event
    /// </summary>
    public class UriDroppedEventArgs : EventArgs
    {
        #region Fields

        //	I decided to leave it a string because I can't really
        //	be sure it is a Uri.
        private string _uriText;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Text of the dropped file or uri
        /// </summary>
        public string UriText
        {
            get { return _uriText; }
            set { _uriText = value; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the UriDroppedEventArgs class.
        /// </summary>
        /// <param name="uriText">Text of the dropped file or uri</param>
        public UriDroppedEventArgs(string uriText)
        {
            _uriText = uriText;
        }

        #endregion Constructors
    }
}
