#region Using Directives

using System;
using System.Collections.ObjectModel;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Data structure used to store DropMarkers in the AllDocumentDropMarkers property.
    /// </summary>
    public class DropMarkerList : KeyedCollection<Guid, DropMarker>
    {
        #region Methods

        protected override Guid GetKeyForItem(DropMarker item)
        {
            return item.Key;
        }

        #endregion Methods
    }
}
