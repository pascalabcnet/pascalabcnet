#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class SnippetsConfigList : List<SnippetsConfig>
    {
        #region Fields

        private Color _activeSnippetColor;
        private int? _activeSnippetIndicator;
        private IndicatorStyle? _activeSnippetIndicatorStyle;
        private char? _defaultDelimeter;
        private Color _inactiveSnippetColor;
        private int? _inactiveSnippetIndicator;
        private IndicatorStyle? _inactiveSnippetIndicatorStyle;
        private bool? _inherit;
        private bool? _isEnabled;
        private bool? _isOneKeySelectionEmbedEnabled;

        #endregion Fields


        #region Properties

        public Color ActiveSnippetColor
        {
            get
            {
                return _activeSnippetColor;
            }
            set
            {
                _activeSnippetColor = value;
            }
        }


        public int? ActiveSnippetIndicator
        {
            get
            {
                return _activeSnippetIndicator;
            }
            set
            {
                _activeSnippetIndicator = value;
            }
        }


        public IndicatorStyle? ActiveSnippetIndicatorStyle
        {
            get
            {
                return _activeSnippetIndicatorStyle;
            }
            set
            {
                _activeSnippetIndicatorStyle = value;
            }
        }


        public char? DefaultDelimeter
        {
            get
            {
                return _defaultDelimeter;
            }
            set
            {
                _defaultDelimeter = value;
            }
        }


        public Color InactiveSnippetColor
        {
            get
            {
                return _inactiveSnippetColor;
            }
            set
            {
                _inactiveSnippetColor = value;
            }
        }


        public int? InactiveSnippetIndicator
        {
            get
            {
                return _inactiveSnippetIndicator;
            }
            set
            {
                _inactiveSnippetIndicator = value;
            }
        }


        public IndicatorStyle? InactiveSnippetIndicatorStyle
        {
            get
            {
                return _inactiveSnippetIndicatorStyle;
            }
            set
            {
                _inactiveSnippetIndicatorStyle = value;
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


        public bool? IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }


        public bool? IsOneKeySelectionEmbedEnabled
        {
            get
            {
                return _isOneKeySelectionEmbedEnabled;
            }
            set
            {
                _isOneKeySelectionEmbedEnabled = value;
            }
        }

        #endregion Properties
    }
}
