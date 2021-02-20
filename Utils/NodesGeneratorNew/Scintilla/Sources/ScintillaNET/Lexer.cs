#region Using Directives

using System;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Built in lexers supported by Scintilla
    /// </summary>
    public enum Lexer
    {
        /// <summary>
        ///     No lexing is performed, the Containing application must respond to StyleNeeded events
        /// </summary>
        Container = 0,

        /// <summary>
        ///     No lexing is performed
        /// </summary>
        Null = 1,

        Python = 2,
        Cpp = 3,
        Hypertext = 4,
        Xml = 5,
        Perl = 6,
        Sql = 7,
        VB = 8,
        Properties = 9,
        ErrorList = 10,
        MakeFile = 11,
        Batch = 12,
        XCode = 13,
        Latex = 14,
        Lua = 15,
        Diff = 16,
        Conf = 17,
        Pascal = 18,
        Ave = 19,
        Ada = 20,
        Lisp = 21,
        Ruby = 22,
        Eiffel = 23,
        EiffelKw = 24,
        Tcl = 25,
        NnCronTab = 26,
        Bullant = 27,
        VBScript = 28,
        Asp = 29,
        Php = 30,
        Baan = 31,
        MatLab = 32,
        Scriptol = 33,
        Asm = 34,
        CppNoCase = 35,
        Fortran = 36,
        F77 = 37,
        Css = 38,
        Pov = 39,
        Lout = 40,
        EScript = 41,
        Ps = 42,
        Nsis = 43,
        Mmixal = 44,
        Clw = 45,
        ClwNoCase = 46,
        Lot = 47,
        Yaml = 48,
        Tex = 49,
        MetaPost = 50,
        PowerBasic = 51,
        Forth = 52,
        ErLang = 53,
        Octave = 54,
        MsSql = 55,
        Verilog = 56,
        Kix = 57,
        Gui4Cli = 58,
        Specman = 59,
        Au3 = 60,
        Apdl = 61,
        Bash = 62,
        Asn1 = 63,
        Vhdl = 64,
        Caml = 65,
        BlitzBasic = 66,
        PureBasic = 67,
        Haskell = 68,
        PhpScript = 69,
        Tads3 = 70,
        Rebol = 71,
        Smalltalk = 72,
        Flagship = 73,
        CSound = 74,
        FreeBasic = 75,
        InnoSetup = 76,
        Opal = 77,
        Spice = 78,
        D = 79,
        CMake = 80,
        Gap = 81,
        Plm = 82,
        Progress = 83,
        Automatic = 1000
    }
}
