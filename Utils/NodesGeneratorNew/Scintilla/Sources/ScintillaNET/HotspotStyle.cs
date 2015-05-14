#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class HotspotStyle : TopLevelHelper
    {
        #region Fields

        private bool _useActiveForeColor = true;
        private bool _useActiveBackColor = true;

        #endregion Fields


        #region Methods

        private void ResetActiveBackColor()
        {
            ActiveBackColor = SystemColors.Window;
        }


        private void ResetActiveForeColor()
        {
            ActiveForeColor = SystemColors.HotTrack;
        }


        private void ResetActiveUnderline()
        {
            ActiveUnderline = true;
        }


        private void ResetSingleLine()
        {
            SingleLine = true;
        }


        private void ResetUseActiveBackColor()
        {
            UseActiveBackColor = true;
        }


        private void ResetUseActiveForeColor()
        {
            UseActiveForeColor = true;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeActiveBackColor() ||
                ShouldSerializeActiveForeColor() ||
                ShouldSerializeActiveUnderline() ||
                ShouldSerializeSingleLine() ||
                ShouldSerializeUseActiveBackColor() ||
                ShouldSerializeUseActiveForeColor();
        }


        private bool ShouldSerializeActiveBackColor()
        {
            return ActiveBackColor != SystemColors.Window;
        }


        private bool ShouldSerializeActiveForeColor()
        {
            return ActiveForeColor != SystemColors.HotTrack;
        }


        private bool ShouldSerializeActiveUnderline()
        {
            return !ActiveUnderline;
        }


        private bool ShouldSerializeSingleLine()
        {
            return !SingleLine;
        }


        private bool ShouldSerializeUseActiveBackColor()
        {
            return !UseActiveBackColor;
        }


        private bool ShouldSerializeUseActiveForeColor()
        {
            return !UseActiveForeColor;
        }

        #endregion Methods


        #region Properties

        public Color ActiveBackColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey("HotspotStyle.ActiveBackColor"))
                    return Scintilla.ColorBag["HotspotStyle.ActiveBackColor"];

                return Utilities.RgbToColor(NativeScintilla.GetHotspotActiveBack());
            }
            set
            {
                if (value.IsKnownColor)
                    Scintilla.ColorBag["HotspotStyle.ActiveBackColor"] = value;
                else
                    Scintilla.ColorBag.Remove("HotspotStyle.ActiveBackColor");

                NativeScintilla.SetHotspotActiveBack(_useActiveBackColor, Utilities.ColorToRgb(value));
            }
        }


        public Color ActiveForeColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey("HotspotStyle.ActiveForeColor"))
                    return Scintilla.ColorBag["HotspotStyle.ActiveForeColor"];

                return Utilities.RgbToColor(NativeScintilla.GetHotspotActiveFore());
            }
            set
            {
                if (value.IsKnownColor)
                    Scintilla.ColorBag["HotspotStyle.ActiveForeColor"] = value;
                else
                    Scintilla.ColorBag.Remove("HotspotStyle.ActiveForeColor");

                NativeScintilla.SetHotspotActiveFore(_useActiveForeColor, Utilities.ColorToRgb(value));
            }
        }


        public bool ActiveUnderline
        {
            get
            {
                return NativeScintilla.GetHotspotActiveUnderline();
            }
            set
            {
                NativeScintilla.SetHotspotActiveUnderline(value);
            }
        }


        public bool SingleLine
        {
            get
            {
                return NativeScintilla.GetHotspotSingleLine();
            }
            set
            {
                NativeScintilla.SetHotspotSingleLine(value);
            }
        }


        public bool UseActiveBackColor
        {
            get
            {
                return _useActiveBackColor;
            }
            set
            {
                _useActiveBackColor = value;
            }
        }


        public bool UseActiveForeColor
        {
            get
            {
                return _useActiveForeColor;
            }
            set
            {
                _useActiveForeColor = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal HotspotStyle(Scintilla scintilla) : base(scintilla)
        {
            ActiveForeColor = SystemColors.HotTrack;
            ActiveBackColor = SystemColors.Window;
        }

        #endregion Constructors
    }
}
