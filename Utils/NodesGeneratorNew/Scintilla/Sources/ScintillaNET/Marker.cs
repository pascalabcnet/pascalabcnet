#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Defines a marker's appearance in a <see cref="Scintilla"/> control.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Marker : ScintillaHelperBase
    {
        #region Fields

        private int _number;

        #endregion Fields


        #region Methods

        public MarkerInstance AddInstanceTo(int line)
        {
            return new MarkerInstance(Scintilla, this, NativeScintilla.MarkerAdd(line, _number));
        }


        public MarkerInstance AddInstanceTo(Line line)
        {
            return AddInstanceTo(line.Number);
        }


        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return ((Marker)obj).Number == this.Number;
            
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public void Reset()
        {
            ResetAlpha();
            ResetBackColor();
            ResetForeColor();
            ResetSymbol();
        }


        private void ResetAlpha()
        {
            Alpha = 0xff;
        }


        private void ResetBackColor()
        {
            BackColor = Color.White;
        }


        private void ResetForeColor()
        {
            ForeColor = Color.Black;
        }


        private void ResetSymbol()
        {
            Symbol = MarkerSymbol.Circle;
        }


        internal void SetBackColorInternal(Color value)
        {
            Scintilla.ColorBag[ToString() + ".BackColor"] = value;
            NativeScintilla.MarkerSetBack(_number, Utilities.ColorToRgb(value));
        }


        internal void SetForeColorInternal(Color value)
        {
            Scintilla.ColorBag[ToString() + ".ForeColor"] = value;
            NativeScintilla.MarkerSetFore(_number, Utilities.ColorToRgb(value));
        }


        public void SetImage(string xpmImage)
        {
            NativeScintilla.MarkerDefinePixmap(_number, xpmImage);
        }


        public void SetImage(Bitmap image)
        {
            NativeScintilla.MarkerDefinePixmap(_number, XpmConverter.ConvertToXPM(image));
        }


        public void SetImage(Bitmap image, Color transparentColor)
        {
            NativeScintilla.MarkerDefinePixmap(_number, XpmConverter.ConvertToXPM(image, Utilities.ColorToHtml(transparentColor)));
        }


        internal void SetSymbolInternal(MarkerSymbol value)
        {
            Scintilla.PropertyBag[ToString() + ".Symbol"] = value;
            NativeScintilla.MarkerDefine(_number, (int)value);
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeAlpha() ||
                ShouldSerializeBackColor() ||
                ShouldSerializeForeColor() ||
                ShouldSerializeSymbol();
        }


        private bool ShouldSerializeAlpha()
        {
            return Alpha != 0xff;
        }


        private bool ShouldSerializeBackColor()
        {
            if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
                return BackColor != Color.White;

            return false;
        }


        private bool ShouldSerializeForeColor()
        {
            if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
                return ForeColor != Color.Black;

            return false;
        }


        private bool ShouldSerializeSymbol()
        {
            if (Scintilla.Folding.MarkerScheme == FoldMarkerScheme.Custom)
                return Symbol != MarkerSymbol.Circle;

            return false;
        }


        public override string ToString()
        {
            return "MarkerNumber" + _number;
        }

        #endregion Methods


        #region Properties

        public int Alpha
        {
            get
            {

                try
                {
                    if (Scintilla.PropertyBag.ContainsKey(ToString() + ".Alpha"))
                        return (int)Scintilla.PropertyBag[ToString() + ".Alpha"];

                    return 0xff;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return 0xff;
                }
            }
            set
            {
                Scintilla.PropertyBag[ToString() + ".Alpha"] = value;
                NativeScintilla.MarkerSetAlpha(_number, value);
            }
        }


        public Color BackColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey(ToString() + ".BackColor"))
                    return Scintilla.ColorBag[ToString() + ".BackColor"];

                return Color.White;
            }
            set
            {
                SetBackColorInternal(value);
                Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
            }
        }


        public Color ForeColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey(ToString() + ".ForeColor"))
                    return Scintilla.ColorBag[ToString() + ".ForeColor"];

                return Color.Black;
            }
            set
            {
                SetForeColorInternal(value);
                Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
            }
        }


        public uint Mask
        {
            get
            {
                uint result = ((uint)1) << Number;
                return result;
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


        /// <summary>
        ///     Gets or sets the marker symbol.
        /// </summary>
        /// <returns>One of the <see cref="MarkerSymbol" /> values. The default is <see cref="MarkerSymbol.Circle" />.</returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="MarkerSymbol" /> values.
        /// </exception>
        public MarkerSymbol Symbol
        {
            get
            {
                if (Scintilla.PropertyBag.ContainsKey(ToString() + ".Symbol"))
                    return (MarkerSymbol)Scintilla.PropertyBag[ToString() + ".Symbol"];

                return MarkerSymbol.Circle;
            }
            set
            {
                if (!Enum.IsDefined(typeof(MarkerSymbol), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(MarkerSymbol));

                if (value != Symbol)
                {
                    SetSymbolInternal(value);
                    Scintilla.Folding.MarkerScheme = FoldMarkerScheme.Custom;
                }
            }
        }

        #endregion Properties


        #region Constructors

        internal Marker(Scintilla scintilla, int number) : base(scintilla)
        {
            _number = number;
        }

        #endregion Constructors
    }
}
