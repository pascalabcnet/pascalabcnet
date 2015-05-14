#region Using Directives

using System;
using System.Text;
using System.ComponentModel;
using System.Collections;

#endregion Using Directives


namespace ScintillaNET
{
    public abstract class ScintillaHelperBase : IDisposable
    {
        #region Fields

        private bool _isDisposed = false;
        private INativeScintilla _nativeScintilla;
        private Scintilla _scintilla;

        #endregion Fields


        #region Methods

        private void scintilla_Load(object sender, EventArgs e)
        {
            Initialize();
        }


        public virtual void Dispose()
        {
            _isDisposed = true;
        }


        //	[workitem:24911] 2000-10-04 Chris Rickard
        //	This was put in specifically for Markers but it actually makes a lot of sense for all
        //	ScintillaHelpers. 
        //	Original Code by fxa. I changed the implementation slightly so that I can make Equals
        //	abstract, forcing all Helpers to implement their own.
        /// <summary>
        ///     Abstract Equals Override. All Helpers must implement this. Use IsSameHelperFamily to
        ///     determine if the types are compatible and they have the same Scintilla. For most top 
        ///     level helpers like Caret and Lexing this should be enough. Helpers like Marker and
        ///     Line also need to take other variables into consideration.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract override bool Equals(object obj);


        //	Supress warning
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        protected internal virtual void Initialize(){}


        /// <summary>
        ///     Determines if obj belongs to the same Scintilla and is of compatible type
        /// </summary>
        protected bool IsSameHelperFamily(object obj)
        {
            ScintillaHelperBase other = obj as ScintillaHelperBase;
            if (other == null)
                return false;

            if (_scintilla == null || other._scintilla == null)
                return false;

             if(!this._scintilla.Equals(other._scintilla))
                 return false;

             return this.GetType().IsAssignableFrom(obj.GetType());
        }

        #endregion Methods


        #region Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
            set
            {
                _isDisposed = value;
            }
        }


        protected internal INativeScintilla NativeScintilla
        {
            get { return _nativeScintilla; }
        }


        protected internal Scintilla Scintilla
        {
            get { return _scintilla; }
            set 
            { 
                _scintilla = value;
                _nativeScintilla = (INativeScintilla)_scintilla;
            }
        }

        #endregion Properties


        #region Constructors

        protected internal ScintillaHelperBase(Scintilla scintilla)
        {
            _scintilla = scintilla;
            _nativeScintilla = (INativeScintilla)scintilla;
        }

        #endregion Constructors
    }
}
