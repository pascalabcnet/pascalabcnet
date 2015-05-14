#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ScintillaNET.Configuration;
using ScintillaNET.Design;
using ScintillaNET.Properties;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents a Scintilla text editor control.
    /// </summary>
    [Designer(typeof(ScintillaDesigner))]
    [Docking(DockingBehavior.Ask)]
    [DefaultBindingProperty("Text")]
    [DefaultProperty("Text")]
    [DefaultEvent("DocumentChanged")]
    public partial class Scintilla : Control, INativeScintilla, ISupportInitialize
    {
        #region Fields

        private static IntPtr _moduleHandle;
        private static string _moduleName = (IntPtr.Size == 4 ? Resources.ModuleName : Resources.ModuleName64);
        private static NativeMethods.Scintilla_DirectFunction _directFunction;
        private IntPtr _directPointer;

        private static readonly object _annotationChangedEventKey = new object();
        private static readonly object _autoCompleteAcceptedEventKey = new object();
        private static readonly object _beforeTextDeleteEventKey = new object();
        private static readonly object _beforeTextInsertEventKey = new object();
        private static readonly object _borderStyleChangedEventKey = new object();
        private static readonly object _callTipClickEventKey = new object();
        private static readonly object _charAddedEventKey = new object();
        private static readonly object _documentChangeEventKey = new object();
        private static readonly object _dropMarkerCollectEventKey = new object();
        private static readonly object _dwellEndEventKey = new object();
        private static readonly object _dwellStartEventKey = new object();
        private static readonly object _foldChangedEventKey = new object();
        private static readonly object _hotspotClickedEventKey = new object();
        private static readonly object _hotspotDoubleClickedEventKey = new object();
        private static readonly object _hotspotReleaseClickEventKey = new object();
        private static readonly object _indicatorClickEventKey = new object();
        private static readonly object _linesNeedShownEventKey = new object();
        private static readonly object _loadEventKey = new object();
        private static readonly object _macroRecordEventKey = new object();
        private static readonly object _marginClickEventKey = new object();
        private static readonly object _markerChangedEventKey = new object();
        private static readonly object _modifiedChangedEventKey = new object();
        private static readonly object _readOnlyModifyAttemptEventKey = new object();
        private static readonly object _scrollEventKey = new object();
        private static readonly object _selectionChangedEventKey = new object();
        private static readonly object _styleNeededEventKey = new object();
        private static readonly object _textDeletedEventKey = new object();
        private static readonly object _textInsertedEventKey = new object();
        private static readonly object _uriDroppedEventKey = new object();
        private static readonly object _userListEventKey = new object();
        private static readonly object _zoomChangedEventKey = new object();

        private static readonly int _modifiedState = BitVector32.CreateMask();
        private static readonly int _acceptsReturnState = BitVector32.CreateMask(_modifiedState);
        private static readonly int _acceptsTabState = BitVector32.CreateMask(_acceptsReturnState);
        private BitVector32 _state;

        private AnnotationCollection _annotations;
        private LineWrapping _lineWrapping;

        private List<TopLevelHelper> _helpers = new List<TopLevelHelper>();
        private AutoComplete _autoComplete;
        private CallTip _callTip;
        private CaretInfo _caret;
        private Clipboard _clipboard;
        private Commands _commands;
        private Configuration.ConfigurationManager _configurationManager;
        private DocumentHandler _documentHandler;
        private DocumentNavigation _documentNavigation;
        private DropMarkers _dropMarkers;
        private Encoding _encoding;
        private EndOfLine _endOfLine;
        private FindReplace _findReplace;
        private Folding _folding;
        private GoTo _goto;
        private HotspotStyle _hotspotStyle;
        private Indentation _indentation;
        private IndicatorCollection _indicators;
        private Lexing _lexing;
        private LineCollection _lines;
        private LongLines _longLines;
        private MarginCollection _margins;
        private MarkerCollection _markers;
        private Printing _printing;
        private Scrolling _scrolling;
        private Selection _selection;
        private SnippetManager _snippets;
        private StyleCollection _styles;
        private UndoRedo _undoRedo;
        private Whitespace _whitespace;

        private bool _allowDrop;
        private string _caption;
        private Dictionary<string, Color> _colorBag = new Dictionary<string, Color>();

        /// <summary>
        ///     Enables the brace matching from current position.
        /// </summary>
        private bool _isBraceMatching = false;
        private bool _isCustomPaintingEnabled = true;
        private bool _isInitializing = false;
        private List<ManagedRange> _managedRanges = new List<ManagedRange>();
        private bool _matchBraces = true;

        private INativeScintilla _ns;
        private Hashtable _propertyBag = new Hashtable();
        private SearchFlags _searchFlags = SearchFlags.Empty;
        private bool _supressControlCharacters = true;

        // List of Scintilla Supported encodings
        internal static readonly IList<Encoding> ValidCodePages = new Encoding[]
        {
            Encoding.ASCII,
            Encoding.UTF8,
            Encoding.Unicode,           // UTF-16
            Encoding.GetEncoding(932),  // shift_jis - Japanese (Shift-JIS)
            Encoding.GetEncoding(936),  // gb2312 - Chinese Simplified (GB2312)
            Encoding.GetEncoding(949),  // ks_c_5601-1987  - Korean
            Encoding.GetEncoding(950),  // big5 - Chinese Traditional (Big5)
            Encoding.GetEncoding(1361)  // Johab - Korean (Johab)
        };

        // This has to be set *early* because CreateParams is called before our constructor
        private BorderStyle _borderStyle = BorderStyle.Fixed3D;
        private VisualStyleRenderer _renderer;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Adds a line _end marker to the _end of the document
        /// </summary>
        public void AddLastLineEnd()
        {
            EndOfLineMode eolMode = _endOfLine.Mode;
            string eolMarker = "\r\n";

            if (eolMode == EndOfLineMode.CR)
                eolMarker = "\r";
            else if (eolMode == EndOfLineMode.LF)
                eolMarker = "\n";

            int tl = TextLength;
            int start = tl - eolMarker.Length;

            if (start < 0 || GetRange(start, start + eolMarker.Length).Text != eolMarker)
                AppendText(eolMarker);
        }


        /// <summary>
        ///     Appends a copy of the specified string to the _end of this instance.
        /// </summary>
        /// <param name="text">The <see cref="String"/> to append.</param>
        /// <returns>A <see cref="Range"/> representing the appended text.</returns>
        public Range AppendText(string text)
        {
            int oldLength = TextLength;
            NativeInterface.AppendText(Encoding.GetByteCount(text), text);
            return GetRange(oldLength, TextLength);
        }


        public void BeginInit()
        {
            _isInitializing = true;
        }


        public char CharAt(int position)
        {
            return _ns.GetCharAt(position);
        }


        /// <summary>
        ///     Creates and returns a new <see cref="AnnotationCollection" /> object.
        /// </summary>
        /// <returns>A new <see cref="AnnotationCollection" /> object.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual AnnotationCollection CreateAnnotationsInstance()
        {
            return new AnnotationCollection(this);
        }


        /// <summary>
        ///     Creates and returns a new <see cref="LineWrapping" /> object.
        /// </summary>
        /// <returns>A new <see cref="LineWrapping" /> object.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual LineWrapping CreateLineWrappingInstance()
        {
            return new LineWrapping(this);
        }


        /// <summary>
        ///     Sends the specified message directly to the native Scintilla window,
        ///     bypassing any managed APIs.
        /// </summary>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message <c>wparam</c> field.</param>
        /// <param name="lParam">The message <c>lparam</c> field.</param>
        /// <returns>An <see cref="IntPtr"/> representing the result of the message request.</returns>
        /// <remarks>
        ///     Warning: The Surgeon General Has Determined that Calling the Underlying Scintilla
        ///     Window Directly May Result in Unexpected Behavior!
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        ///     The method was called from a thread other than the thread it was created on.
        /// </exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IntPtr DirectMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            // Enforce illegal cross-thread calls
            if (Form.CheckForIllegalCrossThreadCalls && InvokeRequired)
            {
                string message = string.Format(Resources.Culture, Resources.Exception_IllegalCrossThreadCall, Name);
                throw new InvalidOperationException(message);
            }

            // Call the direct function delegate
            return _directFunction(_directPointer, msg, wParam, lParam);
        }


        /// <summary>
        ///     Overridden. Releases the unmanaged resources used by the <see cref="Control" /> and
        ///     its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            foreach (ScintillaHelperBase heler in _helpers)
            {
                heler.Dispose();
            }

            if (disposing && IsHandleCreated)
            {
                // wi11811 2008-07-28 Chris Rickard
                // Since we eat the destroy message in WndProc
                // we have to manually let Scintilla know to
                // clean up its resources.
                Message destroyMessage = new Message();
                destroyMessage.Msg = NativeMethods.WM_DESTROY;
                destroyMessage.HWnd = Handle;
                base.DefWndProc(ref destroyMessage);
            }

            base.Dispose(disposing);
        }


        public void EndInit()
        {
            _isInitializing = false;
            foreach (ScintillaHelperBase helper in _helpers)
            {
                helper.Initialize();
            }
        }


        /// <summary>
        ///     Exports a HTML representation of the current document.
        /// </summary>
        /// <returns>A <see cref="String"/> containing the contents of the document formatted as HTML.</returns>
        /// <remarks>Only ASCII documents are supported. Other encoding types have undefined behavior.</remarks>
        public string ExportHtml()
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                ExportHtml(sw, "Untitled", false);

            return sb.ToString();
        }


        /// <summary>
        ///     Exports a HTML representation of the current document.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter"/>with which to write. </param>
        /// <param name="title">The title of the HTML document.</param>
        /// <param name="allStyles">
        ///     <c>true</c> to output all styles including those not
        ///     used in the document; otherwise, <c>false</c>.
        /// </param>
        /// <remarks>Only ASCII documents are supported. Other encoding types have undefined behavior.</remarks>
        public void ExportHtml(TextWriter writer, string title, bool allStyles)
        {
            // Make sure the document is current
            // Lexing.Colorize();

            // Get the styles used
            int length = NativeInterface.GetLength();
            bool[] stylesUsed = new bool[(int)StylesCommon.Max + 1];
            if (allStyles)
            {
                for (int i = 0; i < stylesUsed.Length; i++)
                    stylesUsed[i] = true;
            }
            else
            {
                // Record all the styles used
                for (int i = 0; i < length; i++)
                    stylesUsed[Styles.GetStyleAt(i) & (int)StylesCommon.Max] = true;
            }

            // The tab width
            int tabWidth = Indentation.TabWidth;

            // Start writing
            writer.WriteLine(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01 Transitional//EN"" ""http://www.w3.org/TR/html4/loose.dtd"">");
            writer.WriteLine("<html>");
            writer.WriteLine("<head>");
            writer.WriteLine("<title>{0}</title>", title);
            writer.WriteLine(@"<style type=""text/css"">");
            writer.WriteLine();

            // Write the body style
            writer.WriteLine("body {");
            writer.WriteLine("background-color: {0};", Utilities.ColorToHtml(Styles.Default.BackColor));
            if (LineWrapping.Mode == LineWrappingMode.None)
                writer.WriteLine("white-space: nowrap;");
            writer.WriteLine("}");
            writer.WriteLine();

            // Write the styles
            for (int i = 0; i < stylesUsed.Length; i++)
            {
                if (!stylesUsed[i])
                    continue;

                Style s = Styles[i];
                writer.WriteLine("span.s{0} {{", i);
                writer.WriteLine("font-family: \"" + s.FontName + "\";");
                writer.WriteLine("font-size: {0}pt;", s.Size);
                if (s.Italic)
                    writer.WriteLine("font-style: italic;");
                if (s.Bold)
                    writer.WriteLine("font-weight: bold;");
                if (!s.ForeColor.IsEmpty && s.ForeColor != Color.Transparent)
                    writer.WriteLine("color: {0};", Utilities.ColorToHtml(s.ForeColor));
                if (!s.BackColor.IsEmpty && s.BackColor != Color.Transparent)
                    writer.WriteLine("background-color: {0};", Utilities.ColorToHtml(s.BackColor));

                writer.WriteLine("}");
                writer.WriteLine();
            }

            writer.WriteLine("</style>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");

            // Write the document
            // TODO There's more to be done here to support codepages/UTF-8
            char lc;
            char c = '\0';
            int lastStyle = -1;
            for (int i = 0; i < length; i++)
            {
                lc = c;
                c = NativeInterface.GetCharAt(i);
                int style = Styles.GetStyleAt(i);
                if(style != lastStyle)
                {
                    if(lastStyle != -1)
                        writer.Write("</span>");

                    writer.Write(@"<span class=""s{0}"">", style);
                    lastStyle = style;
                }

                switch (c)
                {
                    case '\0':
                        continue;

                    case ' ':
                        if (lc == ' ')
                            writer.Write("&nbsp;");
                        else
                            writer.Write(c);
                        continue;

                    case '\t':
                        for (int t = 0; t < tabWidth; t++)
                            writer.Write("&nbsp; ");
                        continue;

                    case '\r':
                    case '\n':
                        if (c == '\r' && i < length - 1 && NativeInterface.GetCharAt(i + 1) == '\n')
                            i++;

                        if (lastStyle != -1)
                            writer.Write("</span>");

                        writer.WriteLine("<br />");
                        lastStyle = -1;
                        continue;

                    case '<':
                        writer.Write("&lt;");
                        continue;

                    case '>':
                        writer.Write("&gt;");
                        continue;

                    case '&':
                        writer.Write("&amp;");
                        continue;

                    default:
                        writer.Write(c);
                        continue;
                }
            }

            if (lastStyle != -1)
                writer.Write("</span>");

            writer.WriteLine();
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
        }


        public int FindColumn(int line, int column)
        {
            return _ns.FindColumn(line, column);
        }


        internal void FireCallTipClick(int arrow)
        {
            CallTipArrow a = (CallTipArrow)arrow;
            OverloadList ol = CallTip.OverloadList;
            CallTipClickEventArgs e;

            if (ol == null)
            {
                e = new CallTipClickEventArgs(a, -1, -1, null, CallTip.HighlightStart, CallTip.HighlightEnd);
            }
            else
            {
                int newIndex = ol.CurrentIndex;

                if (a == CallTipArrow.Down)
                {
                    if (ol.CurrentIndex == ol.Count - 1)
                        newIndex = 0;
                    else
                        newIndex++;
                }
                else if (a == CallTipArrow.Up)
                {
                    if (ol.CurrentIndex == 0)
                        newIndex = ol.Count - 1;
                    else
                        newIndex--;
                }

                e = new CallTipClickEventArgs(a, ol.CurrentIndex, newIndex, ol, CallTip.HighlightStart, CallTip.HighlightEnd);
            }

            OnCallTipClick(e);

            if (e.Cancel)
            {
                CallTip.Cancel();
            }
            else
            {
                if (ol != null)
                {
                    // We allow them to alse replace the list entirely or just
                    // manipulate the New Index
                    CallTip.OverloadList = e.OverloadList;
                    CallTip.OverloadList.CurrentIndex = e.NewIndex;
                    CallTip.ShowOverloadInternal();
                }
            }
        }


        internal void FireKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }


        internal void FireMarginClick(SCNotification n)
        {
            Margin m = Margins[n.margin];
            Keys k = Keys.None;

            if ((n.modifiers & (int)KeyMod.Alt) == (int)KeyMod.Alt)
                k |= Keys.Alt;

            if ((n.modifiers & (int)KeyMod.Ctrl) == (int)KeyMod.Ctrl)
                k |= Keys.Control;

            if ((n.modifiers & (int)KeyMod.Shift) == (int)KeyMod.Shift)
                k |= Keys.Shift;

            OnMarginClick(new MarginClickEventArgs(k, n.position, Lines.FromPosition(n.position), m, m.AutoToggleMarkerNumber, m.IsFoldMargin));
        }


        public int GetColumn(int position)
        {
            return _ns.GetColumn(position);
        }


        /// <summary>
        ///     Gets the text of the line containing the caret.
        /// </summary>
        /// <returns>A <see cref="String" /> representing the text of the line containing the caret.</returns>
        public unsafe string GetCurrentLine()
        {
            int tmp;
            return GetCurrentLine(out tmp);
        }


        /// <summary>
        ///     Gets the text of the line containing the caret and the current caret position within that line.
        /// </summary>
        /// <param name="caretPosition">When this method returns, contains the byte offset of the current caret position with the line.</param>
        /// <returns>A <see cref="String" /> representing the text of the line containing the caret.</returns>
        public unsafe string GetCurrentLine(out int caretPosition)
        {
            int length = DirectMessage(NativeMethods.SCI_GETCURLINE, IntPtr.Zero, IntPtr.Zero).ToInt32();
            byte[] buffer = new byte[length];
            fixed (byte* bp = buffer)
                caretPosition = DirectMessage(NativeMethods.SCI_GETCURLINE, new IntPtr(buffer.Length), new IntPtr(bp)).ToInt32();

            return Encoding.GetString(buffer, 0, length - 1);
        }


        public Range GetRange()
        {
            return new Range(0, _ns.GetTextLength(), this);
        }


        public Range GetRange(int position)
        {
            return new Range(position, position + 1, this);
        }


        public Range GetRange(int startPosition, int endPosition)
        {
            return new Range(startPosition, endPosition, this);
        }


        /// <summary>
        ///     Gets a word from the specified position
        /// </summary>
        public string GetWordFromPosition(int position)
        {
            // Chris Rickard 2008-07-28
            // Fixing implementation to actually return the word at the position...
            // Credit goes to Stumpii for the code.
            // As a side note: I think the previous code was implemented based off
            // some funky code I made for the snippet keyword detection, but since
            // it doesn't reference this method there's no reason to keep the buggy
            // behavior. I also removed the try..catch because in theory this
            // shouldn't throw and we REALLY shouldn't be eating exceptions at the
            // System.Exception level. If some _start popping up I can add some
            // conditionals or catch more specific Exceptions.
            int startPosition = NativeInterface.WordStartPosition(position, true);
            int endPosition = NativeInterface.WordEndPosition(position, true);
            return GetRange(startPosition, endPosition).Text;
        }


        /// <summary>
        ///     Inserts text at the current cursor position
        /// </summary>
        /// <param name="text">Text to insert</param>
        /// <returns>The range inserted</returns>
        public Range InsertText(string text)
        {
            NativeInterface.AddText(Encoding.GetByteCount(text), text);
            return GetRange(_caret.Position, Encoding.GetByteCount(text));
        }


        /// <summary>
        ///     Inserts text at the given position
        /// </summary>
        /// <param name="position">The position to insert text in</param>
        /// <param name="text">Text to insert</param>
        /// <returns>The text range inserted</returns>
        public Range InsertText(int position, string text)
        {
            NativeInterface.InsertText(position, text);
            return GetRange(position, Encoding.GetByteCount(text));
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.IsInputKey"/>.
        /// </summary>
        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Shift) != Keys.None)
                keyData ^= Keys.Shift;

            switch (keyData)
            {
                case Keys.Tab:
                    return _state[_acceptsTabState];
                case Keys.Enter:
                    return _state[_acceptsReturnState];
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.F:

                    return true;
            }

            return base.IsInputKey(keyData);
        }


        private static void LoadModule()
        {
            if (_moduleHandle == IntPtr.Zero)
            {
                // Load the Scintilla module into memory
                if ((_moduleHandle = NativeMethods.LoadLibrary(_moduleName)) == IntPtr.Zero)
                {
                    string message = string.Format(Resources.Culture, Resources.Exception_CannotLoadModule, _moduleName);
                    throw new Win32Exception(message, new Win32Exception(Marshal.GetLastWin32Error()));
                }

                // Get the direct function. We use GetProcAddress instead of DllImport
                // because we don't know the name of the module ahead of time.
                _directFunction = Marshal.GetDelegateForFunctionPointer(
                    NativeMethods.GetProcAddress(_moduleHandle, "Scintilla_DirectFunction"),
                    typeof(NativeMethods.Scintilla_DirectFunction)) as NativeMethods.Scintilla_DirectFunction;

                if (_directFunction == null)
                {
                    string message = string.Format(Resources.Culture, Resources.Exception_InvalidModule, _moduleName);
                    throw new Win32Exception(message, new Win32Exception(Marshal.GetLastWin32Error()));
                }
            }
        }


        private List<ManagedRange> ManagedRangesInRange(int firstPos, int lastPos)
        {
            // TODO: look into optimizing this so that it isn't a linear
            // search. This is fine for a few markers per document but
            // can be greatly improved if there are a large # of markers
            List<ManagedRange> ret = new List<ManagedRange>();
            foreach (ManagedRange mr in _managedRanges)
                if (mr.Start >= firstPos && mr.Start <= lastPos)
                    ret.Add(mr);

            return ret;
        }


        /// <summary>
        ///     Raises the <see cref="AnnotationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="AnnotationChangedEventArgs" /> that contains the event data.</param>
        protected virtual void OnAnnotationChanged(AnnotationChangedEventArgs e)
        {
            EventHandler<AnnotationChangedEventArgs> handler = Events[_annotationChangedEventKey] as EventHandler<AnnotationChangedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="AutoCompleteAccepted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="AutoCompleteAcceptedEventArgs"/> that contains the event data.</param>
        protected virtual void OnAutoCompleteAccepted(AutoCompleteAcceptedEventArgs e)
        {
            EventHandler<AutoCompleteAcceptedEventArgs> handler = Events[_autoCompleteAcceptedEventKey] as EventHandler<AutoCompleteAcceptedEventArgs>;
            if (handler != null)
                handler(this, e);

            if (e.Cancel)
                AutoComplete.Cancel();
        }


        /// <summary>
        ///     Raises the <see cref="BackColorChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data. </param>
        protected override void OnBackColorChanged(EventArgs e)
        {
            ResetStyles();
            base.OnBackColorChanged(e);
        }


        /// <summary>
        ///     Raises the <see cref="BeforeTextDelete"/> event.
        /// </summary>
        /// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
        protected virtual void OnBeforeTextDelete(TextModifiedEventArgs e)
        {
            int firstPos = e.Position;
            int lastPos = firstPos + e.Length;

            List<ManagedRange> deletedRanges = new List<ManagedRange>();
            foreach (ManagedRange mr in _managedRanges)
            {

                //	These ranges lie within the deleted range so
                //	the ranges themselves need to be deleted
                if (mr.Start >= firstPos && mr.End <= lastPos)
                {

                    //	If the entire range is being delete and NOT a superset of the range,
                    //	don't delete it, only collapse it.
                    if (!mr.IsPoint && e.Position == mr.Start && (e.Position + e.Length == mr.End))
                    {
                        mr.Change(mr.Start, mr.Start);
                    }
                    else
                    {
                        //	Notify the virtual Range that it needs to cleanup
                        mr.Change(-1, -1);

                        //	Mark for deletion after this foreach:
                        deletedRanges.Add(mr);

                    }
                }
                else if (mr.Start >= lastPos)
                {
                    //	These ranges are merely offset by the deleted range
                    mr.Change(mr.Start - e.Length, mr.End - e.Length);
                }
                else if (mr.Start >= firstPos && mr.Start <= lastPos)
                {
                    //	The left side of the managed range is getting
                    //	cut off
                    mr.Change(firstPos, mr.End - e.Length);
                }
                else if (mr.Start < firstPos && mr.End >= firstPos && mr.End >= lastPos)
                {
                    mr.Change(mr.Start, mr.End - e.Length);
                }
                else if (mr.Start < firstPos && mr.End >= firstPos && mr.End < lastPos)
                {
                    mr.Change(mr.Start, firstPos);
                }

            }

            foreach (ManagedRange mr in deletedRanges)
                mr.Dispose();

            EventHandler<TextModifiedEventArgs> handler = Events[_beforeTextDeleteEventKey] as EventHandler<TextModifiedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="BeforeTextInsert"/> event.
        /// </summary>
        /// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
        protected virtual void OnBeforeTextInsert(TextModifiedEventArgs e)
        {
            List<ManagedRange> offsetRanges = new List<ManagedRange>();
            foreach (ManagedRange mr in _managedRanges)
            {
                if (mr.Start == e.Position && mr.PendingDeletion)
                {
                    mr.PendingDeletion = false;
                    ManagedRange lmr = mr;
                    BeginInvoke(new MethodInvoker(delegate() { lmr.Change(e.Position, e.Position + e.Length); }));
                }

                //	If the Range is a single point we treat it slightly
                //	differently than a spanned range
                if (mr.IsPoint)
                {
                    //	Unlike a spanned range, if the insertion point of
                    //	the new text == the _start of the range (and thus
                    //	the _end as well) we offset the entire point.
                    if (mr.Start >= e.Position)
                        mr.Change(mr.Start + e.Length, mr.End + e.Length);
                    else if (mr.End >= e.Position)
                        mr.Change(mr.Start, mr.End + e.Length);
                }
                else
                {
                    //	We offset a spanned range entirely only if the
                    //	_start occurs after the insertion point of the new
                    //	text.
                    if (mr.Start > e.Position)
                        mr.Change(mr.Start + e.Length, mr.End + e.Length);
                    else if (mr.End >= e.Position)
                    {
                        //	However it the _start of the range == the insertion
                        //	point of the new text instead of offestting the
                        //	range we expand it.
                        mr.Change(mr.Start, mr.End + e.Length);
                    }
                }

            }

            EventHandler<TextModifiedEventArgs> handler = Events[_beforeTextInsertEventKey] as EventHandler<TextModifiedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="BorderStyleChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            EventHandler handler = Events[_borderStyleChangedEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="CallTipClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="CallTipClickEventArgs"/> that contains the event data.</param>
        protected virtual void OnCallTipClick(CallTipClickEventArgs e)
        {
            EventHandler<CallTipClickEventArgs> handler = Events[_callTipClickEventKey] as EventHandler<CallTipClickEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="CharAdded"/> event.
        /// </summary>
        /// <param name="e">An <see cref="CharAddedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCharAdded(CharAddedEventArgs e)
        {
            EventHandler<CharAddedEventArgs> handler = Events[_charAddedEventKey] as EventHandler<CharAddedEventArgs>;
            if (handler != null)
                handler(this, e);

            if (_indentation.SmartIndentType != SmartIndent.None)
                _indentation.CheckSmartIndent(e.Ch);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnCreateControl"/>.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            OnLoad(EventArgs.Empty);
        }


        /// <summary>
        ///     Raises the <see cref="DocumentChange"/> event.
        /// </summary>
        /// <param name="e">An <see cref="NativeScintillaEventArgs"/> that contains the event data.</param>
        protected virtual void OnDocumentChange(NativeScintillaEventArgs e)
        {
            EventHandler<NativeScintillaEventArgs> handler = Events[_documentChangeEventKey] as EventHandler<NativeScintillaEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Provides the support for code block selection
        /// </summary>
        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            if (_isBraceMatching)
            {
                int position = CurrentPos - 1,
                       bracePosStart = -1,
                       bracePosEnd = -1;

                char character = (char)CharAt(position);

                switch (character)
                {
                    case '{':
                    case '(':
                    case '[':
                        if (!this.PositionIsOnComment(position))
                        {
                            bracePosStart = position;
                            bracePosEnd = _ns.BraceMatch(position, 0) + 1;
                            _selection.Start = bracePosStart;
                            _selection.End = bracePosEnd;
                        }
                        break;
                }
            }
        }


        /// <summary>
        ///     Raises the <see cref="DropMarkerCollect"/> event.
        /// </summary>
        /// <param name="e">An <see cref="DropMarkerCollectEventArgs"/> that contains the event data.</param>
        protected internal virtual void OnDropMarkerCollect(DropMarkerCollectEventArgs e)
        {
            EventHandler<DropMarkerCollectEventArgs> handler = Events[_dropMarkerCollectEventKey] as EventHandler<DropMarkerCollectEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="DwellEnd"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
        protected virtual void OnDwellEnd(ScintillaMouseEventArgs e)
        {
            EventHandler<ScintillaMouseEventArgs> handler = Events[_dwellEndEventKey] as EventHandler<ScintillaMouseEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="DwellStart"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
        protected virtual void OnDwellStart(ScintillaMouseEventArgs e)
        {
            EventHandler<ScintillaMouseEventArgs> handler = Events[_dwellStartEventKey] as EventHandler<ScintillaMouseEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="FoldChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="FoldChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnFoldChanged(FoldChangedEventArgs e)
        {
            EventHandler<FoldChangedEventArgs> handler = Events[_foldChangedEventKey] as EventHandler<FoldChangedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="FontChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            ResetStyles();
            base.OnFontChanged(e);
        }


        /// <summary>
        ///     Raises the <see cref="ForeColorChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data. </param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            ResetStyles();
            base.OnForeColorChanged(e);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnGotFocus"/>.
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            if (!Selection.Hidden)
                _ns.HideSelection(false);

            _ns.SetSelBack(Selection.BackColor != Color.Transparent, Utilities.ColorToRgb(Selection.BackColor));
            _ns.SetSelFore(Selection.ForeColor != Color.Transparent, Utilities.ColorToRgb(Selection.ForeColor));

            base.OnGotFocus(e);
        }


        /// <summary>
        ///     Overridden. Raises the <see cref="Control.HandleCreated"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            // The Scintilla native control kindly registers itself as a target of OLE drag-and-drop operations,
            // however, that prevents us from doing the same. Unregistering it prior to calling base.OnHandleCreated
            // will re-register us according to the AllowDrop property and we'll get support for the Drag events.
            if (IsHandleCreated)
                NativeMethods.RevokeDragDrop(Handle);

            // Get the Scintilla direct pointer
            _directPointer = NativeMethods.SendMessage(Handle, NativeMethods.SCI_GETDIRECTPOINTER, IntPtr.Zero, IntPtr.Zero);
            if (_directPointer == IntPtr.Zero)
                throw new Win32Exception(Resources.Exception_CannotCreateDirectFunction);

            base.OnHandleCreated(e);
        }


        /// <summary>
        ///     Raises the <see cref="HotspotClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs"/> that contains the event data.</param>
        protected virtual void OnHotspotClick(HotspotClickEventArgs e)
        {
            EventHandler<HotspotClickEventArgs> handler = Events[_hotspotClickEventKey] as EventHandler<HotspotClickEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="HotspotDoubleClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs"/> that contains the event data.</param>
        protected virtual void OnHotspotDoubleClick(HotspotClickEventArgs e)
        {
            EventHandler<HotspotClickEventArgs> handler = Events[_hotspotDoubleClickEventKey] as EventHandler<HotspotClickEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="HotspotReleaseClick"/> event.
        /// </summary>
        /// <param name="e">A <see cref="HotspotClickEventArgs"/> that contains the event data.</param>
        protected virtual void OnHotspotReleaseClick(HotspotClickEventArgs e)
        {
            EventHandler<HotspotClickEventArgs> handler = Events[_hotspotReleaseClickEventKey] as EventHandler<HotspotClickEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="IndicatorClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ScintillaMouseEventArgs"/> that contains the event data.</param>
        protected virtual void OnIndicatorClick(ScintillaMouseEventArgs e)
        {
            EventHandler<ScintillaMouseEventArgs> handler = Events[_indicatorClickEventKey] as EventHandler<ScintillaMouseEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnKeyDown"/>.
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
                e.SuppressKeyPress = _commands.ProcessKey(e);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnKeyPress"/>.
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (_supressControlCharacters && (int)e.KeyChar < 32)
                e.Handled = true;

            if (_snippets.IsEnabled && _snippets.IsOneKeySelectionEmbedEnabled && _selection.Length > 0)
            {
                Snippet s;
                if (_snippets.List.TryGetValue(e.KeyChar.ToString(), out s))
                {
                    if (s.IsSurroundsWith)
                    {
                        _snippets.InsertSnippet(s);
                        e.Handled = true;
                    }
                }
            }

            base.OnKeyPress(e);
        }


        /// <summary>
        ///     Raises the <see cref="LinesNeedShown"/> event.
        /// </summary>
        /// <param name="e">An <see cref="LinesNeedShownEventArgs"/> that contains the event data.</param>
        protected virtual void OnLinesNeedShown(LinesNeedShownEventArgs e)
        {
            EventHandler<LinesNeedShownEventArgs> handler = Events[_linesNeedShownEventKey] as EventHandler<LinesNeedShownEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnLoad(EventArgs e)
        {
            EventHandler handler = Events[_loadEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnLostFocus"/>.
        /// </summary>
        protected override void OnLostFocus(EventArgs e)
        {
            if (Selection.HideSelection)
                _ns.HideSelection(true);

            _ns.SetSelBack(Selection.BackColorUnfocused != Color.Transparent, Utilities.ColorToRgb(Selection.BackColorUnfocused));
            _ns.SetSelFore(Selection.ForeColorUnfocused != Color.Transparent, Utilities.ColorToRgb(Selection.ForeColorUnfocused));

            base.OnLostFocus(e);
        }


        /// <summary>
        ///     Raises the <see cref="MacroRecord"/> event.
        /// </summary>
        /// <param name="e">An <see cref="MacroRecordEventArgs"/> that contains the event data.</param>
        protected virtual void OnMacroRecord(MacroRecordEventArgs e)
        {
            EventHandler<MacroRecordEventArgs> handler = Events[_macroRecordEventKey] as EventHandler<MacroRecordEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="MarginClick"/> event.
        /// </summary>
        /// <param name="e">An <see cref="MarginClickEventArgs"/> that contains the event data.</param>
        protected virtual void OnMarginClick(MarginClickEventArgs e)
        {
            EventHandler<MarginClickEventArgs> handler = Events[_marginClickEventKey] as EventHandler<MarginClickEventArgs>;
            if (handler != null)
                handler(this, e);

            if (e.ToggleMarkerNumber >= 0)
            {
                int mask = (int)Math.Pow(2, e.ToggleMarkerNumber);
                if ((e.Line.GetMarkerMask() & mask) == mask)
                    e.Line.DeleteMarker(e.ToggleMarkerNumber);
                else
                    e.Line.AddMarker(e.ToggleMarkerNumber);
            }

            if (e.ToggleFold)
                e.Line.ToggleFoldExpanded();
        }


        /// <summary>
        ///     Raises the <see cref="MarkerChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="MarkerChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnMarkerChanged(MarkerChangedEventArgs e)
        {
            EventHandler<MarkerChangedEventArgs> handler = Events[_markerChangedEventKey] as EventHandler<MarkerChangedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="ModifiedChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnModifiedChanged(EventArgs e)
        {
            EventHandler handler = Events[_modifiedChangedEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.OnPaint"/>.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            paintRanges(e.Graphics);
        }


        /// <summary>
        ///     Raises the <see cref="ReadOnlyModifyAttempt"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnReadOnlyModifyAttempt(EventArgs e)
        {
            EventHandler handler = Events[_readOnlyModifyAttemptEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="Scroll"/> event.
        /// </summary>
        /// <param name="e">An <see cref="ScrollEventArgs"/> that contains the event data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            EventHandler<ScrollEventArgs> handler = Events[_scrollEventKey] as EventHandler<ScrollEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            //this is being fired in tandem with the cursor blink...
            EventHandler handler = Events[_selectionChangedEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            if (_isBraceMatching && (_selection.Length == 0))
            {
                int position = CurrentPos - 1,
                    bracePosStart = -1,
                    bracePosEnd = -1;

                char character = (char)CharAt(position);

                switch (character)
                {
                    case '{':
                    case '}':
                    case '(':
                    case ')':
                    case '[':
                    case ']':
                        if (!this.PositionIsOnComment(position))
                        {
                            bracePosStart = position;
                            bracePosEnd = _ns.BraceMatch(position,0);

                            if(bracePosEnd >= 0)
                            {
                                _ns.BraceHighlight(bracePosStart, bracePosEnd);
                            }
                            else
                            {
                                _ns.BraceBadLight(bracePosStart);
                            }
                        }
                        break;
                    default:
                        position = CurrentPos;
                        character = (char)CharAt(position); //this is not being used anywhere... --Cory
                        _ns.BraceHighlight(bracePosStart, bracePosEnd);
                        break;
                }
            }
        }


        /// <summary>
        ///     Raises the <see cref="StyleNeeded"/> event.
        /// </summary>
        /// <param name="e">An <see cref="StyleNeededEventArgs"/> that contains the event data.</param>
        protected virtual void OnStyleNeeded(StyleNeededEventArgs e)
        {
            EventHandler<StyleNeededEventArgs> handler = Events[_styleNeededEventKey] as EventHandler<StyleNeededEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="TextDeleted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
        protected virtual void OnTextDeleted(TextModifiedEventArgs e)
        {
            EventHandler<TextModifiedEventArgs> handler = Events[_textDeletedEventKey] as EventHandler<TextModifiedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="TextInserted"/> event.
        /// </summary>
        /// <param name="e">An <see cref="TextModifiedEventArgs"/> that contains the event data.</param>
        protected virtual void OnTextInserted(TextModifiedEventArgs e)
        {
            EventHandler<TextModifiedEventArgs> handler = Events[_textInsertedEventKey] as EventHandler<TextModifiedEventArgs>;
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="ZoomChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected virtual void OnZoomChanged(EventArgs e)
        {
            EventHandler handler = Events[_zoomChangedEventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }


        private void paintRanges(Graphics g)
        {
            //	First we want to get the range (in positions) of what
            //	will be painted so that we know which markers to paint
            int firstLine = Lines.FirstVisibleIndex;
            int lastLine = firstLine + _ns.LinesOnScreen();
            int firstPos = _ns.PositionFromLine(firstLine);
            int lastPos = _ns.PositionFromLine(lastLine + 1) - 1;

            //	If the lastLine was outside the defined document range it will
            //	contain -1, defualt it to the last doc position
            if (lastPos < 0)
                lastPos = _ns.GetLength();

            List<ManagedRange> mrs = ManagedRangesInRange(firstPos, lastPos);


            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            foreach (ManagedRange mr in mrs)
            {
                mr.Paint(g);
            }
        }


        public int PointXFromPosition(int position)
        {
            return _ns.PointXFromPosition(position);
        }


        public int PointYFromPosition(int position)
        {
            return _ns.PointYFromPosition(position);
        }


        public int PositionFromPoint(int x, int y)
        {
            return _ns.PositionFromPoint(x, y);
        }


        public int PositionFromPointClose(int x, int y)
        {
            return _ns.PositionFromPointClose(x, y);
        }


        /// <summary>
        ///     Checks that if the specified position is on comment.
        /// </summary>
        public bool PositionIsOnComment(int position)
        {
            //this.Colorize(0, -1);
            return PositionIsOnComment(position, _lexing.Lexer);
        }


        /// <summary>
        ///     Checks that if the specified position is on comment.
        /// </summary>
        public bool PositionIsOnComment(int position, Lexer lexer)
        {
            int style = _styles.GetStyleAt(position);
            if ((lexer == Lexer.Python || lexer == Lexer.Lisp)
                && (style == 1
                || style == 12))
            {
                return true; // python or lisp
            }
            else if ((lexer == Lexer.Cpp || lexer == Lexer.Pascal || lexer == Lexer.Tcl || lexer == Lexer.Bullant)
                && (style == 1
                || style == 2
                || style == 3
                || style == 15
                || style == 17
                || style == 18))
            {
                return true; // cpp, tcl, bullant or pascal
            }
            else if ((lexer == Lexer.Hypertext || lexer == Lexer.Xml)
                && (style == 9
                || style == 20
                || style == 29
                || style == 30
                || style == 42
                || style == 43
                || style == 44
                || style == 57
                || style == 58
                || style == 59
                || style == 72
                || style == 82
                || style == 92
                || style == 107
                || style == 124
                || style == 125))
            {
                return true; // html or xml
            }
            else if ((lexer == Lexer.Perl || lexer == Lexer.Ruby || lexer == Lexer.Clw || lexer == Lexer.Bash)
                && style == 2)
            {
                return true; // perl, bash, clarion/clw or ruby
            }
            else if ((lexer == Lexer.Sql)
                && (style == 1
                || style == 2
                || style == 3
                || style == 13
                || style == 15
                || style == 17
                || style == 18))
            {
                return true; // sql
            }
            else if ((lexer == Lexer.VB || lexer == Lexer.Properties || lexer == Lexer.MakeFile || lexer == Lexer.Batch || lexer == Lexer.Diff || lexer == Lexer.Conf || lexer == Lexer.Ave || lexer == Lexer.Eiffel || lexer == Lexer.EiffelKw || lexer == Lexer.Tcl || lexer == Lexer.VBScript || lexer == Lexer.MatLab || lexer == Lexer.Fortran || lexer == Lexer.F77 || lexer == Lexer.Lout || lexer == Lexer.Mmixal || lexer == Lexer.Yaml || lexer == Lexer.PowerBasic || lexer == Lexer.ErLang || lexer == Lexer.Octave || lexer == Lexer.Kix || lexer == Lexer.Asn1)
                && style == 1)
            {
                return true; // asn1, vb, diff, batch, makefile, avenue, eiffel, eiffelkw, vbscript, matlab, crontab, fortran, f77, lout, mmixal, yaml, powerbasic, erlang, octave, kix or properties
            }
            else if ((lexer == Lexer.Latex)
                && style == 4)
            {
                return true; // latex
            }
            else if ((lexer == Lexer.Lua || lexer == Lexer.EScript || lexer == Lexer.Verilog)
                && (style == 1
                || style == 2
                || style == 3))
            {
                return true; // lua, verilog or escript
            }
            else if ((lexer == Lexer.Ada)
                && style == 10)
            {
                return true; // ada
            }
            else if ((lexer == Lexer.Baan || lexer == Lexer.Pov || lexer == Lexer.Ps || lexer == Lexer.Forth || lexer == Lexer.MsSql || lexer == Lexer.Gui4Cli || lexer == Lexer.Au3 || lexer == Lexer.Apdl || lexer == Lexer.Vhdl || lexer == Lexer.Rebol)
                && (style == 1
                || style == 2))
            {
                return true; // au3, apdl, baan, ps, mssql, rebol, forth, gui4cli, vhdl or pov
            }
            else if ((lexer == Lexer.Asm)
                && (style == 1
                || style == 11))
            {
                return true; // asm
            }
            else if ((lexer == Lexer.Nsis)
                && (style == 1
                || style == 18))
            {
                return true; // nsis
            }
            else if ((lexer == Lexer.Specman)
                && (style == 2
                || style == 3))
            {
                return true; // specman
            }
            else if ((lexer == Lexer.Tads3)
                && (style == 3
                || style == 4))
            {
                return true; // tads3
            }
            else if ((lexer == Lexer.CSound)
                && (style == 1
                || style == 9))
            {
                return true; // csound
            }
            else if ((lexer == Lexer.Caml)
                && (style == 12
                || style == 13
                || style == 14
                || style == 15))
            {
                return true; // caml
            }
            else if ((lexer == Lexer.Haskell)
                && (style == 13
                || style == 14
                || style == 15
                || style == 16))
            {
                return true; // haskell
            }
            else if ((lexer == Lexer.Flagship)
                && (style == 1
                || style == 2
                || style == 3
                || style == 4
                || style == 5
                || style == 6))
            {
                return true; // flagship
            }
            else if ((lexer == Lexer.Smalltalk)
                && style == 3)
            {
                return true; // smalltalk
            }
            else if ((lexer == Lexer.Css)
                && style == 9)
            {
                return true; // css
            }
            return false;
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.ProcessKeyMessage"/>.
        /// </summary>
        protected override bool ProcessKeyMessage(ref Message m)
        {
            //	For some reason IsInputKey isn't working for
            //	Key.Enter. This seems to make it work as expected
            if ((int)m.WParam == (int)Keys.Enter && !AcceptsReturn)
            {
                return true;
            }
            else
            {
                return base.ProcessKeyMessage(ref m);
            }
        }


        private void ResetCaption()
        {
            Caption = GetType().FullName;
        }


        private void ResetMargins()
        {
            _margins.Reset();
        }


        private void ResetStyles()
        {
            // One of the core appearance properties has changed. When this happens
            // we restyle the document (overriding any existing styling) in the core
            // appearance properties. This behavior is consistent with the RichTextBox.
            NativeInterface.StartStyling(0, 0x7F);
            NativeInterface.SetStyling(NativeInterface.GetLength(), 0);
            Styles[0].Reset();
            Styles[0].Font = Font;
            Styles[0].ForeColor = ForeColor;
            Styles[0].BackColor = BackColor;
            Styles.Default.BackColor = BackColor;
        }


        /// <summary>
        ///     Custom way to find the matching brace when BraceMatch() does not work
        /// </summary>
        internal int SafeBraceMatch(int position)
        {
            int match = this.CharAt(position);
            int toMatch = 0;
            int length = TextLength;
            int ch;
            int sub = 0;
            Lexer lexer = _lexing.Lexer;
            _lexing.Colorize(0, -1);
            bool comment = PositionIsOnComment(position, lexer);
            switch (match)
            {
                case '{':
                    toMatch = '}';
                    goto down;
                case '(':
                    toMatch = ')';
                    goto down;
                case '[':
                    toMatch = ']';
                    goto down;
                case '}':
                    toMatch = '{';
                    goto up;
                case ')':
                    toMatch = '(';
                    goto up;
                case ']':
                    toMatch = '[';
                    goto up;
            }
            return -1;
        // search up
        up:
            while (position >= 0)
            {
                position--;
                ch = CharAt(position);
                if (ch == match)
                {
                    if (comment == PositionIsOnComment(position, lexer)) sub++;
                }
                else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
                {
                    sub--;
                    if (sub < 0) return position;
                }
            }
            return -1;
        // search down
        down:
            while (position < length)
            {
                position++;
                ch = CharAt(position);
                if (ch == match)
                {
                    if (comment == PositionIsOnComment(position, lexer)) sub++;
                }
                else if (ch == toMatch && comment == PositionIsOnComment(position, lexer))
                {
                    sub--;
                    if (sub < 0) return position;
                }
            }
            return -1;
        }


        private void ScnModified(ref NativeMethods.SCNotification scn)
        {
            if ((scn.modificationType & NativeMethods.SC_MOD_CHANGEANNOTATION) == NativeMethods.SC_MOD_CHANGEANNOTATION)
            {
                AnnotationChangedEventArgs acea = new AnnotationChangedEventArgs(scn.line, scn.annotationLinesAdded);
                OnAnnotationChanged(acea);
            }
        }


        /// <summary>
        ///     Sets the application-wide default module name of the native Scintilla library.
        /// </summary>
        /// <param name="moduleName">The native Scintilla module name.</param>
        /// <remarks>This method must be called prior to the first <see cref="Scintilla"/> control being created.</remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="moduleName"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="moduleName"/> is an empty string.</exception>
        /// <exception cref="InvalidOperationException">This method was called after the first <see cref="Scintilla"/> control was created.</exception>
        public static void SetModuleName(string moduleName)
        {
            const string paramName = "moduleName";

            if (moduleName == null)
                throw new ArgumentNullException(paramName);

            if (moduleName.Length == 0)
                throw new ArgumentException(string.Format(Resources.Culture, Resources.Exception_EmptyStringArgument, paramName), paramName);

            if (_moduleHandle != IntPtr.Zero)
                throw new InvalidOperationException(Resources.Exception_ModuleAlreadyLoaded);

            _moduleName = moduleName;
        }


        private bool SetRenderer(VisualStyleElement element)
        {
            if (!Application.RenderWithVisualStyles)
                return false;

            if (!VisualStyleRenderer.IsElementDefined(element))
                return false;

            if (_renderer == null)
                _renderer = new VisualStyleRenderer(element);
            else
                _renderer.SetParameters(element);

            return true;
        }


        private bool ShouldSerializeAnnotations()
        {
            return _annotations != null && _annotations.ShouldSerialize();
        }


        private bool ShouldSerializeAutoComplete()
        {
            return _autoComplete.ShouldSerialize();
        }


        private bool ShouldSerializeCallTip()
        {
            return _callTip.ShouldSerialize();
        }


        private bool ShouldSerializeCaption()
        {
            return Caption != GetType().FullName;
        }


        private bool ShouldSerializeCaret()
        {
            return _caret.ShouldSerialize();
        }


        private bool ShouldSerializeClipboard()
        {
            return _clipboard.ShouldSerialize();
        }


        private bool ShouldSerializeCommands()
        {
            return _commands.ShouldSerialize();
        }


        private bool ShouldSerializeConfigurationManager()
        {
            return _configurationManager.ShouldSerialize();
        }


        private bool ShouldSerializeDocumentNavigation()
        {
            return _documentNavigation.ShouldSerialize();
        }


        private bool ShouldSerializeDropMarkers()
        {
            return _dropMarkers.ShouldSerialize();
        }


        private bool ShouldSerializeEndOfLine()
        {
            return _endOfLine.ShouldSerialize();
        }


        private bool ShouldSerializeFindReplace()
        {
            return _findReplace.ShouldSerialize();
        }


        private bool ShouldSerializeFolding()
        {
            return _folding.ShouldSerialize();
        }


        private bool ShouldSerializeHotspotStyle()
        {
            return _hotspotStyle.ShouldSerialize();
        }


        private bool ShouldSerializeIndentation()
        {
            return _indentation.ShouldSerialize();
        }


        private bool ShouldSerializeLexing()
        {
            return _lexing.ShouldSerialize();
        }


        private bool ShouldSerializeLineWrapping()
        {
            return LineWrapping.ShouldSerialize();
        }


        private bool ShouldSerializeLongLines()
        {
            return _longLines.ShouldSerialize();
        }


        private bool ShouldSerializeMargins()
        {
            return _margins.ShouldSerialize();
        }


        private bool ShouldSerializeMarkers()
        {
            return _markers.ShouldSerialize();
        }


        private bool ShouldSerializePrinting()
        {
            return _printing.ShouldSerialize();
        }


        private bool ShouldSerializeScrolling()
        {
            return _scrolling.ShouldSerialize();
        }


        private bool ShouldSerializeSelection()
        {
            return _selection.ShouldSerialize();
        }


        private bool ShouldSerializeSnippets()
        {
            return _snippets.ShouldSerialize();
        }


        private bool ShouldSerializeStyles()
        {
            return _styles.ShouldSerialize();
        }


        public bool ShouldSerializeUndoRedo()
        {
            return _undoRedo.ShouldSerialize();
        }


        private void WmNcPaint(ref Message m)
        {
            // We only paint when border is 3D
            if (BorderStyle != BorderStyle.Fixed3D)
            {
                base.WndProc(ref m);
                return;
            }

            // Configure the renderer
            VisualStyleElement element = VisualStyleElement.TextBox.TextEdit.Normal;
            /*if (!Enabled)
                element = VisualStyleElement.TextBox.TextEdit.Disabled;
            else*/ if (IsReadOnly)
                element = VisualStyleElement.TextBox.TextEdit.ReadOnly;
            else if (Focused)
                element = VisualStyleElement.TextBox.TextEdit.Focused;

            if (!SetRenderer(element))
            {
                base.WndProc(ref m);
                return;
            }

            NativeMethods.RECT windowRect;
            NativeMethods.GetWindowRect(m.HWnd, out windowRect);
            Size borderSize = SystemInformation.Border3DSize;
            IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);
            try
            {
                using (Graphics graphics = Graphics.FromHdc(hDC))
                {
                    // Clip everything except the border
                    Rectangle bounds = new Rectangle(0, 0, windowRect.right - windowRect.left, windowRect.bottom - windowRect.top);
                    graphics.ExcludeClip(Rectangle.Inflate(bounds, -borderSize.Width, -borderSize.Height));

                    // Paint the theme border
                    if (_renderer.IsBackgroundPartiallyTransparent())
                        _renderer.DrawParentBackground(graphics, bounds, this);
                    _renderer.DrawBackground(graphics, bounds);
                }
            }
            finally
            {
                NativeMethods.ReleaseDC(m.HWnd, hDC);
            }

            // Create a new region to pass to the default proc that excludes our border
            IntPtr clipRegion = NativeMethods.CreateRectRgn(
                windowRect.left + borderSize.Width,
                windowRect.top + borderSize.Height,
                windowRect.right - borderSize.Width,
                windowRect.bottom - borderSize.Height);

            if (m.WParam != (IntPtr)1)
                NativeMethods.CombineRgn(clipRegion, clipRegion, m.WParam, NativeMethods.RGN_AND);

            // Call default proc to get the scrollbars, etc... painted
            m.WParam = clipRegion;
            DefWndProc(ref m);
            m.Result = IntPtr.Zero;
        }


        private void WmReflectCommand(ref Message m)
        {
            switch(Utilities.SignedHiWord(m.WParam))
            {
                case NativeMethods.SCEN_CHANGE:
                    // TODO It looks like TextChanged is firing twice for every change.
                    // This appears to be a Scintilla behavior, not us, but we might be able to work around it.
                    OnTextChanged(EventArgs.Empty);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }


        private void WmReflectNotify(ref Message m)
        {
            // New *internal* structure...
            NativeMethods.SCNotification scn = (NativeMethods.SCNotification)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.SCNotification));

            // Old *public* *outdated* structure and event args...
            SCNotification scnOld = (SCNotification)Marshal.PtrToStructure(m.LParam, typeof(SCNotification));
            NativeScintillaEventArgs nsea = new NativeScintillaEventArgs(m, scnOld);

            switch (scnOld.nmhdr.code)
            {
                case Constants.SCN_AUTOCSELECTION:
                    FireAutoCSelection(nsea);
                    break;

                case Constants.SCN_CALLTIPCLICK:
                    FireCallTipClick(nsea);
                    break;

                case Constants.SCN_CHARADDED:
                    FireCharAdded(nsea);
                    break;

                case Constants.SCN_DOUBLECLICK:
                    FireDoubleClick(nsea);
                    break;

                case Constants.SCN_DWELLEND:
                    FireDwellEnd(nsea);
                    break;

                case Constants.SCN_DWELLSTART:
                    FireDwellStart(nsea);
                    break;

                case NativeMethods.SCN_HOTSPOTCLICK:
                    OnHotspotClick(new HotspotClickEventArgs(scn.position));
                    break;

                case NativeMethods.SCN_HOTSPOTDOUBLECLICK:
                    OnHotspotDoubleClick(new HotspotClickEventArgs(scn.position));
                    break;

                case NativeMethods.SCN_HOTSPOTRELEASECLICK:
                    OnHotspotReleaseClick(new HotspotClickEventArgs(scn.position));
                    break;

                case Constants.SCN_INDICATORCLICK:
                    FireIndicatorClick(nsea);
                    break;

                case Constants.SCN_INDICATORRELEASE:
                    FireIndicatorRelease(nsea);
                    break;

                case Constants.SCN_KEY:
                    FireKey(nsea);
                    break;

                case Constants.SCN_MACRORECORD:
                    FireMacroRecord(nsea);
                    break;

                case Constants.SCN_MARGINCLICK:
                    FireMarginClick(nsea);
                    break;

                case Constants.SCN_MODIFIED:
                    ScnModified(ref scn);
                    FireModified(nsea);
                    break;

                case Constants.SCN_MODIFYATTEMPTRO:
                    FireModifyAttemptRO(nsea);
                    break;

                case Constants.SCN_NEEDSHOWN:
                    FireNeedShown(nsea);
                    break;

                case Constants.SCN_PAINTED:
                    FirePainted(nsea);
                    break;

                case Constants.SCN_SAVEPOINTLEFT:
                    FireSavePointLeft(nsea);
                    break;

                case Constants.SCN_SAVEPOINTREACHED:
                    FireSavePointReached(nsea);
                    break;

                case Constants.SCN_STYLENEEDED:
                    FireStyleNeeded(nsea);
                    break;

                case Constants.SCN_UPDATEUI:
                    FireUpdateUI(nsea);
                    break;

                case Constants.SCN_URIDROPPED:
                    FireUriDropped(nsea);
                    break;

                case Constants.SCN_USERLISTSELECTION:
                    FireUserListSelection(nsea);
                    break;

                case Constants.SCN_ZOOM:
                    FireZoom(nsea);
                    break;
            }
        }


        private void WmScroll(ref Message m)
        {
            ScrollOrientation so = ScrollOrientation.VerticalScroll;
            int oldScroll = 0, newScroll = 0;
            ScrollEventType set = (ScrollEventType)(Utilities.SignedLoWord(m.WParam));
            if (m.Msg == NativeMethods.WM_HSCROLL)
            {
                so = ScrollOrientation.HorizontalScroll;
                oldScroll = _ns.GetXOffset();

                //	Let Scintilla Handle the scroll Message to actually perform scrolling
                base.WndProc(ref m);
                newScroll = _ns.GetXOffset();
            }
            else
            {
                so = ScrollOrientation.VerticalScroll;
                oldScroll = Lines.FirstVisibleIndex;
                base.WndProc(ref m);
                newScroll = Lines.FirstVisibleIndex;
            }

            OnScroll(new ScrollEventArgs(set, oldScroll, newScroll, so));
        }


        /// <summary>
        ///     Overridden. Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_DESTROY:
                    // wi11811 2008-07-28 Chris Rickard
                    // If we get a destroy message we make this window a message-only window so that it doesn't actually
                    // get destroyed, causing Scintilla to wipe out all its settings associated with this window handle.
                    // We do send a WM_DESTROY message to Scintilla in the Dispose() method so that it does clean up its
                    // resources when this control is actually done with. Credit (blame :) goes to tom103 for figuring
                    // this one out.

                    if (this.IsHandleCreated)
                    {
                        NativeMethods.SetParent(this.Handle, NativeMethods.HWND_MESSAGE);
                        return;
                    }

                    base.WndProc(ref m);
                    break;

                case NativeMethods.WM_NCPAINT:
                    WmNcPaint(ref m);
                    break;

                case NativeMethods.WM_PAINT:
                    // I tried toggling the ControlStyles.UserPaint flag and sending the message
                    // to both base.WndProc and DefWndProc in order to get the best of both worlds,
                    // Scintilla Paints as normal and .NET fires the Paint Event with the proper
                    // clipping regions and such. This didn't work too well, I kept getting weird
                    // phantom paints, or sometimes the .NET paint events would seem to get painted
                    // over by Scintilla. This technique I use below seems to work perfectly.

                    base.WndProc(ref m);
                    if (_isCustomPaintingEnabled)
                    {
                        ScintillaNET.RECT r;
                        if (!NativeMethods.GetUpdateRect(Handle, out r, false))
                            r = ClientRectangle;

                        using (Graphics g = CreateGraphics())
                        {
                            g.SetClip(r);
                            OnPaint(new PaintEventArgs(g, r));
                        }
                    }
                    break;

                case NativeMethods.WM_SETCURSOR:
                    base.DefWndProc(ref m);
                    break;

                case NativeMethods.WM_GETTEXT:
                    m.WParam = (IntPtr)(Caption.Length + 1);
                    Marshal.Copy(Caption.ToCharArray(), 0, m.LParam, Caption.Length);
                    m.Result = (IntPtr)Caption.Length;
                    break;

                case NativeMethods.WM_GETTEXTLENGTH:
                    m.Result = (IntPtr)Caption.Length;
                    break;

                case NativeMethods.WM_REFLECT + NativeMethods.WM_NOTIFY:
                    WmReflectNotify(ref m);
                    break;

                case NativeMethods.WM_REFLECT + NativeMethods.WM_COMMAND:
                    WmReflectCommand(ref m);
                    break;

                case NativeMethods.WM_HSCROLL:
                case NativeMethods.WM_VSCROLL:
                    WmScroll(ref m);
                    break;

                default:
                    if ((int)m.Msg >= 10000) // TODO Remove "magic number"
                    {
                        _commands.Execute((BindableCommand)m.Msg);
                        return;
                    }

                    base.WndProc(ref m);
                    break;
            }
        }


        public void ZoomIn()
        {
            _ns.ZoomIn();
        }


        private void ZoomOut()
        {
            _ns.ZoomOut();
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets or sets a value indicating whether pressing ENTER creates a new line of text in the
        ///     control or activates the default button for the form.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the ENTER key creates a new line of text; <c>false</c> if the ENTER key activates
        ///     the default button for the form. The default is <c>false</c>.
        /// </returns>
        [DefaultValue(true), Category("Behavior")]
        [Description("Indicates if return characters are accepted as text input.")]
        public bool AcceptsReturn
        {
            get { return _state[_acceptsReturnState]; }
            set { _state[_acceptsReturnState] = value; }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether pressing the TAB key types a TAB character in the control
        ///     instead of moving the focus to the next control in the tab order.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if users can enter tabs using the TAB key; <c>false</c> if pressing the TAB key
        ///     moves the focus. The default is <c>false</c>.
        /// </returns>
        [DefaultValue(true), Category("Behavior")]
        [Description("Indicates if tab characters are accepted as text input.")]
        public bool AcceptsTab
        {
            get { return _state[_acceptsTabState]; }
            set { _state[_acceptsTabState] = value; }
        }


        /// <summary>
        ///     Gets a collection containing all annotations in the control.
        /// </summary>
        /// <returns>
        ///     A <see cref="AnnotationCollection" /> that contains all the annotations in the <see cref="Scintilla" /> control.
        /// </returns>
        [Category("Appearance")]
        [Description("The annotations and options.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AnnotationCollection Annotations
        {
            get
            {
                if (_annotations == null)
                    _annotations = CreateAnnotationsInstance();

                return _annotations;
            }
        }


        /// <summary>
        ///     Controls autocompletion behavior.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public AutoComplete AutoComplete
        {
            get
            {
                return _autoComplete;
            }
        }


        /// <summary>
        ///     Gets or sets the background color for the control.
        /// </summary>
        /// <value>
        ///     A <see cref="Color"/> that represents the background color of the control.
        ///     The default is <see cref="SystemColors.Window"/>.
        /// </value>
        /// <remarks>Settings this property resets any current document styling.</remarks>
        [DefaultValue(typeof(Color), "Window")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }


        /// <summary>
        ///     This property is not relevant for this class.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }


        /// <summary>
        ///     This property is not relevant for this class.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }


        /// <summary>
        ///     Gets or sets the border style of the control.
        /// </summary>
        /// <value>
        ///     A <see cref="BorderStyle" /> that represents the border type of the control.
        ///     The default is <see cref="BorderStyle.Fixed3D" />.
        /// </value>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="BorderStyle" /> values.
        /// </exception>
        [DefaultValue(BorderStyle.Fixed3D), Category("Appearance")]
        [Description("Indicates whether the control should have a border.")] // TODO Move to a resource
        public BorderStyle BorderStyle
        {
            get
            {
                return _borderStyle;
            }
            set
            {
                if (!Enum.IsDefined(typeof(BorderStyle), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));

                if (value != _borderStyle)
                {
                    _borderStyle = value;

                    // This will cause the CreateParams to be reapplied
                    UpdateStyles();

                    OnBorderStyleChanged(EventArgs.Empty);
                }
            }
        }


        /// <summary>
        ///     Manages CallTip (Visual Studio-like code Tooltip) behaviors
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public CallTip CallTip
        {
            get
            {
                return _callTip;
            }
            set
            {
                _callTip = value;
            }
        }


        /// <summary>
        ///     Gets/Sets the Win32 Window Caption. Defaults to Type's FullName
        /// </summary>
        [Category("Behavior")]
        [Description("Win32 Window Caption")]
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;

                    //	Triggers a new WM_GETTEXT query
                    base.Text = value;
                }

            }
        }


        /// <summary>
        ///     Controls Caret Behavior
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public CaretInfo Caret
        {
            get
            {
                return _caret;
            }
        }


        /// <summary>
        ///     Gets Clipboard access for the control.
        /// </summary>
        /// <returns>A <see cref="Clipboard" /> object the provides Clipboard access for the control.</returns>
        [Category("Behavior")] // TODO Place in resource file
        [Description("Clipboard (cut, copy, paste) options.")] // TODO Place in resource file
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Clipboard Clipboard
        {
            get
            {
                return _clipboard;
            }
        }


        internal Dictionary<string, Color> ColorBag { get { return _colorBag; } }


        /// <summary>
        ///     Controls behavior of keyboard bound commands.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public Commands Commands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
            }
        }


        /// <summary>
        ///     Controls behavior of loading/managing ScintillaNET configurations.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public Configuration.ConfigurationManager ConfigurationManager
        {
            get
            {
                return _configurationManager;
            }
            set
            {
                _configurationManager = value;
            }
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.CreateParams"/>.
        /// </summary>
        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
            get
            {
                // Otherwise Scintilla won't paint. When UserPaint is set to
                // true the base Class (Control) eats the WM_PAINT message.
                // Of course when this set to false we can't use the Paint
                // events. This is why I'm relying on the Paint notification
                // sent from scintilla to paint the Marker Arrows.
                SetStyle(ControlStyles.UserPaint, false);

                // I hope the old man got that tractor beam out if commission,
                // or this is going to be a real short trip. Okay, hit it!
                LoadModule();

                // Tell Windows Forms to create a Scintilla
                // derived Window Class for this control
                CreateParams cp = base.CreateParams;
                cp.ClassName = "Scintilla";

                // Set the window style or extended style
                // to the appropriate border type.
                switch (_borderStyle)
                {
                    case BorderStyle.Fixed3D:
                        cp.ExStyle |= NativeMethods.WS_EX_CLIENTEDGE;
                        cp.Style &= ~NativeMethods.WS_BORDER;
                        break;

                    case BorderStyle.FixedSingle:
                        cp.ExStyle &= ~NativeMethods.WS_EX_CLIENTEDGE;
                        cp.Style |= NativeMethods.WS_BORDER;
                        break;

                    default:
                        cp.ExStyle &= ~NativeMethods.WS_EX_CLIENTEDGE;
                        cp.Style &= ~NativeMethods.WS_BORDER;
                        break;
                }

                return cp;
            }
        }


        /// <summary>
        ///     Gets or sets the character index of the current caret position.
        /// </summary>
        /// <returns>The character index of the current caret position.</returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CurrentPos
        {
            get
            {
                return NativeInterface.GetCurrentPos();
            }
            set
            {
                NativeInterface.GotoPos(value);
            }
        }


        /// <summary>Gets or sets the default cursor for the control.</summary>
        /// <returns>An object of type <see cref="T:System.Windows.Forms.Cursor"></see> representing the current default cursor.</returns>
        protected override Cursor DefaultCursor
        {
            get
            {
                return Cursors.IBeam;
            }
        }


        /// <summary>
        ///     Overridden. See <see cref="Control.DefaultSize"/>.
        /// </summary>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(200, 100);
            }
        }


        /// <summary>
        ///     Controls behavior of Documents
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DocumentHandler DocumentHandler
        {
            get
            {
                return _documentHandler;
            }
            set
            {
                _documentHandler = value;
            }
        }


        /// <summary>
        ///     Controls behavior of automatic document navigation
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public DocumentNavigation DocumentNavigation
        {
            get
            {
                return _documentNavigation;
            }
            set
            {
                _documentNavigation = value;
            }
        }


        /// <summary>
        ///     Controls behavior of Drop Markers
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public DropMarkers DropMarkers
        {
            get
            {
                return _dropMarkers;
            }
        }


        /// <summary>
        ///     Controls Encoding behavior
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Encoding Encoding
        {
            get
            {
                return _encoding;
            }
            set
            {
                //	EncoderFallbackException isn't really the correct exception but
                //	I'm being lazy and you get the point
                if (!ValidCodePages.Contains(value))
                    throw new EncoderFallbackException("Scintilla only supports the following Encodings: " + ValidCodePages.ToString());

                _encoding = value;
                _ns.SetCodePage(_encoding.CodePage);
            }
        }


        /// <summary>
        ///     Controls End Of Line Behavior
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public EndOfLine EndOfLine
        {
            get
            {
                return _endOfLine;
            }
            set
            {
                _endOfLine = value;
            }
        }


        [Category("Behavior")]
        public FindReplace FindReplace
        {
            get
            {
                return _findReplace;
            }
            set
            {
                _findReplace = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public Folding Folding
        {
            get
            {
                return _folding;
            }
            set
            {
                _folding = value;
            }
        }


        /// <summary>
        ///     Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>
        ///     The <see cref="Font"/> to apply to the text displayed by the control.
        ///     The default is the value of the <see cref="DefaultFont"/> property.
        /// </value>
        /// <remarks>Settings this property resets any current document styling.</remarks>
        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }


        /// <summary>
        ///     Gets or sets the foreground color of the control.
        /// </summary>
        /// <value>
        ///     The foreground <see cref="Color"/> of the control.
        ///     The default is <see cref="SystemColors.WindowText"/>.
        /// </value>
        /// <remarks>Settings this property resets any current document styling.</remarks>
        [DefaultValue(typeof(Color), "WindowText")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GoTo GoTo
        {
            get
            {
                return _goto;
            }
            set
            {
                _goto = value;
            }
        }


        protected internal List<TopLevelHelper> Helpers
        {
            get
            {
                return _helpers;
            }
            set
            {
                _helpers = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public HotspotStyle HotspotStyle
        {
            get
            {
                return _hotspotStyle;
            }
            set
            {
                _hotspotStyle = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public Indentation Indentation
        {
            get
            {
                return _indentation;
            }
            set
            {
                _indentation = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IndicatorCollection Indicators
        {
            get { return _indicators; }
        }


        [DefaultValue(false), Category("Behavior")]
        public bool IsBraceMatching
        {
            get { return _isBraceMatching; }
            set { _isBraceMatching = value; }
        }


        [DefaultValue(true), Category("Behavior")]
        public bool IsCustomPaintingEnabled
        {
            get
            {
                return _isCustomPaintingEnabled;
            }
            set
            {
                _isCustomPaintingEnabled = value;
            }
        }


        internal bool IsDesignMode
        {
            get
            {
                return DesignMode;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal bool IsInitializing
        {
            get
            {
                return _isInitializing;
            }
            set
            {
                _isInitializing = value;
            }
        }


        [DefaultValue(false), Category("Behavior")]
        public bool IsReadOnly
        {
            get
            {
                return _ns.GetReadOnly();

            }
            set
            {
                _ns.SetReadOnly(value);
            }
        }


        /// <summary>
        ///     Gets or sets the line layout caching strategy in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>
        ///     One of the <see cref="LayoutCacheMode"/> enumeration values.
        ///     The default is <see cref="LayoutCacheMode.Caret" />.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The value assigned is not one of the <see cref="LayoutCacheMode" /> values.
        /// </exception>
        /// <remarks>Larger cache sizes increase performance at the expense of memory.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LayoutCacheMode LayoutCacheMode
        {
            get
            {
                return (LayoutCacheMode)DirectMessage(NativeMethods.SCI_GETLAYOUTCACHE, IntPtr.Zero, IntPtr.Zero);
            }
            set
            {
                if (!Enum.IsDefined(typeof(LayoutCacheMode), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(LayoutCacheMode));

                DirectMessage(NativeMethods.SCI_SETLAYOUTCACHE, new IntPtr((int)value), IntPtr.Zero);
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public Lexing Lexing
        {
            get
            {
                return _lexing;
            }
            set
            {
                _lexing = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LineCollection Lines
        {
            get
            {

                return _lines;
            }
        }


        /// <summary>
        ///     Gets an object that controls line wrapping options in the <see cref="Scintilla"/> control.
        /// </summary>
        /// <returns>A <see cref="LineWrapping"/> object that manages line wrapping options in a <see cref="Scintilla"/> control.</returns>
        [Category("Behavior")] // TODO Move to resource file
        [Description("The control's line wrapping options.")] // TODO Move to resource file
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public LineWrapping LineWrapping
        {
            get
            {
                if (_lineWrapping == null)
                    _lineWrapping = CreateLineWrappingInstance();

                return _lineWrapping;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public LongLines LongLines
        {
            get
            {
                return _longLines;
            }
            set
            {
                _longLines = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ManagedRange> ManagedRanges
        {
            get { return _managedRanges; }
        }


        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public MarginCollection Margins
        {
            get
            {
                return _margins;
            }
        }


        /// <summary>
        ///     Gets a collection representing the marker objects and options within the control.
        /// </summary>
        /// <returns>A <see cref="MarkerCollection" /> representing the marker objects and options within the control.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public MarkerCollection Markers
        {
            get
            {
                return _markers;
            }
            set
            {
                _markers = value;
            }
        }


        [DefaultValue(true), Category("Behavior")]
        public bool MatchBraces
        {
            get
            {
                return _matchBraces;
            }
            set
            {
                _matchBraces = value;

                //	Clear any active Brace matching that may exist
                if (!value)
                    _ns.BraceHighlight(-1, -1);
            }
        }


        /// <summary>
        ///     Gets or sets a value that indicates that the control has been modified by the user since
        ///     the control was created or its contents were last set.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if the control's contents have been modified; otherwise, <c>false</c>.
        ///     The default is <c>false</c>.
        /// </returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Modified
        {
            get { return _state[_modifiedState]; }
            set
            {
                if (_state[_modifiedState] != value)
                {
                    // Update the local (and native) state
                    _state[_modifiedState] = value;
                    if (!value)
                        _ns.SetSavePoint();

                    OnModifiedChanged(EventArgs.Empty);
                }
            }
        }


        [DefaultValue(true), Category("Behavior")]
        public bool MouseDownCaptures
        {
            get
            {
                return NativeInterface.GetMouseDownCaptures();
            }
            set
            {
                NativeInterface.SetMouseDownCaptures(value);
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public INativeScintilla NativeInterface
        {
            get
            {
                return this as INativeScintilla;
            }
        }


        [DefaultValue(false), Category("Behavior")]
        public bool OverType
        {
            get
            {
                return _ns.GetOvertype();
            }
            set
            {
                _ns.SetOvertype(value);
            }
        }


        /// <summary>
        ///     Gets or sets the position cache size used to layout short runs of text in a <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>The size of the position cache in bytes. The default is 1024.</returns>
        /// <remarks>Larger cache sizes increase performance at the expense of memory.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int PositionCacheSize
        {
            get
            {
                return DirectMessage(NativeMethods.SCI_GETPOSITIONCACHE, IntPtr.Zero, IntPtr.Zero).ToInt32();
            }
            set
            {
                // TODO Some range checking? Scintilla provides no guidance
                DirectMessage(NativeMethods.SCI_SETPOSITIONCACHE, new IntPtr(value), IntPtr.Zero);
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Layout")]
        public Printing Printing
        {
            get
            {
                return _printing;
            }
            set
            {
                _printing = value;
            }
        }


        internal Hashtable PropertyBag { get { return _propertyBag; } }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        unsafe public byte[] RawText
        {
            get
            {
                int length = NativeInterface.GetTextLength() + 1;

                //	May as well avoid all the crap below if we know what the outcome
                //	is going to be :)
                if (length == 1)
                    return new byte[] { 0 };

                //  Allocate a buffer the size of the string + 1 for
                //  the NULL terminator. Scintilla always sets this
                //  regardless of the encoding
                byte[] buffer = new byte[length];

                //  Get a direct pointer to the the head of the buffer
                //  to pass to the message along with the wParam.
                //  Scintilla will fill the buffer with string data.
                fixed (byte* bp = buffer)
                {
                    _ns.SendMessageDirect(Constants.SCI_GETTEXT, (IntPtr)length, (IntPtr)bp);
                    return buffer;
                }
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    _ns.ClearAll();
                }
                else
                {
                    //	This byte[] HAS to be NULL terminated or who knows how big
                    //	of an overrun we'll have on our hands
                    if (value[value.Length - 1] != 0)
                    {
                        //	I hate to have to do this becuase it can be very inefficient.
                        //	It can probably be done much better by the client app
                        Array.Resize<byte>(ref value, value.Length + 1);
                        value[value.Length - 1] = 0;
                    }
                    fixed (byte* bp = value)
                        _ns.SendMessageDirect(Constants.SCI_SETTEXT, IntPtr.Zero, (IntPtr)bp);
                }
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Layout")]
        public Scrolling Scrolling
        {
            get
            {
                return _scrolling;
            }
            set
            {
                _scrolling = value;
            }
        }


        [DefaultValue(SearchFlags.Empty), Category("Behavior")]
        [Editor(typeof(Design.FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public SearchFlags SearchFlags
        {
            get
            {
                return _searchFlags;
            }
            set
            {
                _searchFlags = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public Selection Selection
        {
            get
            {
                return _selection;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public SnippetManager Snippets
        {
            get
            {
                return _snippets;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Appearance")]
        public StyleCollection Styles
        {
            get
            {
                return _styles;
            }
            set
            {
                _styles = value;
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether characters not considered alphanumeric (ASCII values 0 through 31)
        ///     are prevented as text input.
        /// </summary>
        /// <returns>
        ///     <c>true</c> to prevent control characters as input; otherwise, <c>false</c>.
        ///     The default is <c>true</c>.
        /// </returns>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SupressControlCharacters
        {
            get
            {
                return _supressControlCharacters;
            }
            set
            {
                _supressControlCharacters = value;
            }
        }


        /// <summary>
        ///     Gets or sets the current text in the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>The text displayed in the control.</returns>
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", typeof(UITypeEditor))]
        public override string Text
        {
            get
            {
                string s;
                _ns.GetText(_ns.GetLength() + 1, out s);
                return s;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    _ns.ClearAll();
                else
                    _ns.SetText(value);
            }
        }


        /// <summary>
        ///     Gets the _length of text in the control.
        /// </summary>
        /// <returns>The number of characters contained in the text of the control.</returns>
        [Browsable(false)]
        public int TextLength
        {
            get
            {
                return NativeInterface.GetTextLength();
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("Behavior")]
        public UndoRedo UndoRedo
        {
            get
            {
                return _undoRedo;
            }
        }


        public new bool UseWaitCursor
        {
            get
            {
                return base.UseWaitCursor;
            }
            set
            {
                base.UseWaitCursor = value;

                if (value)
                    NativeInterface.SetCursor(Constants.SC_CURSORWAIT);
                else
                    NativeInterface.SetCursor(Constants.SC_CURSORNORMAL);
            }
        }


        /// <summary>
        ///     Gets the <see cref="Whitespace"/> display mode and style behavior associated with the <see cref="Scintilla"/> control.
        /// </summary>
        /// <returns>A <see cref="Whitespace"/> object that represents whitespace display mode and style behavior in a <see cref="Scintilla"/> control.</returns>
        [Category("Appearance"), Description("The display mode and style of whitespace characters.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Whitespace Whitespace
        {
            get { return _whitespace; }
        }


        /// <summary>
        /// Gets or sets the current zoom level of the <see cref="Scintilla" /> control.
        /// </summary>
        /// <returns>The factor by which the contents of the control is zoomed.</returns>
        [DefaultValue(0), Category("Appearance")]
        [Description("Defines the current scaling factor of the text display; 0 is normal viewing.")]
        public int Zoom
        {
            get
            {
                return _ns.GetZoom();
            }
            set
            {
                _ns.SetZoom(value);
            }
        }

        #endregion Properties


        #region Events

        /// <summary>
        ///     Occurs when an annotation has changed.
        /// </summary>
        [Category("Scintilla")] // TODO Move to resource
        [Description("Occurs when an annotation has changed.")] // TODO Move to resource
        public event EventHandler<AnnotationChangedEventArgs> AnnotationChanged
        {
            add
            {
                Events.AddHandler(_annotationChangedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(_annotationChangedEventKey, value);
            }
        }


        /// <summary>
        ///     Occurs when the user makes a selection from the auto-complete list.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the user makes a selection from the auto-complete list.")]
        public event EventHandler<AutoCompleteAcceptedEventArgs> AutoCompleteAccepted
        {
            add { Events.AddHandler(_autoCompleteAcceptedEventKey, value); }
            remove { Events.RemoveHandler(_autoCompleteAcceptedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when text is about to be removed from the document.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when text is about to be removed from the document.")]
        public event EventHandler<TextModifiedEventArgs> BeforeTextDelete
        {
            add { Events.AddHandler(_beforeTextDeleteEventKey, value); }
            remove { Events.RemoveHandler(_beforeTextDeleteEventKey, value); }
        }


        /// <summary>
        ///     Occurs when text is about to be inserted into the document.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when text is about to be inserted into the document.")]
        public event EventHandler<TextModifiedEventArgs> BeforeTextInsert
        {
            add { Events.AddHandler(_beforeTextInsertEventKey, value); }
            remove { Events.RemoveHandler(_beforeTextInsertEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the value of the <see cref="BorderStyle" /> property has changed.
        /// </summary>
        [Category("Property Changed")]
        [Description("Occurs when the value of the BorderStyle property changes.")] // TODO Move to resource
        public event EventHandler BorderStyleChanged
        {
            add
            {
                Events.AddHandler(_borderStyleChangedEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(_borderStyleChangedEventKey, value);
            }
        }


        /// <summary>
        ///     Occurs when a user clicks on a call tip.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a user clicks on a call tip.")]
        public event EventHandler<CallTipClickEventArgs> CallTipClick
        {
            add { Events.AddHandler(_callTipClickEventKey, value); }
            remove { Events.RemoveHandler(_callTipClickEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the user types an ordinary text character (as opposed to a command character) into the text.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the user types a text character.")]
        public event EventHandler<CharAddedEventArgs> CharAdded
        {
            add { Events.AddHandler(_charAddedEventKey, value); }
            remove { Events.RemoveHandler(_charAddedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the text or styling of the document changes or is about to change.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the text or styling of the document changes or is about to change.")]
        public event EventHandler<NativeScintillaEventArgs> DocumentChange
        {
            add { Events.AddHandler(_documentChangeEventKey, value); }
            remove { Events.RemoveHandler(_documentChangeEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a <see cref="DropMarker"/> is about to be collected.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a DropMarker is about to be collected.")]
        public event EventHandler<DropMarkerCollectEventArgs> DropMarkerCollect
        {
            add { Events.AddHandler(_dropMarkerCollectEventKey, value); }
            remove { Events.RemoveHandler(_dropMarkerCollectEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a user actions such as a mouse move or key press ends a dwell (hover) activity.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a dwell (hover) activity has ended.")]
        public event EventHandler<ScintillaMouseEventArgs> DwellEnd
        {
            add { Events.AddHandler(_dwellEndEventKey, value); }
            remove { Events.RemoveHandler(_dwellEndEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the user hovers the mouse (dwells) in one position for the dwell period.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the user hovers the mouse (dwells) in one position for the dwell period.")]
        public event EventHandler<ScintillaMouseEventArgs> DwellStart
        {
            add { Events.AddHandler(_dwellStartEventKey, value); }
            remove { Events.RemoveHandler(_dwellStartEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a folding change has occurred.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a folding change has occurred.")]
        public event EventHandler<FoldChangedEventArgs> FoldChanged
        {
            add { Events.AddHandler(_foldChangedEventKey, value); }
            remove { Events.RemoveHandler(_foldChangedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a user clicks on text that is in a style with the hotspot attribute set.
        /// </summary>
        [Category("Scintilla")]
        [Description("Occurs when a user clicks on text with the hotspot style.")] // TODO Move to resource
        public event EventHandler<HotspotClickEventArgs> HotspotClick
        {
            add
            {
                Events.AddHandler(_hotspotClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(_hotspotClickEventKey, value);
            }
        }


        /// <summary>
        ///     Occurs when a user double-clicks on text that is in a style with the hotspot attribute set.
        /// </summary>
        [Category("Scintilla")]
        [Description("Occurs when a user double-clicks on text with the hotspot style.")] // TODO Move to resource
        public event EventHandler<HotspotClickEventArgs> HotspotDoubleClick
        {
            add
            {
                Events.AddHandler(_hotspotDoubleClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(_hotspotDoubleClickEventKey, value);
            }
        }


        /// <summary>
        ///     Occurs when a user releases a click on text that is in a style with the hotspot attribute set.
        /// </summary>
        [Category("Scintilla")]
        [Description("Occurs when a user releases a click on text with the hotspot style.")] // TODO Move to resource
        public event EventHandler<HotspotClickEventArgs> HotspotReleaseClick
        {
            add
            {
                Events.AddHandler(_hotspotReleaseClickEventKey, value);
            }
            remove
            {
                Events.RemoveHandler(_hotspotReleaseClickEventKey, value);
            }
        }


        /// <summary>
        ///     Occurs when the a clicks or releases the mouse on text that has an indicator.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the a clicks or releases the mouse on text that has an indicator.")]
        public event EventHandler<ScintillaMouseEventArgs> IndicatorClick
        {
            add { Events.AddHandler(_indicatorClickEventKey, value); }
            remove { Events.RemoveHandler(_indicatorClickEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a range of lines that is currently invisible should be made visible.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a range of lines that is currently invisible should be made visible.")]
        public event EventHandler<LinesNeedShownEventArgs> LinesNeedShown
        {
            add { Events.AddHandler(_linesNeedShownEventKey, value); }
            remove { Events.RemoveHandler(_linesNeedShownEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the control is first loaded.
        /// </summary>
        [Category("Behavior"), Description("Occurs when the control is first loaded.")]
        public event EventHandler Load
        {
            add { Events.AddHandler(_loadEventKey, value); }
            remove { Events.RemoveHandler(_loadEventKey, value); }
        }


        /// <summary>
        ///     Occurs each time a recordable change occurs.
        /// </summary>
        [Category("Scintilla"), Description("Occurs each time a recordable change occurs.")]
        public event EventHandler<MacroRecordEventArgs> MacroRecord
        {
            add { Events.AddHandler(_macroRecordEventKey, value); }
            remove { Events.RemoveHandler(_macroRecordEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the mouse was clicked inside a margin that was marked as sensitive.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the mouse was clicked inside a margin that was marked as sensitive.")]
        public event EventHandler<MarginClickEventArgs> MarginClick
        {
            add { Events.AddHandler(_marginClickEventKey, value); }
            remove { Events.RemoveHandler(_marginClickEventKey, value); }
        }


        /// <summary>
        ///     Occurs when one or more markers has changed in a line of text.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when one or more markers has changed in a line of text.")]
        public event EventHandler<MarkerChangedEventArgs> MarkerChanged
        {
            add { Events.AddHandler(_markerChangedEventKey, value); }
            remove { Events.RemoveHandler(_markerChangedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the value of the <see cref="Modified"> property has changed.
        /// </summary>
        [Category("Property Changed"), Description("Occurs when the value of the Modified property changes.")]
        public event EventHandler ModifiedChanged
        {
            add { Events.AddHandler(_modifiedChangedEventKey, value); }
            remove { Events.RemoveHandler(_modifiedChangedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when a user tries to modify text when in read-only mode.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when a user tries to modifiy text when in read-only mode.")]
        public event EventHandler ReadOnlyModifyAttempt
        {
            add { Events.AddHandler(_readOnlyModifyAttemptEventKey, value); }
            remove { Events.RemoveHandler(_readOnlyModifyAttemptEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the control is scrolled.
        /// </summary>
        [Category("Action"), Description("Occurs when the control is scrolled.")]
        public event EventHandler<ScrollEventArgs> Scroll
        {
            add { Events.AddHandler(_scrollEventKey, value); }
            remove { Events.RemoveHandler(_scrollEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the selection has changed.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the selection has changed.")]
        public event EventHandler SelectionChanged
        {
            add { Events.AddHandler(_selectionChangedEventKey, value); }
            remove { Events.RemoveHandler(_selectionChangedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the control is about to display or print text that requires styling.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the control is about to display or print text that requires styling.")]
        public event EventHandler<StyleNeededEventArgs> StyleNeeded
        {
            add { Events.AddHandler(_styleNeededEventKey, value); }
            remove { Events.RemoveHandler(_styleNeededEventKey, value); }
        }


        /// <summary>
        ///     Occurs when text has been removed from the document.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when text has been removed from the document.")]
        public event EventHandler<TextModifiedEventArgs> TextDeleted
        {
            add { Events.AddHandler(_textDeletedEventKey, value); }
            remove { Events.RemoveHandler(_textDeletedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when text has been inserted into the document.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when text has been inserted into the document.")]
        public event EventHandler<TextModifiedEventArgs> TextInserted
        {
            add { Events.AddHandler(_textInsertedEventKey, value); }
            remove { Events.RemoveHandler(_textInsertedEventKey, value); }
        }


        /// <summary>
        ///     Occurs when the user zooms the display using the keyboard or the <see cref="Zoom"/> property is set.
        /// </summary>
        [Category("Scintilla"), Description("Occurs when the user zooms the display using the keyboard or the Zoom property is set.")]
        public event EventHandler ZoomChanged
        {
            add { Events.AddHandler(_zoomChangedEventKey, value); }
            remove { Events.RemoveHandler(_zoomChangedEventKey, value); }
        }

        #endregion Events


        #region Constructors

        public Scintilla()
        {
            this._state = new BitVector32(0);
            this._state[_acceptsReturnState] = true;
            this._state[_acceptsTabState] = true;

            _ns = (INativeScintilla)this;

            _caption = GetType().FullName;

            // Set up default encoding to UTF-8 which is the Scintilla's best supported.
            // .NET strings are UTF-16 but should be able to convert without any problems
            this.Encoding = Encoding.UTF8;

            // Ensure all style values have at least defaults
            _ns.StyleClearAll();

            _caret = new CaretInfo(this);
            _lines = new LineCollection(this);
            _selection = new Selection(this);
            _indicators = new IndicatorCollection(this);
            _snippets = new SnippetManager(this);
            _margins = new MarginCollection(this);
            _scrolling = new Scrolling(this);
            _whitespace = new Whitespace(this);
            _endOfLine = new EndOfLine(this);
            _clipboard = new Clipboard(this);
            _undoRedo = new UndoRedo(this);
            _dropMarkers = new DropMarkers(this);
            _hotspotStyle = new HotspotStyle(this);
            _callTip = new CallTip(this);
            _styles = new StyleCollection(this);
            _indentation = new Indentation(this);
            _markers = new MarkerCollection(this);
            _autoComplete = new AutoComplete(this);
            _documentHandler = new DocumentHandler(this);
            _lexing = new Lexing(this);
            _longLines = new LongLines(this);
            _commands = new Commands(this);
            _folding = new Folding(this);
            _configurationManager = new ConfigurationManager(this);
            _printing = new Printing(this);
            _findReplace = new FindReplace(this);
            _documentNavigation = new DocumentNavigation(this);
            _goto = new GoTo(this);


            _helpers.AddRange(new TopLevelHelper[]
            {
                _caret,
                _lines,
                _selection,
                _indicators,
                _snippets,
                _margins,
                _scrolling,
                _whitespace,
                _endOfLine,
                _clipboard,
                _undoRedo,
                _dropMarkers,
                _hotspotStyle,
                _styles,
                _indentation,
                _markers,
                _autoComplete,
                _documentHandler,
                _lexing,
                _longLines,
                _commands,
                _folding,
                _configurationManager,
                _printing,
                _findReplace,
                _documentNavigation,
                _goto
            });


            // Change from Scintilla's default black on white to
            // platform defaults for edit controls.
            base.BackColor = SystemColors.Window;
            base.ForeColor = SystemColors.WindowText;

            Styles[0].Font = Font;
            Styles[0].ForeColor = ForeColor;
            Styles[0].BackColor = BackColor;
            Styles.Default.BackColor = BackColor;
        }

        #endregion Constructors
    }
}
