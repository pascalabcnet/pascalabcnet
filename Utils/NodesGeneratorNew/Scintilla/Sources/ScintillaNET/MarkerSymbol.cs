#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Specifies the symbol displayed by a <see cref="Marker" />.
    /// </summary>
    public enum MarkerSymbol
    {
        /// <summary>
        ///     The marker is drawn as a circle.
        /// </summary>
        Circle = NativeMethods.SC_MARK_CIRCLE,

        /// <summary>
        ///     The marker is drawn as a rectangle with rounded edges.
        /// </summary>
        RoundRectangle = NativeMethods.SC_MARK_ROUNDRECT,

        /// <summary>
        ///     The marker is drawn as a triangle pointing right.
        ///     This symbol is typically used to mark a closed folder.
        /// </summary>
        Arrow = NativeMethods.SC_MARK_ARROW,

        /// <summary>
        ///     The marker is drawn as a horizontal rectangle.
        /// </summary>
        SmallRect = NativeMethods.SC_MARK_SMALLRECT,

        /// <summary>
        ///     The marker is drawn as a small arrow pointing right.
        /// </summary>
        ShortArrow = NativeMethods.SC_MARK_SHORTARROW,

        /// <summary>
        ///     The marker has no visible glpyh.
        ///     This symbol can still be used, however, to mark and track lines.
        /// </summary>
        Empty = NativeMethods.SC_MARK_EMPTY,

        /// <summary>
        ///     The marker is drawn as a triangle pointing down.
        ///     This symbol is typically used to mark an open folder.
        /// </summary>
        ArrowDown = NativeMethods.SC_MARK_ARROWDOWN,

        /// <summary>
        ///     The marker is drawn as a minus sign.
        ///     This symbol is typically used to mark an open folder.
        /// </summary>
        Minus = NativeMethods.SC_MARK_MINUS,

        /// <summary>
        ///     The marker is drawn as a plus sign.
        ///     This symbol is typically used to mark a closed folder.
        /// </summary>
        Plus = NativeMethods.SC_MARK_PLUS,

        /// <summary>
        ///     The marker is drawn as a vertical line.
        ///     This symbol is typically used to mark nested lines of an open folder.
        /// </summary>
        VLine = NativeMethods.SC_MARK_VLINE,

        /// <summary>
        ///     The marker is drawn as straight lines intersecting in an "L" shape.
        ///     This symbol is typically used to mark the end of a folder in a "box style" tree.
        /// </summary>
        LCorner = NativeMethods.SC_MARK_LCORNER,

        /// <summary>
        ///     The marker is drawn as straight lines intersecting in a rotated "T" shape.
        ///     This symbol is typically used to mark the end of a nested folder in a "box style" tree.
        /// </summary>
        TCorner = NativeMethods.SC_MARK_TCORNER,

        /// <summary>
        ///     The marker is drawn as a plus sign surrounded by a rectangle.
        ///     This symbol is typically used to mark a closed folder in a "box style" tree.
        /// </summary>
        BoxPlus = NativeMethods.SC_MARK_BOXPLUS,

        /// <summary>
        ///     The marker is drawn as a plus sign surrounded by a rectangle and vertial lines.
        ///     This symbol is typically used to mark a nested closed folder in a "box style" tree.
        /// </summary>
        BoxPlusConnected = NativeMethods.SC_MARK_BOXPLUSCONNECTED,

        /// <summary>
        ///     The marker is drawn as a minus sign surrounded by a rectangle and a vertical line at the bottom.
        ///     This symbol is typically used to mark an open folder in a "box style" tree.
        /// </summary>
        BoxMinus = NativeMethods.SC_MARK_BOXMINUS,

        /// <summary>
        ///     The marker is drawn as a minus sign surrounded by a rectangle and vertical lines.
        ///     This symbol is typically used to mark a nested open folder in a "box style" tree.
        /// </summary>
        BoxMinusConnected = NativeMethods.SC_MARK_BOXMINUSCONNECTED,

        /// <summary>
        ///     The marker is drawn as curved lines intersecting in an "L" shape.
        ///     This symbol is typically used to mark the end of a folder in a "circle style" tree.
        /// </summary>
        LCornerCurve = NativeMethods.SC_MARK_LCORNERCURVE,

        /// <summary>
        ///     The marker is drawn as curved lines intersecting in a rotated "T" shape.
        ///     This symbol is typically used to mark the end of a nested folder in a "circle style" tree.
        /// </summary>
        TCornerCurve = NativeMethods.SC_MARK_TCORNERCURVE,

        /// <summary>
        ///     The marker is drawn as a plus sign surrounded by a circle.
        ///     This symbol is typically used to mark a closed folder in a "circle style" tree.
        /// </summary>
        CirclePlus = NativeMethods.SC_MARK_CIRCLEPLUS,

        /// <summary>
        ///     The marker is drawn as a plus sign surrounded by a circle and vertial lines.
        ///     This symbol is typically used to mark a nested closed folder in a "circle style" tree.
        /// </summary>
        CirclePlusConnected = NativeMethods.SC_MARK_CIRCLEPLUSCONNECTED,

        /// <summary>
        ///     The marker is drawn as a minus sign surrounded by a circle and a vertical line at the bottom.
        ///     This symbol is typically used to mark an open folder in a "circle style" tree.
        /// </summary>
        CircleMinus = NativeMethods.SC_MARK_CIRCLEMINUS,

        /// <summary>
        ///     The marker is drawn as a minus sign surrounded by a circle and vertical lines.
        ///     This symbol is typically used to mark a nested open folder in a "circle style" tree.
        /// </summary>
        CircleMinusConnected = NativeMethods.SC_MARK_CIRCLEMINUSCONNECTED,

        /// <summary>
        ///     The marker has no visible glyph, however, the background color of the entire text line
        ///     is drawn as specified in the <see cref="Marker.ForeColor" /> property.
        /// </summary>
        Background = NativeMethods.SC_MARK_BACKGROUND,

        /// <summary>
        ///     This marker is drawn as three horizontal dots.
        /// </summary>
        Ellipsis = NativeMethods.SC_MARK_DOTDOTDOT,

        /// <summary>
        ///     The marker is drawn as three consecutive greater than glyphs.
        /// </summary>
        Arrows = NativeMethods.SC_MARK_ARROWS,

        /// <summary>
        ///     The marker is drawn using the image specified in the <see cref="Marker.SetImage" /> method.
        /// </summary>
        PixMap = NativeMethods.SC_MARK_PIXMAP,

        /// <summary>
        ///     The marker has no visible glyph, however, the margin background color is draw as
        ///     specified in the <see cref="Marker.ForeColor" /> property.
        /// </summary>
        FullRectangle = NativeMethods.SC_MARK_FULLRECT,

        /// <summary>
        ///     The marker is drawn as a thick vertical line along the left edge of the margin.
        /// </summary>
        LeftRectangle = NativeMethods.SC_MARK_LEFTRECT,

        /// <summary>
        ///     The marker has no visible glyph, however, it can be used to signify to a plugin
        ///     that the marker is available for a custom purpose.
        /// </summary>
        Available = NativeMethods.SC_MARK_AVAILABLE,

        /// <summary>
        ///     The marker has no visible glyph, however, the entire text line is drawn with an underline in
        ///     the color specified by the <see cref="Marker.ForeColor" /> property.
        /// </summary>
        Underline = NativeMethods.SC_MARK_UNDERLINE,


        //SC_MARK_RGBAIMAGE = 30,
        //SC_MARK_CHARACTER = 10000;
    }
}
