#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using ScintillaNET.Design;
using ScintillaNET.Properties;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Controls line wrapping options in a <see cref="Scintilla" /> control.
    /// </summary>
    [TypeConverterAttribute(typeof(ExpandableObjectConverter))]
    public class LineWrapping
    {
        #region Fields

        private Scintilla _scintilla;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     The number of lines displayed when a line of text is wrapped.
        /// </summary>
        /// <param name="lineIndex">The zero-based index of the line to count.</param>
        /// <returns>The numbers of display lines the line of text occupies.</returns>
        ///// <exception cref="ArgumentOutOfRangeException">
        /////     <paramref name="lineIndex"/> is less than 0 or greater than the line count.
        ///// </exception>
        public virtual int GetWrapCount(int lineIndex)
        {
            //if (lineIndex < 0 || lineIndex > _scintilla.Lines.Count)
            //{
            //    string paramName = "lineIndex";
            //    string message = string.Format(CultureInfo.InvariantCulture, Resources.Exception_MustBeNonNegativeAndLessThan, paramName, "the line count");
            //    throw new ArgumentOutOfRangeException(paramName, message);
            //}

            return _scintilla.DirectMessage(NativeMethods.SCI_WRAPCOUNT, new IntPtr(lineIndex), IntPtr.Zero).ToInt32();
        }


        internal bool ShouldSerialize()
        {
            return
                IndentMode != LineWrappingIndentMode.Fixed ||
                IndentSize != 0 ||
                Mode != LineWrappingMode.None ||
                VisualFlags != LineWrappingVisualFlags.None ||
                VisualFlagsLocations != LineWrappingVisualFlagsLocations.Default;
        }


        /// <summary>
        ///     Forces the line range specified to wrap at the given pixel width. This operates independently
        ///     of the current <see cref="Scintilla"/> line wrapping <see cref="Mode"/> property.
        /// </summary>
        /// <param name="startLine">The zero-based line index to start wrapping.</param>
        /// <param name="endLine">The zero-based line index to stop wrapping.</param>
        /// <param name="width">
        ///     The maximum width in pixels of the lines to wrap. A value of zero resets forced line wrapping.
        /// </param>
        ///// <exception cref="ArgumentOutOfRangeException">
        /////     <paramref name="startLine"/> and <paramref name="endLine"/> do not specify a valid
        /////     range of lines within the document.
        ///// </exception>
        public virtual void WrapLines(int startLine, int endLine, int width)
        {
            //if (startLine < 0)
            //    throw new ArgumentOutOfRangeException("startLine", Resources.Exception_InvalidStartLine);
            //if (endLine < startLine)
            //    throw new ArgumentOutOfRangeException("endLine", Resources.Exception_InvalidLineRange);

            // Convert line indexes to positions within the line
            int startPos = _scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(startLine), IntPtr.Zero).ToInt32();
            int endPos = _scintilla.DirectMessage(NativeMethods.SCI_POSITIONFROMLINE, new IntPtr(endLine), IntPtr.Zero).ToInt32();

            //if (startPos == -1)
            //    throw new ArgumentOutOfRangeException("startLine", string.Format(CultureInfo.InvariantCulture, Resources.Exception_InvalidLine, "start"));
            //if (endPos == -1)
            //    throw new ArgumentOutOfRangeException("endLine", string.Format(CultureInfo.InvariantCulture, Resources.Exception_InvalidLine, "end"));

            // Set the target positions (which Scintilla will convert back to line indexes)
            _scintilla.DirectMessage(NativeMethods.SCI_SETTARGETSTART, new IntPtr(startPos), IntPtr.Zero);
            _scintilla.DirectMessage(NativeMethods.SCI_SETTARGETEND, new IntPtr(endPos), IntPtr.Zero);

            _scintilla.DirectMessage(NativeMethods.SCI_LINESSPLIT, new IntPtr(width), IntPtr.Zero);
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets or sets how wrapped lines are indented.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="LineWrappingIndentMode" /> values.
        ///     The default is <see cref="LineWrappingIndentMode.Fixed" />.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="LineWrappingIndentMode" /> values.
        /// </exception>
        [DefaultValue(LineWrappingIndentMode.Fixed)]
        [Description("Indicates how wrapped lines are indented.")] // TODO Move to resource file.
        public virtual LineWrappingIndentMode IndentMode
        {
            get
            {
                return (LineWrappingIndentMode)_scintilla.DirectMessage(
                    NativeMethods.SCI_GETWRAPINDENTMODE, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                if (!Enum.IsDefined(typeof(LineWrappingIndentMode), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(LineWrappingIndentMode));

                _scintilla.DirectMessage(NativeMethods.SCI_SETWRAPINDENTMODE, new IntPtr((int)value), IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets the size that wrapped lines are indented when <see cref="IndentMode" /> is <see cref="LineWrappingIndentMode.Fixed" />.
        /// </summary>
        /// <returns>An <see cref="Int32" /> representing the size (in characters) that wrapped lines are indented.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The value is less that zero or greater than 256.</exception>
        [DefaultValue(0)]
        [Description("The indentation size of wrapped lines.")] // TODO Move to resource file.
        public virtual int IndentSize
        {
            get
            {
                return _scintilla.DirectMessage(NativeMethods.SCI_GETWRAPSTARTINDENT, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            set
            {
                // A max of 256 is something I came up with myself.
                // It's probably way too high, but I would rather that than make it too low.
                const int max = 256;
                if (value < 0 || value > max)
                {
                    const string paramName = "value";
                    string message = string.Format(CultureInfo.InvariantCulture, Resources.Exception_MustBeNonNegativeAndLessThan, paramName, max);
                    throw new ArgumentOutOfRangeException(paramName, message);
                }

                _scintilla.DirectMessage(NativeMethods.SCI_SETWRAPSTARTINDENT, new IntPtr(value), IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets how and whether line wrapping is performed.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="LineWrappingMode" /> values.
        ///     The default is <see cref="LineWrappingMode.None" />.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="LineWrappingMode" /> values.
        /// </exception>
        [DefaultValue(LineWrappingMode.None)]
        [Description("Specifies how and whether line wrapping is performed.")] // TODO Move to resource file.
        public virtual LineWrappingMode Mode
        {
            get
            {
                return (LineWrappingMode)_scintilla.DirectMessage(
                    NativeMethods.SCI_GETWRAPMODE, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                if (!Enum.IsDefined(typeof(LineWrappingMode), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(LineWrappingMode));

                _scintilla.DirectMessage(NativeMethods.SCI_SETWRAPMODE, new IntPtr((int)value), IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets the visual glyphs displayed on wrapped lines.
        /// </summary>
        /// <returns>
        ///     A bitwise combination of the <see cref="LineWrappingVisualFlags" /> values.
        ///     The default is <see cref="LineWrappingVisualFlags.None" />.
        /// </returns>
        [DefaultValue(LineWrappingVisualFlags.None)]
        [Description("Specifies the visual indicators on wrapped lines.")] // TODO Move to resource file.
        [Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))]
        public virtual LineWrappingVisualFlags VisualFlags
        {
            get
            {
                return (LineWrappingVisualFlags)_scintilla.DirectMessage(
                    NativeMethods.SCI_GETWRAPVISUALFLAGS, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                _scintilla.DirectMessage(NativeMethods.SCI_SETWRAPVISUALFLAGS, new IntPtr((int)value), IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets the location of visual glyphs displayed on wrapped lines.
        /// </summary>
        /// <returns>
        ///     A bitwise combination of the <see cref="LineWrappingVisualFlagsLocations" /> values.
        ///     The default is <see cref="LineWrappingVisualFlagsLocations.Default" />.
        /// </returns>
        [DefaultValue(LineWrappingVisualFlagsLocations.Default)]
        [Description("Specifies the location of visual indicators on wrapped lines.")] // TODO Move to resource file.
        [Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))]
        public virtual LineWrappingVisualFlagsLocations VisualFlagsLocations
        {
            get
            {
                return (LineWrappingVisualFlagsLocations)_scintilla.DirectMessage(
                    NativeMethods.SCI_GETWRAPVISUALFLAGSLOCATION, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                _scintilla.DirectMessage(NativeMethods.SCI_SETWRAPVISUALFLAGSLOCATION, new IntPtr((int)value), IntPtr.Zero);
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LineWrapping" /> class.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla" /> control that created this object.</param>
        protected internal LineWrapping(Scintilla scintilla)
        {
            _scintilla = scintilla;
        }

        #endregion Constructors
    }
}
