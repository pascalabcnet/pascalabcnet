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
using PascalABCCompiler.Parsers;

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

            //string expr = FindFullExpression(doc.TextContent, seg.Offset + logicPos.X,e.LogicalPosition.Line,e.LogicalPosition.Column);

            IParser parser = CodeCompletion.CodeCompletionController.CurrentParser;

            string expr = parser.LanguageInformation.FindExpressionFromAnyPosition(
                lineSegment.Offset + logicPos.X, doc.TextContent, e.LogicalPosition.Line, e.LogicalPosition.Column, out var keyw, out var exprWithoutBrackets);

            // пока непонятно, когда такое может быть  EVA
            if (expr == null)
                expr = exprWithoutBrackets;

            if (expr == null)
                return null;

            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();

            PascalABCCompiler.SyntaxTree.expression expressionTree = parser.GetExpression("test" + Path.GetExtension(fileName),
                expr, Errors, Warnings);

            // такая вторая попытка нигде больше не встречается, пока добавил проверку, что анализируем Паскаль  EVA
            if (Errors.Count > 0 && parser == Languages.Facade.LanguageProvider.Instance.SelectLanguageByName(PascalABCCompiler.StringConstants.pascalLanguageName).Parser)
            {
                string s = expr.TrimStart();
                if (s.Length > 0 && s[0] == '^')
                {
                    Errors.Clear();
                    exprWithoutBrackets = exprWithoutBrackets.TrimStart().Substring(1);
                    expressionTree = parser.GetExpression("test" + Path.GetExtension(fileName), s.Substring(1), Errors, Warnings);
                }
            }

            List<PascalABCCompiler.Errors.Error> Errors2 = new List<PascalABCCompiler.Errors.Error>();

            // проверка для выражения без скобок
            PascalABCCompiler.SyntaxTree.expression expressionTree2 = parser.GetExpression("test" + Path.GetExtension(fileName), exprWithoutBrackets, Errors2, Warnings);

            // если не получается, то сразу возврат
            if (expressionTree2 == null || Errors2.Count > 0)
                return null;

            bool header = false;
            if (expressionTree == null || Errors.Count > 0)
            {
                header = true;
                expressionTree = expressionTree2;
            }

            CodeCompletion.DomConverter domConverter = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];

            if (domConverter == null) return null;

            return domConverter.GetDescription(expressionTree, fileName, exprWithoutBrackets, e.LogicalPosition.Line, e.LogicalPosition.Column, keyw, header);
        }

        static int _mouse_hint_x = 0, _mouse_hint_y = 0;
        static int _hint_hide_d = 0;
        static bool toolTipVisible = false;

        public static void ToolTipService_TextAreaMouseMove(object sender, MouseEventArgs e)
        {
            //System.IO.File.AppendAllText("d:\\logP.txt", "TextAreaMouseMove" + $" {toolTipVisible} {DateTime.Now}\n");
            if (toolTipVisible)
            {
                if (Math.Sqrt((_mouse_hint_x - e.X) * (_mouse_hint_x - e.X) + (_mouse_hint_y - e.Y) * (_mouse_hint_y - e.Y)) > _hint_hide_d)
                    hideToolTip();
            }
        }

        public static void ToolTipService_TextAreaKeyDown(object sender, KeyEventArgs e)
        {
            VisualPABCSingleton.MainForm.MenuActive = false;
            //VisualPABCSingleton.MainForm.Text = "MenuActive = " + false.ToString();
            hideToolTip();
        }

        public static void ToolTipService_TextAreaMouseEvent_HideToolTip(object sender, MouseEventArgs e)
        {
            VisualPABCSingleton.MainForm.MenuActive = false;
            //VisualPABCSingleton.MainForm.Text = "MenuActive = " + false.ToString();
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
            if (CodeCompletion.CodeCompletionController.CurrentParser == null)
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
					dvw.TextEditorControl = textArea.MotherTextEditorControl;
                    int ypos = (textArea.Document.GetVisibleLine(e.LogicalPosition.Y) + 1) * textArea.TextView.FontHeight - textArea.VirtualTop.Y;
                    System.Drawing.Point p = new System.Drawing.Point(0, ypos);
                    p = textArea.PointToScreen(p);
                    p.X = Control.MousePosition.X + 3;
                    p.Y += 5;
                    string txt = GetPopupHintText(textArea, e);
                    dvw.Location = choose_location(p, txt);
                    if (!VisualPABCSingleton.MainForm.MenuActive)
                    {
                        //dvw.ShowInTaskbar = false;
                        dvw.Description = txt;
                    }                            
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
