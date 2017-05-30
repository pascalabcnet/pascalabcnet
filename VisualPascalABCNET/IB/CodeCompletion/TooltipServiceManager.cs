﻿// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            ICSharpCode.TextEditor.TextLocation logicPos = e.LogicalPosition;
            IDocument doc = textArea.Document;
            LineSegment seg = doc.GetLineSegment(logicPos.Y);
            string FileName = textArea.MotherTextEditorControl.FileName;
            PascalABCCompiler.Parsers.KeywordKind keyw = PascalABCCompiler.Parsers.KeywordKind.None;
            if (logicPos.X > seg.Length - 1)
                return null;
            //string expr = FindFullExpression(doc.TextContent, seg.Offset + logicPos.X,e.LogicalPosition.Line,e.LogicalPosition.Column);
            string expr_without_brackets = null;
            string expr = CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionFromAnyPosition(seg.Offset + logicPos.X, doc.TextContent, e.LogicalPosition.Line, e.LogicalPosition.Column, out keyw, out expr_without_brackets);
            if (expr == null)
                expr = expr_without_brackets;
            if (expr_without_brackets == null)
                return null;
            List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
            PascalABCCompiler.SyntaxTree.expression tree = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + Path.GetExtension(FileName), expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
            bool header = false;
            if (tree == null || Errors.Count > 0)
            {
                Errors.Clear();
                tree = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + Path.GetExtension(FileName), expr_without_brackets, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
                header = true;
                if (tree == null || Errors.Count > 0)
                    return null;
            }
            else
            {
                Errors.Clear();
                PascalABCCompiler.SyntaxTree.expression tree2 = WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test" + Path.GetExtension(FileName), expr_without_brackets, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
                //header = true;
                if (tree2 == null || Errors.Count > 0)
                    return null;
                //if (tree is PascalABCCompiler.SyntaxTree.new_expr && (tree as PascalABCCompiler.SyntaxTree.new_expr).params_list == null)
                //	tree = tree2;
            }
            CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[FileName];
            if (dconv == null) return null;
            return dconv.GetDescription(tree, FileName, expr_without_brackets, WorkbenchServiceFactory.Workbench.VisualEnvironmentCompiler.StandartCompiler.ParsersController, e.LogicalPosition.Line, e.LogicalPosition.Column, keyw, header);
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
