#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion Using Directive


namespace ScintillaNET
{
    /// <summary>
    ///     Provides a writer paradigm for building a <see cref="StyleRun" /> list and optionally
    ///     the text that is being styled.
    /// </summary>
    public class StyleRunWriter
    {
        #region Fields

        private List<StyleRun> _styleRuns;
        private StringBuilder _stringBuilder;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Returns the underlying <see cref="StringBuilder" />.
        /// </summary>
        /// <returns>The underlying <see cref="StringBuilder" /> if one was provided; otherwise, null.</returns>
        public StringBuilder GetStringBuilder()
        {
            return _stringBuilder;
        }


        /// <summary>
        ///     Returns a <see cref="StyleRun" /> enumerable built by the <see cref="StyleRunWriter" /> thus far.
        /// </summary>
        /// <returns>A <see cref="StyleRun" /> enumerable representing the style runs written thus far.</returns>
        public IEnumerable<StyleRun> GetStyles()
        {
            return _styleRuns.ToArray();
        }


        //public IEnumerable<StyleRun> GetStyles(Encoding encoding = null)
        //{
        //    // If we don't have access to the text or aren't given an
        //    // encoding then the runs are ready to go.
        //    if (_stringBuilder == null || encoding == null)
        //        return _styleRuns.ToArray();

        //    // Adjust the style runs so that they represent byte lengths
        //    string text = _stringBuilder.ToString();
        //    List<StyleRun> byteRuns = new List<StyleRun>();
        //    int offset = 0;

        //    unsafe
        //    {
        //        fixed (char* cp = text)
        //        {
        //            for (int i = 0; i < _styleRuns.Count; i++)
        //            {
        //                StyleRun sr = _styleRuns[i];
        //                byteRuns.Add(new StyleRun(encoding.GetByteCount(cp + offset, sr.Length), sr.Style));
        //                offset += sr.Length;
        //            }
        //        }
        //    }

        //    return byteRuns;
        //}


        /// <summary>
        ///     Writes a run of the specified string length in the specified style.
        /// </summary>
        /// <param name="value">
        ///     The string that determines the run length. If a <see cref="StringBuilder" /> was used to
        ///     create the <see cref="StyleRunWriter" /> the string value will also be appended.
        /// </param>
        /// <param name="style">The zero-based index of the style for this run.</param>
        public void Write(string value, int style)
        {
            if (string.IsNullOrEmpty(value))
                return;

            _styleRuns.Add(new StyleRun(value.Length, style));
            if (_stringBuilder != null)
                _stringBuilder.Append(value);
        }

        #endregion Methods


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StyleRunWriter" /> class.
        /// </summary>
        /// <param name="stringBuilder">The optional <see cref="StringBuilder" /> to write to.</param>
        public StyleRunWriter(StringBuilder stringBuilder = null)
        {
            _styleRuns = new List<StyleRun>();
            _stringBuilder = stringBuilder;
        }

        #endregion Constructors
    }
}
