using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisualPascalABCPlugins;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;

namespace VisualPascalABC
{
    public partial class DisassemblyWindow : BottomDockContentForm, IDisassemblyWindow
    {
        public DisassemblyWindow(Form1 MainForm):base(MainForm)
        {
            InitializeComponent();
            DisassemblyEditor.Document.HighlightingStrategy = ICSharpCode.TextEditor.Document.HighlightingManager.Manager.FindHighlighterForFile("prog.pas");
        }

        public void SetDisassembledCode(string code)
        {
            DisassemblyEditor.Document.TextContent = code;
            //DisassemblyEditor.Refresh();
        }

        public void ClearWindow()
        {
            SetDisassembledCode("");
        }

        public void ShowDisassembly()
        {
            var tp = WorkbenchServiceFactory.DebuggerManager.GetNativeCodeOfSelectedFunction();
            SetDisassembledCode(tp.Item1);
        }

        bool IDisassemblyWindow.IsVisible
        {
            get
            {
                return !this.IsHidden;
            }
        }
    }

    
}
