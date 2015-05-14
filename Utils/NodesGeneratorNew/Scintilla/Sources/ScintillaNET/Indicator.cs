#region Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Indicator : ScintillaHelperBase
    {
        #region Fields

        private int _number;

        #endregion Fields


        #region Methods

        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return ((Indicator)obj).Number == this.Number;
        }


        private Color GetDefaultColor()
        {
            if (_number == 0)
                return Color.FromArgb(0, 127, 0);
            else if (_number == 1)
                return Color.FromArgb(0, 0, 255);
            else if (_number == 2)
                return Color.FromArgb(255, 0, 0);
            else
                return Color.FromArgb(0, 0, 0);
        }


        private IndicatorStyle GetDefaultStyle()
        {
            if (_number == 0)
                return IndicatorStyle.Squiggle;
            else if (_number == 1)
                return IndicatorStyle.TT;
            else
                return IndicatorStyle.Plain;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public void Reset()
        {
            ResetColor();
            ResetIsDrawnUnder();
            ResetStyle();
        }


        public void ResetColor()
        {
            Color = GetDefaultColor();
        }


        public void ResetIsDrawnUnder()
        {
            IsDrawnUnder = false;
        }


        public void ResetStyle()
        {
            Style = GetDefaultStyle();
        }


        public Range Search()
        {
            return Search(Scintilla.GetRange());
        }


        public Range Search(Range searchRange)
        {
            int foundStart = NativeScintilla.IndicatorEnd(_number, searchRange.Start);
            int foundEnd = NativeScintilla.IndicatorEnd(_number, foundStart);
            if (foundStart < 0 || foundStart > searchRange.End || foundStart == foundEnd)
                return null;


            return new Range(foundStart, foundEnd, Scintilla);
        }


        public Range Search(Range searchRange, Range startingAfterRange)
        {
            int start = startingAfterRange.End;
            if (start > NativeScintilla.GetTextLength())
                return null;

            int foundStart = NativeScintilla.IndicatorEnd(_number, start);
            int foundEnd = NativeScintilla.IndicatorEnd(_number, foundStart);
            if (foundStart < 0 || foundStart > searchRange.End || foundStart == foundEnd)
                return null;
            
            return new Range(foundStart, foundEnd, Scintilla);
        }


        public List<Range> SearchAll()
        {
            return SearchAll(Scintilla.GetRange());
        }


        public List<Range> SearchAll(Range searchRange)
        {
            Range foundRange = Scintilla.GetRange(-1, -1);

            List<Range> ret = new List<Range>();
            do
            {
                foundRange = Search(searchRange, foundRange);
                if (foundRange != null)
                    ret.Add(foundRange);
            }
            while (foundRange != null);
            return ret;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeColor() ||
                ShouldSerializeIsDrawnUnder() ||
                ShouldSerializeStyle();
        }


        private bool ShouldSerializeColor()
        {
            return Color != GetDefaultColor();
        }


        private bool ShouldSerializeIsDrawnUnder()
        {
            return IsDrawnUnder;
        }


        private bool ShouldSerializeStyle()
        {
            return Style != GetDefaultStyle();
        }


        public override string ToString()
        {
            return "Indicator" + _number;
        }

        #endregion Methods


        #region Properties

        public Color Color
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey(ToString() + ".Color"))
                    return Scintilla.ColorBag[ToString() + ".Color"];

                return Utilities.RgbToColor(NativeScintilla.IndicGetFore(_number));
            }
            set
            {
                Scintilla.ColorBag[ToString() + ".Color"] = value;
                NativeScintilla.IndicSetFore(_number, Utilities.ColorToRgb(value));
            }
        }


        public bool IsDrawnUnder
        {
            get
            {
                return NativeScintilla.IndicGetUnder(_number);
            }
            set
            {
                NativeScintilla.IndicSetUnder(_number, value);
            }
        }


        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }


        public IndicatorStyle Style
        {
            get
            {
                return (IndicatorStyle)NativeScintilla.IndicGetStyle(_number);
            }
            set
            {
                NativeScintilla.IndicSetStyle(_number, (int)value);
            }
        }

        #endregion Properties


        #region Constructors

        internal Indicator(int number, Scintilla scintilla) : base(scintilla)
        {
            _number = number;
        }

        #endregion Constructors
    }
}
