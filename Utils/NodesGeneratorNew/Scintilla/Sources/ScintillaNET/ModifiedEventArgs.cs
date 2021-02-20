#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Base class for modified events
    /// </summary>
    /// <remarks>
    ///     ModifiedEventArgs is the base class for all events that are fired 
    ///     in response to an SCN_MODIFY notification message. They all have 
    ///     the Undo/Redo flags in common and I'm also including the raw 
    ///     modificationType integer value for convenience purposes.
    /// </remarks>
    public abstract class ModifiedEventArgs : EventArgs
    {
        #region Fields

        private int _modificationType;
        private UndoRedoFlags _undoRedoFlags;

        #endregion Fields


        #region Properties

        public int ModificationType
        {
            get
            {
                return _modificationType;
            }
            set
            {
                _modificationType = value;
            }
        }


        public UndoRedoFlags UndoRedoFlags
        {
            get
            {
                return _undoRedoFlags;
            }
            set
            {
                _undoRedoFlags = value;
            }
        }

        #endregion Properties


        #region Constructors

        public ModifiedEventArgs(int modificationType)
        {
            _modificationType = modificationType;
            _undoRedoFlags = new UndoRedoFlags(modificationType);
        }

        #endregion Constructors
    }
}
