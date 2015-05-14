#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Style : ScintillaHelperBase
    {
        #region Fields

        private int _index = 0;

        #endregion Fields


        #region Methods

        public void Apply(int length)
        {
            Apply(NativeScintilla.GetCurrentPos(), length);
        }


        public void Apply(int position, int length)
        {
            NativeScintilla.StartStyling(position, 0xff);
            NativeScintilla.SetStyling(length, _index);
        }


        internal bool BackColorNotSet()
        {
            return !Scintilla.ColorBag.ContainsKey(ToString() + ".BackColor");
        }


        public void CopyTo(Style target)
        {
            target.BackColor = BackColor;
            target.Bold = Bold;
            target.Case = Case;
            target.CharacterSet = CharacterSet;
            target.FontName = FontName;
            target.ForeColor = ForeColor;
            target.IsChangeable = IsChangeable;
            target.IsHotspot = IsHotspot;
            target.IsSelectionEolFilled = IsSelectionEolFilled;
            target.IsVisible = IsVisible;
            target.Italic = Italic;
            target.Size = Size;
            target.Underline = Underline;
        }


        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return ((Style)obj).Index == this.Index;
        }


        internal bool FontNotSet()
        {
            return !Scintilla.PropertyBag.ContainsKey(ToString() + ".FontSet");
        }


        internal bool ForeColorNotSet()
        {
            return !Scintilla.ColorBag.ContainsKey(ToString() + ".ForeColor");
        }


        private Color getDefaultBackColor()
        {
            if (_index == (int)StylesCommon.CallTip)
                return SystemColors.Info;
            else if (_index == (int)StylesCommon.LineNumber)
                return SystemColors.Control;

            return Color.FromArgb(0xff, 0xff, 0xff);
        }


        private CharacterSet getDefaultCharacterSet()
        {
            return (CharacterSet)getDefaultFont().GdiCharSet;
        }


        private Font getDefaultFont()
        {
            if (_index == (int)StylesCommon.CallTip)
                return SystemFonts.StatusFont;

            return new Font("Verdana", 8F); 
        }


        private Color getDefaultForeColor()
        {
            if (_index == (int)StylesCommon.CallTip)
                return SystemColors.InfoText;

            return Color.FromArgb(0, 0, 0);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public int GetTextWidth(string text)
        {
            return NativeScintilla.TextWidth(_index, text);
        }


        public void Reset()
        {
            ResetBackColor();
            ResetBold();
            ResetCase();
            ResetCharacterSet();
            ResetFontName();
            ResetForeColor();
            ResetIsChangeable();
            ResetIsHotspot();
            ResetIsSelectionEolFilled();
            ResetIsVisible();
            ResetItalic();
            ResetSize();
            ResetUnderline();
        }


        private void ResetBackColor()
        {
            BackColor = getDefaultBackColor();
        }


        private void ResetBold()
        {
            Bold = getDefaultFont().Bold;
        }


        private void ResetCase()
        {
            Case = StyleCase.Mixed;
        }


        private void ResetCharacterSet()
        {
            CharacterSet = getDefaultCharacterSet();
        }


        internal void ResetFont()
        {
            Font = getDefaultFont();
            Scintilla.PropertyBag.Remove(ToString() + ".FontSet");
        }


        private void ResetFontName()
        {
            FontName = getDefaultFont().Name;
        }


        private void ResetForeColor()
        {
            ForeColor = getDefaultForeColor();
        }


        private void ResetIsChangeable()
        {
            IsChangeable = true;
        }


        private void ResetIsHotspot()
        {
            IsHotspot = false;
        }


        private void ResetIsSelectionEolFilled()
        {
            IsSelectionEolFilled = false;
        }


        private void ResetIsVisible()
        {
            IsVisible = true;
        }


        private void ResetItalic()
        {
            Italic = getDefaultFont().Italic;
        }


        private void ResetSize()
        {
            Size = getDefaultFont().SizeInPoints;
        }


        private void ResetUnderline()
        {
            Underline = getDefaultFont().Underline;
        }


        internal void SetBackColorInternal(Color value)
        {
            NativeScintilla.StyleSetBack(_index, Utilities.ColorToRgb(value));
            Scintilla.ColorBag[ToString() + ".BackColor"] = value;

            if (_index == (int)StylesCommon.CallTip)
                NativeScintilla.CallTipSetBack(Utilities.ColorToRgb(value));
        }


        internal void SetForeColorInternal(Color value)
        {
            Scintilla.ColorBag[ToString() + ".ForeColor"] = value;
            NativeScintilla.StyleSetFore(_index, Utilities.ColorToRgb(value));

            if (_index == (int)StylesCommon.CallTip)
                NativeScintilla.CallTipSetFore(Utilities.ColorToRgb(value));
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeBackColor() ||
                ShouldSerializeBold() ||
                ShouldSerializeCase() ||
                ShouldSerializeCharacterSet() ||
                ShouldSerializeFontName() ||
                ShouldSerializeForeColor() ||
                ShouldSerializeIsChangeable() ||
                ShouldSerializeIsHotspot() ||
                ShouldSerializeIsSelectionEolFilled() ||
                ShouldSerializeIsVisible() ||
                ShouldSerializeItalic() ||
                ShouldSerializeSize() ||
                ShouldSerializeUnderline();
        }


        private bool ShouldSerializeBackColor()
        {
            return BackColor != getDefaultBackColor();
        }


        private bool ShouldSerializeBold()
        {
            return Bold != getDefaultFont().Bold;
        }


        private bool ShouldSerializeCase()
        {
            return Case != StyleCase.Mixed;
        }


        private bool ShouldSerializeCharacterSet()
        {
            return CharacterSet != getDefaultCharacterSet();
        }


        private bool ShouldSerializeFont()
        {
            //	We never serialize the font property, we let the component
            //	properties do the work.
            return false;
        }


        private bool ShouldSerializeFontName()
        {
            return FontName != getDefaultFont().Name;
        }


        private bool ShouldSerializeForeColor()
        {
            return ForeColor != getDefaultForeColor();
        }


        private bool ShouldSerializeIsChangeable()
        {
            return !IsChangeable;
        }


        private bool ShouldSerializeIsHotspot()
        {
            return IsHotspot;
        }


        private bool ShouldSerializeIsSelectionEolFilled()
        {
            return IsSelectionEolFilled;
        }


        private bool ShouldSerializeIsVisible()
        {
            return !IsVisible;
        }


        private bool ShouldSerializeItalic()
        {
            return Italic != getDefaultFont().Italic;
        }


        private bool ShouldSerializeSize()
        {
            return Size != getDefaultFont().SizeInPoints;
        }


        private bool ShouldSerializeUnderline()
        {
            return Underline != getDefaultFont().Underline;
        }


        public override string ToString()
        {
             return "Style" + _index.ToString();
        }

        #endregion Methods


        #region Properties

        public Color BackColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey(ToString() + ".BackColor"))
                    return Scintilla.ColorBag[ToString() + ".BackColor"];

                return Utilities.RgbToColor(NativeScintilla.StyleGetBack(_index));
            }
            set
            {
                SetBackColorInternal(value);

                if (_index == (int)StylesCommon.CallTip)
                    Scintilla.CallTip.SetBackColorInternal(value);
            }
        }


        public bool Bold
        {
            get { return NativeScintilla.StyleGetBold(_index); }
            set
            {
                NativeScintilla.StyleSetBold(_index, value);
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;
            }
        }


        public StyleCase Case
        {
            get
            {
                return (StyleCase)NativeScintilla.StyleGetCase(_index);
            }
            set
            {
                NativeScintilla.StyleSetCase(_index, (int)value);
            }
        }


        public CharacterSet CharacterSet
        {
            get { return (CharacterSet)NativeScintilla.StyleGetCharacterSet(_index); }
            set
            {
                NativeScintilla.StyleSetCharacterSet(_index, (int)value);
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;
            }
        }


        public Font Font
        {
            get
            {
                FontStyle fs = FontStyle.Regular;
                if (Bold) fs |= FontStyle.Bold;
                if (Italic) fs |= FontStyle.Italic;
                if (Underline) fs |= FontStyle.Underline;

                return new Font(FontName, Size, fs, GraphicsUnit.Point, (byte)CharacterSet);
            }
            set
            {
                CharacterSet = (CharacterSet)value.GdiCharSet;
                FontName = value.Name;
                Size = value.SizeInPoints;
                Bold = value.Bold;
                Italic = value.Italic;
                Underline = value.Underline;
            }
        }


        public string FontName
        {
            get
            {
                //	Scintilla has trouble returning some font names, especially those
                //	with spaces in it. They get truncated. So we're storing ourselves.
                //	Oh yeah I wrote the code for SCI_STYLEGETFONT in Scintilla so what 
                //	does that tell you?
                if (!Scintilla.PropertyBag.ContainsKey(ToString() + ".FontName"))
                {
                    string fontName;
                    NativeScintilla.StyleGetFont(_index, out fontName);
                    return fontName;
                }

                return Scintilla.PropertyBag[ToString() + ".FontName"].ToString();
            }
            set
            {
                NativeScintilla.StyleSetFont(_index, value);
                Scintilla.PropertyBag[ToString() + ".FontName"] = value;
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;

            }
        }


        public Color ForeColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey(ToString() + ".ForeColor"))
                    return Scintilla.ColorBag[ToString() + ".ForeColor"];

                return Utilities.RgbToColor(NativeScintilla.StyleGetFore(_index));
            }
            set
            {
                SetForeColorInternal(value);

                if (_index == (int)StylesCommon.CallTip)
                    Scintilla.CallTip.SetForeColorInternal(value);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                return _index;
            }
        }


        public bool IsChangeable
        {
            get
            {
                return NativeScintilla.StyleGetChangeable(_index); ;
            }
            set
            {
                NativeScintilla.StyleSetChangeable(_index, value);
            }
        }


        public bool IsHotspot
        {
            get
            {
                return NativeScintilla.StyleGetHotspot(_index); ;
            }
            set
            {
                NativeScintilla.StyleSetHotspot(_index, value);
            }
        }


        public bool IsSelectionEolFilled
        {
            get
            {
                return NativeScintilla.StyleGetEOLFilled(_index);
            }
            set
            {
                NativeScintilla.StyleSetEOLFilled(_index, value);
            }
        }


        public bool IsVisible
        {
            get
            {
                return NativeScintilla.StyleGetVisible(_index);
            }
            set
            {
                NativeScintilla.StyleSetVisible(_index, value);
            }
        }


        public bool Italic
        {
            get { return NativeScintilla.StyleGetItalic(_index); }
            set
            {
                NativeScintilla.StyleSetItalic(_index, value);
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;
            }
        }


         //	There are 2 problems with Font Sizes, first Scintilla seems to
        //	accept them just fine, but always returns 8. Also it only supports
        //	integer font sizes, and .NET tends to use non integer values like 8.5
        //	which means that it would always be serialized. The solution? store our
        //	own value.
        public float Size
        {
            get
            {
                if (!Scintilla.PropertyBag.ContainsKey(ToString() + ".Size"))
                    return (float)NativeScintilla.StyleGetSize(_index);

                return (float)Scintilla.PropertyBag[ToString() + ".Size"];
            }
            set
            {
                NativeScintilla.StyleSetSize(_index, (int)value);
                Scintilla.PropertyBag[ToString() + ".Size"] = value;
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;
            }
        }


        public bool Underline
        {
            get { return NativeScintilla.StyleGetUnderline(_index); }
            set
            {
                NativeScintilla.StyleSetUnderline(_index, value);
                Scintilla.PropertyBag[ToString() + ".FontSet"] = true;
            }
        }

        #endregion Properties


        #region Constructors

         internal Style(int index, Scintilla scintilla) : base(scintilla)
         {
             _index = index;
         }

        #endregion Constructors
    }
}
