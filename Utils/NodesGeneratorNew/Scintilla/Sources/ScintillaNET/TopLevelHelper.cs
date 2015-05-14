#region Using Directives

using System;
using System.Text;
using System.ComponentModel;
using System.Collections;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Top level ScintillaHelpers Like Style and Folding inherit from this class so they don't have
    ///     to reimplement the same Equals method
    /// </summary>
    public class TopLevelHelper : ScintillaHelperBase
    {
        #region Methods

        public override bool Equals(object obj)
        {
            return IsSameHelperFamily(obj);
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion Methods


        #region Constructors

        internal TopLevelHelper(Scintilla scintilla) : base(scintilla) { }

        #endregion Constructors
    }
}
