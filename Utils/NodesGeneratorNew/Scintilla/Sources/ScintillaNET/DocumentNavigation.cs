#region Using Directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Manages Document Navigation, which is a snapshot history of movements within
    ///     a document.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class DocumentNavigation : TopLevelHelper
    {
        #region Fields

        private bool _supressNext = false;
        private Timer t = null;
        private int _navigationPointTimeout = 200;
        public FakeStack _forewardStack = new FakeStack();
        public FakeStack _backwardStack = new FakeStack();
        private int _maxHistorySize = 50;
        private bool _isEnabled = true;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Causes the current position to navigate to the last snapshotted document position.
        /// </summary>
        public void NavigateBackward()
        {
            if (_backwardStack.Count == 0)
                return;

            int currentPos = Scintilla.Caret.Position;
            if (currentPos == _backwardStack.Current.Start && _backwardStack.Count == 1)
                return;

            int pos = _backwardStack.Pop().Start;

            if (pos != currentPos)
            {
                _forewardStack.Push(NewRange(currentPos));
                Scintilla.Caret.Goto(pos);
            }
            else
            {
                _forewardStack.Push(NewRange(pos));
                Scintilla.Caret.Goto(_backwardStack.Current.Start);
            }

            _supressNext = true;
        }


        /// <summary>
        ///     After 1 or more backwards navigations this command navigates to the previous
        ///     backwards navigation point.
        /// </summary>
        public void NavigateForward()
        {
            if (!CanNavigateForward)
                return;

            int pos = _forewardStack.Pop().Start;
            _backwardStack.Push(NewRange(pos));
            Scintilla.Caret.Goto(pos);

            _supressNext = true;
        }


        private NavigationPont NewRange(int pos)
        {
            NavigationPont mr = new NavigationPont(pos, Scintilla);
            Scintilla.ManagedRanges.Add(mr);
            return mr;
        }


        public void Reset()
        {
            _backwardStack.Clear();
            _forewardStack.Clear();
            ResetIsEnabled();
            ResetMaxHistorySize();
        }


        private void ResetIsEnabled()
        {
            _isEnabled = true;
        }


        private void ResetMaxHistorySize()
        {
            _maxHistorySize = 50;
        }


        private void ResetNavigationPointTimeout()
        {
            _navigationPointTimeout = 200;
        }


        private void scintilla_SelectionChanged(object sender, EventArgs e)
        {
            if (!_isEnabled)
                return;

            if (!_supressNext)
            {
                t.Enabled = false;
                t.Enabled = true;
            }
            else
            {
                _supressNext = false;
            }
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeIsEnabled() || ShouldSerializeMaxHistorySize();
        }


        private bool ShouldSerializeIsEnabled()
        {
            return !_isEnabled;
        }


        private bool ShouldSerializeMaxHistorySize()
        {
            return _maxHistorySize != 50;
        }


        private bool ShouldSerializeNavigationPointTimeout()
        {
            return _navigationPointTimeout != 200;
        }


        private void t_Tick(object sender, EventArgs e)
        {
            t.Enabled = false;
            int pos = NativeScintilla.GetCurrentPos();
            if ((_forewardStack.Count == 0 || _forewardStack.Current.Start != pos) && (_backwardStack.Count == 0 || _backwardStack.Current.Start != pos))
                _backwardStack.Push(NewRange(pos));
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     List of entries that allow you to navigate backwards.
        /// </summary>
        /// <remarks>
        ///     The ForwardStack and BackwardStack can be shared between multiple
        ///     ScintillaNET objects. This is useful in MDI applications when you wish
        ///     to have a shared document navigation that remembers positions in each
        ///     document.
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FakeStack BackwardStack
        {
            get
            {
                return _backwardStack;
            }
            set
            {
                _backwardStack = value;
            }
        }


        /// <summary>
        ///     Returns true if ScintillaNET can perform a successful backward navigation.
        /// </summary>
        [Browsable(false)]
        public bool CanNavigateBackward
        {
            get
            {
                if (_backwardStack.Count == 0 || (NativeScintilla.GetCurrentPos() == _backwardStack.Current.Start && _backwardStack.Count == 1))
                    return false;

                return true;
            }
        }


        /// <summary>
        ///     Returns true if ScintillaNET can perform a successful forward navigation.
        /// </summary>
        [Browsable(false)]
        public bool CanNavigateForward
        {
            get
            {
                return _forewardStack.Count > 0;
            }
        }


        /// <summary>
        ///     List of entries that allow you to navigate forwards.
        /// </summary>
        /// <remarks>
        ///     The ForwardStack and BackwardStack can be shared between multiple
        ///     ScintillaNET objects. This is useful in MDI applications when you wish
        ///     to have a shared document navigation that remembers positions in each
        ///     document.
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FakeStack ForewardStack
        {
            get
            {
                return _forewardStack;
            }
            set
            {
                _forewardStack = value;
            }
        }


        /// <summary>
        ///     Gets/Sets whether Document Navigation is tracked. Defaults to true.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }


        /// <summary>
        ///     Maximum number of places the document navigation remembers. Defaults to 50.
        /// </summary>
        /// <remarks>
        ///     When the max value is reached the oldest entries are removed.
        /// </remarks>
        public int MaxHistorySize
        {
            get
            {
                return _maxHistorySize;
            }
            set
            {
                _maxHistorySize = value;
                _backwardStack.MaxCount = value;
                _forewardStack.MaxCount = value;
            }
        }


        /// <summary>
        ///     Time in milliseconds to wait before a Navigation Point is set. Default is 200
        /// </summary>
        /// <remarks>
        ///     In text editing, the current caret position is constantly changing. Rather than capture every
        ///     change in position, ScintillaNET captures the current position [NavigationPointTimeout]ms after a 
        ///     position changes, only then is it eligable for another snapshot
        /// </remarks>
        public int NavigationPointTimeout
        {
            get
            {
                return _navigationPointTimeout;
            }
            set
            {
                _navigationPointTimeout = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal DocumentNavigation(Scintilla scintilla) : base(scintilla) 
        {
            t = new Timer();
            t.Interval = _navigationPointTimeout;
            t.Tick += new EventHandler(t_Tick);
            scintilla.SelectionChanged += new EventHandler(scintilla_SelectionChanged);
        }

        #endregion Constructors
    }
}
