#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    public partial class FindReplaceDialog : Form
    {
        #region Fields

        private BindingSource _bindingSourceFind = new BindingSource();
        private List<string> _mruFind;
        private BindingSource _bindingSourceReplace = new BindingSource();
        private List<string> _mruReplace;
        private int _mruMaxCount = 10;
        private Scintilla _scintilla;
        private Range _searchRange = null;

        #endregion Fields


        #region Methods

        private void AddFindMru()
        {
            string find = cboFindF.Text;
            _mruFind.Remove(find);

            _mruFind.Insert(0, find);

            if (_mruFind.Count > _mruMaxCount)
                _mruFind.RemoveAt(_mruFind.Count - 1);

            _bindingSourceFind.ResetBindings(false);
            cboFindR.SelectedIndex = 0;
            cboFindF.SelectedIndex = 0;
        }


        private void AddReplacMru()
        {
            string find = cboFindR.Text;
            _mruFind.Remove(find);

            _mruFind.Insert(0, find);

            if (_mruFind.Count > _mruMaxCount)
                _mruFind.RemoveAt(_mruFind.Count - 1);

            string replace = cboReplace.Text;
            if (replace != string.Empty)
            {
                _mruReplace.Remove(replace);

                _mruReplace.Insert(0, replace);

                if (_mruReplace.Count > _mruMaxCount)
                    _mruReplace.RemoveAt(_mruReplace.Count - 1);
            }

            _bindingSourceFind.ResetBindings(false);
            _bindingSourceReplace.ResetBindings(false);
            cboFindR.SelectedIndex = 0;
            cboFindF.SelectedIndex = 0;
            cboReplace.SelectedIndex = 0;
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            Scintilla.Markers.DeleteAll(Scintilla.FindReplace.Marker);
            Scintilla.FindReplace.ClearAllHighlights();
        }


        private void btnFindAll_Click(object sender, EventArgs e)
        {
            if (cboFindF.Text == string.Empty)
                return;

            AddFindMru();

            lblStatus.Text = string.Empty;

            List<Range> foundRanges = null;
            if (rdoRegexF.Checked)
            {
                Regex rr = null;
                try
                {
                    rr = new Regex(cboFindF.Text, GetRegexOptions());
                }
                catch (ArgumentException ex)
                {
                    lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                    return;
                }

                if (chkSearchSelectionF.Checked)
                {
                    if (_searchRange == null)
                    {
                        _searchRange = Scintilla.Selection.Range;
                    }

                    foundRanges = Scintilla.FindReplace.FindAll(_searchRange, rr);
                }
                else
                {
                    _searchRange = null;
                    foundRanges = Scintilla.FindReplace.FindAll(rr);
                }
            }
            else
            {
                if (chkSearchSelectionF.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    foundRanges = Scintilla.FindReplace.FindAll(_searchRange, cboFindF.Text, GetSearchFlags());
                }
                else
                {
                    _searchRange = null;
                    foundRanges = Scintilla.FindReplace.FindAll(cboFindF.Text, GetSearchFlags());
                }
            }

            lblStatus.Text = "Total found: " + foundRanges.Count.ToString();

            btnClear_Click(null, null);

            if (chkMarkLine.Checked)
                Scintilla.FindReplace.MarkAll(foundRanges);

            if (chkHighlightMatches.Checked)
                Scintilla.FindReplace.HighlightAll(foundRanges);
        }


        private void btnFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }


        private void btnFindPrevious_Click(object sender, EventArgs e)
        {
            FindPrevious();
        }


        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            if (cboFindR.Text == string.Empty)
                return;

            lblStatus.Text = string.Empty;

            List<Range> foundRanges = null;

            if (rdoRegexR.Checked)
            {
                Regex rr = null;
                try
                {
                    rr = new Regex(cboFindR.Text, GetRegexOptions());
                }
                catch (ArgumentException ex)
                {
                    lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                    return;
                }

                if (chkSearchSelectionR.Checked)
                {
                    if (_searchRange == null)
                    {
                        _searchRange = Scintilla.Selection.Range;
                    }

                    foundRanges = Scintilla.FindReplace.ReplaceAll(_searchRange, rr, cboReplace.Text);
                }
                else
                {
                    _searchRange = null;
                    foundRanges = Scintilla.FindReplace.ReplaceAll(rr, cboReplace.Text);
                }
            }
            else
            {
                if (chkSearchSelectionR.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    foundRanges = Scintilla.FindReplace.ReplaceAll(_searchRange, cboFindR.Text, cboReplace.Text, GetSearchFlags());
                }
                else
                {
                    _searchRange = null;
                    foundRanges = Scintilla.FindReplace.ReplaceAll(cboFindR.Text, cboReplace.Text, GetSearchFlags());
                }
            }

            lblStatus.Text = "Total Replaced: " + foundRanges.Count.ToString();
        }


        private void btnReplaceNext_Click(object sender, EventArgs e)
        {
            ReplaceNext();
        }


        private void btnReplacePrevious_Click(object sender, EventArgs e)
        {
            if (cboFindR.Text == string.Empty)
                return;

            AddReplacMru();
            lblStatus.Text = string.Empty;

            Range nextRange = null;
            try
            {
                nextRange = ReplaceNext(true);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                return;
            }


            if (nextRange == null)
            {
                lblStatus.Text = "Match could not be found";
            }
            else
            {
                if (nextRange.Start > Scintilla.Caret.Anchor)
                {
                    if (chkSearchSelectionR.Checked)
                        lblStatus.Text = "Search match wrapped to the begining of the selection";
                    else
                        lblStatus.Text = "Search match wrapped to the begining of the document";
                }

                nextRange.Select();
                MoveFormAwayFromSelection();
            }
        }


        private void chkEcmaScript_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                chkExplicitCaptureF.Checked = false;
                chkExplicitCaptureR.Checked = false;
                chkExplicitCaptureF.Enabled = false;
                chkExplicitCaptureR.Enabled = false;
                chkIgnorePatternWhitespaceF.Checked = false;
                chkIgnorePatternWhitespaceR.Checked = false;
                chkIgnorePatternWhitespaceF.Enabled = false;
                chkIgnorePatternWhitespaceR.Enabled = false;
                chkRightToLeftF.Checked = false;
                chkRightToLeftR.Checked = false;
                chkRightToLeftF.Enabled = false;
                chkRightToLeftR.Enabled = false;
                chkSinglelineF.Checked = false;
                chkSinglelineR.Checked = false;
                chkSinglelineF.Enabled = false;
                chkSinglelineR.Enabled = false;
            }
            else
            {
                chkExplicitCaptureF.Enabled = true;
                chkIgnorePatternWhitespaceF.Enabled = true;
                chkRightToLeftF.Enabled = true;
                chkSinglelineF.Enabled = true;
                chkExplicitCaptureR.Enabled = true;
                chkIgnorePatternWhitespaceR.Enabled = true;
                chkRightToLeftR.Enabled = true;
                chkSinglelineR.Enabled = true;
            }
        }


        public void FindNext()
        {
            if (cboFindF.Text == string.Empty)
                return;

            AddFindMru();
            lblStatus.Text = string.Empty;

            Range foundRange = null;

            try
            {
                foundRange = FindNextF(false);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                return;
            }

            if (foundRange == null)
            {
                lblStatus.Text = "Match could not be found";
            }
            else
            {
                if (foundRange.Start < Scintilla.Caret.Anchor)
                {
                    if (chkSearchSelectionF.Checked)
                        lblStatus.Text = "Search match wrapped to the begining of the selection";
                    else
                        lblStatus.Text = "Search match wrapped to the begining of the document";
                }

                foundRange.Select();
                MoveFormAwayFromSelection();
            }
        }


        private Range FindNextF(bool searchUp)
        {
            Range foundRange;

            if (rdoRegexF.Checked)
            {
                Regex rr = new Regex(cboFindF.Text, GetRegexOptions());

                if (chkSearchSelectionF.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(rr, chkWrapF.Checked, _searchRange);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(rr, chkWrapF.Checked, _searchRange);
                }
                else
                {
                    _searchRange = null;
                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(rr, chkWrapF.Checked);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(rr, chkWrapF.Checked);
                }
            }
            else
            {
                if (chkSearchSelectionF.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(cboFindF.Text, chkWrapF.Checked, GetSearchFlags(), _searchRange);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(cboFindF.Text, chkWrapF.Checked, GetSearchFlags(), _searchRange);
                }
                else
                {
                    _searchRange = null;
                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(cboFindF.Text, chkWrapF.Checked, GetSearchFlags());
                    else
                        foundRange = Scintilla.FindReplace.FindNext(cboFindF.Text, chkWrapF.Checked, GetSearchFlags());
                }
            }
            return foundRange;
        }


        private Range FindNextR(bool searchUp, ref Regex rr)
        {
            Range foundRange;


            if (rdoRegexR.Checked)
            {
                if (rr == null)
                    rr = new Regex(cboFindR.Text, GetRegexOptions());

                if (chkSearchSelectionR.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(rr, chkWrapR.Checked, _searchRange);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(rr, chkWrapR.Checked, _searchRange);
                }
                else
                {
                    _searchRange = null;
                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(rr, chkWrapR.Checked);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(rr, chkWrapR.Checked);
                }
            }
            else
            {
                if (chkSearchSelectionF.Checked)
                {
                    if (_searchRange == null)
                        _searchRange = Scintilla.Selection.Range;

                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(cboFindR.Text, chkWrapR.Checked, GetSearchFlags(), _searchRange);
                    else
                        foundRange = Scintilla.FindReplace.FindNext(cboFindR.Text, chkWrapR.Checked, GetSearchFlags(), _searchRange);
                }
                else
                {
                    _searchRange = null;
                    if (searchUp)
                        foundRange = Scintilla.FindReplace.FindPrevious(cboFindR.Text, chkWrapF.Checked, GetSearchFlags());
                    else
                        foundRange = Scintilla.FindReplace.FindNext(cboFindR.Text, chkWrapF.Checked, GetSearchFlags());
                }
            }
            return foundRange;
        }


        public void FindPrevious()
        {
            if (cboFindF.Text == string.Empty)
                return;

            AddFindMru();
            lblStatus.Text = string.Empty;
            Range foundRange = null;
            try
            {
                foundRange = FindNextF(true);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                return;
            }

            if (foundRange == null)
            {
                lblStatus.Text = "Match could not be found";
            }
            else
            {
                if (foundRange.Start > Scintilla.Caret.Position)
                {
                    if (chkSearchSelectionF.Checked)
                        lblStatus.Text = "Search match wrapped to the _end of the selection";
                    else
                        lblStatus.Text = "Search match wrapped to the _end of the document";
                }

                foundRange.Select();
                MoveFormAwayFromSelection();
            }
        }


        private void FindReplaceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }


        public RegexOptions GetRegexOptions()
        {
            RegexOptions ro = RegexOptions.None;

            if (tabAll.SelectedTab == tpgFind)
            {
                if (chkCompiledF.Checked)
                    ro |= RegexOptions.Compiled;

                if (chkCultureInvariantF.Checked)
                    ro |= RegexOptions.Compiled;

                if (chkEcmaScriptF.Checked)
                    ro |= RegexOptions.ECMAScript;

                if (chkExplicitCaptureF.Checked)
                    ro |= RegexOptions.ExplicitCapture;

                if (chkIgnoreCaseF.Checked)
                    ro |= RegexOptions.IgnoreCase;

                if (chkIgnorePatternWhitespaceF.Checked)
                    ro |= RegexOptions.IgnorePatternWhitespace;

                if (chkMultilineF.Checked)
                    ro |= RegexOptions.Multiline;

                if (chkRightToLeftF.Checked)
                    ro |= RegexOptions.RightToLeft;

                if (chkSinglelineF.Checked)
                    ro |= RegexOptions.Singleline;
            }
            else
            {
                if (chkCompiledR.Checked)
                    ro |= RegexOptions.Compiled;

                if (chkCultureInvariantR.Checked)
                    ro |= RegexOptions.Compiled;

                if (chkEcmaScriptR.Checked)
                    ro |= RegexOptions.ECMAScript;

                if (chkExplicitCaptureR.Checked)
                    ro |= RegexOptions.ExplicitCapture;

                if (chkIgnoreCaseR.Checked)
                    ro |= RegexOptions.IgnoreCase;

                if (chkIgnorePatternWhitespaceR.Checked)
                    ro |= RegexOptions.IgnorePatternWhitespace;

                if (chkMultilineR.Checked)
                    ro |= RegexOptions.Multiline;

                if (chkRightToLeftR.Checked)
                    ro |= RegexOptions.RightToLeft;

                if (chkSinglelineR.Checked)
                    ro |= RegexOptions.Singleline;
            }

            return ro;
        }


        public SearchFlags GetSearchFlags()
        {
            SearchFlags sf = SearchFlags.Empty;

            if (tabAll.SelectedTab == tpgFind)
            {
                if (chkMatchCaseF.Checked)
                    sf |= SearchFlags.MatchCase;

                if (chkWholeWordF.Checked)
                    sf |= SearchFlags.WholeWord;

                if (chkWordStartF.Checked)
                    sf |= SearchFlags.WordStart;
            }
            else
            {
                if (chkMatchCaseR.Checked)
                    sf |= SearchFlags.MatchCase;

                if (chkWholeWordR.Checked)
                    sf |= SearchFlags.WholeWord;

                if (chkWordStartR.Checked)
                    sf |= SearchFlags.WordStart;
            }

            return sf;
        }


        public void MoveFormAwayFromSelection()
        {
            if (!Visible)
                return;

            int pos = Scintilla.Caret.Position;
            int x = Scintilla.PointXFromPosition(pos);
            int y = Scintilla.PointYFromPosition(pos);

            Point cursorPoint = Scintilla.PointToScreen(new Point(x, y));

            Rectangle r = new Rectangle(Location, Size);
            if (r.Contains(cursorPoint))
            {
                Point newLocation;
                if (cursorPoint.Y < (Screen.PrimaryScreen.Bounds.Height / 2))
                {
                    // Top half of the screen
                    newLocation = Scintilla.PointToClient(
                        new Point(Location.X, cursorPoint.Y + Scintilla.Lines.Current.Height * 2)
                        );
                }
                else
                {
                    // Bottom half of the screen
                    newLocation = Scintilla.PointToClient(
                        new Point(Location.X, cursorPoint.Y - Height - (Scintilla.Lines.Current.Height * 2))
                        );
                }
                newLocation = Scintilla.PointToScreen(newLocation);
                Location = newLocation;
            }
        }


        protected override void OnActivated(EventArgs e)
        {
            if (Scintilla.Selection.Length > 0)
            {
                chkSearchSelectionF.Enabled = true;
                chkSearchSelectionR.Enabled = true;
            }
            else
            {
                chkSearchSelectionF.Enabled = false;
                chkSearchSelectionR.Enabled = false;
                chkSearchSelectionF.Checked = false;
                chkSearchSelectionR.Checked = false;
            }

            //	if they leave the dialog and come back any "Search Selection"
            //	range they might have had is invalidated
            _searchRange = null;

            lblStatus.Text = string.Empty;

            MoveFormAwayFromSelection();

            base.OnActivated(e);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            //	So what we're doing here is testing for any of the find/replace
            //	command shortcut bindings. If the key combination matches we send
            //	the KeyEventArgs back to Scintilla so it can be processed. That
            //	way things like Find Next, Show Replace are all available from
            //	the dialog using Scintilla's configured Shortcuts

            List<KeyBinding> findNextBinding = Scintilla.Commands.GetKeyBindings(BindableCommand.FindNext);
            List<KeyBinding> findPrevBinding = Scintilla.Commands.GetKeyBindings(BindableCommand.FindPrevious);
            List<KeyBinding> showFindBinding = Scintilla.Commands.GetKeyBindings(BindableCommand.ShowFind);
            List<KeyBinding> showReplaceBinding = Scintilla.Commands.GetKeyBindings(BindableCommand.ShowReplace);

            KeyBinding kb = new KeyBinding(e.KeyCode, e.Modifiers);

            if (findNextBinding.Contains(kb) || findPrevBinding.Contains(kb) || showFindBinding.Contains(kb) || showReplaceBinding.Contains(kb))
            {
                Scintilla.FireKeyDown(e);
            }

            if (e.KeyCode == Keys.Escape)
                Hide();

            base.OnKeyDown(e);
        }


        private void rdoStandardF_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStandardF.Checked)
                pnlStandardOptionsF.BringToFront();
            else
                pnlRegexpOptionsF.BringToFront();
        }


        private void rdoStandardR_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStandardR.Checked)
                pnlStandardOptionsR.BringToFront();
            else
                pnlRegexpOptionsR.BringToFront();
        }


        public void ReplaceNext()
        {
            if (cboFindR.Text == string.Empty)
                return;

            AddReplacMru();
            lblStatus.Text = string.Empty;

            Range nextRange = null;
            try
            {
                nextRange = ReplaceNext(false);
            }
            catch (ArgumentException ex)
            {
                lblStatus.Text = "Error in Regular Expression: " + ex.Message;
                return;
            }

            if (nextRange == null)
            {
                lblStatus.Text = "Match could not be found";
            }
            else
            {
                if (nextRange.Start < Scintilla.Caret.Anchor)
                {
                    if (chkSearchSelectionR.Checked)
                        lblStatus.Text = "Search match wrapped to the begining of the selection";
                    else
                        lblStatus.Text = "Search match wrapped to the begining of the document";
                }

                nextRange.Select();
                MoveFormAwayFromSelection();
            }
        }


        private Range ReplaceNext(bool searchUp)
        {
            Regex rr = null;
            Range selRange = Scintilla.Selection.Range;

            //	We only do the actual replacement if the current selection exactly
            //	matches the find.
            if (selRange.Length > 0)
            {
                if (rdoRegexR.Checked)
                {
                    rr = new Regex(cboFindR.Text, GetRegexOptions());
                    string selRangeText = selRange.Text;

                    if (selRange.Equals(Scintilla.FindReplace.Find(selRange, rr, false)))
                    {
                        //	If searching up we do the replacement using the range object.
                        //	Otherwise we use the selection object. The reason being if
                        //	we use the range the caret is positioned before the replaced
                        //	text. Conversely if we use the selection object the caret will
                        //	be positioned after the replaced text. This is very important
                        //	becuase we don't want the new text to be potentially matched
                        //	in the next search.
                        if (searchUp)
                            selRange.Text = rr.Replace(selRangeText, cboReplace.Text);
                        else
                            Scintilla.Selection.Text = rr.Replace(selRangeText, cboReplace.Text);
                    }
                }
                else
                {
                    if (selRange.Equals(Scintilla.FindReplace.Find(selRange, cboFindR.Text, false)))
                    {
                        if (searchUp)
                            selRange.Text = cboReplace.Text;
                        else
                            Scintilla.Selection.Text = cboReplace.Text;
                    }
                }
            }
            return FindNextR(searchUp, ref rr);
        }


        private void tabAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabAll.SelectedTab == tpgFind)
            {
                cboFindF.Text = cboFindR.Text;

                rdoStandardF.Checked = rdoStandardR.Checked;
                rdoRegexF.Checked = rdoRegexR.Checked;

                chkWrapF.Checked = chkWrapR.Checked;
                chkSearchSelectionF.Checked = chkSearchSelectionR.Checked;

                chkMatchCaseF.Checked = chkMatchCaseR.Checked;
                chkWholeWordF.Checked = chkWholeWordR.Checked;
                chkWordStartF.Checked = chkWordStartR.Checked;

                chkCompiledF.Checked = chkCompiledR.Checked;
                chkCultureInvariantF.Checked = chkCultureInvariantR.Checked;
                chkEcmaScriptF.Checked = chkEcmaScriptR.Checked;
                chkExplicitCaptureF.Checked = chkExplicitCaptureR.Checked;
                chkIgnoreCaseF.Checked = chkIgnoreCaseR.Checked;
                chkIgnorePatternWhitespaceF.Checked = chkIgnorePatternWhitespaceR.Checked;
                chkMultilineF.Checked = chkMultilineR.Checked;
                chkRightToLeftF.Checked = chkRightToLeftR.Checked;
                chkSinglelineF.Checked = chkSinglelineR.Checked;

                AcceptButton = btnFindNext;
            }
            else
            {
                cboFindR.Text = cboFindF.Text;

                rdoStandardR.Checked = rdoStandardF.Checked;
                rdoRegexR.Checked = rdoRegexF.Checked;

                chkWrapR.Checked = chkWrapF.Checked;
                chkSearchSelectionR.Checked = chkSearchSelectionF.Checked;

                chkMatchCaseR.Checked = chkMatchCaseF.Checked;
                chkWholeWordR.Checked = chkWholeWordF.Checked;
                chkWordStartR.Checked = chkWordStartF.Checked;

                chkCompiledR.Checked = chkCompiledF.Checked;
                chkCultureInvariantR.Checked = chkCultureInvariantF.Checked;
                chkEcmaScriptR.Checked = chkEcmaScriptF.Checked;
                chkExplicitCaptureR.Checked = chkExplicitCaptureF.Checked;
                chkIgnoreCaseR.Checked = chkIgnoreCaseF.Checked;
                chkIgnorePatternWhitespaceR.Checked = chkIgnorePatternWhitespaceF.Checked;
                chkMultilineR.Checked = chkMultilineF.Checked;
                chkRightToLeftR.Checked = chkRightToLeftF.Checked;
                chkSinglelineR.Checked = chkSinglelineF.Checked;

                AcceptButton = btnReplaceNext;
            }
        }

        #endregion Methods


        #region Properties

        public List<string> MruFind
        {
            get
            {
                return _mruFind;
            }
            set
            {
                _mruFind = value;
                _bindingSourceFind.DataSource = _mruFind;
            }
        }


        public int MruMaxCount
        {
            get { return _mruMaxCount; }
            set { _mruMaxCount = value; }
        }


        public List<string> MruReplace
        {
            get
            {
                return _mruReplace;
            }
            set
            {
                _mruReplace = value;
                _bindingSourceReplace.DataSource = _mruReplace;
            }
        }


        public Scintilla Scintilla
        {
            get
            {
                return _scintilla;
            }
            set
            {
                _scintilla = value;
            }
        }

        #endregion Properties


        #region Constructors

        public FindReplaceDialog()
        {
            InitializeComponent();

            _mruFind = new List<string>();
            _mruReplace = new List<string>();
            _bindingSourceFind.DataSource = _mruFind;
            _bindingSourceReplace.DataSource = _mruReplace;
            cboFindF.DataSource = _bindingSourceFind;
            cboFindR.DataSource = _bindingSourceFind;
            cboReplace.DataSource = _bindingSourceReplace;
        }

        #endregion Constructors
    }
}
