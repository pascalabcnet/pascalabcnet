#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class MarginConfigList : List<MarginConfig>
    {
        #region Fields

        private Color _foldMarginColor;
        private Color _foldMarginHighlightColor;
        private bool? _inherit;
        private int? _left;
        private int? _right;

        #endregion Fields


        #region Properties

        public Color FoldMarginColor
        {
            get
            {
                return _foldMarginColor;
            }
            set
            {
                _foldMarginColor = value;
            }
        }


        public Color FoldMarginHighlightColor
        {
            get
            {
                return _foldMarginHighlightColor;
            }
            set
            {
                _foldMarginHighlightColor = value;
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


        public int? Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }


        public int? Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        #endregion Properties
    }
}
