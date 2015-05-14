#region Using Directives

using System;

#endregion Using Directives

namespace ScintillaNET
{
    /// <summary>
    ///     Used internally to signify an ignored parameter by overloads of SendMessageDirect
    ///     that match the native Scintilla's Message signatures.
    /// </summary>
    public enum VOID
    {
        NULL
    }
}
