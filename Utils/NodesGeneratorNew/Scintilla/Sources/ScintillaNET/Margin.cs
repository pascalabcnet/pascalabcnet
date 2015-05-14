#region Using Directives

using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Margin : ScintillaHelperBase
    {
        #region Fields

        private int _autoToggleMarkerNumber = -1;
        private int _number;

        #endregion Fields


        #region Methods

        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return this.Number == ((Margin)obj).Number;
        }


        public Rectangle GetClientRectangle()
        {
            int left = 0;
            for (int i = 0; i < _number; i++)
                left += NativeScintilla.GetMarginWidthN(i);

            return new Rectangle(left, 0, Width, Scintilla.ClientSize.Height);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public void Reset()
        {
            ResetAutoToggleMarkerNumber();
            ResetIsClickable();
            ResetIsFoldMargin();
            ResetIsMarkerMargin();
            ResetType();
            ResetWidth();
        }


        private void ResetAutoToggleMarkerNumber()
        {
            _autoToggleMarkerNumber = -1;
        }


        internal void ResetIsClickable()
        {
            if (_number == 2)
                IsClickable = true;
            else
                IsClickable = false;
        }


        internal void ResetIsFoldMargin()
        {
            if (_number == 2)
                IsFoldMargin = true;
            else
                IsFoldMargin = false;
        }


        internal void ResetIsMarkerMargin()
        {
            if (_number == 1)
                IsMarkerMargin = true;
            else
                IsMarkerMargin = false;
        }


        internal void ResetType()
        {
            if (_number == 0)
                Type = MarginType.Number;
            else
                Type = MarginType.Symbol;
        }


        internal void ResetWidth()
        {
            if (_number == 1)
                Width = 16;
            else
                Width = 0;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeIsFoldMargin() ||
                ShouldSerializeIsClickable() ||
                ShouldSerializeType() ||
                ShouldSerializeWidth() ||
                ShouldSerializeAutoToggleMarkerNumber() ||
                ShouldSerializeIsMarkerMargin();
        }


        private bool ShouldSerializeAutoToggleMarkerNumber()
        {
            return _autoToggleMarkerNumber != -1;
        }


        private bool ShouldSerializeIsClickable()
        {
            if (_number == 2)
                return !IsClickable;

            return IsClickable;
        }


        private bool ShouldSerializeIsFoldMargin()
        {
            if (_number == 2)
                return !IsFoldMargin;

            return IsFoldMargin;
        }


        private bool ShouldSerializeIsMarkerMargin()
        {
            if (_number == 1)
                return !IsMarkerMargin;
            else
                return IsMarkerMargin;
        }


        private bool ShouldSerializeType()
        {
            // Margin 0 defaults to Number, all the rest
            // default to Symbol
            if (_number == 0)
                return Type != MarginType.Number;

            return Type != MarginType.Symbol;
        }


        private bool ShouldSerializeWidth()
        {
            // Margin 1 defaults to 16, all the rest
            // default to 0
            if (_number == 1)
                return Width != 16;

            return Width != 0;
        }


        public override string ToString()
        {
            if (_number == 0)
                return "(Default Line Numbers)";
            else if (_number == 1)
                return "(Default Markers)";
            else if (_number == 2)
                return "(Default Folds)";

            return base.ToString();
        }

        #endregion Methods


        #region Properties

        public int AutoToggleMarkerNumber
        {
            get
            {
                return _autoToggleMarkerNumber;
            }
            set
            {
                _autoToggleMarkerNumber = value;
            }
        }


        public bool IsClickable
        {
            get
            {
                return NativeScintilla.GetMarginSensitiveN(_number);
            }
            set
            {
                NativeScintilla.SetMarginSensitiveN(_number, value);
            }
        }


        public bool IsFoldMargin
        {
            get
            {
                return (Mask & Constants.SC_MASK_FOLDERS) == Constants.SC_MASK_FOLDERS;
            }
            set
            {
                if (value)
                    Mask |= Constants.SC_MASK_FOLDERS;
                else
                    Mask &= ~Constants.SC_MASK_FOLDERS;
            }
        }


        public bool IsMarkerMargin
        {
            // As best as I can divine, this value is like SC_MASK_FOLDERS but applies
            // instead to regular markers. I can't seem to find it in any of the documentation
            // or even a constant defined for it though.
            get
            {
                return (Mask & 0x1FFFFFF) == 0x1FFFFFF;
            }

            set
            {
                if (value)
                    Mask |= 0x1FFFFFF;
                else
                    Mask &= ~0x1FFFFFF;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Mask
        {
            get
            {
                return NativeScintilla.GetMarginMaskN(_number);
            }
            set
            {
                NativeScintilla.SetMarginMaskN(_number, value);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Number
        {
            get
            {
                return _number;
            }
        }


        public MarginType Type
        {
            get
            {
                return (MarginType)NativeScintilla.GetMarginTypeN(_number);
            }
            set
            {
                NativeScintilla.SetMarginTypeN(_number, (int)value);
            }
        }


        public int Width
        {
            get
            {
                return NativeScintilla.GetMarginWidthN(_number);
            }
            set
            {
                NativeScintilla.SetMarginWidthN(_number, value);
            }
        }

        #endregion Properties


        #region Constructors

        protected internal Margin(Scintilla scintilla, int number) : base(scintilla)
        {
            _number = number;
        }

        #endregion Constructors
    }
}
