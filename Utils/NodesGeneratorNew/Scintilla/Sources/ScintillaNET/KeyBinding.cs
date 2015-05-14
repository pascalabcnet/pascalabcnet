#region Using Directives

using System;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Represents the Binding Combination of a Keyboard Key + Modifiers
    /// </summary>
    public struct KeyBinding
    {
        #region Fields

        private Keys _keycode;
        private Keys _modifiers;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Overridden.
        /// </summary>
        /// <param name="obj">Another KeyBinding struct</param>
        /// <returns>True if the Keycode and Modifiers are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is KeyBinding))
                return false;

            KeyBinding kb = (KeyBinding)obj;

            return _keycode == kb._keycode && _modifiers == kb._modifiers;
        }

        /// <summary>
        ///     Overridden
        /// </summary>
        /// <returns>Hashcode of ToString()</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        ///     Overridden. Returns string representation of the Keyboard shortcut
        /// </summary>
        /// <returns>Returns string representation of the Keyboard shortcut</returns>
        public override string ToString()
        {
            return ((int)_keycode).ToString() + ((int)_modifiers).ToString();
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets/Sets Key to trigger command
        /// </summary>
        public Keys KeyCode
        {
            get
            {
                return _keycode;
            }
            set
            {
                _keycode = value;
            }
        }


        /// <summary>
        ///     Gets sets key modifiers to the Keyboard shortcut
        /// </summary>
        public Keys Modifiers
        {
            get
            {
                return _modifiers;
            }
            set
            {
                _modifiers = value;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the KeyBinding structure.
        /// </summary>
        /// <param name="keycode">Key to trigger command</param>
        /// <param name="modifiers"> key modifiers to the Keyboard shortcut</param>
        public KeyBinding(Keys keycode, Keys modifiers)
        {
            _keycode = keycode;
            _modifiers = modifiers;
        }

        #endregion Constructors
    }
}
