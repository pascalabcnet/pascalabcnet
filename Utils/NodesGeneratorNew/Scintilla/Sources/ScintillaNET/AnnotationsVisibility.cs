#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Specifies the visibility and appearance of annotations in a <see cref="Scintilla" /> control.
    /// </summary>
    public enum AnnotationsVisibility
    {
        /// <summary>
        ///     Annotations are not displayed.
        /// </summary>
        Hidden = NativeMethods.ANNOTATION_HIDDEN,

        /// <summary>
        ///     Annotations are drawn left-justified with no adorment.
        /// </summary>
        Standard = NativeMethods.ANNOTATION_STANDARD,

        /// <summary>
        ///     Annotations are indented to match the text and are surrounded by a box.
        /// </summary>
        Boxed = NativeMethods.ANNOTATION_BOXED
    }
}
