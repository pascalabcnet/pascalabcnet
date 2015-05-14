#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    public class ToolStripIncrementalSearcher : ToolStripControlHost
    {
        #region Properties

        public Scintilla Scintilla
        {
            get { return Searcher.Scintilla; }
            set { Searcher.Scintilla = value; }
        }


        public IncrementalSearcher Searcher
        {
            get { return Control as IncrementalSearcher; }
        }

        #endregion Properties


        #region Constructors

        public ToolStripIncrementalSearcher() : base(new IncrementalSearcher(true))
        {
        }

        #endregion Constructors
    }
}
