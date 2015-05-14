#region Using Directives

using System;
using System.Collections.Generic;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Mostly behaves like a stack but internally maintains a List for more flexability
    /// </summary>
    /// <remarks>
    ///     FakeStack is not a general purpose datastructure and can only hold NavigationPoint objects
    /// </remarks>
    public class FakeStack : List<NavigationPont>
    {
        #region Fields

        private int _maxCount = 50;

        #endregion Fields


        #region Methods

        public NavigationPont Pop()
        {
            NavigationPont ret = this[Count - 1];
            RemoveAt(Count - 1);
            return ret;
        }


        public void Push(NavigationPont value)
        {
            Add(value);
            if (Count > MaxCount)
                RemoveAt(0);
        }

        #endregion Methods


        #region Properties

        public NavigationPont Current
        {
            get
            {
                return this[Count - 1];
            }
        }


        public int MaxCount
        {
            get
            {
                return _maxCount;
            }
            set
            {
                _maxCount = value;
            }
        }

        #endregion Properties
    }
}
