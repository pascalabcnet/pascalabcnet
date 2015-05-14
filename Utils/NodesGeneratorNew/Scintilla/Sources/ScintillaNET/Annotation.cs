#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using ScintillaNET.Properties;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents a customizable read-only block of text which can be displayed below
    ///     each line in a <see cref="Scintilla" /> control.
    /// </summary>
    public class Annotation
    {
        #region Constants

        private const int INDIVIDUAL_STYLES = 0x100; // From the Scintilla source code: "implies array of styles"

        #endregion Constants


        #region Fields

        private Scintilla _scintilla;
        private int _lineIndex;

        #endregion Fields


        #region Methods

        private void CheckInvalid()
        {
            // Are we in a state where we can no longer accurately
            // represent the annotation we were originally created for?
            if (_lineIndex == -1)
                throw new InvalidOperationException(Resources.Exception_InvalidAnnotation);
        }


        /// <summary>
        ///     Removes all text and styles associated with the annotation.
        /// </summary>
        public virtual void Clear()
        {
            CheckInvalid();

            // Remove the annotation
            _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETTEXT, new IntPtr(_lineIndex), IntPtr.Zero);
        }


        /// <summary>
        ///     Overridden. Determines whether the specified <see cref="Object" /> is equal to the current <see cref="Object" />.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///     true if the specified <see cref="Object" /> is equal to the
        ///     current <see cref="Object" />; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is Annotation)
            {
                // If another annotation has the same Scintilla
                // control and line index--it is the same.
                Annotation a = (Annotation)obj;
                if (a._scintilla == _scintilla && a._lineIndex == _lineIndex)
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     Determines whether the specified <see cref="Annotation" /> is equal to the current <see cref="Annotation" />.
        /// </summary>
        /// <param name="a">The annotation to compare with the current annotation.</param>
        /// <returns>
        ///     true if the specified <see cref="Annotation" /> is equal to the
        ///     current <see cref="Annotation" />; otherwise, false.
        /// </returns>
        public virtual bool Equals(Annotation a)
        {
            // Per Microsoft's recommendations we have an Equals for
            // our specific type "to enhance performance".

            if (a != null)
            {
                // Just remember to keep in sync with standard Equals above
                if (a._scintilla == _scintilla && a._lineIndex == _lineIndex)
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     Overridden. Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Object" />.</returns>
        public override int GetHashCode()
        {
            return _scintilla.GetHashCode() ^ _lineIndex;
        }


        /// <summary>
        ///     Returns a <see cref="StyleRun" /> enumerable representing the individual character styling of the annotation text.
        /// </summary>
        /// <returns>
        ///     A <see cref="StyleRun" /> enumerable representing the individual character styling,
        ///     where the <see cref="StyleRun.Length" /> property of each run represents the number
        ///     of characters the run spans.
        /// </returns>
        public virtual IEnumerable<StyleRun> GetStyles()
        {
            CheckInvalid();

            // We need to translate the array Scintilla gives us representing the style of each text
            // byte into a list of style runs. Our run lengths, however, are measured in characters,
            // not bytes, so we need to also read the annotation text and adjust as necessary when we find
            // characters that span more than one byte.

            int length = _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), IntPtr.Zero).ToInt32();
            byte[] textBuffer = new byte[length];
            byte[] stylesBuffer = new byte[length];

            unsafe
            {
                fixed (byte* bp = textBuffer)
                    _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), new IntPtr(bp)).ToInt32();
                fixed (byte* bp = stylesBuffer)
                    _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETSTYLES, new IntPtr(_lineIndex), new IntPtr(bp)).ToInt32();
            }

            List<StyleRun> styles = new List<StyleRun>();
            Decoder decoder = _scintilla.Encoding.GetDecoder();
            StyleRun sr = new StyleRun() { Style = -1 };
            int index = 0;
            int count = 1;

            while (index < stylesBuffer.Length)
            {
                if (sr.Style != stylesBuffer[index])
                {
                    // A new style has been encountered. Save the last one
                    // to the list we're building and start tracking a new one
                    if (sr.Length > 0)
                        styles.Add(sr);

                    sr = new StyleRun();
                    sr.Style = stylesBuffer[index];
                }

                // At the end of this loop, the 'count' variable will tell us
                // how many bytes there are for one character.
                while (decoder.GetCharCount(textBuffer, index, count) != 1)
                    count++;

                sr.Length++;
                index += count;
                count = 1;
            }

            // Add the last style run
            styles.Add(sr);

            return styles.ToArray();
        }


        /*
        private void ScintillaTextChangedHandler(object sender, EventArgs e)
        {
            // Why listen for text change events? We're not meant to be a long living
            // object. Should any user forget that and assume we're still valid after
            // the text has changed we'll remind them. In the future we could optimze
            // this by only invalidating if our line has been changed (deleted).
            _scintilla.TextChanged -= new EventHandler(ScintillaTextChangedHandler);
            _lineIndex = -1;
        }
        */


        /// <summary>
        ///     Uses the enumerable <see cref="StyleRun" /> specified to individually style characters in the annotation text.
        /// </summary>
        /// <param name="styles">
        ///     The enumerable <see cref="StyleRun" /> indicating how to style the annotation text,
        ///     where the <see cref="StyleRun.Length" /> property of each run represents the number
        ///     of characters the run spans.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="styles" /> is null.</exception>
        /// <remarks>
        ///     The <see cref="Text" /> property must be set prior to styling and the sum length of
        ///     all runs should match the text length.
        /// </remarks>
        public virtual void SetStyles(IEnumerable<StyleRun> styles)
        {
            CheckInvalid();

            if (styles == null)
                throw new ArgumentNullException("styles");

            // We need to build a byte array with a style byte for each text byte.
            // Our style runs are in character lengths (good for our user) but we need to
            // convert them to byte lengths (bad for us). To do that we need to analyze the
            // annotation text and determine if any of our character lengths span more than
            // one byte and fill the array accordingly.

            int length = _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), IntPtr.Zero).ToInt32();
            byte[] textBuffer = new byte[length];
            byte[] stylesBuffer = new byte[length];

            unsafe
            {
                fixed (byte* bp = textBuffer)
                    _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), new IntPtr(bp)).ToInt32();
            }

            Decoder decoder = _scintilla.Encoding.GetDecoder();
            StyleRun sr;
            int index = 0;
            int count = 1;

            using (IEnumerator<StyleRun> enumerator = styles.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    sr = enumerator.Current;
                    while (sr.Length > 0 && index < stylesBuffer.Length)
                    {
                        // At the end of this loop, the 'count' variable will tell us
                        // how many bytes there are for one character.
                        while (decoder.GetCharCount(textBuffer, index, count) != 1)
                            count++;

                        // For each character of text (the unit of our style runs)
                        // add the appropriate number of style bytes.
                        for (int i = 0; i < count; i++)
                            stylesBuffer[index + i] = (byte)sr.Style;

                        index += count;
                        count = 1;
                        sr.Length--;
                    };
                }
            }

            // Our processing above is designed to stop if we are given more style run data
            // than we have bytes to fill. If we are given less style run data than we have
            // bytes to fill, the remaining bytes just get their default value of 0.

            unsafe
            {
                fixed (byte* bp = stylesBuffer)
                    _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETSTYLES, new IntPtr(_lineIndex), new IntPtr(bp));
            }
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets the total number of text lines in the annotation.
        /// </summary>
        /// <remarks>An  <see cref="Int32" /> representing the total number of text lines in the annotation.</remarks>
        public virtual int LineCount
        {
            get
            {
                CheckInvalid();
                return _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETLINES, new IntPtr(_lineIndex), IntPtr.Zero).ToInt32();
            }
        }


        /// <summary>
        ///     Gets the index of the document line containing the annotation.
        /// </summary>
        /// <returns>
        ///     An <see cref="Int32" /> representing the zero-based index of the document line
        ///     containing the annotation, or -1 if the annotation has been rendered invalid
        ///     from a change in the <see cref="Scintilla" /> control that created it.
        /// </returns>
        public int LineIndex
        {
            get
            {
                return _lineIndex;
            }
        }


        /// <summary>
        ///     Gets or sets the index of the style used to style the annotation text.
        /// </summary>
        /// <returns>
        ///     An <see cref="Int32" /> representing the zero-based index of the style used to style the annotation text,
        ///     or -1 if the annotation has individually style characters.
        /// </returns>
        public virtual int Style
        {
            get
            {
                CheckInvalid();

                // By default Scintilla will return a value of 256 when there are individual styles. In the .NET
                // world that would be a little weird for our users. A more common pattern would be to return -1.
                int style = _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETSTYLE, new IntPtr(_lineIndex), IntPtr.Zero).ToInt32();
                if (style == INDIVIDUAL_STYLES)
                    return -1;

                return style;
            }
            set
            {
                CheckInvalid();
                _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETSTYLE, new IntPtr(_lineIndex), new IntPtr(value)).ToInt32();
            }
        }


        /// <summary>
        ///     Gets or sets the text of the annotation.
        /// </summary>
        /// <returns>A <see cref="String" /> representing the annotation text, or null if there is no annotation.</returns>
        /// <remarks>
        ///     Only line feed characters ('\n') are recognized as line breaks.
        ///     All other control characters are not rendered.
        /// </remarks>
        public virtual string Text
        {
            get
            {
                CheckInvalid();

                // Normally you wouldn't want to return null from a text property because
                // an empty string usually means the same thing and avoids null reference
                // exceptions. However, in Scintilla a null annotation is very different
                // from an empty string in the way they are rendered. For that reason we have
                // to support returning null from this property and this is the only reliable
                // way that I've found to do it.
                if (LineCount == 0)
                    return null;

                // Determine the buffer size, fill it, and convert it to a string
                int length = _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), IntPtr.Zero).ToInt32();
                byte[] buffer = new byte[length];
                unsafe
                {
                    fixed (byte* bp = buffer)
                        length = (int)_scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETTEXT, new IntPtr(_lineIndex), new IntPtr(bp)).ToInt32();
                }

                return _scintilla.Encoding.GetString(buffer, 0, length);
            }
            set
            {
                CheckInvalid();

                if (value == null)
                {
                    // Same thing...
                    Clear();
                    return;
                }

                unsafe
                {
                    // Set the annotation text
                    fixed (byte* bp = Utilities.GetZeroTerminatedBytes(value, _scintilla.Encoding))
                        _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETTEXT, new IntPtr(_lineIndex), new IntPtr(bp));
                }
            }
        }

        #endregion Properties


        #region Operators

        /// <summary>
        ///     Tests whether two <see cref="Annotation" /> object differ in location or content.
        /// </summary>
        /// <param name="left">The <see cref="Annotation" /> object that is to the left of the inequality operator.</param>
        /// <param name="right">The <see cref="Annotation" /> object that is to the right of the inequality operator.</param>
        /// <returns>true if the objects are considered unequal; otherwise, false.</returns>
        public static bool operator !=(Annotation left, Annotation right)
        {
            return !(left == right);
        }


        /// <summary>
        ///     Tests whether two <see cref="Annotation" /> objects have equal location and content.
        /// </summary>
        /// <param name="left">The <see cref="Annotation" /> object that is to the left of the equality operator.</param>
        /// <param name="right">The <see cref="Annotation" /> object that is to the right of the equality operator.</param>
        /// <returns>true if the objects are considered equal; otherwise, false.</returns>
        public static bool operator ==(Annotation left, Annotation right)
        {
            // If both are null, or both are same instance
            if (Object.ReferenceEquals(left, right))
                return true;

            // If one is null, but not both
            if (((object)left == null) || ((object)right == null))
                return false;

            return left.Equals((Annotation)right);
        }

        #endregion Operators


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Annotation" /> class.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla" /> control that created this object.</param>
        /// <param name="lineIndex">The zero-based index of the document line containing the annotation.</param>
        protected internal Annotation(Scintilla scintilla, int lineIndex)
        {
            _lineIndex = lineIndex;
            _scintilla = scintilla;
            /*_scintilla.TextChanged += new EventHandler(ScintillaTextChangedHandler);*/
        }

        #endregion Constructors
    }
}
