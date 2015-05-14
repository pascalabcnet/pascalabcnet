#region Using Directives

using System;
using System.ComponentModel;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class UndoRedo : TopLevelHelper
    {
        #region Methods

        public void BeginUndoAction()
        {
            NativeScintilla.BeginUndoAction();
        }


        public void EmptyUndoBuffer()
        {
            NativeScintilla.EmptyUndoBuffer();
        }


        public void EndUndoAction()
        {
            NativeScintilla.EndUndoAction();
        }


        public void Redo()
        {
            NativeScintilla.Redo();
        }


        private void ResetIsUndoEnabled()
        {
            IsUndoEnabled = true;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeIsUndoEnabled();
        }


        private bool ShouldSerializeIsUndoEnabled()
        {
            return !IsUndoEnabled;
        }


        public void Undo()
        {
            NativeScintilla.Undo();
        }

        #endregion Methods


        #region Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanRedo
        {
            get
            {
                return NativeScintilla.CanRedo();
            }
        }


        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanUndo
        {
            get
            {
                return NativeScintilla.CanUndo();
            }
        }


        public bool IsUndoEnabled
        {
            get
            {
                return NativeScintilla.GetUndoCollection();
            }
            set
            {
                NativeScintilla.SetUndoCollection(value);
            }
        }

        #endregion Properties


        #region Constructors

        internal UndoRedo(Scintilla scintilla) : base(scintilla)
        {
        }

        #endregion Constructors
    }
}
