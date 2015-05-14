#region Using Directives

using System;
using System.Collections.ObjectModel;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class IndicatorConfigList : KeyedCollection<int, IndicatorConfig>
    {
        #region Fields

        private bool? _inherit;

        #endregion Fields


        #region Methods

        protected override int GetKeyForItem(IndicatorConfig item)
        {
            return item.Number;
        }

        #endregion Methods


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
