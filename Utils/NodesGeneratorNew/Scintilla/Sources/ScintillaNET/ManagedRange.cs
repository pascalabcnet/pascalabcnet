#region Using Directives

using System;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    public class ManagedRange : Range, IDisposable
    {
        #region Fields

        internal bool PendingDeletion = false;

        #endregion Fields


        #region Methods

        public virtual void Change(int newStart, int newEnd)
        {
            base.Start = newStart;
            base.End = newEnd;
        }


        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Scintilla.ManagedRanges.Remove(this);
                Scintilla = null;
                IsDisposed = true;
            }
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected internal virtual void Paint(Graphics g)
        {

        }

        #endregion Methods


        #region Properties

        public override int End
        {
            get
            {
                return base.End;
            }
            set
            {
                base.End = value;
            }
        }


        // This is important for determining how to treat 
        // managed ranges during insert and delete events.
        public virtual bool IsPoint { get { return false; } }


        public override int Start
        {
            get
            {
                return base.Start;
            }
            set
            {
                base.Start = value;
            }
        }

        #endregion Properties


        #region Constructors

        protected internal ManagedRange() { }

        public ManagedRange(int start, int end, Scintilla scintilla) : base(start, end, scintilla)
        {

        }


        protected internal ManagedRange(Range range) : this(range.Start, range.End, range.Scintilla)
        {

        }

        #endregion Constructors
    }
}
