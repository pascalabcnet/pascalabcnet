#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the MarginClick event
    /// </summary>
    public class MarginClickEventArgs : EventArgs
    {
        #region Fields

        private Line _line;
        private Margin _margin;
        private Keys _modifiers;
        private int _position;
        private bool _toggleFold;
        private int _toggleMarkerNumber;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Returns the Document line # where the click occured
        /// </summary>
        public Line Line
        {
            get
            {
                return _line;
            }
        }


        /// <summary>
        ///     Returns the Margin where the click occured
        /// </summary>
        public Margin Margin
        {
            get
            {
                return _margin;
            }
        }


        /// <summary>
        ///     Returns any Modifier keys (shift, alt, ctrl) that were in use at the
        ///     time the click event occured
        /// </summary>
        public Keys Modifiers
        {
            get
            {
                return _modifiers;
            }
        }


        /// <summary>
        ///     Returns the Document position of the line where the click occured
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }
        }


        /// <summary>
        ///     Gets/Sets whether the fold at the current line should be toggled
        /// </summary>
        public bool ToggleFold
        {
            get
            {
                return _toggleFold;
            }
            set
            {
                _toggleFold = value;
            }
        }


        /// <summary>
        ///     Gets/Sets the marker number that should be toggled in result of the click
        /// </summary>
        public int ToggleMarkerNumber
        {
            get
            {
                return _toggleMarkerNumber;
            }
            set
            {
                _toggleMarkerNumber = value;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the MarginClickEventArgs class.
        /// </summary>
        /// <param name="modifiers">
        ///     Any Modifier keys (shift, alt, ctrl) that were in use at the
        ///     time the click event occured
        /// </param>
        /// <param name="position">Document position of the line where the click occured</param>
        /// <param name="line">Document line # where the click occured</param>
        /// <param name="margin">Margin where the click occured</param>
        /// <param name="toggleMarkerNumber"> marker number that should be toggled in result of the click</param>
        /// <param name="toggleFold">Whether the fold at the current line should be toggled</param>
        public MarginClickEventArgs(Keys modifiers, int position, Line line, Margin margin, int toggleMarkerNumber, bool toggleFold)
        {
            _modifiers = modifiers;
            _position = position;
            _line = line;
            _margin = margin;
            _toggleMarkerNumber = toggleMarkerNumber;
            _toggleFold = toggleFold;
        }

        #endregion Constructors
    } 
}
