#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Manages the Native Scintilla's Document features.
    /// </summary>
    /// <remarks>
    ///     See Scintilla's documentation on multiple views for an understanding of Documents.
    ///     Note that all ScintillaNET specific features are considered to be part of the View, not document.
    /// </remarks>
    public class DocumentHandler : TopLevelHelper
    {
        #region Methods

        /// <summary>
        ///     Creates a new Document
        /// </summary>
        /// <returns></returns>
        public Document Create()
        {
            return new Document(Scintilla, NativeScintilla.CreateDocument());
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets/Sets the currently loaded Document
        /// </summary>
        public Document Current
        {
            get
            {
                return new Document(Scintilla, NativeScintilla.GetDocPointer());
            }
            set
            {
                NativeScintilla.SetDocPointer(value.Handle);
            }
        }

        #endregion Properties


        #region Constructors

        internal DocumentHandler(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
