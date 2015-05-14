#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides data for the <see cref="Scintilla.AnnotationChanged" /> event.
    /// </summary>
    public class AnnotationChangedEventArgs : EventArgs
    {
        #region Fields

        private int _lineIndex;
        private int _lineCountDelta;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Gets the number of lines added to or removed from the changed annotation.
        /// </summary>
        /// <returns>
        ///     An <see cref="Int32" /> representing the number of lines added to or removed from the annotation.
        ///     Postive values indicate lines have been added; negative values indicate lines have been removed.
        /// </returns>
        public int LineCountDelta
        {
            get
            {
                return _lineCountDelta;
            }
        }


        /// <summary>
        ///     Gets the index of the document line containing the changed annotation.
        /// </summary>
        /// <returns>The zero-based index of the document line containing the changed annotation.</returns>
        public int LineIndex
        {
            get
            {
                return _lineIndex;
            }
        }

        #endregion Properties


        #region Consructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnnotationChangedEventArgs" /> class.
        /// </summary>
        /// <param name="lineIndex">The document line index of the annotation that changed.</param>
        /// <param name="lineCountDelta">The number of lines added to or removed from the annotation that changed.</param>
        public AnnotationChangedEventArgs(int lineIndex, int lineCountDelta)
        {
            _lineIndex = lineIndex;
        }

        #endregion Constructors
    }
}
