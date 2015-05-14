#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     List of strings to be used with <see cref="CallTip"/>.
    /// </summary>
    public class OverloadList : List<string>
    {
        #region Fields

        private int _currentIndex;

        #endregion Fields


        #region Properties

        /// <summary>
        ///     Text of the overload to be displayed in the CallTip
        /// </summary>
        public string Current
        {
            get
            {
                return this[_currentIndex];
            }
            set
            {
                _currentIndex = this.IndexOf(value);
            }
        }


        /// <summary>
        ///     Index of the overload to be displayed in the CallTip
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
            internal set
            {
                _currentIndex = value;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Creates a new instance of an OverLoadList
        /// </summary>
        public OverloadList() : base() { }

        /// <summary>
        ///     Creates a new instance of an OverLoadList. The list of overloads is supplied by collection
        /// </summary>
        public OverloadList(IEnumerable<string> collection) : base(collection) { }

        /// <summary>
        ///     Creates a new instance of an OverLoadList. The
        /// </summary>
        public OverloadList(int capacity) : base(capacity) { }

        #endregion Constructors
    }
}
