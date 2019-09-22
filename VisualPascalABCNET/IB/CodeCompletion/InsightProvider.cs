// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.TextEditor.Gui.InsightWindow;

namespace VisualPascalABC
{
    class DefaultInsightDataProvider : ICSharpCode.TextEditor.Gui.InsightWindow.IInsightDataProvider
    {
        private string fileName = null;
        private ICSharpCode.TextEditor.Document.IDocument document = null;
        private TextArea textArea = null;
        private string[] methods; //= new List<CodeCompletion.ProcScope>();
        private int lookupOffset;
        private bool setupOnlyOnce;
        private int initialOffset;
        private char pressed_key;
        public int defaultIndex = 0;
        public int num_param = 1;
        public int cur_param_num = 1;
        public int param_count;

        public DefaultInsightDataProvider(int lookupOffset, bool setupOnlyOnce, char pressed_key)
        {
            this.lookupOffset = lookupOffset;
            this.setupOnlyOnce = setupOnlyOnce;
            this.pressed_key = pressed_key;
        }

        private string FindExpression(int off, string Text, int line, int col)
        {
            if (CodeCompletion.CodeCompletionController.CurrentParser != null)
                return CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.FindExpressionForMethod(off, Text, line, col, pressed_key, ref num_param);
            return null;
        }

        public void SetupDataProvider(string fileName, TextArea textArea)
        {
            try
            {
                if (setupOnlyOnce && this.textArea != null)
                {
                    if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsMethodCallParameterSeparator(pressed_key))
                    {
                        FindExpression(textArea.Caret.Offset, textArea.Document.TextContent.Substring(0, textArea.Caret.Offset),
                                              textArea.Caret.Line, textArea.Caret.Column);
                        num_param++;
                        return;
                    }
                    else return;
                }
                this.fileName = fileName;
                this.textArea = textArea;
                this.document = textArea.Document;
                int useOffset = (lookupOffset < 0) ? textArea.Caret.Offset : lookupOffset;
                initialOffset = useOffset;
                int i = initialOffset - 1;
                int off = textArea.Caret.Offset;
                string Text = textArea.Document.TextContent.Substring(0, textArea.Caret.Offset);
                int line = textArea.Caret.Line;
                int col = textArea.Caret.Column;

                string expr = FindExpression(off, Text, line, col);
                List<PascalABCCompiler.Errors.Error> Errors = new List<PascalABCCompiler.Errors.Error>();
                PascalABCCompiler.SyntaxTree.expression e = VisualPABCSingleton.MainForm.VisualEnvironmentCompiler.StandartCompiler.ParsersController.GetExpression("test.pas", expr, Errors, new List<PascalABCCompiler.Errors.CompilerWarning>());
                if (e == null || Errors.Count > 0) return;
                CodeCompletion.DomConverter dconv = (CodeCompletion.DomConverter)CodeCompletion.CodeCompletionController.comp_modules[fileName];
                string fname = fileName;
                if (dconv != null)
                {
                    //if (pressed_key == '(' || pressed_key == ',')
                    if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsOpenBracketForMethodCall(pressed_key) || CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsMethodCallParameterSeparator(pressed_key))
                        methods = dconv.GetNameOfMethod(e, expr, line, col, num_param, ref defaultIndex, cur_param_num, out param_count);
                    else if (CodeCompletion.CodeCompletionController.CurrentParser.LanguageInformation.IsOpenBracketForIndex(pressed_key))
                        methods = dconv.GetIndex(e, line, col);
                }
            }
            catch (Exception e)
            {

            }
        }

        public bool CaretOffsetChanged()
        {
            bool closeDataProvider = textArea.Caret.Offset <= initialOffset;
            int brackets = 0;
            if (!closeDataProvider)
            {
                bool insideChar = false;
                bool insideString = false;
                for (int offset = initialOffset; offset < Math.Min(textArea.Caret.Offset, document.TextLength); ++offset)
                {
                    char ch = document.GetCharAt(offset);
                    switch (ch)
                    {
                        case '\'':
                            insideChar = !insideChar;
                            break;
                        case '[':
                            if (!(insideChar || insideString))
                            {
                                ++brackets;
                            }
                            break;
                        case ']':
                            if (!(insideChar || insideString))
                            {
                                --brackets;
                            }
                            if (brackets <= 0)
                            {
                                return true;
                            }
                            break;
                        case '(':
                            if (!(insideChar || insideString))
                            {
                                ++brackets;
                            }
                            break;
                        case ')':
                            if (!(insideChar || insideString))
                            {
                                --brackets;
                            }
                            if (brackets <= 0)
                            {
                                return true;
                            }
                            break;
                        case ';':
                            if (!(insideChar || insideString))
                            {
                                return true;
                            }
                            break;
                    }
                }
            }

            return closeDataProvider;
        }

        public string GetInsightData(int number)
        {
            if (number >= methods.Length)
                number = methods.Length - 1;
            return methods[number];
        }

        public int InsightDataCount
        {
            get
            {
                if (methods != null)
                    return methods.Length;
                return 0;
            }
        }

        public int DefaultIndex
        {
            get
            {
                return Math.Min(defaultIndex, InsightDataCount-1);
            }
        }
    }
}

