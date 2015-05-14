#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class MarginConfig
    {
        #region Fields

        private int? _autoToggleMarkerNumber;
        private bool? _inherit;
        private bool? _isClickable;
        private bool? _isFoldMargin;
        private bool? _isMarkerMargin;
        private int _number;
        private MarginType? _type;
        private int? _width;

        #endregion Fields


        #region Properties

        public int? AutoToggleMarkerNumber
        {
            get
            {
                return _autoToggleMarkerNumber;
            }
            set
            {
                _autoToggleMarkerNumber = value;
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


        public bool? IsClickable
        {
            get
            {
                return _isClickable;
            }
            set
            {
                _isClickable = value;
            }
        }


        public bool? IsFoldMargin
        {
            get
            {
                return _isFoldMargin;
            }
            set
            {
                _isFoldMargin = value;
            }
        }


        public bool? IsMarkerMargin
        {
            get
            {
                return _isMarkerMargin;
            }
            set
            {
                _isMarkerMargin = value;
            }
        }


        public int Number
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


        public MarginType? Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }


        public int? Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        #endregion Properties
    }
}
