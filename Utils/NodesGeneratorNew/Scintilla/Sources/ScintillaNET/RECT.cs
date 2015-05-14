#region Using Directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        #region Fields

        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        #endregion Fields


        #region Properties

        public int Height { get { return Bottom - Top; } }
        public Point Location { get { return new Point(Left, Top); } }
        public Size Size { get { return new Size(Width, Height); } }
        public int Width { get { return Right - Left; } }

        #endregion Properties


        #region Methods

        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }


        public override int GetHashCode()
        {
            return Left ^ ((Top << 13) | (Top >> 0x13))
              ^ ((Width << 0x1a) | (Width >> 6))
              ^ ((Height << 7) | (Height >> 0x19));
        }


        // Handy method for converting to a System.Drawing.Rectangle
        public Rectangle ToRectangle()
        {
            return Rectangle.FromLTRB(Left, Top, Right, Bottom);
        }

        #endregion Methods


        #region Operators

        public static implicit operator RECT(Rectangle rect)
        {
            return FromRectangle(rect);
        }


        public static implicit operator Rectangle(RECT rect)
        {
            return rect.ToRectangle();
        }

        #endregion Operators


        #region Constructors

        public RECT(int left_, int top_, int right_, int bottom_)
        {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }

        #endregion Constructors
    } 
}
