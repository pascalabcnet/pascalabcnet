#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Used to display CallTips and Manages CallTip settings.
    /// </summary>
    /// <remarks>
    ///     CallTips are a special form of ToolTip that can be displayed specifically for
    ///     a document position. It also display a list of method overloads and
    ///     highlighight a portion of the message. This is useful in IDE scenarios.
    /// </remarks>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class CallTip : TopLevelHelper
    {
        #region Fields

        public const char UpArrow = '\u0001';
        public const char DownArrow = '\u0002';

        private int _lastPos = -1;
        private OverloadList _overloadList = null;
        private string _message = null;
        private int _highlightStart = -1;
        private int _highlightEnd = -1;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Hides the calltip
        /// </summary>
        /// <remarks>
        ///     <see cref="Hide"/> and <see cref="Cancel"/> do the same thing
        /// </remarks>
        public void Cancel()
        {
            NativeScintilla.CallTipCancel();
        }


        /// <summary>
        /// Hides the calltip
        /// </summary>
        /// <remarks>
        /// <see cref="Hide"/> and <see cref="Cancel"/> do the same thing
        /// </remarks>
        public void Hide()
        {
            NativeScintilla.CallTipCancel();
        }


        private void ResetBackColor()
        {
            BackColor = SystemColors.Info;
        }


        private void ResetForeColor()
        {
            ForeColor = SystemColors.InfoText;
        }


        private void ResetHighlightEnd()
        {
            _highlightEnd = -1;
        }


        private void ResetHighlightStart()
        {
            _highlightStart = -1;
        }


        private void ResetHighlightTextColor()
        {
            HighlightTextColor = SystemColors.Highlight;
        }


        internal void SetBackColorInternal(Color value)
        {
            if (value == SystemColors.Info)
                Scintilla.ColorBag.Remove("CallTip.BackColor");
            else
                Scintilla.ColorBag["CallTip.BackColor"] = value;

            NativeScintilla.CallTipSetBack(Utilities.ColorToRgb(value));
        }


        internal void SetForeColorInternal(Color value)
        {
            if (value == SystemColors.InfoText)
                Scintilla.ColorBag.Remove("CallTip.ForeColor");
            else
                Scintilla.ColorBag["CallTip.ForeColor"] = value;

            NativeScintilla.CallTipSetFore(Utilities.ColorToRgb(value));
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeBackColor() ||
                ShouldSerializeForeColor() ||
                ShouldSerializeHighlightEnd() ||
                ShouldSerializeHighlightStart() ||
                ShouldSerializeHighlightTextColor();
        }


        private bool ShouldSerializeBackColor()
        {
            return BackColor != SystemColors.Info;
        }


        private bool ShouldSerializeForeColor()
        {
            return ForeColor != SystemColors.InfoText;
        }


        private bool ShouldSerializeHighlightEnd()
        {
            return _highlightEnd >= 0;
        }


        private bool ShouldSerializeHighlightStart()
        {
            return _highlightStart >= 0;
        }


        private bool ShouldSerializeHighlightTextColor()
        {
            return HighlightTextColor != SystemColors.Highlight;
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <remarks>
        ///     The <see cref="Message"/> must already be populated. The calltip will be displayed at the current document position
        ///     with no highlight.
        /// </remarks>
        public void Show()
        {
            Show(_message, -1, -1, -1);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     The <see cref="Message"/> must already be populated. The calltip will be displayed at the current document position
        /// </remarks>
        public void Show(int highlightStart, int highlightEnd)
        {
            Show(_message, -1, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="position">The document position to show the calltip</param>
        /// <remarks>
        ///     The <see cref="Message"/> must already be populated. The calltip with no highlight
        /// </remarks>
        public void Show(int position)
        {
            Show(_message, position, -1, -1);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="position">The document position to show the calltip</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     The <see cref="Message"/> must already be populated.
        /// </remarks>
        public void Show(int position, int highlightStart, int highlightEnd)
        {
            Show(_message, position, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="message">The calltip message to be displayed</param>
        /// <remarks>
        ///     The calltip will be displayed at the current document position with no highlight
        /// </remarks>
        public void Show(string message)
        {
            Show(message, -1, -1, -1);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="message">The calltip message to be displayed</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     The calltip will be displayed at the current document position
        /// </remarks>
        public void Show(string message, int highlightStart, int highlightEnd)
        {
            Show(message, -1, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="message">The calltip message to be displayed</param>
        /// <param name="position">The document position to show the calltip</param>
        /// <remarks>
        ///     The calltip will be displayed with no highlight
        /// </remarks>
        public void Show(string message, int position)
        {
            Show(message, position, -1, -1);
        }


        /// <summary>
        ///     Displays a calltip without overloads
        /// </summary>
        /// <param name="message">The calltip message to be displayed</param>
        /// <param name="position">The document position to show the calltip</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        public void Show(string message, int position, int highlightStart, int highlightEnd)
        {
            _lastPos = position;
            if (position < 0)
                position = NativeScintilla.GetCurrentPos();

            _overloadList = null;
            _message = message;
            NativeScintilla.CallTipShow(position, message);
            HighlightStart = highlightStart;
            HighlightEnd = highlightEnd;
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. It will be displayed at the current document
        ///     position starting at overload 0 with no highlight.
        /// </remarks>
        public void ShowOverload()
        {
            ShowOverload(_overloadList, -1, 0, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. It will be displayed at the current document
        ///     position starting at overload 0
        /// </remarks>
        public void ShowOverload(int highlightStart, int highlightEnd)
        {
            ShowOverload(_overloadList, -1, 0, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. The overload at position 0 will be displayed
        ///     with no highlight.
        /// </remarks>
        public void ShowOverload(int position)
        {
            ShowOverload(_overloadList, position, 0, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. The overload at position 0 will be displayed.
        /// </remarks>
        public void ShowOverload(int position, int highlightStart, int highlightEnd)
        {
            ShowOverload(_overloadList, position, 0, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. It will be displayed at the current document
        ///     position with no highlight
        /// </remarks>
        public void ShowOverload(int position, uint startIndex)
        {
            ShowOverload(_overloadList, position, startIndex, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated.
        /// </remarks>
        public void ShowOverload(int position, uint startIndex, int highlightStart, int highlightEnd)
        {
            ShowOverload(_overloadList, position, startIndex, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The current document position will be used starting at position 0 with no highlight
        /// </remarks>
        public void ShowOverload(OverloadList overloadList)
        {
            ShowOverload(overloadList, -1, 0, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The current document position will be used starting at position 0
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, int highlightStart, int highlightEnd)
        {
            ShowOverload(overloadList, -1, 0, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The overload startIndex will be 0 with no Highlight
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, int position)
        {
            ShowOverload(overloadList, position, 0, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The overload startIndex will be 0
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, int position, int highlightStart, int highlightEnd)
        {
            ShowOverload(overloadList, position, 0, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="position">The document position where the calltip should be displayed</param>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, int position, uint startIndex, int highlightStart, int highlightEnd)
        {
            _lastPos = position;
            _overloadList = overloadList;
            unchecked
            {
                _overloadList.CurrentIndex = (int)startIndex;
            }
            _highlightEnd = highlightEnd;
            _highlightStart = highlightStart;
            ShowOverloadInternal();
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The current document position will be used with no highlight
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, uint startIndex)
        {
            ShowOverload(overloadList, -1, startIndex, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="overloadList">List of overloads to be displayed see <see cref="OverLoadList"/></param>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The current document position will be used
        /// </remarks>
        public void ShowOverload(OverloadList overloadList, uint startIndex, int highlightStart, int highlightEnd)
        {
            ShowOverload(overloadList, -1, startIndex, highlightStart, highlightEnd);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. It will be displayed at the current document
        ///     position with no highlight.
        /// </remarks>
        public void ShowOverload(uint startIndex)
        {
            ShowOverload(_overloadList, -1, startIndex, -1, -1);
        }


        /// <summary>
        ///     Shows the calltip with overloads
        /// </summary>
        /// <param name="startIndex">The index of the initial overload to display</param>
        /// <param name="highlightStart">Start posision of the part of the message that should be selected</param>
        /// <param name="highlightEnd">End posision of the part of the message that should be selected</param>
        /// <remarks>
        ///     ShowOverload automatically handles displaying a calltip with a list of overloads. It automatically shows the
        ///     up and down arrows and cycles through the list of overloads in response to mouse clicks.
        ///     The <see cref="OverLoadList"/> must already be populated. It will be displayed at the current document
        ///     position.
        /// </remarks>
        public void ShowOverload(uint startIndex, int highlightStart, int highlightEnd)
        {
            ShowOverload(_overloadList, -1, startIndex, highlightStart, highlightEnd);
        }


        internal void ShowOverloadInternal()
        {
            int pos = _lastPos;
            if (pos < 0)
                pos = NativeScintilla.GetCurrentPos();

            // Scintilla's way of handling "1 of n" type calltips
            string prefix = string.Format(CultureInfo.CurrentUICulture, "\u0001 {0} of {1} \u0002 ", _overloadList.CurrentIndex + 1, _overloadList.Count);

            // Show me the money
            NativeScintilla.CallTipCancel();
            NativeScintilla.CallTipShow(pos, prefix + _overloadList.Current);
            NativeScintilla.CallTipSetHlt(_highlightStart + prefix.Length, _highlightEnd + prefix.Length); // Adjust the highlight offsets
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets/Sets the background color of all CallTips
        /// </summary>
        public Color BackColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey("CallTip.BackColor"))
                    return Scintilla.ColorBag["CallTip.BackColor"];

                return SystemColors.Info;
            }
            set
            {
                SetBackColorInternal(value);

                Scintilla.Styles.CallTip.SetBackColorInternal(value);
            }
        }


        /// <summary>
        ///     Gets/Sets Text color of all CallTips
        /// </summary>
        public Color ForeColor
        {
            get
            {
                if (Scintilla.ColorBag.ContainsKey("CallTip.ForeColor"))
                    return Scintilla.ColorBag["CallTip.ForeColor"];

                return SystemColors.InfoText;
            }
            set
            {
                SetForeColorInternal(value);

                Scintilla.Styles.CallTip.SetForeColorInternal(value);
            }
        }


        /// <summary>
        ///     End position of the text to be highlighted in the CalTip
        /// </summary>
        public int HighlightEnd
        {
            get
            {
                return _highlightEnd;
            }
            set
            {
                _highlightEnd = value;
                NativeScintilla.CallTipSetHlt(_highlightStart, _highlightEnd);
            }
        }


        /// <summary>
        ///     Start position of the text to be highlighted in the CalTip
        /// </summary>
        public int HighlightStart
        {
            get
            {
                return _highlightStart;
            }
            set
            {
                _highlightStart = value;
                NativeScintilla.CallTipSetHlt(_highlightStart, _highlightEnd);
            }
        }


        /// <summary>
        ///     Gets/Sets the Text Color of the portion of the CallTip that is highlighted
        /// </summary>
        public Color HighlightTextColor
        {
            //	Note the default Color of this is SystemColors.Highlight, instead
            //	of HighlightText, which one would normally think. However since
            //	there is no Contrasting HighlightBackColor a light Highlight Color
            //	on the light InfoTip background is nearly impossible to see.
            get
            {
                if (Scintilla.ColorBag.ContainsKey("CallTip.HighlightTextColor"))
                    return Scintilla.ColorBag["CallTip.HighlightTextColor"];

                return SystemColors.Highlight;
            }
            set
            {
                if (value == SystemColors.Highlight)
                    Scintilla.ColorBag.Remove("CallTip.HighlightTextColor");
                else
                    Scintilla.ColorBag["CallTip.HighlightTextColor"] = value;

                NativeScintilla.CallTipSetForeHlt(Utilities.ColorToRgb(value));
            }
        }


        /// <summary>
        ///     Returns true if a CallTip is currently displayed
        /// </summary>
        public bool IsActive
        {
            get
            {
                return NativeScintilla.CallTipActive();
            }
        }


        /// <summary>
        ///     The message displayed in the calltip
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }


        /// <summary>
        ///     List of method overloads to display in the calltip
        /// </summary>
        /// <remarks>
        ///     This is used to display IDE type toolips that include Up/Down arrows that cycle
        ///     through the list of overloads when clicked
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public OverloadList OverloadList
        {
            get
            {
                return _overloadList;
            }
            set
            {
                _overloadList = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal CallTip(Scintilla scintilla) : base(scintilla)
        {
            //	Go ahead and enable this. It's all pretty idiosyncratic IMO. For one
            //	thing you can't turn it off. We set the CallTip styles by default
            //	anyhow.
            NativeScintilla.CallTipUseStyle(10);
            Scintilla.BeginInvoke(new MethodInvoker(delegate()
            {
                HighlightTextColor = HighlightTextColor;
                ForeColor = ForeColor;
                BackColor = BackColor;
            }));
        }

        #endregion Constructors
    }
}
