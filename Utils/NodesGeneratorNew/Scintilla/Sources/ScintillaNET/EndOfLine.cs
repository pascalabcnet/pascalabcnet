#region Using Directives

using System;
using System.ComponentModel;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Manages End of line settings for the Scintilla Control
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class EndOfLine : TopLevelHelper
    {
        #region Methods

        /// <summary>
        ///     Converts all lines in the document to the given mode.
        /// </summary>
        /// <param name="toMode">The EndOfLineMode to convert all lines to </param>
        public void ConvertAllLines(EndOfLineMode toMode)
        {
            NativeScintilla.ConvertEols((int)toMode);
        }


        private void ResetIsVisible()
        {
            IsVisible = false;
        }


        private void ResetMode()
        {
            Mode = EndOfLineMode.Crlf;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeIsVisible() || ShouldSerializeMode();
        }


        private bool ShouldSerializeIsVisible()
        {
            return IsVisible;
        }


        private bool ShouldSerializeMode()
        {
            // Yeah I'm assuming Windows, if this does ever make it to another platform 
            // a check should be made to make it platform specific
            return Mode != EndOfLineMode.Crlf;
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Return as a string the characters used to mean _end-of-line. This depends solely on the
        ///     selected EOL mode.
        /// </summary>
        /// <remarks>Should Mode not be CR, LF or CrLf, this function returns the empty string.</remarks>
        public string EolString
        {
            get
            {
                switch (Mode)
                {
                    case EndOfLineMode.CR:
                        return "\r";
                    case EndOfLineMode.LF:
                        return "\n";
                    case EndOfLineMode.Crlf:
                        return "\r\n";
                }
                return "";
            }
        }


        /// <summary>
        ///     Gets/Sets if End of line markers are visible in the Scintilla control.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return NativeScintilla.GetViewEol();
            }
            set
            {
                NativeScintilla.SetViewEol(value);
            }
        }


        /// <summary>
        ///     Gets/Sets the <see cref="EndOfLineMode"/> for the document. Default is CrLf.
        /// </summary>
        /// <remarks>
        ///     Changing this value does NOT change all EOL marks in a currently-loaded document.
        ///     To do this, use <see cref="ConvertAllLines"/>ConvertAllLines.
        /// </remarks>
        public EndOfLineMode Mode
        {
            get
            {
                return (EndOfLineMode)NativeScintilla.GetEolMode();
            }
            set
            {
                NativeScintilla.SetEolMode((int)value);
            }
        }

        #endregion Properties


        #region Constructors

        internal EndOfLine(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
