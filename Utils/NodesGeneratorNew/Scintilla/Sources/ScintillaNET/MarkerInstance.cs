#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    public class MarkerInstance : ScintillaHelperBase
    {
        #region Fields

        private int _handle;
        private Marker _marker;

        #endregion Fields


        #region Methods

        public void Delete()
        {
            NativeScintilla.MarkerDeleteHandle(_handle);
        }


        public override bool Equals(object obj)
        {
            if (!IsSameHelperFamily(obj))
                return false;

            return ((MarkerInstance)obj).Handle == this.Handle;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Methods


        #region Properties

        public int Handle
        {
            get
            {
                return _handle;
            }
        }


        public Line Line
        {
            get
            {
                int lineNo = NativeScintilla.MarkerLineFromHandle(_handle);
                if (lineNo < 0)
                    return null;

                return new Line(Scintilla, lineNo);

            }
        }


        public Marker Marker
        {
            get
            {
                return _marker;
            }
        }

        #endregion Properties


        #region Constructors

        internal MarkerInstance(Scintilla scintilla, Marker marker, int handle) : base(scintilla)
        {
            _marker = marker;
            _handle = handle;
        }

        #endregion Constructors
    }
}
