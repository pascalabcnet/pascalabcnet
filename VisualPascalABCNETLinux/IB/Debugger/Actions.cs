// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using ICSharpCode.TextEditor;

namespace VisualPascalABC
{
    public class AddToWatchAction : ICSharpCode.TextEditor.Actions.AbstractEditAction
    {

        public override void Execute(TextArea textArea)
        {
            try
            {
                int num_line = 0;

                string var = null;
                if (!textArea.SelectionManager.HasSomethingSelected)
                    var = WorkbenchServiceFactory.DebuggerManager.GetVariable(textArea, out num_line);
                else
                {
                    var = textArea.SelectionManager.SelectedText;
                    //num_line = textArea.SelectionManager.SelectionStart.Line;
                }
                if (var == null || var == "")
                {
                    WorkbenchServiceFactory.DebuggerOperationsService.GotoWatch();
                    return;
                    //VisualPABCSingleton.MainForm.SetCursorInWatch();
                }
                ValueItem vi = WorkbenchServiceFactory.DebuggerManager.FindVarByName(var, num_line) as ValueItem;
                if (vi != null)
                {
                    VisualPABCSingleton.MainForm.AddVariable(vi);
                }
                else
                {
                    WorkbenchServiceFactory.DebuggerOperationsService.AddVariable(var, true);
                }
            }
            catch (System.Exception)
            {
                WorkbenchServiceFactory.DebuggerOperationsService.GotoWatch();
            }
        }
    }
}