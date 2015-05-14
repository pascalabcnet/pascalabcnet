#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class StyleCollection : TopLevelHelper
    {
        #region Methods

        public void ClearAll()
        {
            NativeScintilla.StyleClearAll();
        }


        public void ClearDocumentStyle()
        {
            NativeScintilla.ClearDocumentStyle();
        }


        public int GetEndStyled()
        {
            return NativeScintilla.GetEndStyled();
        }


        public byte GetStyleAt(int position)
        {
            return NativeScintilla.GetStyleAt(position);
        }


        public string GetStyleNameAt(int position)
        {
            int styleNumber = GetStyleAt(position);
            foreach (KeyValuePair<string, int> map in Scintilla.Lexing.StyleNameMap)
                if (map.Value == styleNumber)
                    return map.Key;

            return null;
        }


        public void Reset()
        {
            for (int i = 0; i < 32; i++)
                this[i].Reset();
        }


        private void ResetBits()
        {
#pragma warning disable 618
            Bits = 7;
#pragma warning restore 618
        }


        public void ResetDefault()
        {
            NativeScintilla.StyleResetDefault();
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeBits() ||
                ShouldSerializeBraceBad() ||
                ShouldSerializeBraceLight() ||
                ShouldSerializeCallTip() ||
                ShouldSerializeControlChar() ||
                ShouldSerializeDefault() ||
                ShouldSerializeIndentGuide() ||
                ShouldSerializeLastPredefined() ||
                ShouldSerializeLineNumber() ||
                ShouldSerializeMax();
        }


        private bool ShouldSerializeBits()
        {
#pragma warning disable 618
            return Bits != 7;
#pragma warning restore 618

        }


        private bool ShouldSerializeBraceBad()
        {
            return BraceBad.ShouldSerialize();
        }


        private bool ShouldSerializeBraceLight()
        {
            return BraceLight.ShouldSerialize();
        }


        private bool ShouldSerializeCallTip()
        {
            return CallTip.ShouldSerialize();
        }


        private bool ShouldSerializeControlChar()
        {
            return ControlChar.ShouldSerialize();
        }


        private bool ShouldSerializeDefault()
        {
            return BraceBad.ShouldSerialize();
        }


        private bool ShouldSerializeIndentGuide()
        {
            return IndentGuide.ShouldSerialize();
        }


        private bool ShouldSerializeLastPredefined()
        {
            return LastPredefined.ShouldSerialize();
        }


        private bool ShouldSerializeLineNumber()
        {
            return LineNumber.ShouldSerialize();
        }


        private bool ShouldSerializeMax()
        {
            return Max.ShouldSerialize();
        }

        #endregion Methods


        #region Properties

        [Obsolete("The modern style indicators make this obsolete, this should always be 7")]
        public int Bits
        {
            get
            {
                return NativeScintilla.GetStyleBits();
            }
            set
            {
                NativeScintilla.SetStyleBits(value);
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style BraceBad
        {
            get
            {
                return this[StylesCommon.BraceBad];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style BraceLight
        {
            get
            {
                return this[StylesCommon.BraceLight];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style CallTip
        {
            get
            {
                return this[StylesCommon.CallTip];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style ControlChar
        {
            get
            {
                return this[StylesCommon.ControlChar];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style Default
        {
            get
            {
                return this[StylesCommon.Default];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style IndentGuide
        {
            get
            {
                return this[StylesCommon.IndentGuide];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style LastPredefined
        {
            get
            {
                return this[StylesCommon.LastPredefined];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style LineNumber
        {
            get
            {
                return this[StylesCommon.LineNumber];
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Style Max
        {
            get
            {
                return this[StylesCommon.Max];
            }
        }

        #endregion Properties


        #region Indexers

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Style this[int index]
        {
            get
            {
                return new Style(index, Scintilla);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Style this[StylesCommon index]
        {
            get
            {
                return new Style((int)index, Scintilla);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Style this[string styleName]
        {
            get
            {
                return new Style(Scintilla.Lexing.StyleNameMap[styleName], Scintilla);
            }
        }

        #endregion Indexers


        #region Constructors

        internal StyleCollection(Scintilla scintilla) : base(scintilla)
        {
#pragma warning disable 618
            Bits = 7;
#pragma warning restore 618

            //	Defaulting CallTip Settings to Platform defaults
            Style s = CallTip;
            s.ForeColor = SystemColors.InfoText;
            s.BackColor = SystemColors.Info;
            s.Font = SystemFonts.StatusFont;

            //	Making Line Number's BackColor have a named system color
            //	instead of just the value
            LineNumber.BackColor = SystemColors.Control;
        }

        #endregion Constructors
    }
}
