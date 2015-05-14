#region Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class IndicatorCollection : TopLevelHelper
    {
        #region Methods

        public void Reset()
        {
            for (int i = 0; i < 32; i++)
                this[i].Reset();
        }

        #endregion Methods


        #region Properties

        public Indicator this[int number]
        {
            get
            {
                return new Indicator(number, Scintilla);
            }
        }

        #endregion Properties


        #region Constructors

        internal IndicatorCollection(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
