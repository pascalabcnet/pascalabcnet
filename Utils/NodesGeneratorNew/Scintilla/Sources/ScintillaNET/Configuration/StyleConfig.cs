#region Using Directives

using System;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class StyleConfig
    {
        #region Fields

        private Color _backColor;
        private bool? _bold;
        private StyleCase? _case;
        private CharacterSet? _characterSet;
        private string _fontName;
        private Color _foreColor;
        private bool? _inherit;
        private bool? _isChangeable;
        private bool? _isHotspot;
        private bool? _isSelectionEolFilled;
        private bool? _isVisible;
        private bool? _italic;
        private string _name;
        private int? _number;
        private int? _size;
        private bool? _underline;

        #endregion Fields


        #region Methods

        public override string ToString()
        {
            return "Name = \"" + _name + "\" Number=" + _number.ToString();
        }

        #endregion Methods


        #region Properties

        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }


        public bool? Bold
        {
            get
            {
                return _bold;
            }
            set
            {
                _bold = value;
            }
        }


        public StyleCase? Case
        {
            get
            {
                return _case;
            }
            set
            {
                _case = value;
            }
        }


        public CharacterSet? CharacterSet
        {
            get
            {
                return _characterSet;
            }
            set
            {
                _characterSet = value;
            }
        }


        public string FontName
        {
            get
            {
                return _fontName;
            }
            set
            {
                _fontName = value;
            }
        }


        public Color ForeColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                _foreColor = value;
            }
        }


        public bool? Inherit
        {
            get
            {
                return _inherit;
            }
            set
            {
                _inherit = value;
            }
        }


        public bool? IsChangeable
        {
            get
            {
                return _isChangeable;
            }
            set
            {
                _isChangeable = value;
            }
        }


        public bool? IsHotspot
        {
            get
            {
                return _isHotspot;
            }
            set
            {
                _isHotspot = value;
            }
        }


        public bool? IsSelectionEolFilled
        {
            get
            {
                return _isSelectionEolFilled;
            }
            set
            {
                _isSelectionEolFilled = value;
            }
        }


        public bool? IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }


        public bool? Italic
        {
            get
            {
                return _italic;
            }
            set
            {
                _italic = value;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }


        public int? Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }


        public int? Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }


        public bool? Underline
        {
            get
            {
                return _underline;
            }
            set
            {
                _underline = value;
            }
        }

        #endregion Properties
    }
}
