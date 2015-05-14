#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET
{
    public class SnippetLink
    {
        #region Fields

        private string _key;
        private List<SnippetLinkRange> _ranges = new List<SnippetLinkRange>();

        #endregion Fields


        #region Properties

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


        public List<SnippetLinkRange> Ranges
        {
            get
            {
                return _ranges;
            }
            set
            {
                _ranges = value;
            }
        }

        #endregion Properties


        #region Constructors

        public SnippetLink(string key)
        {
            _key = key;
        }

        #endregion Constructors
    }
}
