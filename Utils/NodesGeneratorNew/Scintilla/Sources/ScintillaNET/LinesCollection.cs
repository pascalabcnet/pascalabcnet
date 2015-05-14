#region Using Directives

using System;
using System.Collections;

#endregion Using Directives


namespace ScintillaNET
{
    public class LineCollection : TopLevelHelper, ICollection
    {
        #region Methods

        public void CopyTo(Array array, int index)
        {
            if(array == null)
                throw new ArgumentNullException("array");

            if(index < 0)
                throw new ArgumentOutOfRangeException("index");

            if(index >= array.Length)
                throw new ArgumentException("index is equal to or greater than the _length of array.");

            int count = Count;
            if(count > array.Length - index)
                throw new ArgumentException("The number of elements in the source ICollection is greater than the available space from number to the _end of the destination array.");

            for(int i=index; i<count; i++)
                array.SetValue(this[i], i);
        }


        public Line FromPosition(int position)
        {
            return this[NativeScintilla.LineFromPosition(position)];
        }


        public Line FromVisibleLineNumber(int displayLine)
        {
            return new Line(Scintilla, NativeScintilla.DocLineFromVisible(displayLine));
        }


        public IEnumerator GetEnumerator()
        {
            return new LinesEnumerator(this);
        }


        public Line GetMaxLineWithState()
        {
            int line = NativeScintilla.GetMaxLineState();
            if (line < 0)
                return null;

            return this[line];
        }


        public void Hide(int startLine, int endLine)
        {
            NativeScintilla.HideLines(startLine, endLine);
        }


        public void Join(int startLine, int endLine)
        {
            NativeScintilla.SetTargetStart(startLine);
            NativeScintilla.SetTargetEnd(endLine);
            Scintilla.DirectMessage(NativeMethods.SCI_LINESJOIN, IntPtr.Zero, IntPtr.Zero);
        }


        private void Show(int startLine, int endLine)
        {
            NativeScintilla.ShowLines(startLine, endLine);
        }

        #endregion Methods


        #region Properties

        public int Count
        {
            get
            {
                return Scintilla.DirectMessage(NativeMethods.SCI_GETLINECOUNT, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
        }


        public Line Current
        {
            get
            {
                return this[Scintilla.Caret.LineNumber];
            }
        }


        public Line FirstVisible
        {
            get
            {
                return this[FirstVisibleIndex];
            }
        }



        // TODO I'm not totally happy with the name of this property
        /// <summary>
        /// Gets or sets the first visible line in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>The zero-based index of the first visible line in a <see cref="Scintilla" /> control.</returns>
        /// <remarks>The value is a visible line rather than a document line.</remarks>
        public int FirstVisibleIndex
        {
            get
            {
                return Scintilla.DirectMessage(NativeMethods.SCI_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            set
            {
                Scintilla.DirectMessage(NativeMethods.SCI_SETFIRSTVISIBLELINE, new IntPtr(value), IntPtr.Zero);
            }
        }


        public bool IsSynchronized
        {
            get { return false; }
        }


        public object SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }


        public Line this[int index]
        {
            get
            {
                return new Line(Scintilla, index);
            }
        }


        public int VisibleCount
        {
            get
            {
                return NativeScintilla.LinesOnScreen();
            }
        }


        public Line[] VisibleLines
        {
            get
            {
                // [workitem:21678] 2009-10-14 Chris Rickard
                // This whole thing was fubard. mjpa fixed part of it but another issue arose
                // that VisibleCount returns how many *possible* lines are visible, not 
                // taking into account that there may not be that many lines defined in 
                // the document.
                int min = FirstVisibleIndex;
                int max = min + VisibleCount + 1;
                if (max > this.Count)
                    max = this.Count;

                Line[] ret = new Line[max - min];

                for (int i = min; i < max; i++)
                    ret[i - min] = FromVisibleLineNumber(i);

                return ret;
            }
        }

        #endregion Properties


        #region Constructors

        protected internal LineCollection(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors


        #region Types

        private class LinesEnumerator : IEnumerator
        {
            #region Fields

            private int _count;
            private int _index = -1;
            private LineCollection _lines;

            #endregion Fields


            #region Methods

            public bool MoveNext()
            {
                if(++_index >= _count)
                    return false;

                return true;
            }


            public void Reset()
            {
                _index = -1;
            }

            #endregion Methods


            #region Properties

            public object Current
            {
                get { return _lines[_index]; }
            }

            #endregion Properties


            #region Constructors

            public LinesEnumerator(LineCollection lines)
            {
                _lines = lines;
                _count = lines.Count;
            }

            #endregion Constructors
        }

        #endregion Types
    }
}
