#region Using Directives

using System;
using System.ComponentModel;
using System.Security.Permissions;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Provides methods to place data on and retrieve data from the system Clipboard.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Clipboard : TopLevelHelper
    {
        #region Methods

        /// <summary>
        ///     Copies the current selection in the document to the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Copy()
        {
            Copy(false);
        }


        /// <summary>
        ///     Copies the current selection, or the current line if there is no selection, to the Clipboard.
        /// </summary>
        /// <param name="copyLine">
        ///     Indicates whether to copy the current line if there is no selection.
        /// </param>
        /// <remarks>
        ///     A line copied in this mode is given a "MSDEVLineSelect" marker when added to the Clipboard and
        ///     then used in the <see cref="Paste" /> method to paste the whole line before the current line.
        /// </remarks>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Copy(bool copyLine)
        {
            if (copyLine)
                Scintilla.DirectMessage(NativeMethods.SCI_COPYALLOWLINE, IntPtr.Zero, IntPtr.Zero);
            else
                Scintilla.DirectMessage(NativeMethods.SCI_COPY, IntPtr.Zero, IntPtr.Zero);
        }


        /// <summary>
        ///     Copies the specified range of text (bytes) in the document to the Clipboard.
        /// </summary>
        /// <param name="startPosition">The zero-based byte position to start copying.</param>
        /// <param name="endPosition">The zero-based byte position to stop copying.</param>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Copy(int startPosition, int endPosition)
        {
            Scintilla.DirectMessage(NativeMethods.SCI_COPYRANGE, new IntPtr(startPosition), new IntPtr(endPosition));
        }


        /// <summary>
        ///     Moves the current document selection to the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
        public void Cut()
        {
            Scintilla.DirectMessage(NativeMethods.SCI_CUT, IntPtr.Zero, IntPtr.Zero);
        }


        /// <summary>
        ///     Replaces the current document selection with the contents of the Clipboard.
        /// </summary>
        [UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.AllClipboard)]
        public void Paste()
        {
            Scintilla.DirectMessage(NativeMethods.SCI_PASTE, IntPtr.Zero, IntPtr.Zero);
        }


        internal bool ShouldSerialize()
        {
            return !ConvertLineBreaksOnPaste;
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets a value indicating whether text (bytes) can be copied given the current selection.
        /// </summary>
        /// <returns>true if the text can be copied; otherwise, false.</returns>
        /// <remarks>This is equivalent to determining if there is a valid selection.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanCopy
        {
            get
            {
                return (Scintilla.DirectMessage(NativeMethods.SCI_GETSELECTIONSTART, IntPtr.Zero, IntPtr.Zero) !=
                    Scintilla.DirectMessage(NativeMethods.SCI_GETSELECTIONEND, IntPtr.Zero, IntPtr.Zero));
            }
        }


        /// <summary>
        ///     Gets a value indicating whether text (bytes) can be cut given the current selection.
        /// </summary>
        /// <returns>true if the text can be cut; otherwise, false.</returns>
        /// <remarks>This is equivalent to determining if there is a valid selection.</remarks>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanCut
        {
            get
            {
                // Look familiar? :)
                return (Scintilla.DirectMessage(NativeMethods.SCI_GETSELECTIONSTART, IntPtr.Zero, IntPtr.Zero) !=
                    Scintilla.DirectMessage(NativeMethods.SCI_GETSELECTIONEND, IntPtr.Zero, IntPtr.Zero));
            }
        }


        /// <summary>
        ///     Gets a value indicating whether the document can accept text currently stored in the Clipboard.
        /// </summary>
        /// <returns>true if text can be pasted; otherwise, false.</returns>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanPaste
        {
            get
            {
                // TODO Check the Clipboard.ContainsText(*) method to determine if the text is compatible
                return (Scintilla.DirectMessage(NativeMethods.SCI_CANPASTE, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero);
            }
        }


        /// <summary>
        ///     Gets or sets whether pasted line break characters are converted to match the document's end-of-line mode.
        /// </summary>
        /// <returns>
        ///     true if line break characters are converted; otherwise, false.
        ///     The default is true.
        /// </returns>
        [DefaultValue(true)]
        [Description("Indicates whether line breaks are converted to match the document's end-of-line mode when pasted.")] // TODO Place in resource file
        public bool ConvertLineBreaksOnPaste
        {
            get
            {
                return (Scintilla.DirectMessage(NativeMethods.SCI_GETPASTECONVERTENDINGS, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero);
            }
            set
            {
                Scintilla.DirectMessage(NativeMethods.SCI_SETPASTECONVERTENDINGS, (value ? new IntPtr(1) : IntPtr.Zero), IntPtr.Zero);
            }
        }

        #endregion Properties


        #region Constructors

        internal Clipboard(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
