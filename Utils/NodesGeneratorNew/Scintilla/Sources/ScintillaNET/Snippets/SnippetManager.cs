#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class SnippetManager : TopLevelHelper
    {
        #region Fields

        private Color _activeSnippetColor = Color.Lime;
        private int _activeSnippetIndicator = 15;
        private IndicatorStyle _activeSnippetIndicatorStyle = IndicatorStyle.RoundBox;
        private char _defaultDelimeter = '$';
        private Color _inactiveSnippetColor = Color.Lime;
        private int _inactiveSnippetIndicator = 16;
        private IndicatorStyle _inactiveSnippetIndicatorStyle = IndicatorStyle.Box;
        private bool _isEnabled = true;
        
        //	Yeah I know this is a bit unwieldly but I can't come up with a better name
        private bool _isOneKeySelectionEmbedEnabled = false;
        private SnippetList _list;
        private bool _pendingUndo = false;
        private SnippetChooser _snipperChooser;
        private SnippetLinkCollection _snippetLinks = new SnippetLinkCollection();
        private Timer _snippetLinkTimer = new Timer();
        private readonly Regex snippetRegex1 = new Regex(string.Format(@"(?<dm>{0}DropMarker(?<dmi>\[[0-9]*\])?{0})|(?<c>{0}caret{0})|(?<a>{0}anchor{0})|(?<e>{0}end{0})|(?<s>{0}selected{0})|(?<l>{0}.+?(?<li>\[[0-9]*\])?{0})", Snippet.RealDelimeter), RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        #endregion Fields


        #region Methods

        public bool AcceptActiveSnippets()
        {
            if (_snippetLinks.IsActive && !Scintilla.AutoComplete.IsActive)
            {
                int pos = Scintilla.Caret.Position;
                bool end = false;
                foreach (SnippetLink sl in _snippetLinks.Values)
                {
                    foreach (SnippetLinkRange sr in sl.Ranges)
                    {
                        if (sr.PositionInRange(pos))
                        {
                            end = true;
                            break;
                        }
                    }
                    if (end)
                        break;
                }

                if (end)
                {
                    cascadeSnippetLinkRangeChange(_snippetLinks.ActiveSnippetLink, _snippetLinks.ActiveRange);

                    if (_snippetLinks.EndPoint != null)
                        Scintilla.Caret.Goto(_snippetLinks.EndPoint.Start);

                    IsActive = false;
                    Scintilla.Commands.StopProcessingCommands = true;
                    return true;
                }
            }

            return false;
        }


        private SnippetLinkRange addSnippetLink(SnippetLinkRange range)
        {
            string key = range.Key;
            SnippetLink sl = null;
            for (int i = 0; i < _snippetLinks.Count; i++)
            {
                if (_snippetLinks[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    sl = _snippetLinks[i];
                    break;
                }
            }
            if (sl == null)
            {
                sl = new SnippetLink(key);
                _snippetLinks.Add(sl);
            }

            sl.Ranges.Add(range);
            range.Parent = sl.Ranges;

            return range;
        }


        public bool CancelActiveSnippets()
        {
            if (_snippetLinks.IsActive && !Scintilla.AutoComplete.IsActive)
            {
                IsActive = false;
                Scintilla.Commands.StopProcessingCommands = true;
                return true;
            }

            return false;
        }


        private void cascadeSnippetLinkRangeChange(SnippetLink oldActiveSnippetLink, SnippetLinkRange oldActiveRange)
        {
            Scintilla.ManagedRanges.Sort();

            int offset = 0;

            string newText = oldActiveRange.Text;


            Scintilla.NativeInterface.SetModEventMask(0);
            foreach (ManagedRange mr in Scintilla.ManagedRanges)
            {
                if (offset != 0)
                    mr.Change(mr.Start + offset, mr.End + offset);

                SnippetLinkRange slr = mr as SnippetLinkRange;
                if (slr == null || !oldActiveSnippetLink.Ranges.Contains(slr) || slr.Text == newText)
                    continue;

                int oldLength = slr.Length;
                slr.Text = newText;
                slr.End += newText.Length - oldLength;
                offset += newText.Length - oldLength;
            }

            Scintilla.NativeInterface.SetModEventMask(Constants.SC_MODEVENTMASKALL);
        }


        public bool DoSnippetCheck()
        {
            if (!_isEnabled || _snippetLinks.IsActive || Scintilla.AutoComplete.IsActive || Scintilla.Selection.Length > 0)
                return false;

            int pos = Scintilla.NativeInterface.GetCurrentPos();

            //	In order to even _start searching for a snippet keyword the
            //	current position needs to meet these conditions:
            //	Can't be at the very beginning of the document. Why? becuase
            //	then obviously there can't be a preceding keyword then can it?
            //	The preceding character can't be whitespace (same reason) 
            //
            //	I decided I like expanding a template in the middle of a word
            if (pos <= 0 || Scintilla.NativeInterface.GetCharAt(pos - 1).ToString().IndexOfAny(Scintilla.Lexing.WhitespaceCharsArr) >= 0)
                return false;

            //	We also don't want a template expand if we're in a Comment or 
            //	String. Be sure and mask out any indicator style that may be applied
            int currentStyle = Scintilla.NativeInterface.GetStyleAt(pos - 1) & 0x1f;
            if (currentStyle == 1 || currentStyle == 2 || currentStyle == 4)
                return false;

            //	Let Scintilla figure out where the beginning of
            //	the word to the left is.
            int newPos = Scintilla.NativeInterface.WordStartPosition(pos, true);

            Range snipRange = Scintilla.GetRange(newPos, pos);
            string keyworkCandidate = snipRange.Text;

            Snippet snip;
            if (!_list.TryGetValue(keyworkCandidate, out snip))
            {
                //	Not a match. Buh-bye
                return false;
            }

            InsertSnippet(snip, newPos);
            Scintilla.Commands.StopProcessingCommands = true;
            return true;
        }


        public void InsertSnippet(string shortcut)
        {
            Snippet snip;
            if (!_list.TryGetValue(shortcut, out snip))
            {
                //	Not a match. Buh-bye
                return;
            }

            InsertSnippet(snip, Math.Min(NativeScintilla.GetCurrentPos(), NativeScintilla.GetAnchor()));
        }


        public void InsertSnippet(Snippet snip)
        {
            InsertSnippet(snip, Math.Min(NativeScintilla.GetCurrentPos(), NativeScintilla.GetAnchor()));
        }


        internal void InsertSnippet(Snippet snip, int startPos)
        {
            NativeScintilla.BeginUndoAction();
            IsActive = false;

            string snippet = snip.RealCode;

            //	First properly indent the template. We do this by
            //	getting the indent string of the current line and
            //	adding it to all newlines
            int indentPoint = 0;
            string line = Scintilla.Lines.Current.Text;
            if(line != string.Empty)
            {				
                while (indentPoint < line.Length)
                {
                    char c = line[indentPoint];
                    if (c != ' ' && c != '\t')
                        break;

                    indentPoint++;
                }
            }

            //	Grab the current selected text in case we have a surrounds with scenario.
            string selText = Scintilla.Selection.Text;
            //	Now we clear the selection
            if (selText != string.Empty)
                Scintilla.Selection.Clear();

            if (indentPoint > 0)
            {
                string indent = line.Substring(0, indentPoint);

                //	This is a bit of a tough decision, but I think the best way to handle it
                //	is to assume that the Snippet's Eol Marker matches the Platform DOCUMENT_DEFAULT
                //	but the target Eol Marker should match the Document's.
                snippet = snippet.Replace(Environment.NewLine, Scintilla.EndOfLine.EolString + indent);

                //	Same deal with the selected text if any				
                selText = selText.Replace(Environment.NewLine, Scintilla.EndOfLine.EolString + indent);
            }

            int anchorPos = -1;
            int caretPos = -1;
            int endPos = -1;
            int selPos = -1;
            SortedList<int, int> dropMarkers = new SortedList<int, int>();
            SortedList<int, SnippetLinkRange> indexedRangesToActivate = new SortedList<int, SnippetLinkRange>();
            List<SnippetLinkRange> unindexedRangesToActivate = new List<SnippetLinkRange>();
            Match m = snippetRegex1.Match(snippet);

            while (m.Success)
            {
                //	Did it match a $DropMarker$ token?
                if (m.Groups["dm"].Success)
                {
                    //	Yep, was it an indexed or unindexed DropMarker
                    if (m.Groups["dmi"].Success)
                    {
                        //	Indexed, set the indexed drop marker's character offset
                        //	if it is specified more than once the last one wins.
                        dropMarkers[int.Parse(m.Groups["dmi"].Value)] = m.Groups["dm"].Index;
                    }
                    else
                    {
                        //	Unindexed, just tack it on at the _end
                        dropMarkers[dropMarkers.Count] = m.Groups["dm"].Index;
                    }

                    //	Take the token out of the string
                    snippet = snippet.Remove(m.Groups["dm"].Index, m.Groups["dm"].Length);
                }
                else if (m.Groups["c"].Success)
                {
                    //	We matched the $Caret$ Token. Since there can be 
                    //	only 1 we set the caretPos. If this is specified
                    //	more than once the last one wins
                    caretPos = m.Groups["c"].Index;

                    //	Take the token out of the string
                    snippet = snippet.Remove(m.Groups["c"].Index, m.Groups["c"].Length);
                }
                else if (m.Groups["a"].Success)
                {
                    //	We matched the $Anchor$ Token. Since there can be 
                    //	only 1 we set the anchorPos. If this is specified
                    //	more than once the last one wins
                    anchorPos = m.Groups["a"].Index;

                    //	Take the token out of the string
                    snippet = snippet.Remove(m.Groups["a"].Index, m.Groups["a"].Length);
                }
                else if (m.Groups["e"].Success)
                {
                    //	We matched the $End$ Token. Since there can be 
                    //	only 1 we set the endPos. If this is specified
                    //	more than once the last one wins
                    endPos = m.Groups["e"].Index;

                    //	Take the token out of the string
                    snippet = snippet.Remove(m.Groups["e"].Index, m.Groups["e"].Length);
                }
                else if (m.Groups["s"].Success)
                {
                    //	We matched the $Selection$ Token. Simply insert the
                    //	selected text at this position
                    selPos = m.Groups["s"].Index;

                    //	Take the token out of the string
                    snippet = snippet.Remove(m.Groups["s"].Index, m.Groups["s"].Length);
                    snippet = snippet.Insert(m.Groups["s"].Index, selText);
                }
                else if (m.Groups["l"].Success)
                {
                    //	Finally match for Snippet Link Ranges. This is at the bottom of the if/else
                    //	because we want the more specific regex groups to match first so that this
                    //	generic expression group doesn't create a SnippetLinkRange for say the 
                    //	$Caret$ Token.
                    Group g = m.Groups["l"];

                    int rangeIndex;
                    string groupKey;

                    if (m.Groups["li"].Success)
                    {
                        //	We have a subindexed SnippetLinkRange along the lines of $sometoken[1]$
                        Group sg = m.Groups["li"];

                        //	At this point g.Value = $sometoken[1]$
                        //	and sg.Value = [1].
                        //	We want the range's Key, which would be sometoken
                        groupKey = g.Value.Substring(1, g.Value.Length - sg.Length - 2);

                        //	Now we need the range's Index which would be 1 in our fictitional case
                        rangeIndex = int.Parse(sg.Value.Substring(1, sg.Value.Length - 2));

                        //	Now we need to determine the actual _start and _end positions of the range.
                        //	Keep in mind we'll be stripping out the 2 $ and the subindex string (eg [1])
                        int start = startPos + g.Index;
                        int end = start + g.Length - sg.Length - 2;

                        //	And now we add (or replace) the snippet link range at the index
                        //	keep in mind duplicates will stomp all over each other, the last
                        //	one wins. Replaced tokens won't get a range
                        indexedRangesToActivate[rangeIndex] = new SnippetLinkRange(start, end, Scintilla, groupKey); ;

                        //	And remove all the token info including the subindex from the snippet text 
                        //	leaving only the key
                        snippet = snippet.Remove(g.Index, 1).Remove(g.Index - 2 + g.Length - sg.Length, sg.Length + 1);
                    }
                    else
                    {
                        //	We have a regular old SnippetLinkRange along the lines of $sometoken$

                        //	We want the range's Key, which would be sometoken
                        groupKey = g.Value.Substring(1, g.Value.Length - 2);

                        //	Now we need to determine the actual _start and _end positions of the range.
                        //	Keep in mind we'll be stripping out the 2 $
                        int start = startPos + g.Index;
                        int end = start + g.Length - 2;

                        //	Now create the range object
                        unindexedRangesToActivate.Add(new SnippetLinkRange(start, end, Scintilla, groupKey));

                        //	And remove all the token info from the snippet text 
                        //	leaving only the key
                        snippet = snippet.Remove(g.Index, 1).Remove(g.Index + g.Length - 2, 1);
                    }
                }
                //	Any more matches? Note that I'm rerunning the regexp query
                //	on the snippet string becuase it's contents have been modified
                //	and we need to get the updated index values.
                m = snippetRegex1.Match(snippet);
            }

            //	Replace the snippet Keyword with the snippet text. Or if this
            //	isn't triggered by a shortcut, it will insert at the current
            //	Caret Position
            Scintilla.GetRange(startPos, NativeScintilla.GetCurrentPos()).Text = snippet;

            //	Now that we have the text set we can activate our link ranges
            //	we couldn't do it before becuase they were managed ranges and
            //	would get offset by the text change

            //	Since we are done adding new SnippetLinkRanges we can tack
            //	on the unindexed ranges to the _end of the indexed ranges
            SnippetLinkRange[] allLinks = new SnippetLinkRange[indexedRangesToActivate.Count + unindexedRangesToActivate.Count];
            for (int i = 0; i < indexedRangesToActivate.Values.Count; i++)
                allLinks[i] = indexedRangesToActivate[i];

            for (int i = 0; i < unindexedRangesToActivate.Count; i++)
                allLinks[i + indexedRangesToActivate.Count] = unindexedRangesToActivate[i];

            foreach (SnippetLinkRange slr in allLinks)
                addSnippetLink(slr);

            foreach (SnippetLinkRange slr in allLinks)
                slr.Init();

            //	Now we need to activate the Snippet links. However we have a bit
            //	of a styling confilct. If we set the indicator styles before the
            //	SQL Lexer styles the newly added text it won't get styled. So to
            //	make sure we set the Indicator Styles after we put the call on
            //	a timer.
            if (_snippetLinks.Count > 0)
            {
                Timer t = new Timer();
                t.Interval = 10;

                //	Oh how I love anonymous delegates, this is starting to remind
                //	me of JavaScript and SetTimeout...
                t.Tick += new EventHandler(delegate(object sender, EventArgs te)
                {
                    t.Dispose();
                    IsActive = true;
                });
                t.Start();
            }

            //	Add all the Drop markers in the indexed order. The
            //	order is reversed of course because drop markers work
            //	in a FILO manner
            for (int i = dropMarkers.Count - 1; i >= 0; i--)
                Scintilla.DropMarkers.Drop(startPos + dropMarkers.Values[i]);

            //	Place the caret at either the position of the token or
            //	at the _end of the snippet text.
            if (caretPos >= 0)
                Scintilla.Caret.Goto(startPos + caretPos);
            else
                Scintilla.Caret.Goto(startPos + snippet.Length);

            //	Ahoy, way anchor!
            if (anchorPos >= 0)
                Scintilla.Caret.Anchor = startPos + anchorPos;

            //	Do we have an _end cursor?
            if (endPos >= 0)
            {
                //	If they have snippet link ranges activated in this snippet
                //	go ahead and set up an EndPoint marker
                if (allLinks.Length > 0)
                {
                    SnippetLinkEnd eci = new SnippetLinkEnd(endPos + startPos, Scintilla);
                    Scintilla.ManagedRanges.Add(eci);
                    _snippetLinks.EndPoint = eci;
                }
                else
                {
                    //	Otherwise we treat it like an Anchor command because
                    //	the SnippetLink mode isn't activated
                    Scintilla.Caret.Goto(endPos + startPos);
                }
            }

            NativeScintilla.EndUndoAction();
        }


        public bool NextSnippetRange()
        {
            //	This would be a whole lot easier if I had the Command Contexts set
            //	up. The way it's working now is that this command will always execute
            //	irregardlessly of if the SnippetLinks are active. Since we may not have
            //	a valid context to execute we don't necessarily want to eat the
            //	keystroke in all circumstances, hence the bool return
            if (!_snippetLinks.IsActive || Scintilla.AutoComplete.IsActive)
                return false;

            //	OK So we want to find the next SnippetLink in
            //	whatever order they are in and then select it
            //	so that they can fill it out.
            SnippetLink sl = _snippetLinks.NextActiveSnippetLink;
            if (sl != null)
            {
                //	However it is possible that all of this Snippet Links'
                //	ranges have been deleted by the user. If this is the case
                //	we need to remove this snippet link from the list and go
                //	to the next link.

                while (sl.Ranges.Count == 0)
                {
                    _snippetLinks.Remove(sl);
                    sl = _snippetLinks.NextActiveSnippetLink;

                    //	No more snippet links? Nothing to do but quit
                    if (sl == null)
                    {
                        Scintilla.Commands.StopProcessingCommands = true;
                        return true;
                    }
                }

                //	Yay we have it. Select the first Range in the Snippet Link's Series
                sl.Ranges[0].Select();
                Scintilla.Commands.StopProcessingCommands = true;
                return true;
            }

            return false;
        }


        public bool PreviousSnippetRange()
        {
            //	Same as NextSnippetRange but going in the opposite direction
            if (!_snippetLinks.IsActive || Scintilla.AutoComplete.IsActive)
                return false;

            SnippetLink sl = _snippetLinks.PreviousActiveSnippetLink;
            if (sl != null)
            {
                while (sl.Ranges.Count == 0)
                {
                    _snippetLinks.Remove(sl);
                    sl = _snippetLinks.PreviousActiveSnippetLink;
                    if (sl == null)
                    {
                        Scintilla.Commands.StopProcessingCommands = true;
                        return true;
                    }
                }

                sl.Ranges[0].Select();
                Scintilla.Commands.StopProcessingCommands = true;
                return true;
            }

            return false;
        }


        private void ResetActiveSnippetColor()
        {
            _activeSnippetColor = Color.Lime;
        }


        private void ResetActiveSnippetIndicator()
        {
            _activeSnippetIndicator = 15;
        }


        private void ResetActiveSnippetIndicatorStyle()
        {
            _activeSnippetIndicatorStyle = IndicatorStyle.RoundBox;
        }


        private void ResetDefaultDelimeter()
        {
            _defaultDelimeter = '$';
        }


        private void ResetInactiveSnippetColor()
        {
            _inactiveSnippetColor = Color.Lime;
        }


        private void ResetInactiveSnippetIndicator()
        {
            _inactiveSnippetIndicator = 16;
        }


        private void ResetInactiveSnippetIndicatorStyle()
        {
            _inactiveSnippetIndicatorStyle = IndicatorStyle.Box;
        }


        private void ResetIsEnabled()
        {
            _isEnabled = true;
        }


        private void ResetIsOneKeySelectionEmbedEnabled()
        {
            _isOneKeySelectionEmbedEnabled = false;
        }


        private void Scintilla_BeforeTextDelete(object sender, TextModifiedEventArgs e)
        {
            if (!_isEnabled)
                return;

            if (_snippetLinks.IsActive && !_pendingUndo && !(e.UndoRedoFlags.IsUndo || e.UndoRedoFlags.IsRedo))
            {
                _pendingUndo = true;
                Scintilla.UndoRedo.BeginUndoAction();
                _snippetLinkTimer.Enabled = true;
            }

            ManagedRange undoneSnippetLinkRange = null;
            if (e.UndoRedoFlags.IsUndo && _snippetLinks.IsActive)
            {
                foreach (ManagedRange mr in Scintilla.ManagedRanges)
                {
                    if (mr.Start == e.Position && mr.Length == e.Length && mr.Length > 1)
                    {
                        undoneSnippetLinkRange = mr;

                        //	Expanding the range So that it won't get marked for deletion
                        mr.End++;
                    }
                }
            }

            //	It's possible that the _end point may have been deleted. The endpoint
            //	is an ultra persistent marker that cannot be deleted until the Snippet
            //	Link mode is deactivated. Place a new EndPoint at the begining of the
            //	deleted range.
            if (_snippetLinks.IsActive && _snippetLinks.EndPoint != null && _snippetLinks.EndPoint.Scintilla == null)
            {
                SnippetLinkEnd eci = new SnippetLinkEnd(e.Position, Scintilla);
                Scintilla.ManagedRanges.Add(eci);
                _snippetLinks.EndPoint = eci;
            }

            //	Now collapse the Undone range in preparation for the
            //	newly inserted text that will be put in here
            if (undoneSnippetLinkRange != null)
                undoneSnippetLinkRange.End = undoneSnippetLinkRange.Start;

            //	Check to see if all SnippetLink ranges have been deleted.
            //	If this is the case we need to turn Deactivate SnippetLink
            //	mode.

            bool deactivate = true;
            foreach (SnippetLink sl in _snippetLinks.Values)
            {
                if (sl.Ranges.Count > 0)
                {
                    foreach (SnippetLinkRange slr in sl.Ranges)
                    {
                        if (slr.Scintilla != null)
                        {
                            deactivate = false;
                            break;
                        }
                    }
                }
                if (!deactivate)
                    break;
            }

            if (deactivate && IsActive)
                IsActive = false;
        }


        private void Scintilla_BeforeTextInsert(object sender, TextModifiedEventArgs e)
        {
            if (_snippetLinks.IsActive && !_pendingUndo && !(e.UndoRedoFlags.IsUndo || e.UndoRedoFlags.IsRedo))
            {
                _pendingUndo = true;
                Scintilla.UndoRedo.BeginUndoAction();
                _snippetLinkTimer.Enabled = true;
            }
        }


        private void Scintilla_SelectionChanged(object sender, EventArgs e)
        {
            Range sr = Scintilla.Selection.Range;

            if (_snippetLinks.IsActive)
            {
                SnippetLink oldActiveSnippetLink = _snippetLinks.ActiveSnippetLink;
                SnippetLinkRange oldActiveRange = _snippetLinks.ActiveRange;

                _snippetLinks.ActiveSnippetLink = null;
                _snippetLinks.ActiveRange = null;

                for (int i = 0; i < _snippetLinks.Count; i++)
                {

                    SnippetLink sl = _snippetLinks[i];

                    foreach (SnippetLinkRange r in sl.Ranges)
                    {
                        if (r.IntersectsWith(sr))
                        {
                            _snippetLinks.ActiveSnippetLink = sl;
                            _snippetLinks.ActiveRange = r;
                            break;
                        }
                    }
                    if (_snippetLinks.ActiveRange != null)
                        break;
                }

                foreach (SnippetLink sl in _snippetLinks.Values)
                    foreach (Range r in sl.Ranges)
                    {
                        if (sl == _snippetLinks.ActiveSnippetLink)
                        {
                            r.ClearIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                            r.SetIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                        }
                        else
                        {
                            r.SetIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                            r.ClearIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                        }
                    }
            }
        }


        private void Scintilla_TextInserted(object sender, TextModifiedEventArgs e)
        {
            //	I'm going to have to look into making this a little less "sledge hammer to
            //	the entire documnet"ish
            if (_snippetLinks.IsActive && (e.UndoRedoFlags.IsUndo || e.UndoRedoFlags.IsRedo))
                Scintilla.NativeInterface.Colourise(0, -1);
        }


        internal void SetIndicators()
        {
            Scintilla.Indicators[_activeSnippetIndicator].Style = _activeSnippetIndicatorStyle;
            Scintilla.Indicators[_activeSnippetIndicator].Color = _activeSnippetColor;

            Scintilla.Indicators[_inactiveSnippetIndicator].Style = _inactiveSnippetIndicatorStyle;
            Scintilla.Indicators[_inactiveSnippetIndicator].Color = _inactiveSnippetColor;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeActiveSnippetColor() ||
                ShouldSerializeActiveSnippetIndicator() ||
                ShouldSerializeActiveSnippetIndicatorStyle() ||
                ShouldSerializeInactiveSnippetColor() ||
                ShouldSerializeInactiveSnippetIndicator() ||
                ShouldSerializeInactiveSnippetIndicatorStyle()||
                ShouldSerializeIsOneKeySelectionEmbedEnabled() ||
                ShouldSerializeIsEnabled() ||
                ShouldSerializeDefaultDelimeter();
        }


        private bool ShouldSerializeActiveSnippetColor()
        {
            return _activeSnippetColor != Color.Lime;
        }


        private bool ShouldSerializeActiveSnippetIndicator()
        {
            return _activeSnippetIndicator != 15;
        }


        private bool ShouldSerializeActiveSnippetIndicatorStyle()
        {
            return _activeSnippetIndicatorStyle != IndicatorStyle.RoundBox;
        }


        private bool ShouldSerializeDefaultDelimeter()
        {
            return _defaultDelimeter != '$';
        }


        private bool ShouldSerializeInactiveSnippetColor()
        {
            return _inactiveSnippetColor != Color.Lime;
        }


        private bool ShouldSerializeInactiveSnippetIndicator()
        {
            return _inactiveSnippetIndicator != 16;
        }


        private bool ShouldSerializeInactiveSnippetIndicatorStyle()
        {
            return _inactiveSnippetIndicatorStyle != IndicatorStyle.Box;
        }


        private bool ShouldSerializeIsEnabled()
        {
            return !_isEnabled;
        }


        private bool ShouldSerializeIsOneKeySelectionEmbedEnabled()
        {
            return _isOneKeySelectionEmbedEnabled;
        }


         public void ShowSnippetList()
        {
            if (_list.Count == 0)
                return;

            if (_snipperChooser == null)
            {
                _snipperChooser = new SnippetChooser();
                _snipperChooser.Scintilla = Scintilla;
                _snipperChooser.SnippetList = _list.ToString();
                _snipperChooser.Scintilla.Controls.Add(_snipperChooser);
            }
            _snipperChooser.SnippetList = _list.ToString();
            _snipperChooser.Show();
        }


        public void ShowSurroundWithList()
        {
            SnippetList sl = new SnippetList(null);
            foreach (Snippet s in _list)
            {
                if (s.IsSurroundsWith)
                    sl.Add(s);
            }

            if (sl.Count == 0)
                return;

            if (_snipperChooser == null)
            {
                _snipperChooser = new SnippetChooser();
                _snipperChooser.Scintilla = Scintilla;
                _snipperChooser.SnippetList = _list.ToString();
                _snipperChooser.Scintilla.Controls.Add(_snipperChooser);
            }
            _snipperChooser.SnippetList = sl.ToString();
            _snipperChooser.Show();
        }


        private void snippetLinkTimer_Tick(object sender, EventArgs e)
        {
            _snippetLinkTimer.Enabled = false;
            Range sr = Scintilla.Selection.Range;

            if (_snippetLinks.IsActive)
            {
                SnippetLink oldActiveSnippetLink = _snippetLinks.ActiveSnippetLink;
                SnippetLinkRange oldActiveRange = _snippetLinks.ActiveRange;

                if (oldActiveRange != null && (oldActiveRange.IntersectsWith(sr) || oldActiveRange.Equals(sr)))
                {
                    Scintilla.BeginInvoke(new MethodInvoker(delegate()
                    {
                        cascadeSnippetLinkRangeChange(oldActiveSnippetLink, oldActiveRange);

                        foreach (SnippetLink sl in _snippetLinks.Values)
                            foreach (Range r in sl.Ranges)
                            {
                                if (sl == _snippetLinks.ActiveSnippetLink)
                                {
                                    r.ClearIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                                    r.SetIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                                }
                                else
                                {
                                    r.SetIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                                    r.ClearIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                                }

                            }

                        if (_pendingUndo)
                        {
                            _pendingUndo = false;
                            Scintilla.UndoRedo.EndUndoAction();
                        }

                        Scintilla.NativeInterface.Colourise(0, -1);
                    }));
                }
            }
        }

        #endregion Methods


        #region Properties

        public Color ActiveSnippetColor
        {
            get
            {
                return _activeSnippetColor;
            }
            set
            {
                _activeSnippetColor = value;
            }
        }


        public int ActiveSnippetIndicator
        {
            get
            {
                return _activeSnippetIndicator;
            }
            set
            {
                _activeSnippetIndicator = value;
            }
        }


        public IndicatorStyle ActiveSnippetIndicatorStyle
        {
            get
            {
                return _activeSnippetIndicatorStyle;
            }
            set
            {
                _activeSnippetIndicatorStyle = value;
            }
        }


        public char DefaultDelimeter
        {
            get
            {
                return _defaultDelimeter;
            }
            set
            {
                _defaultDelimeter = value;
            }
        }


        public Color InactiveSnippetColor
        {
            get
            {
                return _inactiveSnippetColor;
            }
            set
            {
                _inactiveSnippetColor = value;
            }
        }


        public int InactiveSnippetIndicator
        {
            get
            {
                return _inactiveSnippetIndicator;
            }
            set
            {
                _inactiveSnippetIndicator = value;
            }
        }


        public IndicatorStyle InactiveSnippetIndicatorStyle
        {
            get
            {
                return _inactiveSnippetIndicatorStyle;
            }
            set
            {
                _inactiveSnippetIndicatorStyle = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsActive
        {
            get
            {
                return _snippetLinks.IsActive;
            }
            internal set
            {
                _snippetLinks.IsActive = value;

                if (value)
                {
                    SetIndicators();
                    _snippetLinks[0].Ranges[0].Select();
                }
                else
                {
                    //	Deactivating Snippet Link mode. First make sure all
                    //	the snippet link ranges have their indicators cleared
                    foreach (SnippetLink sl in _snippetLinks.Values)
                        foreach (Range r in sl.Ranges)
                        {
                            r.ClearIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                            r.ClearIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                        }

                    //	Then clear out the _snippetLinks list cuz we're done with them
                    _snippetLinks.Clear();

                    if (_snippetLinks.EndPoint != null)
                    {
                        _snippetLinks.EndPoint.Dispose();
                        _snippetLinks.EndPoint = null;
                    }
                }
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;

                if (value)
                {
                    Scintilla.TextInserted += new EventHandler<TextModifiedEventArgs>(Scintilla_TextInserted);
                    Scintilla.BeforeTextInsert += new EventHandler<TextModifiedEventArgs>(Scintilla_BeforeTextInsert);
                    Scintilla.BeforeTextDelete += new EventHandler<TextModifiedEventArgs>(Scintilla_BeforeTextDelete);
                    Scintilla.SelectionChanged += new EventHandler(Scintilla_SelectionChanged);
                }
                else
                {
                    Scintilla.TextInserted -= new EventHandler<TextModifiedEventArgs>(Scintilla_TextInserted);
                    Scintilla.BeforeTextInsert -= new EventHandler<TextModifiedEventArgs>(Scintilla_BeforeTextInsert);
                    Scintilla.BeforeTextDelete -= new EventHandler<TextModifiedEventArgs>(Scintilla_BeforeTextDelete);
                    Scintilla.SelectionChanged -= new EventHandler(Scintilla_SelectionChanged);
                }
            }
        }


        public bool IsOneKeySelectionEmbedEnabled
        {
            get
            {
                return _isOneKeySelectionEmbedEnabled;
            }
            set
            {
                _isOneKeySelectionEmbedEnabled = value;
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SnippetList List
        {
            get
            {
                return _list;
            }
            set
            {
                _list = value;
            }
        }

        #endregion Properties


        #region Constructors

        public SnippetManager(Scintilla scintilla) : base(scintilla)
        {
            _list = new SnippetList(this);

            _snippetLinkTimer.Interval = 1;
            _snippetLinkTimer.Tick += new EventHandler(snippetLinkTimer_Tick);

            IsEnabled = _isEnabled;
        }

        #endregion Constructors
    }
}