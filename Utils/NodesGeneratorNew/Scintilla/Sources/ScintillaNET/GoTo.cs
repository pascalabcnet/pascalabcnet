#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    public class GoTo : TopLevelHelper
    {
        #region Methods

        public void Line(int number)
        {
            NativeScintilla.GotoLine(number);
        }


        public void Position(int pos)
        {
            NativeScintilla.GotoPos(pos);
        }


        public void ShowGoToDialog()
        {
            GoToDialog gd = new GoToDialog();

            gd.CurrentLineNumber = Scintilla.Lines.Current.Number;
            gd.MaximumLineNumber = Scintilla.Lines.Count;
            gd.Scintilla = Scintilla;

            if (gd.ShowDialog() == DialogResult.OK)
                Line(gd.GotoLineNumber);

            Scintilla.Focus();
        }

        #endregion Methods


        #region Constructors

        internal GoTo(Scintilla scintilla) : base(scintilla) {}

        #endregion Constructors
    }
}
