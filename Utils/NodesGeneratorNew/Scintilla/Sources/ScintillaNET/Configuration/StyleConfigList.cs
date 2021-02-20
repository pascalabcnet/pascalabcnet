#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class StyleConfigList : List<StyleConfig>
    {
        #region Fields

        private int? _bits;
        private bool? _inherit;

        #endregion Fields


        #region Properties

        public int? Bits
        {
            get
            {
                return _bits;
            }
            set
            {
                _bits = value;
            }
        }


        public bool? Inherit
        {
            get
            {
                return _inherit;
            }
            set
            {
                _inherit = value;
            }
        }

        #endregion Properties
    }
}
