#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Manages commands, which are actions in ScintillaNET that can be bound to key combinations.
    /// </summary>
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Commands : TopLevelHelper
    {
        #region Fields

        private Dictionary<KeyBinding, List<BindableCommand>> _boundCommands = new Dictionary<KeyBinding, List<BindableCommand>>();
        private CommandComparer _commandComparer = new CommandComparer();

        // Hmmm.. This is getting more and more hackyish
        internal bool StopProcessingCommands = false;

        private bool _allowDuplicateBindings = true;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Adds a key combination to a Command
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="command">Command to execute</param>
        public void AddBinding(char shortcut, BindableCommand command)
        {
            AddBinding(Utilities.GetKeys(shortcut), Keys.None, command);
        }


        /// <summary>
        ///     Adds a key combination to a Command
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <param name="command">Command to execute</param>
        public void AddBinding(char shortcut, Keys modifiers, BindableCommand command)
        {
            AddBinding(Utilities.GetKeys(shortcut), modifiers, command);
        }


        /// <summary>
        ///     Adds a key combination to a Command
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="command">Command to execute</param>
        public void AddBinding(Keys shortcut, BindableCommand command)
        {
            AddBinding(shortcut, Keys.None, command);
        }


        /// <summary>
        ///     Adds a key combination to a Command
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <param name="command">Command to execute</param>
        public void AddBinding(Keys shortcut, Keys modifiers, BindableCommand command)
        {
            KeyBinding kb = new KeyBinding(shortcut, modifiers);
            if (!_boundCommands.ContainsKey(kb))
                _boundCommands.Add(kb, new List<BindableCommand>());

            List<BindableCommand> l = _boundCommands[kb];
            if (_allowDuplicateBindings || !l.Contains(command))
                l.Add(command);
        }


        /// <summary>
        ///     Executes a Command
        /// </summary>
        /// <param name="command">Any <see cref="BindableCommand"/></param>
        /// <returns>Value to indicate whether other bound commands should continue to execute</returns>
        public bool Execute(BindableCommand command)
        {
            if ((int)command < 10000)
            {
                NativeScintilla.SendMessageDirect((uint)command, IntPtr.Zero, IntPtr.Zero);
                return true;
            }

            switch (command)
            {
                case BindableCommand.AutoCShow:
                    Scintilla.AutoComplete.Show();
                    return true;

                case BindableCommand.AcceptActiveSnippets:
                    return Scintilla.Snippets.AcceptActiveSnippets();

                case BindableCommand.CancelActiveSnippets:
                    return Scintilla.Snippets.CancelActiveSnippets();

                case BindableCommand.DoSnippetCheck:
                    return Scintilla.Snippets.DoSnippetCheck();

                case BindableCommand.NextSnippetRange:
                    return Scintilla.Snippets.NextSnippetRange();

                case BindableCommand.PreviousSnippetRange:
                    return Scintilla.Snippets.PreviousSnippetRange();

                case BindableCommand.DropMarkerCollect:
                    Scintilla.DropMarkers.Collect();
                    return false;

                case BindableCommand.DropMarkerDrop:
                    Scintilla.DropMarkers.Drop();
                    return true;

                case BindableCommand.Print:
                    Scintilla.Printing.Print();
                    return true;

                case BindableCommand.PrintPreview:
                    Scintilla.Printing.PrintPreview();
                    return true;

                case BindableCommand.ShowFind:
                    Scintilla.FindReplace.ShowFind();
                    return true;

                case BindableCommand.ShowReplace:
                    Scintilla.FindReplace.ShowReplace();
                    return true;

                case BindableCommand.FindNext:
                    Scintilla.FindReplace.Window.FindNext();
                    return true;

                case BindableCommand.FindPrevious:
                    Scintilla.FindReplace.Window.FindPrevious();
                    return true;

                case BindableCommand.IncrementalSearch:
                    Scintilla.FindReplace.IncrementalSearch();
                    return true;

                case BindableCommand.LineComment:
                    Scintilla.Lexing.LineComment();
                    return true;

                case BindableCommand.LineUncomment:
                    Scintilla.Lexing.LineUncomment();
                    return true;

                case BindableCommand.DocumentNavigateForward:
                    Scintilla.DocumentNavigation.NavigateForward();
                    return true;

                case BindableCommand.DocumentNavigateBackward:
                    Scintilla.DocumentNavigation.NavigateBackward();
                    return true;

                case BindableCommand.ToggleLineComment:
                    Scintilla.Lexing.ToggleLineComment();
                    return true;

                case BindableCommand.StreamComment:
                    Scintilla.Lexing.StreamComment();
                    return true;

                case BindableCommand.ShowSnippetList:
                    Scintilla.Snippets.ShowSnippetList();
                    return true;

                case BindableCommand.ShowSurroundWithList:
                    Scintilla.Snippets.ShowSurroundWithList();
                    return true;

                case BindableCommand.ShowGoTo:
                    Scintilla.GoTo.ShowGoToDialog();
                    break;
            }

            return false;
        }


        /// <summary>
        ///     Returns a list of Commands bound to a keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <returns>List of Commands bound to a keyboard shortcut</returns>
        private List<BindableCommand> GetCommands(char shortcut)
        {
            return GetCommands(Utilities.GetKeys(shortcut), Keys.None);
        }


        /// <summary>
        ///     Returns a list of Commands bound to a keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <returns>List of Commands bound to a keyboard shortcut</returns>
        private List<BindableCommand> GetCommands(char shortcut, Keys modifiers)
        {
            return GetCommands(Utilities.GetKeys(shortcut), modifiers);
        }


        /// <summary>
        ///     Returns a list of Commands bound to a keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <returns>List of Commands bound to a keyboard shortcut</returns>
        private List<BindableCommand> GetCommands(Keys shortcut)
        {
            return GetCommands(shortcut, Keys.None);
        }


        /// <summary>
        ///     Returns a list of Commands bound to a keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <returns>List of Commands bound to a keyboard shortcut</returns>
        private List<BindableCommand> GetCommands(Keys shortcut, Keys modifiers)
        {
            KeyBinding kb = new KeyBinding(shortcut, modifiers);
            if (!_boundCommands.ContainsKey(kb))
                return new List<BindableCommand>();

            return _boundCommands[kb];
        }


        /// <summary>
        ///     Returns a list of KeyBindings bound to a given command
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>List of KeyBindings bound to the given command</returns>
        public List<KeyBinding> GetKeyBindings(BindableCommand command)
        {
            List<KeyBinding> ret = new List<KeyBinding>();
            foreach (KeyValuePair<KeyBinding, List<BindableCommand>> item in _boundCommands)
            {
                if (item.Value.Contains(command))
                    ret.Add(item.Key);
            }

            return ret;
        }


        internal bool ProcessKey(KeyEventArgs e)
        {
            StopProcessingCommands = false;

            KeyBinding kb = new KeyBinding(e.KeyCode, e.Modifiers);
            if (!_boundCommands.ContainsKey(kb))
                return false;

            List<BindableCommand> cmds = _boundCommands[kb];
            if (cmds.Count == 0)
                return false;

            cmds.Sort(_commandComparer);

            bool ret = false;
            foreach (BindableCommand cmd in cmds)
            {
                ret |= Execute(cmd);

                if (StopProcessingCommands)
                    return ret;
            }

            return ret;
        }


        /// <summary>
        ///     Removes all key command bindings
        /// </summary>
        /// <remarks>
        ///     Performing this action will make ScintillaNET virtually unusable until you assign new command bindings.
        ///     This removes even basic functionality like arrow keys, common clipboard commands, home/_end, etc.
        /// </remarks>
        public void RemoveAllBindings()
        {
            _boundCommands.Clear();
        }


        /// <summary>
        ///     Removes all commands bound to a  keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        public void RemoveBinding(char shortcut)
        {
            RemoveBinding(Utilities.GetKeys(shortcut), Keys.None);
        }


        /// <summary>
        ///     Removes a keyboard shortcut / command combination
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="command">Command to execute</param>
        public void RemoveBinding(char shortcut, BindableCommand command)
        {
            RemoveBinding(Utilities.GetKeys(shortcut), Keys.None, command);
        }


        /// <summary>
        ///     Removes all commands bound to a  keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        public void RemoveBinding(char shortcut, Keys modifiers)
        {
            RemoveBinding(Utilities.GetKeys(shortcut), modifiers);
        }


        /// <summary>
        ///     Removes a keyboard shortcut / command combination
        /// </summary>
        /// <param name="shortcut">Character corresponding to a (keyboard) key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <param name="command">Command to execute</param>
        public void RemoveBinding(char shortcut, Keys modifiers, BindableCommand command)
        {
            RemoveBinding(Utilities.GetKeys(shortcut), modifiers, command);
        }


        /// <summary>
        ///     Removes all commands bound to a  keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        public void RemoveBinding(Keys shortcut)
        {
            RemoveBinding(shortcut, Keys.None);
        }


        /// <summary>
        ///     Removes a keyboard shortcut / command combination
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="command">Command to execute</param>
        public void RemoveBinding(Keys shortcut, BindableCommand command)
        {
            RemoveBinding(shortcut, Keys.None, command);
        }


        /// <summary>
        ///     Removes all commands bound to a  keyboard shortcut
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        public void RemoveBinding(Keys shortcut, Keys modifiers)
        {
            _boundCommands.Remove(new KeyBinding(shortcut, modifiers));
        }


        /// <summary>
        ///     Removes a keyboard shortcut / command combination
        /// </summary>
        /// <param name="shortcut">Key to trigger command</param>
        /// <param name="modifiers">Shift, alt, ctrl</param>
        /// <param name="command">Command to execute</param>
        public void RemoveBinding(Keys shortcut, Keys modifiers, BindableCommand command)
        {
            KeyBinding kb = new KeyBinding(shortcut, modifiers);
            if (!_boundCommands.ContainsKey(kb))
                return;

            _boundCommands[kb].Remove(command);
        }


        private void ResetAllowDuplicateBindings()
        {
            _allowDuplicateBindings = true;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeAllowDuplicateBindings();
        }


        private bool ShouldSerializeAllowDuplicateBindings()
        {
            return !_allowDuplicateBindings;
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Gets/Sets if a key combination can be bound to more than one command. (default is true)
        /// </summary>
        /// <remarks>
        ///     When set to false only the first command bound to a key combination is kept.
        ///     Subsequent requests are ignored. 
        /// </remarks>
        public bool AllowDuplicateBindings
        {
            get
            {
                return _allowDuplicateBindings;
            }
            set
            {
                _allowDuplicateBindings = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal Commands(Scintilla scintilla) : base(scintilla)
        {

            //	Ha Ha Ha Ha all your commands are belong to us!
            NativeScintilla.ClearAllCmdKeys();

            //	The reason we're doing this is because ScintillaNET is going to own
            //	all the command bindings. There are two reasons for this: #1 it makes
            //	it easier to handle ScintillaNET specific commands, we don't have to
            //	do special logic if its a native command vs. ScintillaNET extension.

            //	#2 Scintilla's built in support for commands binding only allows 1
            //	command per key combination. Our key handling allows for any number
            //	of commands to be bound to a keyboard combination. 

            //	Other future enhancements that I want to do in the future are:
            //	Visual Studioesque Key/Chord commands like Ctrl+D, w

            //	Binding contexts. This is another CodeRush inspired idea where
            //	commands can only execute if a given context is satisfied (or not).
            //	Some examples are "At beginning of line", "In comment", 
            //	"Autocomplete window active", "In Snippet Range".

            //	OK in order for these commands to play nice with each other some of them 
            //	have to have knowledge of each other AND they have to execute in a certain
            //	order. 

            //	Since all the native Scintilla Commands already know how to work together
            //	properly they all have the same order. But our commands have to execute first

            _commandComparer.CommandOrder.Add(BindableCommand.AutoCShow, 100);
            _commandComparer.CommandOrder.Add(BindableCommand.AutoCComplete, 100);
            _commandComparer.CommandOrder.Add(BindableCommand.AutoCCancel, 100);
            _commandComparer.CommandOrder.Add(BindableCommand.DoSnippetCheck, 200);
            _commandComparer.CommandOrder.Add(BindableCommand.AcceptActiveSnippets, 200);
            _commandComparer.CommandOrder.Add(BindableCommand.CancelActiveSnippets, 200);
            _commandComparer.CommandOrder.Add(BindableCommand.NextSnippetRange, 200);
            _commandComparer.CommandOrder.Add(BindableCommand.PreviousSnippetRange, 200);

            AddBinding(Keys.Down , Keys.None, BindableCommand.LineDown);
            AddBinding(Keys.Down , Keys.Shift, BindableCommand.LineDownExtend);
            AddBinding(Keys.Down , Keys.Control, BindableCommand.LineScrollDown);
            AddBinding(Keys.Down , Keys.Alt | Keys.Shift, BindableCommand.LineDownRectExtend);
            AddBinding(Keys.Up , Keys.None, BindableCommand.LineUp);
            AddBinding(Keys.Up , Keys.Shift, BindableCommand.LineUpExtend);
            AddBinding(Keys.Up , Keys.Control, BindableCommand.LineScrollUp);
            AddBinding(Keys.Up , Keys.Alt | Keys.Shift, BindableCommand.LineUpRectExtend);
            AddBinding('[',  Keys.Control, BindableCommand.ParaUp);
            AddBinding('[' , Keys.Control | Keys.Shift, BindableCommand.ParaUpExtend);
            AddBinding(']' , Keys.Control, BindableCommand.ParaDown);
            AddBinding(']' , Keys.Control | Keys.Shift, BindableCommand.ParaDownExtend);
            AddBinding(Keys.Left , Keys.None, BindableCommand.CharLeft);
            AddBinding(Keys.Left , Keys.Shift, BindableCommand.CharLeftExtend);
            AddBinding(Keys.Left , Keys.Control, BindableCommand.WordLeft);
            AddBinding(Keys.Left , Keys.Control | Keys.Shift, BindableCommand.WordLeftExtend);
            AddBinding(Keys.Left , Keys.Alt | Keys.Shift, BindableCommand.CharLeftRectExtend);
            AddBinding(Keys.Right , Keys.None, BindableCommand.CharRight);
            AddBinding(Keys.Right , Keys.Shift, BindableCommand.CharRightExtend);
            AddBinding(Keys.Right , Keys.Control, BindableCommand.WordRight);
            AddBinding(Keys.Right , Keys.Control | Keys.Shift, BindableCommand.WordRightExtend);
            AddBinding(Keys.Right , Keys.Alt | Keys.Shift, BindableCommand.CharRightRectExtend);
            AddBinding('/' , Keys.Control, BindableCommand.WordPartLeft);
            AddBinding('/' , Keys.Control | Keys.Shift, BindableCommand.WordPartLeftExtend);
            AddBinding('\\' , Keys.Control, BindableCommand.WordPartRight);
            AddBinding('\\' , Keys.Control | Keys.Shift, BindableCommand.WordPartRightExtend);
            AddBinding(Keys.Home , Keys.None, BindableCommand.VCHome);
            AddBinding(Keys.Home , Keys.Shift, BindableCommand.VCHomeExtend);
            AddBinding(Keys.Home , Keys.Control, BindableCommand.DocumentStart);
            AddBinding(Keys.Home , Keys.Control | Keys.Shift, BindableCommand.DocumentStartExtend);
            AddBinding(Keys.Home , Keys.Alt, BindableCommand.HomeDisplay);
            AddBinding(Keys.Home , Keys.Alt | Keys.Shift, BindableCommand.VCHomeRectExtend);
            AddBinding(Keys.End , Keys.None, BindableCommand.LineEnd);
            AddBinding(Keys.End , Keys.Shift, BindableCommand.LineEndExtend);
            AddBinding(Keys.End , Keys.Control, BindableCommand.DocumentEnd);
            AddBinding(Keys.End , Keys.Control | Keys.Shift, BindableCommand.DocumentEndExtend);
            AddBinding(Keys.End , Keys.Alt, BindableCommand.LineEndDisplay);
            AddBinding(Keys.End , Keys.Alt | Keys.Shift, BindableCommand.LineEndRectExtend);
            AddBinding(Keys.PageUp , Keys.None, BindableCommand.PageUp);
            AddBinding(Keys.PageUp , Keys.Shift, BindableCommand.PageUpExtend);
            AddBinding(Keys.PageUp , Keys.Alt | Keys.Shift, BindableCommand.PageUpRectExtend);
            AddBinding(Keys.PageDown , Keys.None, BindableCommand.PageDown);
            AddBinding(Keys.PageDown , Keys.Shift, BindableCommand.PageDownExtend);
            AddBinding(Keys.PageDown , Keys.Alt | Keys.Shift, BindableCommand.PageDownRectExtend);
            AddBinding(Keys.Delete , Keys.None, BindableCommand.Clear);
            AddBinding(Keys.Delete , Keys.Shift, BindableCommand.Cut);
            AddBinding(Keys.Delete , Keys.Control, BindableCommand.DelWordRight);
            AddBinding(Keys.Delete , Keys.Control | Keys.Shift, BindableCommand.DelLineRight);
            AddBinding(Keys.Insert , Keys.None, BindableCommand.EditToggleOvertype);
            AddBinding(Keys.Insert , Keys.Shift, BindableCommand.Paste);
            AddBinding(Keys.Insert , Keys.Control, BindableCommand.Copy);
            AddBinding(Keys.Escape , Keys.None, BindableCommand.Cancel);
            AddBinding(Keys.Back , Keys.None, BindableCommand.DeleteBack);
            AddBinding(Keys.Back , Keys.Shift, BindableCommand.DeleteBack);
            AddBinding(Keys.Back , Keys.Control, BindableCommand.DelWordLeft);
            AddBinding(Keys.Back , Keys.Alt, BindableCommand.Undo);
            AddBinding(Keys.Back , Keys.Control | Keys.Shift, BindableCommand.DelLineLeft);
            AddBinding(Keys.Z, Keys.Control, BindableCommand.Undo);
            AddBinding(Keys.Y, Keys.Control, BindableCommand.Redo);
            AddBinding(Keys.X, Keys.Control, BindableCommand.Cut);
            AddBinding(Keys.C, Keys.Control, BindableCommand.Copy);
            AddBinding(Keys.V, Keys.Control, BindableCommand.Paste);
            AddBinding(Keys.A, Keys.Control, BindableCommand.SelectAll);
            AddBinding(Keys.Tab , Keys.None, BindableCommand.Tab);
            AddBinding(Keys.Tab , Keys.Shift, BindableCommand.BackTab);
            AddBinding(Keys.Enter , Keys.None, BindableCommand.NewLine);
            AddBinding(Keys.Enter , Keys.Shift, BindableCommand.NewLine);
            AddBinding(Keys.Add , Keys.Control, BindableCommand.ZoomIn);
            AddBinding(Keys.Subtract , Keys.Control, BindableCommand.ZoomOut);
            AddBinding(Keys.Divide, Keys.Control, BindableCommand.SetZoom);
            AddBinding(Keys.L, Keys.Control, BindableCommand.LineCut);
            AddBinding(Keys.L, Keys.Control | Keys.Shift, BindableCommand.LineDelete);
            AddBinding(Keys.T , Keys.Control | Keys.Shift, BindableCommand.LineCopy);
            AddBinding(Keys.T, Keys.Control, BindableCommand.LineTranspose);
            AddBinding(Keys.D, Keys.Control, BindableCommand.SelectionDuplicate);
            AddBinding(Keys.U, Keys.Control, BindableCommand.LowerCase);
            AddBinding(Keys.U, Keys.Control | Keys.Shift, BindableCommand.UpperCase);

            AddBinding(Keys.Space, Keys.Control, BindableCommand.AutoCShow);
            AddBinding(Keys.Tab, BindableCommand.DoSnippetCheck);
            AddBinding(Keys.Tab, BindableCommand.NextSnippetRange);
            AddBinding(Keys.Tab, Keys.Shift, BindableCommand.PreviousSnippetRange);
            AddBinding(Keys.Escape, BindableCommand.CancelActiveSnippets);
            AddBinding(Keys.Enter, BindableCommand.AcceptActiveSnippets);

            AddBinding(Keys.P, Keys.Control, BindableCommand.Print);
            AddBinding(Keys.P, Keys.Control | Keys.Shift, BindableCommand.PrintPreview);

            AddBinding(Keys.F, Keys.Control, BindableCommand.ShowFind);
            AddBinding(Keys.H, Keys.Control, BindableCommand.ShowReplace);
            AddBinding(Keys.F3, BindableCommand.FindNext);
            AddBinding(Keys.F3, Keys.Shift, BindableCommand.FindPrevious);
            AddBinding(Keys.I, Keys.Control, BindableCommand.IncrementalSearch);

            AddBinding(Keys.Q, Keys.Control, BindableCommand.LineComment);
            AddBinding(Keys.Q, Keys.Control | Keys.Shift, BindableCommand.LineUncomment);

            AddBinding('-', Keys.Control, BindableCommand.DocumentNavigateBackward);
            AddBinding('-', Keys.Control | Keys.Shift, BindableCommand.DocumentNavigateForward);

            AddBinding(Keys.J, Keys.Control, BindableCommand.ShowSnippetList);

            AddBinding(Keys.M, Keys.Control, BindableCommand.DropMarkerDrop);
            AddBinding(Keys.Escape, BindableCommand.DropMarkerCollect);

            AddBinding(Keys.G, Keys.Control, BindableCommand.ShowGoTo);
        }

        #endregion Constructors


        #region Types

        private class CommandComparer : IComparer<BindableCommand>
        {
            #region Fields

            private Dictionary<BindableCommand, int> _commandOrder = new Dictionary<BindableCommand, int>();

            #endregion Fields


            #region Methods

            public int Compare(BindableCommand x, BindableCommand y)
            {
                return GetCommandOrder(y).CompareTo(GetCommandOrder(x));
            }


            private int GetCommandOrder(BindableCommand cmd)
            {
                if (!_commandOrder.ContainsKey(cmd))
                    return 0;
                return _commandOrder[cmd];
            }

            #endregion Methods


            #region Properties

            public Dictionary<BindableCommand, int> CommandOrder
            {
                get
                {
                    return _commandOrder;
                }
                set
                {
                    _commandOrder = value;
                }
            }

            #endregion Properties
        }

        #endregion Types
    }
}
