#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET
{
    public class SnippetLinkRange : ManagedRange
    {
        #region Fields

        private bool _active = false;
        private string _key;
        private List<SnippetLinkRange> _parent;

        #endregion Fields


        #region Methods

        public override void Dispose()
        {
            if (!IsDisposed)
            {
                _parent.Remove(this);
                base.Dispose();
            }
        }


        internal void Init()
        {
            Scintilla.ManagedRanges.Add(this);
        }

        #endregion Methods


        #region Properties

        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;

                if (value)
                {
                    ClearIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                    SetIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                }
                else
                {
                    SetIndicator(Scintilla.Snippets.InactiveSnippetIndicator);
                    ClearIndicator(Scintilla.Snippets.ActiveSnippetIndicator);
                }
            }
        }


        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }


        public List<SnippetLinkRange> Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        #endregion Properties


        #region Constructors

        public SnippetLinkRange(int start, int end, Scintilla scintilla, string key) : base()
        {
            Scintilla = scintilla;
            Start = start;
            End = end;
            _key = key;
        }

        #endregion Constructors
    }
}
