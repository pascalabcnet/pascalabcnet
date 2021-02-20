#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    public class Snippet : IComparable<Snippet>
    {
        #region Constants

        internal const char RealDelimeter = '\x1';

        #endregion Constants


        #region Fields

        private string _code;
        private char _delimeter;
        private bool _isSurroundsWith;
        private List<string> _languages = new List<string>();
        private string _realCode;
        private string _shortcut;
        public char DefaultDelimeter = '$';

        #endregion Fields


        #region Methods

        public int CompareTo(Snippet other)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(_shortcut, other._shortcut);
        }

        #endregion Methods


        #region Properties

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                _realCode = _code.Replace(_delimeter, RealDelimeter);
            }
        }


        public char Delimeter
        {
            get
            {
                return _delimeter;
            }
            set
            {
                _delimeter = value;
            }
        }


        public bool IsSurroundsWith
        {
            get
            {
                return _isSurroundsWith;
            }
            set
            {
                _isSurroundsWith = value;
            }
        }


        public List<string> Languages
        {
            get
            {
                return _languages;
            }
            set
            {
                _languages = value;
            }
        }


        internal string RealCode
        {
            get
            {
                return _realCode;
            }
            set
            {
                _realCode = value;
            }
        }


        public string Shortcut
        {
            get
            {
                return _shortcut;
            }
            set
            {
                _shortcut = value;
            }
        }

        #endregion Properties


        #region Constructors

        public Snippet(string shortcut, string code) : this(shortcut, code, '$', false)
        {
        }


        public Snippet(string shortcut, string code, char delimeter, bool isSurroundsWith)
        {
            _isSurroundsWith = isSurroundsWith;
            _shortcut = shortcut;
            _delimeter = delimeter;
            Code = code;
        }

        #endregion Constructors
    }
}
