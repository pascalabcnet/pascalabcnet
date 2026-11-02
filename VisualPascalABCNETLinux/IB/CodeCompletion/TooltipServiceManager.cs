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
        static DeclarationWindow dvw;

        private static string GetPopupHintText(TextArea textArea, ToolTipRequestEventArgs e)
        {
            TextLocation logicPos = e.LogicalPosition;
            IDocument doc = textArea.Document;
            LineSegment lineSegment = doc.GetLineSegment(logicPos.Y);

            string fileName = textArea.MotherTextEditorControl.FileName;

            if (logicPos.X > lineSegment.Length - 1)
                return null;

            //string expr = FindFullExpression(doc.TextContent, seg.Offset + logicPos.X,e.LogicalPosition.Line,e.LogicalPosition.Column);

            var currentLanguage = CodeCompletion.CodeCompletionController.CurrentLanguage;

            string expr = currentLanguage.LanguageIntellisenseSupport.FindExpressionFromAnyPosition(
                lineSegment.Offset + logicPos.X, doc.TextContent, e.LogicalPosition.Line, e.LogicalPosition.Column, out var keyw, out var exprWithoutBrackets);

            // пока непонятно, когда такое может быть  EVA
            if (expr == null)
                expr = exprWithoutBrackets;

            if (expr == null)
                return null;

            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            List<PascalABCCompiler.Errors.CompilerWarning> Warnings = new List<PascalABCCompiler.Errors.CompilerWarning>();

            PascalABCCompiler.SyntaxTree.expression expressionTree = currentLanguage.Parser.GetExpression("test" + Path.GetExtension(fileName),
                expr, Errors, Warnings);

            // Пока добавили проверку, что анализируем Паскаль  EVA
            if (Errors.Count > 0 && currentLanguage == Languages.Facade.LanguageProvider.Instance.MainLanguage)
            {
                string s = expr.TrimStart();
                if (s.Length > 0 && s[0] == '^')
                {
                    Errors.Clear();
                    exprWithoutBrackets = exprWithoutBrackets.TrimStart().Substring(1);
                    expressionTree = currentLanguage.Parser.GetExpression("test" + Path.GetExtension(fileName), s.Substring(1), Errors, Warnings);
                }
            }

            List<PascalABCCompiler.Errors.Error> Errors2 = new List<PascalABCCompiler.Errors.Error>();

            // проверка для выражения без скобок
            PascalABCCompiler.SyntaxTree.expression expressionTree2 = currentLanguage.Parser.GetExpression("test" + Path.GetExtension(fileName), exprWithoutBrackets, Errors2, Warnings);

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

            if (domConverter == null)
            {
                HoverTrace.Write("no-analysis file=" + fileName);
                return null;
            }

            return domConverter.GetDescription(expressionTree, fileName, exprWithoutBrackets, e.LogicalPosition.Line, e.LogicalPosition.Column, keyw, header);
        }

        static int _mouse_hint_x = 0, _mouse_hint_y = 0;
        static int _hint_hide_d = 0;
        static bool toolTipVisible = false;
        private static TextArea hintTextArea;

        public static void ToolTipService_TextAreaMouseMove(object sender, MouseEventArgs e)
        {
            HoverTrace.MouseMove((TextArea)sender, e);
            //System.IO.File.AppendAllText("d:\\logP.txt", "TextAreaMouseMove" + $" {toolTipVisible} {DateTime.Now}\n");
            if (toolTipVisible && ReferenceEquals(sender, hintTextArea))
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
            HoverTrace.Write("hide-tooltip");
            if (dvw != null)
            {
                dvw.Description = null;
                toolTipVisible = false;
            }
            hintTextArea = null;
        }

        public static void ToolTipService_TextAreaToolTipRequest(object sender, ToolTipRequestEventArgs e)
        {
            TextArea textArea = sender as TextArea;
            if (textArea == null) return;
            if (HoverTrace.Enabled)
                HoverTrace.Write("request file=" + ((TextArea)sender).MotherTextEditorControl.FileName +
                    " position=" + e.LogicalPosition + " inDocument=" + e.InDocument +
                    " hintEnabled=" + VisualPABCSingleton.MainForm.UserOptions.CodeCompletionHint +
                    " intellisense=" + CodeCompletion.CodeCompletionController.IntellisenseAvailable() +
                    " menuActive=" + VisualPABCSingleton.MainForm.MenuActive + " otherTooltip=" + e.ToolTipShown);
            // Every Linux editor has its own hover timer, including hidden tabs.
            // Such a timer must not show or close another editor's description.
            if (!textArea.Visible || !e.InDocument)
            {
                HoverTrace.Write("ignore-hidden-or-outside-editor");
                if (ReferenceEquals(textArea, hintTextArea)) hideToolTip();
                return;
            }
            if (!VisualPABCSingleton.MainForm.UserOptions.CodeCompletionHint)
                return;
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                return;
            try
            {
                if (VisualPABCSingleton.MainForm.MenuActive)
                {
                    HoverTrace.Write("menu-active");
                    hideToolTip();
                    return;
                }
                if (e.ToolTipShown)
                {
                    if (ReferenceEquals(textArea, hintTextArea)) hideToolTip();
                    return;
                }
                if (dvw != null && dvw.Visible && dvw.Description != null && ReferenceEquals(textArea, hintTextArea))
                {
                    HoverTrace.Write("keep-existing-tooltip");
                    return;
                }
                if (e.InDocument)
                {

                    if (dvw == null)
                    {
                        dvw = new DeclarationWindow(VisualPABCSingleton.MainForm);
                        HoverTrace.Observe(dvw);
                        dvw.Font = Constants.CompletionFont;
						
                        //dvw.ShowDeclarationViewWindow();
                    }
                    int ypos = (textArea.Document.GetVisibleLine(e.LogicalPosition.Y) + 1) * textArea.TextView.FontHeight - textArea.VirtualTop.Y;
                    System.Drawing.Point p = new System.Drawing.Point(0, ypos);
                    p = textArea.PointToScreen(p);
                    p.X = Control.MousePosition.X + 3;
                    p.Y += 5;
                    string txt = GetPopupHintText(textArea, e);
                    HoverTrace.Write("description-length=" + (txt == null ? -1 : txt.Length));
                    if (string.IsNullOrEmpty(txt))
                    {
                        if (ReferenceEquals(textArea, hintTextArea)) hideToolTip();
                        return;
                    }
                    dvw.Location = p;
                    if (!VisualPABCSingleton.MainForm.MenuActive)
                    {
                        //dvw.ShowInTaskbar = false;
                        hintTextArea = textArea;
                        dvw.Description = txt;
                        HoverTrace.Write("show-result visible=" + dvw.Visible + " size=" + dvw.ClientSize + " bounds=" + dvw.Bounds);
                    }                            
                    _hint_hide_d = dvw.Font.Height / 2;
                    _mouse_hint_x = e.MousePosition.X;
                    _mouse_hint_y = e.MousePosition.Y;
                    toolTipVisible = dvw.Visible;
                }
            }
            catch (System.Exception ex)
            {
                HoverTrace.Write("ERROR " + ex);
                //VisualPABCSingleton.MainForm.WriteToOutputBox(ex.Message);// ICSharpCode.Core.MessageService.ShowError(ex);
            }
            finally
            {

            }
        }

    }
}
