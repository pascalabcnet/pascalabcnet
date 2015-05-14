#region Using Directives

using System;
using System.Runtime.InteropServices;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     A range within the editor. Start and End are both Positions.
    /// </summary>
    public class Range : ScintillaHelperBase, IComparable
    {
        #region Fields

        private int _end;
        private int _start;

        #endregion Fields


        #region Methods

        public void ClearIndicator(int indicator)
        {
            NativeScintilla.SetIndicatorCurrent(indicator);
            NativeScintilla.IndicatorClearRange(_start, Length);
        }


        public void ClearIndicator(Indicator indicator)
        {
            NativeScintilla.SetIndicatorCurrent(indicator.Number);
            NativeScintilla.IndicatorClearRange(_start, Length);
        }


        /// <summary>
        ///     Collapses all folds
        /// </summary>
        public void CollapseAllFolds()
        {
            for (int i = startingLine; i < endingLine; i++)
            {
                int maxSubOrd = NativeScintilla.GetLastChild(i, -1);
                NativeScintilla.SetFoldExpanded(i, false);
                NativeScintilla.HideLines(i + 1, maxSubOrd);
            }
        }


        public void Colorize()
        {
            NativeScintilla.Colourise(_start, _end);
        }


        public int CompareTo(object otherObj)
        {
            Range other = otherObj as Range;

            if(other == null)
                return 1;

            if(other._start < _start)
                return 1;
            else if(other._start > _start)
                return -1;

            //	Starts must equal, lets try ends
            if(other._end < _end)
                return 1;
            else if(other._end > _end)
                return -1;

            //	Start and End equal. Comparitavely the same
            return 0;
        }


        public void Copy() 
        {
            Copy(CopyFormat.Text);
        }


        public void Copy(CopyFormat format)
        {
            if(format == CopyFormat.Text)
            {
                Scintilla.Clipboard.Copy(_start, _end);
            }
            else if(format == CopyFormat.Rtf)
            {
                throw new NotImplementedException("Someday...");
            }
            else
            {

            }
        }


        public override bool Equals(object obj)
        {
            Range r = obj as Range;
            if(r == null)
                return false;

            return r._start == _start && r._end == _end;
        }


        /// <summary>
        ///     Expands all folds
        /// </summary>
        public void ExpandAllFolds()
        {
            for (int i = startingLine; i < endingLine; i++)
            {
                NativeScintilla.SetFoldExpanded(i, true);
                NativeScintilla.ShowLines(i + 1, i + 1);
            }
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public void GotoEnd()
        {
            NativeScintilla.GotoPos(_end);
        }


        public void GotoStart()
        {
            NativeScintilla.GotoPos(_start);
        }


        public void HideLines()
        {
            NativeScintilla.HideLines(startingLine, endingLine);
        }


        public bool IntersectsWith(Range otherRange)
        {
            return otherRange.PositionInRange(_start) | otherRange.PositionInRange(_end) | PositionInRange(otherRange.Start) | PositionInRange(otherRange.End);
        }


        public bool PositionInRange(int position)
        {
            return position >= _start && position <= _end;
        }


        public void Select()
        {
            NativeScintilla.SetSel(_start, _end);
        }


        // Chris Rickard 7/10/2007
        // Woo hoo! Modern Indicator support. We won't even
        // mess with legacy indicators as they'll be removed
        // from Scintilla Someday
        public void SetIndicator(int indicator)
        {
            NativeScintilla.SetIndicatorCurrent(indicator);
            NativeScintilla.IndicatorFillRange(_start, Length);
        }


        // Now the Scintilla documentation is a little unclear to me,
        // but it seems as though the whole indicator value doesn't 
        // really do anything yet, but may in the future.
        public void SetIndicator(int indicator, int value)
        {
            NativeScintilla.SetIndicatorValue(value);
            NativeScintilla.SetIndicatorCurrent(indicator);
            NativeScintilla.IndicatorFillRange(_start, Length);
        }


        public void SetStyle(string styleName)
        {
            SetStyle(Scintilla.Lexing.StyleNameMap[styleName]);
        }


        public void SetStyle(int style)
        {
            SetStyle(0xff, style);
        }


        public void SetStyle(byte styleMask, string styleName)
        {
            SetStyle(styleMask, Scintilla.Lexing.StyleNameMap[styleName]);
        }


        public void SetStyle(byte styleMask, int style)
        {
            NativeScintilla.StartStyling(_start, (int)styleMask);
            NativeScintilla.SetStyling(Length, style);
        }


        public void ShowLines()
        {
            NativeScintilla.ShowLines(startingLine, endingLine);
        }


        /// <summary>
        ///     Removes trailing spaces from each line
        /// </summary>
        public void StripTrailingSpaces()
        {
            NativeScintilla.BeginUndoAction();

            for (int line = startingLine; line < endingLine; line++)
            {
                int lineStart = NativeScintilla.PositionFromLine(line);
                int lineEnd = NativeScintilla.GetLineEndPosition(line);
                int i = lineEnd - 1;
                char ch = NativeScintilla.GetCharAt(i);
                while ((i >= lineStart) && ((ch == ' ') || (ch == '\t')))
                {
                    i--;
                    ch = NativeScintilla.GetCharAt(i);
                }
                if (i == lineStart - 1)
                {
                    ch = NativeScintilla.GetCharAt(i + 1);
                    while (i < lineEnd && ch == '\t')
                    {
                        i++;
                        ch = NativeScintilla.GetCharAt(i + 1);
                    }
                }
                if (i < (lineEnd - 1))
                {
                    NativeScintilla.SetTargetStart(i + 1);
                    NativeScintilla.SetTargetEnd(lineEnd);
                    NativeScintilla.ReplaceTarget(0, string.Empty);
                }
            }
            NativeScintilla.EndUndoAction();
        }


        public override string ToString()
        {
        
            return "{Start=" + _start + ", End=" + _end + ", Length=" + Length + "}";
        }

        #endregion Methods


        #region Properties

        public bool Collapsed
        {
            get { return _start == _end; }
        }


        public virtual int End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
            }
        }


        private int endingLine
        {
            get
            {
                return NativeScintilla.LineFromPosition(_end);
            }
        }


        public Line EndingLine
        {
            get
            {
                return new Line(Scintilla, endingLine);
            }
        }


        public bool IsMultiLine
        {
            get
            {
                return !StartingLine.Equals(EndingLine);
            }
        }


        public int Length
        {
            get
            {
                return _end - _start;
            }
        }


        public virtual int Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
            }
        }


        private int startingLine
        {
            get
            {
                return NativeScintilla.LineFromPosition(_start);
            }
        }


        public Line StartingLine
        {
            get
            {
                return new Line(Scintilla, startingLine);
            }
        }


        public byte[] StyledText
        {
            get
            {
                if(Start < 0 || End < 0 || Scintilla == null)
                    return new byte[0];

                int bufferLength    = (Length * 2) + 2;
                TextRange rng       = new TextRange();
                rng.lpstrText       = Marshal.AllocHGlobal(bufferLength);
                rng.chrg.cpMin      = _start;
                rng.chrg.cpMax      = _end;
                
                NativeScintilla.GetStyledText(ref rng);

                byte[] ret = new byte[bufferLength];
                    Marshal.Copy(rng.lpstrText, ret, 0, bufferLength);

                Marshal.FreeHGlobal(rng.lpstrText);
                return ret;
            }
        }


        public string Text
        {
            get
            {
                if (Start < 0 || End < 0 || Scintilla == null)
                    return String.Empty;

                TextRange rng = new TextRange();
                try
                {
                    rng.lpstrText = Marshal.AllocHGlobal(Length + 1);
                    rng.chrg.cpMin = _start;
                    rng.chrg.cpMax = _end;
                    
                    int len = (int)NativeScintilla.GetTextRange(ref rng);
                    string ret = Utilities.IntPtrToString(Scintilla.Encoding, rng.lpstrText, len);
                    return ret ?? String.Empty;
                }
                finally
                {
                    Marshal.FreeHGlobal(rng.lpstrText);
                }
            }
            set
            {
                NativeScintilla.SetTargetStart(_start);
                NativeScintilla.SetTargetEnd(_end);
                NativeScintilla.ReplaceTarget(-1, value);
            }
        }

        #endregion Properties


        #region Constructors

        protected internal Range() : base(null) { }


        public Range(int start, int end, Scintilla scintilla) : base(scintilla)
        {
            if (start < end)
            {
                _start = start;
                _end = end;
            }
            else
            {
                _start = end;
                _end = start;
            }
        }

        #endregion Constructors
    }
}
