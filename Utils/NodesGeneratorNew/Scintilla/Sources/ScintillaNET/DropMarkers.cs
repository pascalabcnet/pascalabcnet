#region Using Directives

using System.Collections.Generic;
using System.ComponentModel;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Manages DropMarkers, a Stack Based document bookmarking system.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class DropMarkers : TopLevelHelper
    {
        #region Fields

        private Stack<DropMarker> _markerStack = new Stack<DropMarker>();
        private static Dictionary<string, Stack<DropMarker>> _sharedStack = new Dictionary<string, Stack<DropMarker>>();
        private DropMarkerList _allDocumentDropMarkers = new DropMarkerList();
        private string _sharedStackName = string.Empty;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Collects the last dropped DropMarker
        /// </summary>
        /// <remarks>
        ///     When a DropMarker is collected the current document posision is moved
        ///     to the DropMarker posision, the DropMarker is removed from the stack
        ///     and the visual indicator is removed.
        /// </remarks>
        public void Collect()
        {
            while (_markerStack.Count > 0)
            {
                DropMarker dm = _markerStack.Pop();

                //	If the Drop Marker was deleted in the document by
                //	a user action it will be disposed but not removed
                //	from the marker stack. In this case just pretend
                //	like it doesn't exist and go on to the next one
                if (dm.IsDisposed)
                    continue;

                //	The MarkerCollection fires a cancellable event.
                //	If it is canclled the Collect() method will return
                //	false. In this case we need to push the marker back
                //	on the stack so that it will still be collected in
                //	the future.
                if (!dm.Collect())
                    _markerStack.Push(dm);

                return;
            }
        }


        /// <summary>
        ///     Drops a DropMarker at the current document position
        /// </summary>
        /// <remarks>
        ///     Dropping a DropMarker creates a visual marker (red triangle)
        ///     indicating the DropMarker point.
        /// </remarks>
        /// <returns>The newly created DropMarker</returns>
        public DropMarker Drop()
        {
            return Drop(NativeScintilla.GetCurrentPos());
        }


        /// <summary>
        ///     Drops a DropMarker at the specified document position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The newly created DropMarker</returns>
        /// <remarks>
        ///     Dropping a DropMarker creates a visual marker (red triangle)
        ///     indicating the DropMarker point.
        /// </remarks>
        public DropMarker Drop(int position)
        {
            DropMarker dm = new DropMarker(position, position, GetCurrentTopOffset(), Scintilla);
            _allDocumentDropMarkers.Add(dm);
            _markerStack.Push(dm);
            Scintilla.ManagedRanges.Add(dm);

            //	Force the Drop Marker to paint
            Scintilla.Invalidate(dm.GetClientRectangle());
            return dm;
        }


        private int GetCurrentTopOffset()
        {
            return -1;
        }


        private void ResetSharedStackName()
        {
            _sharedStackName = string.Empty;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeSharedStackName();
        }


        private bool ShouldSerializeSharedStackName()
        {
            return _sharedStackName != string.Empty;
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets/Sets a list of All DropMarkers specific to this Scintilla control
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DropMarkerList AllDocumentDropMarkers
        {
            get
            {
                return _allDocumentDropMarkers;
            }
            set
            {
                _allDocumentDropMarkers = value;
            }
        }


        /// <summary>
        ///     Gets/Sets the Stack of DropMarkers 
        /// </summary>
        /// <remarks>
        ///     You can manually set this to implement your own shared DropMarker stack
        ///     between Scintilla Controls. 
        /// </remarks>
        /// <seealso cref="SharedStackName"/>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Stack<DropMarker> MarkerStack
        {
            get
            {
                return _markerStack;
            }

            //  That's right kids you can actually provide your own MarkerStack. This
            //  is really useful for MDI applications where you want a single master
            //  MarkerStack that will automatically switch documents (a la CodeRush).
            //  Of course you can let the control do this for you automatically by 
            //  setting the SharedStackName property of multiple instances.
            set
            {
                _markerStack = value;
            }
        }


        /// <summary>
        ///     Gets/Sets a shared name associated with other Scintilla controls. 
        /// </summary>
        /// <remarks>
        ///     All Scintilla controls with the same SharedStackName share a common
        ///     DropMarker stack. This is useful in MDI applications where you want
        ///     the DropMarker stack not to be specific to one document.
        /// </remarks>
        public string SharedStackName
        {
            get
            {
                return _sharedStackName;
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                if (_sharedStackName == value)
                    return;


                if (value == string.Empty)
                {
                    // If we had a shared stack name but are now clearing it
                    // we need to create our own private DropMarkerStack again
                    _markerStack = new Stack<DropMarker>();

                    // If this was the last subscriber of a shared stack
                    // remove the name to free up resources
                    if (_sharedStack.ContainsKey(_sharedStackName) && _sharedStack[_sharedStackName].Count == 1)
                        _sharedStack.Remove(_sharedStackName);
                }
                else
                {
                    // We're using one of the shared stacks. Of course if it hasn't 
                    // already been registered with the list we need to create it.
                    if (!_sharedStack.ContainsKey(_sharedStackName))
                        _sharedStack[_sharedStackName] = new Stack<DropMarker>();

                    _markerStack = _sharedStack[_sharedStackName];
                }

                _sharedStackName = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal DropMarkers(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
