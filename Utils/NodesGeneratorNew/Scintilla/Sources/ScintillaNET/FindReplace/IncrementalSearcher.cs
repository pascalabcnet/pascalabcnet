#region Using Directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    public partial class IncrementalSearcher : UserControl
    {
        #region Fields

        private Scintilla _scintilla;
        private bool _toolItem = false;

        #endregion Fields


        #region Methods

        private void brnPrevious_Click(object sender, EventArgs e)
        {
            findPrevious();
        }


        private void btnClearHighlights_Click(object sender, EventArgs e)
        {
            if (Scintilla == null) 
                return;
            Scintilla.FindReplace.ClearAllHighlights();
        }


        private void btnHighlightAll_Click(object sender, EventArgs e)
        {
            if (txtFind.Text == string.Empty)
                return;
            if (Scintilla == null)
                return;
            Scintilla.FindReplace.HighlightAll(Scintilla.FindReplace.FindAll(txtFind.Text));
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            findNext();
        }


        private void findNext()
        {
            if (txtFind.Text == string.Empty)
                return;
            if (Scintilla == null)
                return;

            Range r = Scintilla.FindReplace.FindNext(txtFind.Text, true, Scintilla.FindReplace.Window.GetSearchFlags());
            if (r != null)
                r.Select();

            moveFormAwayFromSelection();
        }


        private void findPrevious()
        {
            if (txtFind.Text == string.Empty)
                return;
            if (Scintilla == null)
                return;

            Range r = Scintilla.FindReplace.FindPrevious(txtFind.Text, true, Scintilla.FindReplace.Window.GetSearchFlags());
            if (r != null)
                r.Select();

            moveFormAwayFromSelection();
        }


        public void moveFormAwayFromSelection()
        {
            if (!Visible || Scintilla == null)
                return;

            int pos = Scintilla.Caret.Position;
            int x = Scintilla.PointXFromPosition(pos);
            int y = Scintilla.PointYFromPosition(pos);

            Point cursorPoint = new Point(x, y);

            Rectangle r = new Rectangle(Location, Size);
            if (r.Contains(cursorPoint))
            {
                Point newLocation;
                if (cursorPoint.Y < (Screen.PrimaryScreen.Bounds.Height / 2))
                {
                    // Top half of the screen
                    newLocation = new Point(Location.X, cursorPoint.Y + Scintilla.Lines.Current.Height * 2);
                        
                }
                else
                {
                    // Bottom half of the screen
                    newLocation = new Point(Location.X, cursorPoint.Y - Height - (Scintilla.Lines.Current.Height * 2));
                }
                
                Location = newLocation;
            }
        }


        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            moveFormAwayFromSelection();
            txtFind.Focus();
        }


        protected override void OnLeave(EventArgs e)
        {
            base.OnLostFocus(e);
            if(!_toolItem)
            Hide();
        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            txtFind.Text = string.Empty;
            txtFind.BackColor = SystemColors.Window;

            moveFormAwayFromSelection();

            if (Visible)
                txtFind.Focus();
            else if(Scintilla!=null)
                Scintilla.Focus();
        }


        private void txtFind_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                case Keys.Down:
                    findNext();
                    e.Handled = true;
                    break;
                case Keys.Up:
                    findPrevious();
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    if(!_toolItem)
                    Hide();
                    break;
            }
        }


        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            txtFind.BackColor = SystemColors.Window;
            if (txtFind.Text == string.Empty)
                return;
            if (Scintilla == null)
                return;

            int pos = Math.Min(Scintilla.Caret.Position, Scintilla.Caret.Anchor);
            Range r = Scintilla.FindReplace.Find(pos, Scintilla.TextLength, txtFind.Text, Scintilla.FindReplace.Window.GetSearchFlags());
            if (r == null)
                r = Scintilla.FindReplace.Find(0, pos, txtFind.Text, Scintilla.FindReplace.Window.GetSearchFlags());

            if (r != null)
                r.Select();
            else
                txtFind.BackColor = Color.Tomato;

            moveFormAwayFromSelection();
        }

        #endregion Methods


        #region Properties

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

        #endregion Properties


        #region Constructors

        public IncrementalSearcher()
        {
            InitializeComponent();
        }


        public IncrementalSearcher(bool toolItem)
        {
            InitializeComponent();
            _toolItem = toolItem;
            if (toolItem)
                BackColor = Color.Transparent;
        }

        #endregion Constructors
    }
}
