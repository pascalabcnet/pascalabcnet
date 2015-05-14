#region Using Directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Scrolling : TopLevelHelper
    {
        #region Methods

        internal void ResetEndAtLastLine()
        {
            EndAtLastLine = true;
        }


        internal void ResetHorizontalWidth()
        {
            HorizontalWidth = 2000;
        }


        private void ResetScrollBars()
        {
            ScrollBars = ScrollBars.Both;
        }


        internal void ResetXOffset()
        {
            XOffset = 0;
        }


        public void ScrollBy(int columns, int lines)
        {
            NativeScintilla.LineScroll(columns, lines);
        }


        public void ScrollToCaret()
        {
            NativeScintilla.ScrollCaret();
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeEndAtLastLine() ||
                ShouldSerializeHorizontalWidth() ||
                ShouldSerializeScrollBars() ||
                ShouldSerializeXOffset();
        }


        private bool ShouldSerializeEndAtLastLine()
        {
            return !EndAtLastLine;
        }


        private bool ShouldSerializeHorizontalWidth()
        {
            return HorizontalWidth != 2000;
        }


        private bool ShouldSerializeScrollBars()
        {
            return ScrollBars != ScrollBars.Both;
        }


        private bool ShouldSerializeXOffset()
        {
            return XOffset != 0;
        }

        #endregion Methods


        #region Properties

        public bool EndAtLastLine
        {
            get
            {
                return NativeScintilla.GetEndAtLastLine();
            }
            set
            {
                NativeScintilla.SetEndAtLastLine(value);
            }
        }


        public int HorizontalWidth
        {
            get
            {
                return NativeScintilla.GetScrollWidth();
            }
            set
            {
                NativeScintilla.SetScrollWidth(value);
            }
        }


        public ScrollBars ScrollBars
        {
            get
            {
                bool h = NativeScintilla.GetHScrollBar();
                bool v = NativeScintilla.GetVScrollBar();

                if (h && v)
                    return ScrollBars.Both;
                else if (h)
                    return ScrollBars.Horizontal;
                else if (v)
                    return ScrollBars.Vertical;
                else
                    return ScrollBars.None;
            }
            set
            {
                NativeScintilla.SetHScrollBar((value & ScrollBars.Horizontal) > 0);
                NativeScintilla.SetVScrollBar((value & ScrollBars.Vertical) > 0);
            }
        }


        public int XOffset
        {
            get
            {
                return NativeScintilla.GetXOffset();
            }
            set
            {
                NativeScintilla.SetXOffset(value);
            }
        }

        #endregion Properties


        #region Constructors

        internal Scrolling(Scintilla scintilla) : base(scintilla)
        {
        }

        #endregion Constructors
    }
}
