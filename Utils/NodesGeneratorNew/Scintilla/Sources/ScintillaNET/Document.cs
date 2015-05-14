#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides an abstraction over Scintilla's Document Pointer
    /// </summary>
    public class Document : ScintillaHelperBase
    {
        #region Fields

        private IntPtr _handle;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Increases the document's reference count
        /// </summary>
        /// <remarks>No, you aren't looking at COM, move along.</remarks>
        public void AddRef()
        {
            NativeScintilla.AddRefDocument(_handle);
        }


        /// <summary>
        ///     Overridden. 
        /// </summary>
        /// <param name="obj">Another Document Object</param>
        /// <returns>True if both Documents have the same Handle</returns>
        public override bool Equals(object obj)
        {
            Document d = obj as Document;

            if (_handle == IntPtr.Zero)
                return false;

            return _handle.Equals(d._handle);
        }


        /// <summary>
        ///     Overridden
        /// </summary>
        /// <returns>Document Pointer's hashcode</returns>
        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }


        /// <summary>
        ///     Decreases the document's reference count
        /// </summary>
        /// <remarks>
        ///     When the document's reference count reaches 0 Scintilla will destroy the document
        /// </remarks>
        public void Release()
        {
            NativeScintilla.ReleaseDocument(_handle);
        }

        #endregion Methods


        #region Properties

        /// <summary>
        /// Scintilla's internal document pointer.
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
            set
            {
                _handle = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal Document(Scintilla scintilla, IntPtr handle) : base(scintilla) 
        {
            _handle = handle;
        }

        #endregion Constructors
    }
}
