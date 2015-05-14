#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class MarkersConfigList : System.Collections.ObjectModel.KeyedCollection<int, MarkersConfig>
    {
        #region Fields

        private bool? _inherit;

        #endregion Fields


        #region Methods

        protected override int GetKeyForItem(MarkersConfig item)
        {
            return item.Number.Value;
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
