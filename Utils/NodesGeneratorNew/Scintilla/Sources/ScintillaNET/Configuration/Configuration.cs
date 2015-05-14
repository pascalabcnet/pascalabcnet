#region Using Directives

using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

#endregion Using Directives


namespace ScintillaNET.Configuration
{
    public class Configuration
    {
        #region Fields

        private bool? _autoComplete_AutoHide;
        private bool? _autoComplete_AutomaticLengthEntered;
        private bool? _autoComplete_cancelAtStart;
        private bool? _autoComplete_DropRestOfWord;
        private string _autoComplete_fillUpCharacters;
        private char? _autoComplete_ImageSeperator;
        private bool? _autoComplete_IsCaseSensitive;
        private string _autoComplete_List;
        private bool? _autoComplete_ListInherit;
        private char? _autoComplete_ListSeperator;
        private int? _autoComplete_MaxHeight;
        private int? _autoComplete_MaxWidth;
        private bool? _autoComplete_singleLineAccept;
        private string _autoComplete_StopCharacters;
        private Color _callTip_BackColor;
        private Color _callTip_ForeColor;
        private Color _callTip_HighlightTextColor;
        private int? _caret_BlinkRate;
        private Color _caret_Color;
        private int? _caret_CurrentLineBackgroundAlpha;
        private Color _caret_CurrentLineBackgroundColor;
        private bool? _caret_HighlightCurrentLine;
        private bool? _caret_IsSticky;
        private CaretStyle? _caret_Style;
        private int? _caret_Width;
        private bool? _clipboard_ConvertLineBreaksOnPaste;
        private CommandBindingConfigList _commands_KeyBindingList = new CommandBindingConfigList();
        private string _dropMarkers_SharedStackName;
        private bool? _endOfLine_IsVisisble;
        private EndOfLineMode? _endOfLine_Mode;
        private FoldFlag? _folding_Flags;
        private bool? _folding_IsEnabled;
        private FoldMarkerScheme? _folding_MarkerScheme;
        private bool? _folding_UseCompactFolding;
        private bool _hasData = false;
        private Color _hotspot_ActiveBackColor;
        private Color _hotspot_ActiveForeColor;
        private bool? _hotspot_ActiveUnderline;
        private bool? _hotspot_SingleLine;
        private bool? _hotspot_UseActiveBackColor;
        private bool? _hotspot_UseActiveForeColor;
        private bool? _indentation_BackspaceUnindents;
        private int? _indentation_IndentWidth;
        private bool? _indentation_ShowGuides;
        private SmartIndent? _indentation_SmartIndentType;
        private bool? _indentation_TabIndents;
        private int? _indentation_TabWidth;
        private bool? _indentation_UseTabs;
        private IndicatorConfigList _indicator_List = new IndicatorConfigList();
        private string _language;
        private KeyWordConfigList _lexing_Keywords = new KeyWordConfigList();
        private string _lexing_Language;
        private string _lexing_LineCommentPrefix;
        private LexerPropertiesConfig _lexing_Properties = new LexerPropertiesConfig();
        private string _lexing_StreamCommentPrefix;
        private string _lexing_StreamCommentSuffix;
        private string _lexing_WhitespaceChars;
        private string _lexing_WordChars;
        private LineWrappingMode? _lineWrapping_Mode;
        private LineWrappingIndentMode? _lineWrapping_IndentMode;
        private int? _lineWrapping_IndentSize;
        private LineWrappingVisualFlags? _lineWrapping_VisualFlags;
        private LineWrappingVisualFlagsLocations? _lineWrapping_VisualFlagsLocations;
        private Color _longLines_EdgeColor;
        private int? _longLines_EdgeColumn;
        private EdgeMode? _longLines_EdgeMode;
        private MarginConfigList _margin_List = new MarginConfigList();
        private MarkersConfigList _markers_List;
        private bool? _scrolling_EndAtLastLine;
        private int? _scrolling_HorizontalWidth;
        private ScrollBars? _scrolling_ScrollBars;
        private int? _scrolling_XOffset;
        private Color _selection_BackColor;
        private Color _selection_BackColorUnfocused;
        private Color _selection_ForeColor;
        private Color _selection_ForeColorUnfocused;
        private bool? _selection_Hidden;
        private bool? _selection_HideSelection;
        private SelectionMode? _selection_Mode;
        private SnippetsConfigList _snippetsConfigList = new SnippetsConfigList();
        private StyleConfigList _styles = new StyleConfigList();
        private bool? _undoRedoIsUndoEnabled;
        private Color _whitespace_BackColor;
        private Color _whitespace_ForeColor;
        private WhitespaceMode? _whitespace_Mode;

        #endregion Fields


        #region Methods

        private bool? getBool(string s)
        {
            s = s.ToLower();

            switch (s)
            {
                case "true":
                case "t":
                case "1":
                case "y":
                case "yes":
                    return true;
                case "false":
                case "f":
                case "0":
                case "n":
                case "no":
                    return false;
            }

            return null;
        }


        private char? getChar(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return s[0];
        }


        private Color getColor(string s)
        {
            return (Color)new ColorConverter().ConvertFromString(s);
        }


        private int? getInt(string s)
        {
            int i;
            if (int.TryParse(s, out i))
                return i;

            return null;
        }


        private string getString(XmlAttribute a)
        {
            if (a == null)
                return null;

            return a.Value;
        }


        private StyleConfig getStyleConfigFromElement(XmlReader reader)
        {
            StyleConfig sc = new StyleConfig();
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "name":
                            sc.Name = reader.Value;
                            break;
                        case "number":
                            sc.Number = getInt(reader.Value);
                            break;
                        case "backcolor":
                            sc.BackColor = getColor(reader.Value);
                            break;
                        case "bold":
                            sc.Bold = getBool(reader.Value);
                            break;
                        case "case":
                            sc.Case = (StyleCase)Enum.Parse(typeof(StyleCase), reader.Value, true);
                            break;
                        case "characterset":
                            sc.CharacterSet = (CharacterSet)Enum.Parse(typeof(CharacterSet), reader.Value, true);
                            break;
                        case "fontname":
                            sc.FontName = reader.Value;
                            break;
                        case "forecolor":
                            sc.ForeColor = getColor(reader.Value);
                            break;
                        case "ischangeable":
                            sc.IsChangeable = getBool(reader.Value);
                            break;
                        case "ishotspot":
                            sc.IsHotspot = getBool(reader.Value);
                            break;
                        case "isselectioneolfilled":
                            sc.IsSelectionEolFilled = getBool(reader.Value);
                            break;
                        case "isvisible":
                            sc.IsVisible = getBool(reader.Value);
                            break;
                        case "italic":
                            sc.Italic = getBool(reader.Value);
                            break;
                        case "size":
                            sc.Size = getInt(reader.Value);
                            break;
                        case "underline":
                            sc.Underline = getBool(reader.Value);
                            break;
                        case "inherit":
                            sc.Inherit = getBool(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }

            return sc;
        }


        public void Load(TextReader txtReader)
        {
            XmlDocument configDocument = new XmlDocument();
            configDocument.PreserveWhitespace = true;
            configDocument.Load(txtReader);
            Load(configDocument);
        }


        public void Load(XmlReader reader)
        {
            reader.ReadStartElement();
            

            while (!reader.EOF)
            {
                if (reader.Name.Equals("language", StringComparison.OrdinalIgnoreCase) && reader.HasAttributes)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name.Equals("name", StringComparison.OrdinalIgnoreCase) && reader.Value.Equals(_language, StringComparison.OrdinalIgnoreCase))
                        {
                            ReadLanguage(reader);
                            _hasData = true;
                        }
                    }

                    if (_hasData)
                        reader.Skip();
                }
                else
                {
                    reader.Skip();
                    continue;
                }

                
                reader.Read();
            }

            reader.Close();
        }


        public void Load(XmlDocument configDocument)
        {
            XmlElement langNode = configDocument.DocumentElement.SelectSingleNode("./Language[@Name='" + _language + "']") as XmlElement;
            if (langNode == null)
                return;

            XmlElement autoCNode = langNode.SelectSingleNode("AutoComplete") as XmlElement;
            if (autoCNode != null)
            {
                _autoComplete_AutoHide = getBool(autoCNode.GetAttribute("AutoHide"));
                _autoComplete_AutomaticLengthEntered = getBool(autoCNode.GetAttribute("AutomaticLengthEntered"));
                _autoComplete_cancelAtStart = getBool(autoCNode.GetAttribute("CancelAtStart"));
                _autoComplete_DropRestOfWord = getBool(autoCNode.GetAttribute("DropRestOfWord"));
                _autoComplete_fillUpCharacters = getString(autoCNode.GetAttributeNode("FillUpCharacters"));
                _autoComplete_ImageSeperator = getChar(autoCNode.GetAttribute("AutomaticLengthEntered"));
                _autoComplete_IsCaseSensitive = getBool(autoCNode.GetAttribute("IsCaseSensitive"));
                _autoComplete_ListSeperator = getChar(autoCNode.GetAttribute("ListSeperator"));
                _autoComplete_MaxHeight = getInt(autoCNode.GetAttribute("MaxHeight"));
                _autoComplete_MaxWidth = getInt(autoCNode.GetAttribute("MaxWidth"));
                _autoComplete_singleLineAccept = getBool(autoCNode.GetAttribute("SingleLineAccept"));
                _autoComplete_StopCharacters = getString(autoCNode.GetAttributeNode("StopCharacters"));

                XmlElement listNode = autoCNode.SelectSingleNode("./List") as XmlElement;
                if (listNode != null)
                {
                    _autoComplete_ListInherit = getBool(listNode.GetAttribute("Inherit"));
                    _autoComplete_List = new Regex("\\s+").Replace(listNode.InnerText, " ").Trim();

                }
            }
            autoCNode = null;

            XmlElement callTipNode = langNode.SelectSingleNode("CallTip") as XmlElement;
            if (callTipNode != null)
            {
                _callTip_BackColor = getColor(callTipNode.GetAttribute("BackColor"));
                _callTip_ForeColor = getColor(callTipNode.GetAttribute("ForeColor"));
                _callTip_HighlightTextColor = getColor(callTipNode.GetAttribute("HighlightTextColor"));
            }
            callTipNode = null;

            XmlElement caretNode = langNode.SelectSingleNode("Caret") as XmlElement;
            if (caretNode != null)
            {
                //	This guy is a bit of an oddball becuase null means "I don't Care"
                //	and we need some way of using the OS value.
                string blinkRate = caretNode.GetAttribute("BlinkRate");
                if (blinkRate.ToLower() == "system")
                    _caret_BlinkRate = SystemInformation.CaretBlinkTime;
                else
                    _caret_BlinkRate = getInt(blinkRate);

                _caret_Color = getColor(caretNode.GetAttribute("Color"));
                _caret_CurrentLineBackgroundAlpha = getInt(caretNode.GetAttribute("CurrentLineBackgroundAlpha"));
                _caret_CurrentLineBackgroundColor = getColor(caretNode.GetAttribute("CurrentLineBackgroundColor"));
                _caret_HighlightCurrentLine = getBool(caretNode.GetAttribute("HighlightCurrentLine"));
                _caret_IsSticky = getBool(caretNode.GetAttribute("IsSticky"));
                try
                {
                    _caret_Style = (CaretStyle)Enum.Parse(typeof(CaretStyle), caretNode.GetAttribute("Style"), true);
                }
                catch (ArgumentException) { }
                _caret_Width = getInt(caretNode.GetAttribute("Width"));
            }
            caretNode = null;

            XmlElement clipboardNode = langNode.SelectSingleNode("Clipboard") as XmlElement;
            if (clipboardNode != null)
            {
                _clipboard_ConvertLineBreaksOnPaste = getBool(clipboardNode.GetAttribute("ConvertLineBreaksOnPaste"));
            }
            clipboardNode = null;

            _commands_KeyBindingList = new CommandBindingConfigList();
            XmlElement commandsNode = langNode.SelectSingleNode("Commands") as XmlElement;
            if (commandsNode != null)
            {
                _commands_KeyBindingList.Inherit = getBool(commandsNode.GetAttribute("Inherit"));
                _commands_KeyBindingList.AllowDuplicateBindings = getBool(commandsNode.GetAttribute("AllowDuplicateBindings"));
                foreach (XmlElement el in commandsNode.SelectNodes("./Binding"))
                {
                    KeyBinding kb = new KeyBinding();
                    kb.KeyCode = Utilities.GetKeys(el.GetAttribute("Key"));

                    string modifiers = el.GetAttribute("Modifier");
                    if (modifiers != string.Empty)
                    {
                        foreach (string modifier in modifiers.Split(' '))
                            kb.Modifiers |= (Keys)Enum.Parse(typeof(Keys), modifier.Trim(), true);
                    }

                    BindableCommand cmd = (BindableCommand)Enum.Parse(typeof(BindableCommand), el.GetAttribute("Command"), true);
                    CommandBindingConfig cfg = new CommandBindingConfig(kb, getBool(el.GetAttribute("ReplaceCurrent")), cmd);
                    _commands_KeyBindingList.Add(cfg);
                }
            }
            commandsNode = null;

            XmlElement endOfLineNode = langNode.SelectSingleNode("EndOfLine") as XmlElement;
            if (endOfLineNode != null)
            {
                _endOfLine_IsVisisble = getBool(endOfLineNode.GetAttribute("IsVisible"));

                try
                {
                    _endOfLine_Mode = (EndOfLineMode)Enum.Parse(typeof(EndOfLineMode), endOfLineNode.GetAttribute("Mode"), true);
                }
                catch (ArgumentException) { }
            }
            endOfLineNode = null;

            XmlElement foldingNode = langNode.SelectSingleNode("Folding") as XmlElement;
            if (foldingNode != null)
            {
                string flags = foldingNode.GetAttribute("Flags").Trim();
                if (flags != string.Empty)
                {
                    FoldFlag? ff = null;
                    foreach (string flag in flags.Split(' '))
                        ff |= (FoldFlag)Enum.Parse(typeof(FoldFlag), flag.Trim(), true);

                    if (ff.HasValue)
                        _folding_Flags = ff;
                }

                _folding_IsEnabled = getBool(foldingNode.GetAttribute("IsEnabled"));
                try
                {
                    _folding_MarkerScheme = (FoldMarkerScheme)Enum.Parse(typeof(FoldMarkerScheme), foldingNode.GetAttribute("MarkerScheme"), true);
                }
                catch (ArgumentException) { }

                _folding_UseCompactFolding = getBool(foldingNode.GetAttribute("UseCompactFolding"));
            }
            foldingNode = null;

            XmlElement hotSpotNode = langNode.SelectSingleNode("Hotspot") as XmlElement;
            if (hotSpotNode != null)
            {
                _hotspot_ActiveBackColor = getColor(hotSpotNode.GetAttribute("ActiveBackColor"));
                _hotspot_ActiveForeColor = getColor(hotSpotNode.GetAttribute("ActiveForeColor"));
                _hotspot_ActiveUnderline = getBool(hotSpotNode.GetAttribute("ActiveUnderline"));
                _hotspot_SingleLine = getBool(hotSpotNode.GetAttribute("SingleLine"));
                _hotspot_UseActiveBackColor = getBool(hotSpotNode.GetAttribute("UseActiveBackColor"));
                _hotspot_UseActiveForeColor = getBool(hotSpotNode.GetAttribute("UseActiveForeColor"));
            }
            hotSpotNode = null;

            XmlElement indentationNode = langNode.SelectSingleNode("Indentation") as XmlElement;
            if (indentationNode != null)
            {
                _indentation_BackspaceUnindents = getBool(indentationNode.GetAttribute("BackspaceUnindents"));
                _indentation_IndentWidth = getInt(indentationNode.GetAttribute("IndentWidth"));
                _indentation_ShowGuides = getBool(indentationNode.GetAttribute("ShowGuides"));
                _indentation_TabIndents = getBool(indentationNode.GetAttribute("TabIndents"));
                _indentation_TabWidth = getInt(indentationNode.GetAttribute("TabWidth"));
                _indentation_UseTabs = getBool(indentationNode.GetAttribute("UseTabs"));

                try
                {
                    _indentation_SmartIndentType = (SmartIndent)Enum.Parse(typeof(SmartIndent), indentationNode.GetAttribute("SmartIndentType"), true);
                }
                catch (ArgumentException) { }

            }
            indentationNode = null;

            XmlElement indicatorNode = langNode.SelectSingleNode("Indicators") as XmlElement;
            if (indicatorNode != null)
            {
                _indicator_List.Inherit = getBool(indicatorNode.GetAttribute("Inherit"));
                foreach (XmlElement el in indicatorNode.SelectNodes("Indicator"))
                {
                    IndicatorConfig ic = new IndicatorConfig();
                    ic.Number = int.Parse(el.GetAttribute("Number"));
                    ic.Color = getColor(el.GetAttribute("Color"));
                    ic.Inherit = getBool(el.GetAttribute("Inherit"));
                    ic.IsDrawnUnder = getBool(el.GetAttribute("IsDrawnUnder"));
                    try
                    {
                        ic.Style = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), el.GetAttribute("Style"), true);
                    }
                    catch (ArgumentException) { }

                    _indicator_List.Add(ic);
                }
            }

            _lexing_Properties = new LexerPropertiesConfig();
            _lexing_Keywords = new KeyWordConfigList();
            XmlElement lexerNode = langNode.SelectSingleNode("Lexer") as XmlElement;
            if (lexerNode != null)
            {
                _lexing_WhitespaceChars = getString(lexerNode.GetAttributeNode("WhitespaceChars"));
                _lexing_WordChars = getString(lexerNode.GetAttributeNode("WordChars"));
                _lexing_Language = getString(lexerNode.GetAttributeNode("LexerName"));
                _lexing_LineCommentPrefix = getString(lexerNode.GetAttributeNode("LineCommentPrefix"));
                _lexing_StreamCommentPrefix = getString(lexerNode.GetAttributeNode("StreamCommentPrefix"));
                _lexing_StreamCommentSuffix = getString(lexerNode.GetAttributeNode("StreamCommentSuffix"));

                XmlElement propNode = lexerNode.SelectSingleNode("Properties") as XmlElement;
                if (propNode != null)
                {
                    _lexing_Properties.Inherit = getBool(propNode.GetAttribute("Inherit"));

                    foreach (XmlElement el in propNode.SelectNodes("Property"))
                        _lexing_Properties.Add(el.GetAttribute("Name"), el.GetAttribute("Value"));
                }

                foreach (XmlElement el in lexerNode.SelectNodes("Keywords"))
                    _lexing_Keywords.Add(new KeyWordConfig(getInt(el.GetAttribute("List")).Value, el.InnerText.Trim(), getBool(el.GetAttribute("Inherit"))));

            }
            lexerNode = null;

            XmlElement lineWrapNode = langNode.SelectSingleNode("LineWrapping") as XmlElement;
            if (lineWrapNode != null)
            {
                try
                {
                    _lineWrapping_Mode = (LineWrappingMode)Enum.Parse(typeof(LineWrappingMode), lineWrapNode.GetAttribute("Mode"), true);
                }
                catch (ArgumentException) { }

                _lineWrapping_IndentSize = getInt(lineWrapNode.GetAttribute("IndentSize"));

                try
                {
                    _lineWrapping_IndentMode = (LineWrappingIndentMode)Enum.Parse(typeof(LineWrappingIndentMode), lineWrapNode.GetAttribute("IndentMode"), true);
                }
                catch (ArgumentException) { }

                string flags = lineWrapNode.GetAttribute("VisualFlags").Trim();
                if (flags != string.Empty)
                {
                    LineWrappingVisualFlags? wvf = null;
                    foreach (string flag in flags.Split(' '))
                        wvf |= (LineWrappingVisualFlags)Enum.Parse(typeof(LineWrappingVisualFlags), flag.Trim(), true);

                    if (wvf.HasValue)
                        _lineWrapping_VisualFlags = wvf;
                }

                try
                {
                    _lineWrapping_VisualFlagsLocations = (LineWrappingVisualFlagsLocations)Enum.Parse(typeof(LineWrappingVisualFlagsLocations), lineWrapNode.GetAttribute("VisualFlagsLocations"), true);
                }
                catch (ArgumentException) { }
            }
            lineWrapNode = null;

            XmlElement longLinesNode = langNode.SelectSingleNode("LongLines") as XmlElement;
            if (longLinesNode != null)
            {
                _longLines_EdgeColor = getColor(longLinesNode.GetAttribute("EdgeColor"));
                _longLines_EdgeColumn = getInt(longLinesNode.GetAttribute("EdgeColumn"));
                try
                {
                    _longLines_EdgeMode = (EdgeMode)Enum.Parse(typeof(EdgeMode), longLinesNode.GetAttribute("EdgeMode"), true);
                }
                catch (ArgumentException) { }
            }
            longLinesNode = null;

            _margin_List = new MarginConfigList();
            XmlElement marginNode = langNode.SelectSingleNode("Margins") as XmlElement;
            if (marginNode != null)
            {
                _margin_List.FoldMarginColor = getColor(marginNode.GetAttribute("FoldMarginColor"));
                _margin_List.FoldMarginHighlightColor = getColor(marginNode.GetAttribute("FoldMarginHighlightColor"));
                _margin_List.Left = getInt(marginNode.GetAttribute("Left"));
                _margin_List.Right = getInt(marginNode.GetAttribute("Right"));
                _margin_List.Inherit = getBool(marginNode.GetAttribute("Inherit"));

                foreach (XmlElement el in marginNode.SelectNodes("./Margin"))
                {
                    MarginConfig mc = new MarginConfig();
                    mc.Number = int.Parse(el.GetAttribute("Number"));
                    mc.Inherit = getBool(el.GetAttribute("Inherit"));
                    mc.AutoToggleMarkerNumber = getInt(el.GetAttribute("AutoToggleMarkerNumber"));
                    mc.IsClickable = getBool(el.GetAttribute("IsClickable"));
                    mc.IsFoldMargin = getBool(el.GetAttribute("IsFoldMargin"));
                    mc.IsMarkerMargin = getBool(el.GetAttribute("IsMarkerMargin"));
                    try
                    {
                        mc.Type = (MarginType)Enum.Parse(typeof(MarginType), el.GetAttribute("Type"), true);
                    }
                    catch (ArgumentException) { }

                    mc.Width = getInt(el.GetAttribute("Width"));

                    _margin_List.Add(mc);
                }
            }
            marginNode = null;

            XmlElement markersNode = langNode.SelectSingleNode("Markers") as XmlElement;
            _markers_List = new MarkersConfigList();
            if (markersNode != null)
            {
                _markers_List.Inherit = getBool(markersNode.GetAttribute("Inherit"));

                foreach (XmlElement el in markersNode.SelectNodes("Marker"))
                {
                    MarkersConfig mc = new MarkersConfig();
                    mc.Alpha = getInt(el.GetAttribute("Alpha"));
                    mc.BackColor = getColor(el.GetAttribute("BackColor"));
                    mc.ForeColor = getColor(el.GetAttribute("ForeColor"));
                    mc.Name = getString(el.GetAttributeNode("Name"));
                    mc.Number = getInt(el.GetAttribute("Number"));
                    mc.Inherit = getBool(el.GetAttribute("Inherit"));
                    try
                    {
                        mc.Symbol = (MarkerSymbol)Enum.Parse(typeof(MarkerSymbol), el.GetAttribute("Symbol"), true);
                    }
                    catch (ArgumentException) { }
                    _markers_List.Add(mc);
                }
            }

            XmlElement scrollingNode = langNode.SelectSingleNode("Scrolling") as XmlElement;
            if (scrollingNode != null)
            {
                _scrolling_EndAtLastLine = getBool(scrollingNode.GetAttribute("EndAtLastLine"));
                _scrolling_HorizontalWidth = getInt(scrollingNode.GetAttribute("HorizontalWidth"));

                string flags = scrollingNode.GetAttribute("ScrollBars").Trim();
                if (flags != string.Empty)
                {
                    ScrollBars? sb = null;
                    foreach (string flag in flags.Split(' '))
                        sb |= (ScrollBars)Enum.Parse(typeof(ScrollBars), flag.Trim(), true);

                    if (sb.HasValue)
                        _scrolling_ScrollBars = sb;
                }

                _scrolling_XOffset = getInt(scrollingNode.GetAttribute("XOffset"));
            }
            scrollingNode = null;


            XmlElement selectionNode = langNode.SelectSingleNode("Selection") as XmlElement;
            if (selectionNode != null)
            {
                _selection_BackColor = getColor(selectionNode.GetAttribute("BackColor"));
                _selection_BackColorUnfocused = getColor(selectionNode.GetAttribute("BackColorUnfocused"));
                _selection_ForeColor = getColor(selectionNode.GetAttribute("ForeColor"));
                _selection_ForeColorUnfocused = getColor(selectionNode.GetAttribute("ForeColorUnfocused"));
                _selection_Hidden = getBool(selectionNode.GetAttribute("Hidden"));
                _selection_HideSelection = getBool(selectionNode.GetAttribute("HideSelection"));
                try
                {
                    _selection_Mode = (SelectionMode)Enum.Parse(typeof(SelectionMode), selectionNode.GetAttribute("Mode"), true);
                }
                catch (ArgumentException) { }
            }
            selectionNode = null;

            _snippetsConfigList = new SnippetsConfigList();
            XmlElement snippetsNode = langNode.SelectSingleNode("Snippets") as XmlElement;
            if (snippetsNode != null)
            {
                _snippetsConfigList.ActiveSnippetColor = getColor(snippetsNode.GetAttribute("ActiveSnippetColor"));
                _snippetsConfigList.ActiveSnippetIndicator = getInt(snippetsNode.GetAttribute("ActiveSnippetIndicator"));
                _snippetsConfigList.InactiveSnippetColor = getColor(snippetsNode.GetAttribute("InactiveSnippetColor"));
                _snippetsConfigList.InactiveSnippetIndicator = getInt(snippetsNode.GetAttribute("InactiveSnippetIndicator"));

                try
                {
                    _snippetsConfigList.ActiveSnippetIndicatorStyle = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), snippetsNode.GetAttribute("ActiveSnippetIndicatorStyle"), true);
                }
                catch (ArgumentException) { }

                try
                {
                    _snippetsConfigList.InactiveSnippetIndicatorStyle = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), snippetsNode.GetAttribute("InactiveSnippetIndicatorStyle"), true);
                }
                catch (ArgumentException) { }

                _snippetsConfigList.DefaultDelimeter = getChar(snippetsNode.GetAttribute("DefaultDelimeter"));
                _snippetsConfigList.IsEnabled = getBool(snippetsNode.GetAttribute("IsEnabled"));
                _snippetsConfigList.IsOneKeySelectionEmbedEnabled = getBool(snippetsNode.GetAttribute("IsOneKeySelectionEmbedEnabled"));

                foreach (XmlElement el in snippetsNode.SelectNodes("Snippet"))
                {
                    SnippetsConfig sc = new SnippetsConfig();
                    sc.Shortcut = el.GetAttribute("Shortcut");
                    sc.Code = el.InnerText;
                    sc.Delimeter = getChar(el.GetAttribute("Delimeter"));
                    sc.IsSurroundsWith = getBool(el.GetAttribute("IsSurroundsWith"));
                    _snippetsConfigList.Add(sc);
                }
            }
            snippetsNode = null;

            _styles = new StyleConfigList();
            XmlElement stylesNode = langNode.SelectSingleNode("Styles") as XmlElement;
            if (stylesNode != null)
            {
                _styles.Bits = getInt(stylesNode.GetAttribute("Bits"));
                foreach (XmlElement el in stylesNode.SelectNodes("Style"))
                {
                    StyleConfig sc = new StyleConfig();
                    sc.Name = el.GetAttribute("Name");
                    sc.Number = getInt(el.GetAttribute("Number"));
                    sc.BackColor = getColor(el.GetAttribute("BackColor"));
                    sc.Bold = getBool(el.GetAttribute("Bold"));
                    try
                    {
                        sc.Case = (StyleCase)Enum.Parse(typeof(StyleCase), el.GetAttribute("Case"), true);
                    }
                    catch (ArgumentException) { }

                    try
                    {
                        sc.CharacterSet = (CharacterSet)Enum.Parse(typeof(CharacterSet), el.GetAttribute("CharacterSet"), true);
                    }
                    catch (ArgumentException) { }

                    sc.FontName = getString(el.GetAttributeNode("FontName"));
                    sc.ForeColor = getColor(el.GetAttribute("ForeColor"));
                    sc.IsChangeable = getBool(el.GetAttribute("IsChangeable"));
                    sc.IsHotspot = getBool(el.GetAttribute("IsHotspot"));
                    sc.IsSelectionEolFilled = getBool(el.GetAttribute("IsSelectionEolFilled"));
                    sc.IsVisible = getBool(el.GetAttribute("IsVisible"));
                    sc.Italic = getBool(el.GetAttribute("Italic"));
                    sc.Size = getInt(el.GetAttribute("Size"));
                    sc.Underline = getBool(el.GetAttribute("Underline"));
                    sc.Inherit = getBool(el.GetAttribute("Inherit"));
                    
                    _styles.Add(sc);
                }

                //	This is a nifty added on hack made specifically for HTML.
                //	Normally the style config elements are quite managable as there
                //	are typically less than 10 when you don't count common styles.
                //	
                //	However HTML uses 9 different Sub languages that combined make 
                //	use of all 128 styles (well there are some small gaps). In order
                //	to make this more managable I did added a SubLanguage element that
                //	basically just prepends the Language's name and "." to the Style 
                //	Name definition.
                //
                //	So for example if you had the following
                //	<Styles>
                //		<SubLanguage Name="ASP JavaScript">
                //			<Style Name="Keyword" Bold="True" />
                //		</SubLanguage>
                //	</Styles>
                //	That style's name will get interpreted as "ASP JavaScript.Keyword".
                //	which if you look at the html.txt in LexerStyleNames you'll see it
                //	maps to Style # 62

                //	Yeah I copied and pasted from above. I know. Feel free to refactor
                //	this and check it in since you're so high and mighty.
                foreach (XmlElement subLanguage in stylesNode.SelectNodes("SubLanguage"))
                {
                    string subLanguageName = subLanguage.GetAttribute("Name");
                    foreach (XmlElement el in subLanguage.SelectNodes("Style"))
                    {
                        StyleConfig sc = new StyleConfig();
                        sc.Name = subLanguageName + "." + el.GetAttribute("Name");
                        sc.Number = getInt(el.GetAttribute("Number"));
                        sc.BackColor = getColor(el.GetAttribute("BackColor"));
                        sc.Bold = getBool(el.GetAttribute("Bold"));
                        try
                        {
                            sc.Case = (StyleCase)Enum.Parse(typeof(StyleCase), el.GetAttribute("Case"), true);
                        }
                        catch (ArgumentException) { }

                        try
                        {
                            sc.CharacterSet = (CharacterSet)Enum.Parse(typeof(CharacterSet), el.GetAttribute("CharacterSet"), true);
                        }
                        catch (ArgumentException) { }

                        sc.FontName = getString(el.GetAttributeNode("FontName"));
                        sc.ForeColor = getColor(el.GetAttribute("ForeColor"));
                        sc.IsChangeable = getBool(el.GetAttribute("IsChangeable"));
                        sc.IsHotspot = getBool(el.GetAttribute("IsHotspot"));
                        sc.IsSelectionEolFilled = getBool(el.GetAttribute("IsSelectionEolFilled"));
                        sc.IsVisible = getBool(el.GetAttribute("IsVisible"));
                        sc.Italic = getBool(el.GetAttribute("Italic"));
                        sc.Size = getInt(el.GetAttribute("Size"));
                        sc.Underline = getBool(el.GetAttribute("Underline"));
                        sc.Inherit = getBool(el.GetAttribute("Inherit"));

                        _styles.Add(sc);
                    }
                }
            }
            stylesNode = null;

            XmlElement undoRedoNode = langNode.SelectSingleNode("UndoRedo") as XmlElement;
            if (undoRedoNode != null)
            {
                _undoRedoIsUndoEnabled = getBool(undoRedoNode.GetAttribute("IsUndoEnabled"));
            }
            undoRedoNode = null;


            XmlElement whitespaceNode = langNode.SelectSingleNode("Whitespace") as XmlElement;
            if (whitespaceNode != null)
            {
                _whitespace_BackColor = getColor(whitespaceNode.GetAttribute("BackColor"));
                _whitespace_ForeColor = getColor(whitespaceNode.GetAttribute("ForeColor"));
                _whitespace_Mode = (WhitespaceMode)Enum.Parse(typeof(WhitespaceMode), whitespaceNode.GetAttribute("Mode"), true);
            }
            whitespaceNode = null;

            configDocument = null;
        }


        public void Load(string fileName, bool useXmlReader)
        {
            if (useXmlReader)
            {
                XmlReaderSettings s = new XmlReaderSettings();
                s.IgnoreComments = true;
                s.IgnoreWhitespace = true;

                Load(XmlReader.Create(fileName, s));
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.Load(fileName);

                Load(doc);
            }
        }


        public void Load(Stream inStream, bool useXmlReader)
        {
            if (useXmlReader)
            {
                XmlReaderSettings s = new XmlReaderSettings();
                s.IgnoreComments = true;
                s.IgnoreWhitespace = true;

                Load(XmlReader.Create(inStream, s));
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.Load(inStream);
                Load(doc);
            }
        }


        private void ReadAutoComplete(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();

                    switch (attrName)
                    {
                        case "autohide":
                            _autoComplete_AutoHide = getBool(reader.Value);
                            break;
                        case "automaticlengthentered":
                            _autoComplete_AutomaticLengthEntered = getBool(reader.Value);
                            break;
                        case "cancelatstart":
                            _autoComplete_cancelAtStart = getBool(reader.Value);
                            break;
                        case "droprestofword":
                            _autoComplete_DropRestOfWord = getBool(reader.Value);
                            break;
                        case "fillupcharacters":
                            _autoComplete_fillUpCharacters = reader.Value;
                            break;
                        case "imageseperator":
                            _autoComplete_ImageSeperator = getChar(reader.Value);
                            break;
                        case "iscasesensitive":
                            _autoComplete_IsCaseSensitive= getBool(reader.Value);
                            break;
                        case "listseperator":
                            _autoComplete_ListSeperator = getChar(reader.Value);
                            break;
                        case "maxheight":
                            _autoComplete_MaxHeight = getInt(reader.Value);
                            break;
                        case "maxwidth":
                            _autoComplete_MaxWidth = getInt(reader.Value);
                            break;
                        case "singlelineaccept":
                            _autoComplete_singleLineAccept = getBool(reader.Value);
                            break;
                        case "stopcharacters":
                            _autoComplete_StopCharacters = reader.Value;
                            break;
                    }
                }

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("autocomplete", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("list", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                                if (reader.Name.Equals("inherit", StringComparison.OrdinalIgnoreCase))
                                    _autoComplete_ListInherit = getBool(reader.Value);
                            
                            reader.MoveToElement();
                        }
                        _autoComplete_List = new Regex("\\s+").Replace(reader.ReadString(), " ").Trim();
                    }
                }
            }
            reader.Read();
        }


        private void ReadCallTip(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "backcolor":
                            _callTip_BackColor = getColor(reader.Value);
                            break;
                        case "forecolor":
                            _callTip_ForeColor = getColor(reader.Value);
                            break;
                        case "highlighttextcolor":
                            _callTip_HighlightTextColor = getColor(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadCaret(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "blinkrate":
                            //	This guy is a bit of an oddball becuase null means "I don't Care"
                            //	and we need some way of using the OS value.
                            string blinkRate = reader.Value;
                            if (blinkRate.ToLower() == "system")
                                _caret_BlinkRate = SystemInformation.CaretBlinkTime;
                            else
                                _caret_BlinkRate = getInt(blinkRate);
                            break;
                        case "color":
                            _caret_Color = getColor(reader.Value);
                            break;
                        case "currentlinebackgroundalpha":
                            _caret_CurrentLineBackgroundAlpha = getInt(reader.Value);
                            break;
                        case "currentlinebackgroundcolor":
                            _caret_CurrentLineBackgroundColor = getColor(reader.Value);
                            break;
                        case "highlightcurrentline":
                            _caret_HighlightCurrentLine = getBool(reader.Value);
                            break;
                        case "issticky":
                            _caret_IsSticky = getBool(reader.Value);
                            break;
                        case "style":
                            _caret_Style = (CaretStyle)Enum.Parse(typeof(CaretStyle), reader.Value, true);
                            break;
                        case "width":
                            _caret_Width = getInt(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }
            reader.Skip();
        }


        private void ReadClipboard(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "convertlinebreaksonpaste":
                            _clipboard_ConvertLineBreaksOnPaste = getBool(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }
            reader.Skip();
        }


        private void ReadCommands(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "Inherit":
                            _commands_KeyBindingList.Inherit = getBool(reader.Value);
                            break;
                        case "AllowDuplicateBindings":
                            _commands_KeyBindingList.AllowDuplicateBindings = getBool(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("commands", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("binding", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            KeyBinding kb = new KeyBinding();
                            BindableCommand cmd = new BindableCommand();
                            bool? replaceCurrent = null;

                            while (reader.MoveToNextAttribute())
                            {
                                string attrName = reader.Name.ToLower();
                                switch (attrName)
                                {
                                    case "key":
                                        kb.KeyCode = Utilities.GetKeys(reader.Value);
                                        break;
                                    case "modifier":
                                        if (reader.Value != string.Empty)
                                        {
                                            foreach (string modifier in reader.Value.Split(' '))
                                                kb.Modifiers |= (Keys)Enum.Parse(typeof(Keys), modifier.Trim(), true);
                                        }
                                        break;
                                    case "command":
                                        cmd = (BindableCommand)Enum.Parse(typeof(BindableCommand), reader.Value, true);
                                        break;
                                    case "replacecurrent":
                                        replaceCurrent = getBool(reader.Value);
                                        break;
                                }
                            }

                            _commands_KeyBindingList.Add(new CommandBindingConfig(kb, replaceCurrent, cmd));
                        }

                        reader.MoveToElement();
                    }
                }
            }
            reader.Read();
        }


        private void ReadEndOfLine(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "isvisible":
                            _endOfLine_IsVisisble = getBool(reader.Value);
                            break;
                        case "mode":
                            _endOfLine_Mode = (EndOfLineMode)Enum.Parse(typeof(EndOfLineMode), reader.Value, true);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadFolding(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "flags":
                            string flags = reader.Value.Trim();
                            if (flags != string.Empty)
                            {
                                FoldFlag? ff = null;
                                foreach (string flag in flags.Split(' '))
                                    ff |= (FoldFlag)Enum.Parse(typeof(FoldFlag), flag.Trim(), true);

                                if (ff.HasValue)
                                    _folding_Flags = ff;
                            }
                            break;
                        case "IsEnabled":
                            _folding_MarkerScheme = (FoldMarkerScheme)Enum.Parse(typeof(FoldMarkerScheme), reader.Value, true);
                            break;
                        case "usecompactfolding":
                            _folding_UseCompactFolding = getBool(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadHotspot(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "activebackcolor":
                            _hotspot_ActiveBackColor = getColor(reader.Value);
                            break;
                        case "activeforecolor":
                            _hotspot_ActiveForeColor = getColor(reader.Value);
                            break;
                        case "activeunderline":
                            _hotspot_ActiveUnderline = getBool(reader.Value);
                            break;
                        case "singleline":
                            _hotspot_SingleLine = getBool(reader.Value);
                            break;
                        case "useactivebackcolor":
                            _hotspot_UseActiveBackColor = getBool(reader.Value);
                            break;
                        case "useactiveforecolor":
                            _hotspot_UseActiveForeColor = getBool(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadIndentation(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "backspaceunindents":
                            _indentation_BackspaceUnindents = getBool(reader.Value);
                            break;
                        case "indentwidth":
                            _indentation_IndentWidth = getInt(reader.Value);
                            break;
                        case "showguides":
                            _indentation_ShowGuides = getBool(reader.Value);
                            break;
                        case "tabindents":
                            _indentation_TabIndents = getBool(reader.Value);
                            break;
                        case "tabwidth":
                            _indentation_TabWidth = getInt(reader.Value);
                            break;
                        case "usetabs":
                            _indentation_UseTabs = getBool(reader.Value);
                            break;
                        case "smartindenttype":
                            _indentation_SmartIndentType = (SmartIndent)Enum.Parse(typeof(SmartIndent), reader.Value, true);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadIndicators(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "inherit":
                            _indicator_List.Inherit = getBool(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("indicators", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("indicator", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            IndicatorConfig ic = new IndicatorConfig();
                            while (reader.MoveToNextAttribute())
                            {
                                string attrName = reader.Name.ToLower();
                                switch (attrName)
                                {
                                    case "number":
                                        ic.Number = int.Parse(reader.Value);
                                        break;
                                    case "color":
                                        ic.Color = getColor(reader.Value);
                                        break;
                                    case "inherit":
                                        ic.Inherit = getBool(reader.Value);
                                        break;
                                    case "isdrawnunder":
                                        ic.IsDrawnUnder = getBool(reader.Value);
                                        break;
                                    case "style":
                                        ic.Style = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), reader.Value, true);
                                        break;
                                }
                            }
                            _indicator_List.Add(ic);
                            reader.MoveToElement();
                        }
                    }
                }
            }
            reader.Read();
        }


        private void ReadLanguage(XmlReader reader)
        {
            _commands_KeyBindingList = new CommandBindingConfigList();
            _lexing_Properties = new LexerPropertiesConfig();
            _lexing_Keywords = new KeyWordConfigList();
            _margin_List = new MarginConfigList();
            _markers_List = new MarkersConfigList();
            _snippetsConfigList = new SnippetsConfigList();
            _styles = new StyleConfigList();

            reader.Read();
            while (reader.NodeType == XmlNodeType.Element)
            {
                string elName = reader.Name.ToLower();
                switch (elName)
                {
                    case "autocomplete":
                        ReadAutoComplete(reader);
                        break;
                    case "calltip":
                        ReadCallTip(reader);
                        break;
                    case "caret":
                        ReadCaret(reader);
                        break;
                    case "clipboard":
                        ReadClipboard(reader);
                        break;
                    case "commands":
                        ReadCommands(reader);
                        break;
                    case "endofline":
                        ReadEndOfLine(reader);
                        break;
                    case "folding":
                        ReadFolding(reader);
                        break;
                    case "hotspot":
                        ReadHotspot(reader);
                        break;
                    case "indentation":
                        ReadIndentation(reader);
                        break;
                    case "indicators":
                        ReadIndicators(reader);
                        break;
                    case "lexer":
                        ReadLexer(reader);
                        break;
                    case "linewrapping":
                        ReadLineWrapping(reader);
                        break;
                    case "longlines":
                        ReadLongLines(reader);
                        break;
                    case "margins":
                        ReadMargins(reader);
                        break;
                    case "markers":
                        ReadMarkers(reader);
                        break;
                    case "scrolling":
                        ReadScrolling(reader);
                        break;
                    case "selection":
                        ReadSelection(reader);
                        break;
                    case "snippets":
                        ReadSnippets(reader);
                        break;
                    case "styles":
                        ReadStyles(reader);
                        break;
                    case "undoredo":
                        ReadUndoRedo(reader);
                        break;
                    case "whitespace":
                        ReadWhitespace(reader);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
                
            }
        }


        private void ReadLexer(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "whitespacechars":
                            _lexing_WhitespaceChars = reader.Value;
                            break;
                        case "wordchars":
                            _lexing_WordChars = reader.Value;
                            break;
                        case "lexername":
                            _lexing_Language = reader.Value;
                            break;
                        case "linecommentprefix":
                            _lexing_LineCommentPrefix = reader.Value;
                            break;
                        case "streamcommentprefix":
                            _lexing_StreamCommentPrefix = reader.Value;
                            break;
                        case "streamcommentsuffix":
                            _lexing_StreamCommentSuffix = reader.Value;
                            break;
                    }
                }
                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                reader.Read();
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("lexer", StringComparison.OrdinalIgnoreCase)))
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("properties", StringComparison.OrdinalIgnoreCase))
                            ReadLexerProperties(reader);
                        else if (reader.Name.Equals("keywords", StringComparison.OrdinalIgnoreCase))
                            ReadLexerKeywords(reader);
                    }
                }
            }
            reader.Read();
        }


        private void ReadLexerKeywords(XmlReader reader)
        {
            bool? inherit = null;
            int? list = null;
            string keywords = null;

            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "inherit":
                            inherit = getBool(reader.Value);
                            break;
                        case "list":
                            list = getInt(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }
            
            if (!reader.IsEmptyElement)
                keywords = reader.ReadString().Trim();

            _lexing_Keywords.Add(new KeyWordConfig(list.Value, keywords, inherit));

            reader.Read();
        }


        private void ReadLexerProperties(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                    if (reader.Name.ToLower() == "inherit")
                        _lexing_Properties.Inherit = getBool(reader.Value);

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("properties", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("property", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            string name = string.Empty;
                            string value = string.Empty;
                            while (reader.MoveToNextAttribute())
                            {
                                string attrName = reader.Name.ToLower();
                                switch (attrName)
                                {
                                    case "name":
                                        name = reader.Value;
                                        break;
                                    case "value":
                                        value = reader.Value;
                                        break;
                                }
                            }
                            _lexing_Properties.Add(name, value);
                            reader.MoveToElement();
                        }
                    }
                }
            }

            reader.Read();
        }


        private void ReadLineWrapping(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "mode":
                            _lineWrapping_Mode = (LineWrappingMode)Enum.Parse(typeof(LineWrappingMode), reader.Value, true);
                            break;
                        case "indentsize":
                            _lineWrapping_IndentSize = getInt(reader.Value);
                            break;
                        case "indentmode":
                            _lineWrapping_IndentMode = (LineWrappingIndentMode)Enum.Parse(typeof(LineWrappingIndentMode), reader.Value, true);
                            break;
                        case "visualflags":
                            string flags = reader.Value.Trim();
                            if (flags != string.Empty)
                            {
                                LineWrappingVisualFlags? wvf = null;
                                foreach (string flag in flags.Split(' '))
                                    wvf |= (LineWrappingVisualFlags)Enum.Parse(typeof(LineWrappingVisualFlags), flag.Trim(), true);

                                if (wvf.HasValue)
                                    _lineWrapping_VisualFlags = wvf;
                            }
                            break;
                        case "visualflagslocations":
                            _lineWrapping_VisualFlagsLocations = (LineWrappingVisualFlagsLocations)Enum.Parse(typeof(LineWrappingVisualFlagsLocations), reader.Value, true);
                            break;
                    }
                }
                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadLongLines(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "edgecolor":
                            _longLines_EdgeColor = getColor(reader.Value);
                            break;
                        case "edgecolumn":
                            _longLines_EdgeColumn = getInt(reader.Value);
                            break;
                        case "edgemode":
                            _longLines_EdgeMode = (EdgeMode)Enum.Parse(typeof(EdgeMode), reader.Value, true);
                            break;
                    }
                }
                reader.MoveToElement();
            }
            reader.Skip();
        }


        private void ReadMargins(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "foldmargincolor":
                            _margin_List.FoldMarginColor = getColor(reader.Value);
                            break;
                        case "foldmarginhighlightcolor":
                            _margin_List.FoldMarginHighlightColor = getColor(reader.Value);
                            break;
                        case "left":
                            _margin_List.Left = getInt(reader.Value);
                            break;
                        case "right":
                            _margin_List.Right = getInt(reader.Value);
                            break;
                        case "inherit":
                            _margin_List.Inherit = getBool(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }
            
            
            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("margins", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("margin", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            MarginConfig mc = new MarginConfig();
                            while (reader.MoveToNextAttribute())
                            {
                                string attrName = reader.Name.ToLower();
                                switch (attrName)
                                {
                                    case "number":
                                        mc.Number = int.Parse(reader.Value);
                                        break;
                                    case "inherit":
                                        mc.Inherit = getBool(reader.Value);
                                        break;
                                    case "autotogglemarkernumber":
                                        mc.AutoToggleMarkerNumber = getInt(reader.Value);
                                        break;
                                    case "isclickable":
                                        mc.IsClickable = getBool(reader.Value);
                                        break;
                                    case "isfoldmargin":
                                        mc.IsFoldMargin = getBool(reader.Value);
                                        break;
                                    case "ismarkermargin":
                                        mc.IsMarkerMargin = getBool(reader.Value);
                                        break;
                                    case "type":
                                        mc.Type = (MarginType)Enum.Parse(typeof(MarginType), reader.Value, true);
                                        break;
                                    case "width":
                                        mc.Width = getInt(reader.Value);
                                        break;
                                }
                            }
                            _margin_List.Add(mc);
                            reader.MoveToElement();
                        }
                    }
                }
            }

            reader.Read();
        }


        private void ReadMarkers(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                    if(reader.Name.ToLower() == "inherit")
                        _markers_List.Inherit = getBool(reader.Value);

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("markers", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("marker", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            MarkersConfig mc = new MarkersConfig();
                            while (reader.MoveToNextAttribute())
                            {
                                string attrName = reader.Name.ToLower();
                                switch (attrName)
                                {
                                    case "alpha":
                                        mc.Alpha = getInt(reader.Value);
                                        break;
                                    case "backcolor":
                                        mc.BackColor = getColor(reader.Value);
                                        break;
                                    case "forecolor":
                                        mc.ForeColor = getColor(reader.Value);
                                        break;
                                    case "name":
                                        mc.Name = reader.Value;
                                        break;
                                    case "number":
                                        mc.Number = getInt(reader.Value);
                                        break;
                                    case "inherit":
                                        mc.Inherit = getBool(reader.Value);
                                        break;
                                    case "symbol":
                                        mc.Symbol = (MarkerSymbol)Enum.Parse(typeof(MarkerSymbol), reader.Value, true);
                                        break;
                                }
                            }
                            
                            reader.MoveToElement();
                            _markers_List.Add(mc);
                        }
                    }
                }
            }
            reader.Read();
        }


        private void ReadScrolling(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "endatlastline":
                            _scrolling_EndAtLastLine = getBool(reader.Value);
                            break;
                        case "horizontalwidth":
                            _scrolling_HorizontalWidth = getInt(reader.Value);
                            break;
                        case "scrollbars":
                            string flags = reader.Value.Trim();
                            if (flags != string.Empty)
                            {
                                ScrollBars? sb = null;
                                foreach (string flag in flags.Split(' '))
                                    sb |= (ScrollBars)Enum.Parse(typeof(ScrollBars), flag.Trim(), true);

                                if (sb.HasValue)
                                    _scrolling_ScrollBars = sb;
                            }
                            break;
                        case "xoffset":
                            _scrolling_XOffset = getInt(reader.Value);
                            break;
                    }
                }
                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadSelection(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "backcolor":
                            _selection_BackColor = getColor(reader.Value);
                            break;
                        case "backcolorunfocused":
                            _selection_BackColorUnfocused = getColor(reader.Value);
                            break;
                        case "forecolor":
                            _selection_ForeColor = getColor(reader.Value);
                            break;
                        case "forecolorunfocused":
                            _selection_ForeColorUnfocused = getColor(reader.Value);
                            break;
                        case "hidden":
                            _selection_Hidden = getBool(reader.Value);
                            break;
                        case "hideselection":
                            _selection_HideSelection = getBool(reader.Value);
                            break;
                        case "mode":
                            _selection_Mode = (SelectionMode)Enum.Parse(typeof(SelectionMode), reader.Value, true);
                            break;
                    }
                }
                reader.MoveToElement();
            }

            reader.Skip();
        }


        private void ReadSnippets(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "activesnippetcolor":
                            _snippetsConfigList.ActiveSnippetColor = getColor(reader.Value);
                            break;
                        case "activesnippetindicator":
                            _snippetsConfigList.ActiveSnippetIndicator = getInt(reader.Value);
                            break;
                        case "inactivesnippetcolor":
                            _snippetsConfigList.InactiveSnippetColor = getColor(reader.Value);
                            break;
                        case "inactivesnippetindicator":
                            _snippetsConfigList.InactiveSnippetIndicator = getInt(reader.Value);
                            break;
                        case "activesnippetindicatorstyle":
                            _snippetsConfigList.ActiveSnippetIndicatorStyle = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), reader.Value, true);
                            break;
                        case "inactivesnippetindicatorstyle":
                            _snippetsConfigList.InactiveSnippetIndicatorStyle = (IndicatorStyle)Enum.Parse(typeof(IndicatorStyle), reader.Value, true);
                            break;
                        case "defaultdelimeter":
                            _snippetsConfigList.DefaultDelimeter = getChar(reader.Value);
                            break;
                        case "isenabled":
                            _snippetsConfigList.IsEnabled = getBool(reader.Value);
                            break;
                        case "isonekeyselectionembedenabled":
                            _snippetsConfigList.IsOneKeySelectionEmbedEnabled = getBool(reader.Value);
                            break;
                    }
                }

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("snippets", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("snippet", StringComparison.OrdinalIgnoreCase))
                    {
                        if (reader.HasAttributes)
                        {
                            SnippetsConfig sc = new SnippetsConfig();
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    string attrName = reader.Name.ToLower();
                                    switch (attrName)
                                    {

                                        case "shortcut":
                                            sc.Shortcut = reader.Value;
                                            break;
                                        case "delimeter":
                                            sc.Delimeter = getChar(reader.Value);
                                            break;
                                        case "issurroundswith":
                                            sc.IsSurroundsWith = getBool(reader.Value);
                                            break;
                                    }
                                }
                            }
                            reader.MoveToElement();
                            sc.Code = reader.ReadString();
                            _snippetsConfigList.Add(sc);
                        }
                    }
                }
            }

            reader.Read();
        }


        private void ReadStyles(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                    if (reader.Name.ToLower() == "bits")
                        _undoRedoIsUndoEnabled = getBool(reader.Value);

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("styles", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
                    {
                        _styles.Add(getStyleConfigFromElement(reader));
                    }
                    else if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("sublanguage", StringComparison.OrdinalIgnoreCase))
                    {
                        ReadSubLanguage(reader);
                    }
                }
            }

            reader.Read();
        }


        private void ReadSubLanguage(XmlReader reader)
        {
            //	This is a nifty added on hack made specifically for HTML.
            //	Normally the style config elements are quite managable as there
            //	are typically less than 10 when you don't count common styles.
            //	
            //	However HTML uses 9 different Sub languages that combined make 
            //	use of all 128 styles (well there are some small gaps). In order
            //	to make this more managable I did added a SubLanguage element that
            //	basically just prepends the Language's name and "." to the Style 
            //	Name definition.
            //
            //	So for example if you had the following
            //	<Styles>
            //		<SubLanguage Name="ASP JavaScript">
            //			<Style Name="Keyword" Bold="True" />
            //		</SubLanguage>
            //	</Styles>
            //	That style's name will get interpreted as "ASP JavaScript.Keyword".
            //	which if you look at the html.txt in LexerStyleNames you'll see it
            //	maps to Style # 62
            string subLanguageName = string.Empty;
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                    if (reader.Name.ToLower() == "name")
                        subLanguageName = reader.Value;

                reader.MoveToElement();
            }

            if (!reader.IsEmptyElement)
            {
                while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("sublanguage", StringComparison.OrdinalIgnoreCase)))
                {
                    reader.Read();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals("style", StringComparison.OrdinalIgnoreCase))
                    {
                        StyleConfig sc = getStyleConfigFromElement(reader);
                        sc.Name = subLanguageName + "." + sc.Name;
                        _styles.Add(sc);
                    }
                }
            }

            reader.Read();
        }


        private void ReadUndoRedo(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                    if (reader.Name.ToLower() == "isundoenabled")
                        _undoRedoIsUndoEnabled = getBool(reader.Value);

                reader.MoveToElement();
            }
            reader.Skip();
        }


        private void ReadWhitespace(XmlReader reader)
        {
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    string attrName = reader.Name.ToLower();
                    switch (attrName)
                    {
                        case "backcolor":
                            _whitespace_BackColor = getColor(reader.Value);
                            break;
                        case "forecolor":
                            _whitespace_ForeColor = getColor(reader.Value);
                            break;
                        case "mode":
                            _whitespace_Mode = (WhitespaceMode)Enum.Parse(typeof(WhitespaceMode), reader.Value, true);
                            break;
                    }
                }
            }
            reader.Skip();
        }

        #endregion Methods


        #region Properties

        public bool? AutoComplete_AutoHide
        {
            get
            {
                return _autoComplete_AutoHide;
            }
            set
            {
                _autoComplete_AutoHide = value;
            }
        }


        public bool? AutoComplete_AutomaticLengthEntered
        {
            get
            {
                return _autoComplete_AutomaticLengthEntered;
            }
            set
            {
                _autoComplete_AutomaticLengthEntered = value;
            }
        }


        public bool? AutoComplete_CancelAtStart
        {
            get
            {
                return _autoComplete_cancelAtStart;
            }
            set
            {
                _autoComplete_cancelAtStart = value;
            }
        }


        public bool? AutoComplete_DropRestOfWord
        {
            get
            {
                return _autoComplete_DropRestOfWord;
            }
            set
            {
                _autoComplete_DropRestOfWord = value;
            }
        }


        public string AutoComplete_FillUpCharacters
        {
            get
            {
                return _autoComplete_fillUpCharacters;
            }
            set
            {
                _autoComplete_fillUpCharacters = value;
            }
        }


        public char? AutoComplete_ImageSeperator
        {
            get
            {
                return _autoComplete_ImageSeperator;
            }
            set
            {
                _autoComplete_ImageSeperator = value;
            }
        }


        public bool? AutoComplete_IsCaseSensitive
        {
            get
            {
                return _autoComplete_IsCaseSensitive;
            }
            set
            {
                _autoComplete_IsCaseSensitive = value;
            }
        }


        public string AutoComplete_List
        {
            get
            {
                return _autoComplete_List;
            }
            set
            {
                _autoComplete_List = value;
            }
        }


        public bool? AutoComplete_ListInherits
        {
            get
            {
                return _autoComplete_ListInherit;
            }
            set
            {
                _autoComplete_ListInherit = value;
            }
        }


        public char? AutoComplete_ListSeperator
        {
            get
            {
                return _autoComplete_ListSeperator;
            }
            set
            {
                _autoComplete_ListSeperator = value;
            }
        }


        public int? AutoComplete_MaxHeight
        {
            get
            {
                return _autoComplete_MaxHeight;
            }
            set
            {
                _autoComplete_MaxHeight = value;
            }
        }


        public int? AutoComplete_MaxWidth
        {
            get
            {
                return _autoComplete_MaxWidth;
            }
            set
            {
                _autoComplete_MaxWidth = value;
            }
        }


        public bool? AutoComplete_SingleLineAccept
        {
            get
            {
                return _autoComplete_singleLineAccept;
            }
            set
            {
                _autoComplete_singleLineAccept = value;
            }
        }


        public string AutoComplete_StopCharacters
        {
            get
            {
                return _autoComplete_StopCharacters;
            }
            set
            {
                _autoComplete_StopCharacters = value;
            }
        }


        public Color CallTip_BackColor
        {
            get
            {
                return _callTip_BackColor;
            }
            set
            {
                _callTip_BackColor = value;
            }
        }


        public Color CallTip_ForeColor
        {
            get
            {
                return _callTip_ForeColor;
            }
            set
            {
                _callTip_ForeColor = value;
            }
        }


        public Color CallTip_HighlightTextColor
        {
            get
            {
                return _callTip_HighlightTextColor;
            }
            set
            {
                _callTip_HighlightTextColor = value;
            }
        }


        public int? Caret_BlinkRate
        {
            get
            {
                return _caret_BlinkRate;
            }
            set
            {
                _caret_BlinkRate = value;
            }
        }


        public Color Caret_Color
        {
            get
            {
                return _caret_Color;
            }
            set
            {
                _caret_Color = value;
            }
        }


        public int? Caret_CurrentLineBackgroundAlpha
        {
            get
            {
                return _caret_CurrentLineBackgroundAlpha;
            }
            set
            {
                _caret_CurrentLineBackgroundAlpha = value;
            }
        }


        public Color Caret_CurrentLineBackgroundColor
        {
            get
            {
                return _caret_CurrentLineBackgroundColor;
            }
            set
            {
                _caret_CurrentLineBackgroundColor = value;
            }
        }


        public bool? Caret_HighlightCurrentLine
        {
            get
            {
                return _caret_HighlightCurrentLine;
            }
            set
            {
                _caret_HighlightCurrentLine = value;
            }
        }


        public bool? Caret_IsSticky
        {
            get
            {
                return _caret_IsSticky;
            }
            set
            {
                _caret_IsSticky = value;
            }
        }


        public CaretStyle? Caret_Style
        {
            get
            {
                return _caret_Style;
            }
            set
            {
                _caret_Style = value;
            }
        }


        public int? Caret_Width
        {
            get
            {
                return _caret_Width;
            }
            set
            {
                _caret_Width = value;
            }
        }


        public bool? Clipboard_ConvertLineBreaksOnPaste
        {
            get
            {
                return _clipboard_ConvertLineBreaksOnPaste;
            }
            set
            {
                _clipboard_ConvertLineBreaksOnPaste = value;
            }
        }


        public CommandBindingConfigList Commands_KeyBindingList
        {
            get
            {
                return _commands_KeyBindingList;
            }
            set
            {
                _commands_KeyBindingList = value;
            }
        }


        public string DropMarkers_SharedStackName
        {
            get
            {
                return _dropMarkers_SharedStackName;
            }
            set
            {
                _dropMarkers_SharedStackName = value;
            }
        }


        public bool? EndOfLine_IsVisisble
        {
            get
            {
                return _endOfLine_IsVisisble;
            }
            set
            {
                _endOfLine_IsVisisble = value;
            }
        }


        public EndOfLineMode? EndOfLine_Mode
        {
            get
            {
                return _endOfLine_Mode;
            }
            set
            {
                _endOfLine_Mode = value;
            }
        }


        public FoldFlag? Folding_Flags
        {
            get
            {
                return _folding_Flags;
            }
            set
            {
                _folding_Flags = value;
            }
        }


        public bool? Folding_IsEnabled
        {
            get
            {
                return _folding_IsEnabled;
            }
            set
            {
                _folding_IsEnabled = value;
            }
        }


        public FoldMarkerScheme? Folding_MarkerScheme
        {
            get
            {
                return _folding_MarkerScheme;
            }
            set
            {
                _folding_MarkerScheme = value;
            }
        }


        public bool? Folding_UseCompactFolding
        {
            get
            {
                return _folding_UseCompactFolding;
            }
            set
            {
                _folding_UseCompactFolding = value;
            }
        }


        public bool HasData
        {
            get { return _hasData; }
            set
            {
                _hasData = value;
            }
        }


        public Color Hotspot_ActiveBackColor
        {
            get
            {
                return _hotspot_ActiveBackColor;
            }
            set
            {
                _hotspot_ActiveBackColor = value;
            }
        }


        public Color Hotspot_ActiveForeColor
        {
            get
            {
                return _hotspot_ActiveForeColor;
            }
            set
            {
                _hotspot_ActiveForeColor = value;
            }
        }


        public bool? Hotspot_ActiveUnderline
        {
            get
            {
                return _hotspot_ActiveUnderline;
            }
            set
            {
                _hotspot_ActiveUnderline = value;
            }
        }


        public bool? Hotspot_SingleLine
        {
            get
            {
                return _hotspot_SingleLine;
            }
            set
            {
                _hotspot_SingleLine = value;
            }
        }


        public bool? Hotspot_UseActiveBackColor
        {
            get
            {
                return _hotspot_UseActiveBackColor;
            }
            set
            {
                _hotspot_UseActiveBackColor = value;
            }
        }


        public bool? Hotspot_UseActiveForeColor
        {
            get
            {
                return _hotspot_UseActiveForeColor;
            }
            set
            {
                _hotspot_UseActiveForeColor = value;
            }
        }


        public bool? Indentation_BackspaceUnindents
        {
            get
            {
                return _indentation_BackspaceUnindents;
            }
            set
            {
                _indentation_BackspaceUnindents = value;
            }
        }


        public int? Indentation_IndentWidth
        {
            get
            {
                return _indentation_IndentWidth;
            }
            set
            {
                _indentation_IndentWidth = value;
            }
        }


        public bool? Indentation_ShowGuides
        {
            get
            {
                return _indentation_ShowGuides;
            }
            set
            {
                _indentation_ShowGuides = value;
            }
        }


        public SmartIndent? Indentation_SmartIndentType
        {
            get
            {
                return _indentation_SmartIndentType;
            }
            set
            {
                _indentation_SmartIndentType = value;
            }
        }


        public bool? Indentation_TabIndents
        {
            get
            {
                return _indentation_TabIndents;
            }
            set
            {
                _indentation_TabIndents = value;
            }
        }


        public int? Indentation_TabWidth
        {
            get
            {
                return _indentation_TabWidth;
            }
            set
            {
                _indentation_TabWidth = value;
            }
        }


        public bool? Indentation_UseTabs
        {
            get
            {
                return _indentation_UseTabs;
            }
            set
            {
                _indentation_UseTabs = value;
            }
        }


        public IndicatorConfigList Indicator_List
        {
            get
            {
                return _indicator_List;
            }
            set
            {
                _indicator_List = value;
            }
        }


        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }


        public KeyWordConfigList Lexing_Keywords
        {
            get
            {
                return _lexing_Keywords;
            }
            set
            {
                _lexing_Keywords = value;
            }
        }


        public string Lexing_Language
        {
            get
            {
                return _lexing_Language;
            }
            set
            {
                _lexing_Language = value;
            }
        }


        public string Lexing_LineCommentPrefix
        {
            get
            {
                return _lexing_LineCommentPrefix;
            }
            set
            {
                _lexing_LineCommentPrefix = value;
            }
        }


        public LexerPropertiesConfig Lexing_Properties
        {
            get
            {
                return _lexing_Properties;
            }
            set
            {
                _lexing_Properties = value;
            }
        }


        public string Lexing_StreamCommentPrefix
        {
            get
            {
                return _lexing_StreamCommentPrefix;
            }
            set
            {
                _lexing_StreamCommentPrefix = value;
            }
        }


        public string Lexing_StreamCommentSuffix
        {
            get
            {
                return _lexing_StreamCommentSuffix;
            }
            set
            {
                _lexing_StreamCommentSuffix = value;
            }
        }


        public string Lexing_WhitespaceChars
        {
            get
            {
                return _lexing_WhitespaceChars;
            }
            set
            {
                _lexing_WhitespaceChars = value;
            }
        }


        public string Lexing_WordChars
        {
            get
            {
                return _lexing_WordChars;
            }
            set
            {
                _lexing_WordChars = value;
            }
        }

        public LineWrappingIndentMode? LineWrapping_IndentMode
        {
            get
            {
                return _lineWrapping_IndentMode;
            }
            set
            {
                _lineWrapping_IndentMode = value;
            }
        }


        public int? LineWrapping_IndentSize
        {
            get
            {
                return _lineWrapping_IndentSize;
            }
            set
            {
                _lineWrapping_IndentSize = value;
            }
        }


        public LineWrappingMode? LineWrapping_Mode
        {
            get
            {
                return _lineWrapping_Mode;
            }
            set
            {
                _lineWrapping_Mode = value;
            }
        }


        public LineWrappingVisualFlags? LineWrapping_VisualFlags
        {
            get
            {
                return _lineWrapping_VisualFlags;
            }
            set
            {
                _lineWrapping_VisualFlags = value;
            }
        }


        public LineWrappingVisualFlagsLocations? LineWrapping_VisualFlagsLocations
        {
            get
            {
                return _lineWrapping_VisualFlagsLocations;
            }
            set
            {
                _lineWrapping_VisualFlagsLocations = value;
            }
        }


        public Color LongLines_EdgeColor
        {
            get
            {
                return _longLines_EdgeColor;
            }
            set
            {
                _longLines_EdgeColor = value;
            }
        }


        public int? LongLines_EdgeColumn
        {
            get
            {
                return _longLines_EdgeColumn;
            }
            set
            {
                _longLines_EdgeColumn = value;
            }
        }


        public EdgeMode? LongLines_EdgeMode
        {
            get
            {
                return _longLines_EdgeMode;
            }
            set
            {
                _longLines_EdgeMode = value;
            }
        }


        public MarginConfigList Margin_List
        {
            get
            {
                return _margin_List;
            }
            set
            {
                _margin_List = value;
            }
        }


        public MarkersConfigList Markers_List
        {
            get
            {
                return _markers_List;
            }
            set
            {
                _markers_List = value;
            }
        }


        public bool? Scrolling_EndAtLastLine
        {
            get
            {
                return _scrolling_EndAtLastLine;
            }
            set
            {
                _scrolling_EndAtLastLine = value;
            }
        }


        public int? Scrolling_HorizontalWidth
        {
            get
            {
                return _scrolling_HorizontalWidth;
            }
            set
            {
                _scrolling_HorizontalWidth = value;
            }
        }


        public ScrollBars? Scrolling_ScrollBars
        {
            get
            {
                return _scrolling_ScrollBars;
            }
            set
            {
                _scrolling_ScrollBars = value;
            }
        }


        public int? Scrolling_XOffset
        {
            get
            {
                return _scrolling_XOffset;
            }
            set
            {
                _scrolling_XOffset = value;
            }
        }


        public Color Selection_BackColor
        {
            get
            {
                return _selection_BackColor;
            }
            set
            {
                _selection_BackColor = value;
            }
        }


        public Color Selection_BackColorUnfocused
        {
            get
            {
                return _selection_BackColorUnfocused;
            }
            set
            {
                _selection_BackColorUnfocused = value;
            }
        }


        public Color Selection_ForeColor
        {
            get
            {
                return _selection_ForeColor;
            }
            set
            {
                _selection_ForeColor = value;
            }
        }


        public Color Selection_ForeColorUnfocused
        {
            get
            {
                return _selection_ForeColorUnfocused;
            }
            set
            {
                _selection_ForeColorUnfocused = value;
            }
        }


        public bool? Selection_Hidden
        {
            get
            {
                return _selection_Hidden;
            }
            set
            {
                _selection_Hidden = value;
            }
        }


        public bool? Selection_HideSelection
        {
            get
            {
                return _selection_HideSelection;
            }
            set
            {
                _selection_HideSelection = value;
            }
        }


        public SelectionMode? Selection_Mode
        {
            get
            {
                return _selection_Mode;
            }
            set
            {
                _selection_Mode = value;
            }
        }


        public SnippetsConfigList SnippetsConfigList
        {
            get
            {
                return _snippetsConfigList;
            }
            set
            {
                _snippetsConfigList = value;
            }
        }


        public StyleConfigList Styles
        {
            get
            {
                return _styles;
            }
            set
            {
                _styles = value;
            }
        }


        public bool? UndoRedoIsUndoEnabled
        {
            get
            {
                return _undoRedoIsUndoEnabled;
            }
            set
            {
                _undoRedoIsUndoEnabled = value;
            }
        }


        public Color Whitespace_BackColor
        {
            get
            {
                return _whitespace_BackColor;
            }
            set
            {
                _whitespace_BackColor = value;
            }
        }


        public Color Whitespace_ForeColor
        {
            get
            {
                return _whitespace_ForeColor;
            }
            set
            {
                _whitespace_ForeColor = value;
            }
        }


        public WhitespaceMode? Whitespace_Mode
        {
            get
            {
                return _whitespace_Mode;
            }
            set
            {
                _whitespace_Mode = value;
            }
        }

        #endregion Properties


        #region Constructors

        public Configuration(string language)
        {
            _language = language;
        }


        public Configuration(XmlDocument configDocument, string language)
        {
            _language = language;
            Load(configDocument);
        }


        public Configuration(TextReader txtReader, string language)
        {
            _language = language;
            Load(txtReader);
        }


        public Configuration(XmlReader reader, string language)
        {
            _language = language;
            Load(reader);
        }


        public Configuration(string fileName, string language, bool useXmlReader)
        {
            _language = language;
            Load(fileName, useXmlReader);
        }


        public Configuration(Stream inStream, string language, bool useXmlReader)
        {
            _language = language;
            Load(inStream, useXmlReader);
        }

        #endregion Constructors
    }
}

