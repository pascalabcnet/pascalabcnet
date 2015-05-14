#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class LongLines : TopLevelHelper
    {
        #region Methods

        private void ResetEdgeColor()
        {
            EdgeColor = Color.Silver;
        }


        private void ResetEdgeColumn()
        {
            EdgeColumn = 0;
        }


        private void ResetEdgeMode()
        {
            EdgeMode = EdgeMode.None;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeEdgeColor() ||
                ShouldSerializeEdgeColumn() ||
                ShouldSerializeEdgeMode();
        }


        private bool ShouldSerializeEdgeColor()
        {
            return EdgeColor != Color.Silver;
        }


        private bool ShouldSerializeEdgeColumn()
        {
            return EdgeColumn != 0;
        }


        private bool ShouldSerializeEdgeMode()
        {
            return EdgeMode != EdgeMode.None;
        }

        #endregion Methods


        #region Properties

        public Color EdgeColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey("LongLines.EdgeColor"))
                    return Scintilla.ColorBag["LongLines.EdgeColor"];

                return Color.Silver;
            }
            set
            {
                if (value == Color.Silver)
                    Scintilla.ColorBag.Remove("LongLines.EdgeColor");

                Scintilla.ColorBag["LongLines.EdgeColor"] = value;
                NativeScintilla.SetEdgeColour(Utilities.ColorToRgb(value));
            }
        }


        public int EdgeColumn
        {
            get
            {
                return NativeScintilla.GetEdgeColumn();
            }
            set
            {
                NativeScintilla.SetEdgeColumn(value);
            }
        }


        public EdgeMode EdgeMode
        {
            get
            {
                return (EdgeMode)NativeScintilla.GetEdgeMode();
            }
            set
            {
                NativeScintilla.SetEdgeMode((int)value);
            }
        }

        #endregion Properties


        #region Constructors

        internal LongLines(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
