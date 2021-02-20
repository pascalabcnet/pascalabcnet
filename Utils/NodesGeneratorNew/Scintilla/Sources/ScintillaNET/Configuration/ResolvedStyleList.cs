#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class ResolvedStyleList : Dictionary<int, StyleConfig>
    {
        #region Methods

        public StyleConfig FindByName(string name)
        {

            foreach (StyleConfig item in this.Values)
            {
                if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return null;
        }

        #endregion Methods


        #region Constructors

        public ResolvedStyleList()
        {

        }

        #endregion Constructors
    }
}
