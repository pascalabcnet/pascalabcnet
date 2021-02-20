#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    //  Used by TextModifiedEventArgs, StyeChangedEventArgs and FoldChangedEventArgs
    //  this provides a friendly wrapper around the SCNotification's modificationType
    //  flags having to do with Undo and Redo
    /// <summary>
    ///     Contains Undo/Redo information, used by many of the events
    /// </summary>
    public struct UndoRedoFlags
    {
        #region Constants

        private const string STRING_FORMAT = "IsUndo\t\t\t\t:{0}\r\nIsRedo\t\t\t\t:{1}\r\nIsMultiStep\t\t\t:{2}\r\nIsLastStep\t\t\t:{3}\r\nIsMultiLine\t\t\t:{4}";

        #endregion Constants


        #region Fields

        /// <summary>
        ///     Was this action the result of an undo action
        /// </summary>
        public bool IsUndo;

        /// <summary>
        ///     Was this action the result of a redo action
        /// </summary>
        public bool IsRedo;

        /// <summary>
        ///     Is this part of a multiple undo or redo
        /// </summary>
        public bool IsMultiStep;

        /// <summary>
        ///     Is this the last step in an undi or redo
        /// </summary>
        public bool IsLastStep;

        /// <summary>
        ///     Does this affect multiple lines
        /// </summary>
        public bool IsMultiLine;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Overridden
        /// </summary>
        public override string ToString()
        {
            return string.Format(STRING_FORMAT, IsUndo, IsRedo, IsMultiStep, IsLastStep, IsMultiLine);
        }

        #endregion Methods


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the UndoRedoFlags structure.
        /// </summary>
        /// <param name="isUndo">Was this action the result of an undo action</param>
        /// <param name="isRedo">Was this action the result of a redo action</param>
        /// <param name="isMultiStep">Is this part of a multiple undo or redo</param>
        /// <param name="isLastStep">Is this the last step in an undi or redo</param>
        /// <param name="isMultiLine">Does this affect multiple lines</param>
        public UndoRedoFlags(int modificationType)
        {
            IsLastStep = (modificationType & Constants.SC_LASTSTEPINUNDOREDO) > 0;
            IsMultiLine = (modificationType & Constants.SC_MULTILINEUNDOREDO) > 0;
            IsMultiStep = (modificationType & Constants.SC_MULTISTEPUNDOREDO) > 0;
            IsRedo = (modificationType & Constants.SC_PERFORMED_REDO) > 0;
            IsUndo = (modificationType & Constants.SC_PERFORMED_UNDO) > 0;
        }

        #endregion Constructors
    }
}
