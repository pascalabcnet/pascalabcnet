// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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
    public static class DefinitionByMouseClickManager
    {
        private static bool gotoInProgress;

        public static void DefinitionByMouseClickManager_TextAreaMouseMove(object sender, MouseEventArgs e)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                return;

            makeWordUnderline(sender, e);
        }

        public static void DefinitionByMouseClickManager_TextAreaMouseDown(object sender, EventArgs e)
        {
            if (!CodeCompletion.CodeCompletionController.IntellisenseAvailable())
                return;

            TextArea textArea = (TextArea)sender;
            if (Control.ModifierKeys == Keys.Control && textArea.SelectionManager.SelectionCollection.Count == 0)
            {
                
                gotoInProgress = true;
                if (CodeCompletionActionsManager.CanGoToRealization(textArea))
                {
                    CodeCompletionActionsManager.GotoRealization(textArea);
                }
                else if (CodeCompletionActionsManager.CanGoTo(textArea))
                {
                    CodeCompletionActionsManager.GotoDefinition(textArea);
                }
                gotoInProgress = false;
            }
        }

        private static bool isIdentifier(string s)
        {
            s = s.Replace("&", "");
            if (s == "")
                return false;
            for (int i = 0; i < s.Length; i++)
            {
                if (!(char.IsLetter(s[i]) && s[i] != ' ' || s[i] == '_' || i > 0 && char.IsDigit(s[i])))
                    return false;
            }
            return true;
        }

        private static void makeWordUnderline(object sender, MouseEventArgs e)
        {
            try
            {
                TextArea textArea = (TextArea)sender;
                if (gotoInProgress || textArea.SelectionManager.SelectionCollection.Count > 0)
                    return;
                if (Control.ModifierKeys != Keys.Control && textArea.TextView.Cursor == Cursors.IBeam)
                {
                    return;
                }
                //System.Threading.Thread.Sleep(10);
                //if (textArea != curTextArea) return;
                var markers = textArea.Document.MarkerStrategy.GetMarkers(0, textArea.Document.TextContent.Length);
                List<TextMarker> textMarkers = new List<TextMarker>();
                foreach (var marker in markers)
                {
                    if (marker is TextMarker && marker.TextMarkerType == TextMarkerType.Underlined)
                        textMarkers.Add(marker as TextMarker);
                }
                if (textMarkers.Count > 0 && Control.ModifierKeys != Keys.Control)
                {
                    textArea.TextView.Cursor = Cursors.IBeam;
                    foreach (var textMarker in textMarkers)
                    {
                        textArea.Document.MarkerStrategy.RemoveMarker(textMarker);
                    }
                    textArea.Refresh();
                }
                if (Control.ModifierKeys != Keys.Control)
                {
                    textArea.TextView.Cursor = Cursors.IBeam;
                    textArea.Refresh();
                }
                if (Control.ModifierKeys == Keys.Control)
                {
                    ICSharpCode.TextEditor.TextLocation logicPos = textArea.TextView.GetLogicalPosition(Math.Max(0, e.X - textArea.TextView.DrawingPosition.X),
                                                                                                        e.Y - textArea.TextView.DrawingPosition.Y);
                    IDocument doc = textArea.Document;
                    logicPos = textArea.Caret.ValidatePosition(logicPos);
                    LineSegment seg = doc.GetLineSegment(logicPos.Y);
                    if (logicPos.X <= seg.Length - 1)
                    {
                        foreach (TextWord tw in seg.Words)
                        {
                            if (tw.Offset <= logicPos.Column && tw.Offset + tw.Length >= logicPos.Column && isIdentifier(tw.Word))
                            {
                                if (textArea.Document.MarkerStrategy.GetMarkers(seg.Offset + tw.Offset).Count == 0)
                                {
                                    foreach (var textMarker in textMarkers)
                                    {
                                        textArea.Document.MarkerStrategy.RemoveMarker(textMarker);
                                    }
                                    textArea.Document.MarkerStrategy.AddMarker(new TextMarker(seg.Offset + tw.Offset, tw.Length, TextMarkerType.Underlined, Color.Blue));
                                    textArea.TextView.Cursor = Cursors.Hand;
                                    textArea.Refresh();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var textMarker in textMarkers)
                        {
                            textArea.Document.MarkerStrategy.RemoveMarker(textMarker);
                        }

                        textArea.TextView.Cursor = Cursors.IBeam;
                        textArea.Refresh();
                    }
                }
            }
            catch
            {

            }
        }
    }
}