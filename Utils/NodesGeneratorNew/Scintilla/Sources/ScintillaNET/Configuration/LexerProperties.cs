#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class LexerPropertiesConfig : Dictionary<string, string>
    {
        #region Fields

        private bool? _inherit;

        #endregion Fields


        #region Properties

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
