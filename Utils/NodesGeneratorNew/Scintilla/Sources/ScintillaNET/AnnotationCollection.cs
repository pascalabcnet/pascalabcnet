#region Using Directives

using System;
using System.Collections;
using System.ComponentModel;
using ScintillaNET.Design;
using ScintillaNET.Properties;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents a collection of <see cref="Annotation" /> objects and options in a <see cref="Scintilla" /> control.
    /// </summary>
    /// <remarks>
    ///     Annotations are customizable read-only blocks of text which can be displayed below
    ///     each line in a <see cref="Scintilla" /> control.
    /// </remarks>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class AnnotationCollection : /*ICollection,*/ IEnumerable
    {
        #region Fields

        private Scintilla _scintilla;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Removes all annotations from the document.
        /// </summary>
        /// <remarks>This is equivalent to setting the <see cref="Annotation.Text" /> property to null for each line.</remarks>
        public virtual void ClearAll()
        {
            _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONCLEARALL, IntPtr.Zero, IntPtr.Zero);
        }


        /*
        /// <summary>
        ///     Copies the entire <see cref="AnnotationCollection" /> to a compatible one-dimensional <see cref="Array" />.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array" /> that is the destination of the annotations.</param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRange"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="array" /> is multidimensional. -or-
        ///     The number of annotations in the source <see cref="AnnotationCollection" /> is greater than
        ///     the available space from <paramref name="index" /> to the end of the destination <paramref name="array" />.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     The type of the source <see cref="AnnotationCollection" /> cannot be cast
        ///     automatically to the type of the destination <paramref name="array" />.
        /// </exception>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (index < 0 || index > array.Length)
                throw new ArgumentOutOfRangeException("index", Resources.Exception_IndexOutOfRange);

            if (array.Length - index < Count)
                throw new ArgumentException(Resources.Exception_InsufficientSpace);

            for (int i = 0; i < Count; i++)
                array.SetValue(this[i], index + i);
        }
        */


        /// <summary>
        ///     Creates and returns a new <see cref="Annotation" /> object.
        /// </summary>
        /// <returns>A new <see cref="Annotation" /> object.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual Annotation CreateAnnotationInstance(int lineIndex)
        {
            return new Annotation(_scintilla, lineIndex);
        }


        /// <summary>
        ///     Returns an enumerator for the <see cref="AnnotationCollection" />.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> for the <see cref="AnnotationCollection" />.</returns>
        public virtual IEnumerator GetEnumerator()
        {
            return new AnnotationCollectionEnumerator(this);
        }


        internal bool ShouldSerialize()
        {
            return Visibility != AnnotationsVisibility.Hidden;
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets the number of annotations in the <see cref="AnnotationCollection" />.
        /// </summary>
        /// <returns>The number of annotations contained in the <see cref="AnnotationCollection" />.</returns>
        /// <remarks>
        ///     As there can be one annotation per document line,
        ///     this is equivalent to the <see cref="LineCollection.Count" /> property.
        /// </remarks>
        [Browsable(false)]
        public int Count
        {
            get
            {
                return _scintilla.Lines.Count;
            }
        }


        /*
        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return this;
            }
        }
        */


        /// <summary>
        ///     Gets or sets the offset applied to style indexes used in annotations.
        /// </summary>
        /// <returns>The offset applied to style indexes used in annotations.</returns>
        /// <remarks>
        ///     Annotation styles may be completely separated from standard text styles by setting a style offset.
        ///     For example, a value of 512 would shift the range of possible annotation styles to be from 512 to 767
        ///     so they do not overlap with standard text styles. This adjustment is applied automatically when setting
        ///     <see cref="Annotation.Style" /> or calling <see cref="Annotation.SetStyles" /> so the offset should NOT be
        ///     manually factored in by the caller. This property is provided to maintain architectural symmetry with
        ///     the native Scintilla component but is an advanced feature and typically should never need to be changed.
        /// </remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int StyleOffset
        {
            get
            {
                return _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETSTYLEOFFSET, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            set
            {
                // I contemplated throwing an exception if the argument is out of range, however, this being an
                // advanced feature I'm going to leave it for the advanced user to figure out for now.
                _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETSTYLEOFFSET, new IntPtr(value), IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets the visibility style for all annotations.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="AnnotationsVisibility" /> values.
        ///     The default is <see cref="AnnotationsVisibility.Hidden" />.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="AnnotationsVisibility" /> values.
        /// </exception>
        [DefaultValue(AnnotationsVisibility.Hidden)]
        [Description("Indicates the visibility and appearance of annotations.")] // TODO Move to resource file
        public AnnotationsVisibility Visibility
        {
            get
            {
                return (AnnotationsVisibility)_scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONGETVISIBLE, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                if (!Enum.IsDefined(typeof(AnnotationsVisibility), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(AnnotationsVisibility));

                _scintilla.DirectMessage(NativeMethods.SCI_ANNOTATIONSETVISIBLE, new IntPtr((int)value), IntPtr.Zero);
            }
        }

        #endregion Properties


        #region Indexers

        /// <summary>
        ///     Gets the annotation at the specified line index.
        /// </summary>
        /// <param name="lineIndex">The zero-based document line index of the annotation to get.</param>
        /// <returns>The <see cref="Annotation" /> at the specified line index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="lineIndex" /> is less than zero. -or-
        ///     <paramref name="lineIndex" /> is equal to or greater than <see cref="Count" />.
        /// </exception>
        public Annotation this[int lineIndex]
        {
            get
            {
                if (lineIndex < 0 || lineIndex > Count - 1)
                    throw new ArgumentOutOfRangeException("lineIndex", Resources.Exception_IndexOutOfRange);

                // Use our Create method so others can override this class and provide custom annotations
                return CreateAnnotationInstance(lineIndex);
            }
        }

        #endregion Indexers


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnnotationCollection" /> class.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla" /> control that created this object.</param>
        protected internal AnnotationCollection(Scintilla scintilla)
        {
            _scintilla = scintilla;
        }

        #endregion Constructors


        #region Types

        // Just enough to get the job done...
        private class AnnotationCollectionEnumerator : IEnumerator
        {
            private AnnotationCollection _collection;
            private int _index;

            public bool MoveNext()
            {
                _index++;

                if (_index >= _collection.Count)
                    return false;

                return true;
            }


            public void Reset()
            {
                _index = -1;
            }


            public object Current
            {
                get
                {
                    if (_index == -1)
                        throw new InvalidOperationException(Resources.Exception_EnumeratorNotStarted);

                    try
                    {
                        return _collection[_index];
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        throw new InvalidOperationException(Resources.Exception_EnumeratorEnded);
                    }
                }
            }


            public AnnotationCollectionEnumerator(AnnotationCollection collection)
            {
                _collection = collection;
                Reset();
            }
        }

        #endregion Types
    }
}
