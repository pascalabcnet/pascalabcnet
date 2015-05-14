#region Using Directives

using System;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents a DropMarker, currently a single document point.
    /// </summary>
    public class DropMarker : ManagedRange
    {
        #region Fields

        private int _topOffset;
        private Guid _key = Guid.NewGuid();

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Overridden, changes the document position. Start and End should
        ///     match.
        /// </summary>
        /// <param name="newStart">Document _start position</param>
        /// <param name="newEnd">Document _end position</param>
        public override void Change(int newStart, int newEnd)
        {
            Invalidate();
            // This actually changes Start and End
            base.Change(newStart, newEnd);
        }


        /// <summary>
        ///     Collects the DropMarker and causes it to be removed from all
        ///     lists it belongs ti.
        /// </summary>
        public bool Collect()
        {
            return Collect(true);
        }


        internal bool Collect(bool dispose)
        {
            DropMarkerCollectEventArgs e = new DropMarkerCollectEventArgs(this);
            Scintilla.OnDropMarkerCollect(e);

            if (e.Cancel)
                return false;

            GotoStart();

            if (dispose)
                Dispose();

            return true;
        }


        /// <summary>
        ///     Overridden.
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Scintilla.DropMarkers.AllDocumentDropMarkers.Remove(this);
                Invalidate();
                base.Dispose();
            }
        }


        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return ((DropMarker)obj).Key == this.Key;
        }


        /// <summary>
        ///     Gets the Client Rectangle in pixels of the DropMarker's visual indicator.
        /// </summary>
        public Rectangle GetClientRectangle()
        {
            int x = NativeScintilla.PointXFromPosition(Start);
            int y = NativeScintilla.PointYFromPosition(Start) + NativeScintilla.TextHeight(0) - 2;

            // Invalidate the old Marker Location so that we don't get "Ghosts"
            return new Rectangle(x - 2, y, 5, 5);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        /// <summary>
        ///     Forces a repaint of the DropMarker
        /// </summary>
        public void Invalidate()
        {
            if (Scintilla != null && Start > 0)
            {
                //	Invalidate the old Marker Location so that we don't get "Ghosts"
                Scintilla.Invalidate(GetClientRectangle());
            }
        }


        protected internal override void Paint(Graphics g)
        {
            base.Paint(g);

            if (IsDisposed)
                return;

            int x = NativeScintilla.PointXFromPosition(Start);
            int y = NativeScintilla.PointYFromPosition(Start) + NativeScintilla.TextHeight(0) - 2;

            //	Draw a red Triangle with a dark red border at the marker position
            g.FillPolygon(Brushes.Red, new Point[] { new Point(x - 2, y + 4), new Point(x, y), new Point(x + 2, y + 4) });
            g.DrawPolygon(Pens.DarkRed, new Point[] { new Point(x - 2, y + 4), new Point(x, y), new Point(x + 2, y + 4) });
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Overridden. Drop Markers are points, not a spanned range. Though this could change in the future.
        /// </summary>
        public override bool IsPoint
        {
            get
            {
                return Start == End;
            }
        }


        /// <summary>
        ///     Uniquely identifies the DropMarker
        /// </summary>
        public Guid Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }


        /// <summary>
        ///     Not currently used, the offset in pixels from the document view's top.
        /// </summary>
        public int TopOffset
        {
            get
            {
                return _topOffset;
            }
            set
            {
                _topOffset = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal DropMarker(int start, int end, int topOffset, Scintilla scintilla) : base(start, end, scintilla)
        {
            base.Start      = start;
            base.End        = end;
            this._topOffset = topOffset;
        }

        #endregion Constructors
    }
}
