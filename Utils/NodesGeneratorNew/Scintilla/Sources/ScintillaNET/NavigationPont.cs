#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents a point in the document used for navigation.
    /// </summary>
    public class NavigationPont : ManagedRange
    {
        #region Methods

        /// <summary>
        ///     Overridden.
        /// </summary>
        public override void Dispose()
        {
            Scintilla.DocumentNavigation.ForewardStack.Remove(this);
            Scintilla.DocumentNavigation.BackwardStack.Remove(this);

            base.Dispose();
        }

        #endregion Methods


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the NavigationPont class.
        /// </summary>
        public NavigationPont(int pos, Scintilla scintilla) : base(pos, pos, scintilla)
        {
        }

        #endregion Constructors
    }
}
