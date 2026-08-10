// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace VisualPascalABC
{
    public class TooltipServiceManager
    {
        static DeclarationViewWindow dvw;

        private static string GetPopupHintText(TextArea textArea, ToolTipRequestEventArgs e)
        {
            TextLocation logicPos = e.LogicalPosition;
            IDocument doc = textArea.Document;
            LineSegment lineSegment = doc.GetLineSegment(logicPos.Y);
            
            string fileName = textArea.MotherTextEditorControl.FileName;

            if (logicPos.X > lineSegment.Length - 1)
                return null;

            var currentLanguage = CodeCompletion.CodeCompletionController.CurrentLanguage;
            CodeCompletion.DomConverter domConverter = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];

            return CodeCompletion.HoverService.GetDescription(
                currentLanguage,
                domConverter,
                fileName,
                doc.TextContent,
                lineSegment.Offset + logicPos.X,
                e.LogicalPosition.Line,
                e.LogicalPosition.Column);
        }

        static int _mouse_hint_x = 0, _mouse_hint_y = 0;
        static int _hint_hide_d = 0;
        static bool toolTipVisible = false;

        public static void ToolTipService_TextAreaMouseMove(object sender, MouseEventArgs e)
        {
            if (toolTipVisible)
            {
                if (Math.Sqrt((_mouse_hint_x - e.X) * (_mouse_hint_x - e.X) + (_mouse_hint_y - e.Y) * (_mouse_hint_y - e.Y)) > _hint_hide_d)
                    hideToolTip();
            }
        }

        public static void ToolTipService_TextAreaKeyDown(object sender, KeyEventArgs e)
        {
            hideToolTip();
        }

        public static void ToolTipService_TextAreaMouseEvent_HideToolTip(object sender, MouseEventArgs e)
        {
            hideToolTip();
        }

        public static void hideToolTip()
        {
            if (dvw != null)
            {
                dvw.Description = null;
                toolTipVisible = false;
            }
        }

        public static void ToolTipService_TextAreaToolTipRequest(object sender, ToolTipRequestEventArgs e)
        {
            if (!VisualPABCSingleton.MainForm.UserOptions.CodeCompletionHint)
                return;
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                return;
            try
            {
                TextArea textArea = sender as TextArea;
                if (dvw != null && dvw.Description != null)
                {
                    hideToolTip();
                    return;
                }
                if (e.ToolTipShown && dvw != null)
                {
                    hideToolTip();
                    return;
                }
                if (e.InDocument)
                {

                    if (dvw == null)
                    {
                        dvw = new DeclarationWindow(VisualPABCSingleton.MainForm);
                        dvw.Font = new System.Drawing.Font(Constants.CompletionWindowDeclarationViewWindowFontName, dvw.Font.Size);

                        dvw.HideOnClick = true;
                        //dvw.ShowDeclarationViewWindow();
                    }
                    int ypos = (textArea.Document.GetVisibleLine(e.LogicalPosition.Y) + 1) * textArea.TextView.FontHeight - textArea.VirtualTop.Y;
                    System.Drawing.Point p = new System.Drawing.Point(0, ypos);
                    p = textArea.PointToScreen(p);
                    p.X = Control.MousePosition.X + 3;
                    p.Y += 5;
                    string txt = GetPopupHintText(textArea, e);
                    dvw.Location = choose_location(p, txt);
                    dvw.Description = txt;

                    _hint_hide_d = dvw.Font.Height / 2;
                    _mouse_hint_x = e.MousePosition.X;
                    _mouse_hint_y = e.MousePosition.Y;
                    toolTipVisible = true;
                }
            }
            catch (System.Exception ex)
            {
                //VisualPABCSingleton.MainForm.WriteToOutputBox(ex.Message);// ICSharpCode.Core.MessageService.ShowError(ex);
            }
            finally
            {

            }
        }

        private static System.Drawing.Point choose_location(System.Drawing.Point p, string desc)
        {
            Graphics g = Graphics.FromHwnd(dvw.Handle);
            Size sz = Size.Ceiling(g.MeasureString(desc, dvw.Font, Screen.PrimaryScreen.WorkingArea.Width));
            if (p.X + sz.Width > Screen.PrimaryScreen.WorkingArea.Width)
            {
                p.X -= sz.Width - Screen.PrimaryScreen.WorkingArea.Width + p.X;
            }
            return p;
        }
    }
}
