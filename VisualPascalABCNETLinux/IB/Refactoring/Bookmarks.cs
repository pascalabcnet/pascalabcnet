using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VisualPascalABCPlugins;
using VisualPascalABC.Utils;
using Debugger;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;

namespace VisualPascalABC
{
    public class SDBookmark : Bookmark
    {
        public SDBookmark(string fileName, IDocument document, int lineNumber)
            : base(document, lineNumber)
        {
            this.fileName = fileName;
        }

        string fileName;

        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnFileNameChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler FileNameChanged;

        protected virtual void OnFileNameChanged(EventArgs e)
        {
            if (FileNameChanged != null)
            {
                FileNameChanged(this, e);
            }
        }

        bool isSaved = true;

        /// <summary>
        /// Gets/Sets if the bookmark should be saved to the project memento file.
        /// </summary>
        /// <remarks>
        /// Default is true, set this property to false if you are using the bookmark for
        /// something special like like "CurrentLineBookmark" in the debugger.
        /// </remarks>
        public bool IsSaved
        {
            get
            {
                return isSaved;
            }
            set
            {
                isSaved = value;
            }
        }

        bool isVisibleInBookmarkPad = true;

        /// <summary>
        /// Gets/Sets if the bookmark is shown in the bookmark pad.
        /// </summary>
        /// <remarks>
        /// Default is true, set this property to false if you are using the bookmark for
        /// something special like like "CurrentLineBookmark" in the debugger.
        /// </remarks>
        public bool IsVisibleInBookmarkPad
        {
            get
            {
                return isVisibleInBookmarkPad;
            }
            set
            {
                isVisibleInBookmarkPad = value;
            }
        }
    }

    public abstract class SDMarkerBookmark : SDBookmark
    {
        public SDMarkerBookmark(string fileName, IDocument document, int lineNumber)
            : base(fileName, document, lineNumber)
        {
            SetMarker();
        }

        IDocument oldDocument;
        TextMarker oldMarker;

        protected abstract TextMarker CreateMarker();

        void SetMarker()
        {
            RemoveMarker();
            if (Document != null)
            {
                TextMarker marker = CreateMarker();
                // Perform editor update
                Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, LineNumber));
                Document.CommitUpdate();
                oldMarker = marker;
            }
            oldDocument = Document;
        }

        protected override void OnDocumentChanged(EventArgs e)
        {
            base.OnDocumentChanged(e);
            SetMarker();
        }

        public void RemoveMarker()
        {
            if (oldDocument != null)
            {
                oldDocument.MarkerStrategy.RemoveMarker(oldMarker);
                oldDocument.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, LineNumber));
                oldDocument.CommitUpdate();
            }
            oldDocument = null;
            oldMarker = null;
        }
    }
    public class CurrentBreakpointBookmark : SDMarkerBookmark
    {
        static CurrentBreakpointBookmark instance;

        static int startLine;
        static int startColumn;
        static int endLine;
        static int endColumn;
        bool willBeHit;
        static readonly Color defaultColor = Color.FromArgb(180, 38, 38);
        /*public static void SetPosition(IViewContent viewContent, int makerStartLine, int makerStartColumn, int makerEndLine, int makerEndColumn)
        {
            ITextEditorControlProvider tecp = viewContent as ITextEditorControlProvider;
            if (tecp != null)
                SetPosition(tecp.TextEditorControl.FileName, tecp.TextEditorControl.Document, makerStartLine, makerStartColumn, makerEndLine, makerEndColumn);
            else
                Remove();
        }*/
        public virtual bool WillBeHit
        {
            get
            {
                return willBeHit;
            }
            set
            {
                willBeHit = value;
                if (Document != null)
                {
                    Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, LineNumber));
                    Document.CommitUpdate();
                }
            }
        }
        public static void SetPosition(string fileName, IDocument document, int makerStartLine, int makerStartColumn, int makerEndLine, int makerEndColumn)
        {
            Remove();

            startLine = makerStartLine;
            startColumn = makerStartColumn;
            endLine = makerEndLine;
            endColumn = makerEndColumn;

            LineSegment line = document.GetLineSegment(startLine - 1);
            instance = new CurrentBreakpointBookmark(fileName, document, startLine - 1);
            document.BookmarkManager.AddMark(instance);
            document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.LinesBetween, startLine - 1, endLine - 1));
            document.CommitUpdate();
        }

        public static void Remove()
        {
            if (instance != null)
            {
                instance.Document.BookmarkManager.RemoveMark(instance);
                instance.RemoveMarker();
                instance = null;
            }
        }

        public override bool CanToggle
        {
            get
            {
                return false;
            }
        }

        public CurrentBreakpointBookmark(string fileName, IDocument document, int startLine)
            : base(fileName, document, startLine)
        {
            this.IsSaved = false;
            this.IsVisibleInBookmarkPad = false;
        }

        public override void Draw(IconBarMargin margin, Graphics g, Point p)
        {
            margin.DrawBreakpoint(g, p.Y, IsEnabled, WillBeHit);
        }

        protected override TextMarker CreateMarker()
        {
            if (LineNumber >= Document.TotalNumberOfLines)
                LineNumber = Document.TotalNumberOfLines - 1;
            LineSegment lineSeg = Document.GetLineSegment(LineNumber);
            TextMarker marker = new TextMarker(lineSeg.Offset, lineSeg.Length, TextMarkerType.SolidBlock, defaultColor, Color.White);
            Document.MarkerStrategy.InsertMarker(0, marker);
            return marker;
        }
    }

    public class CurrentLineBookmark : SDMarkerBookmark
    {
        static CurrentLineBookmark instance;

        static int startLine;
        static int startColumn;
        static int endLine;
        static int endColumn;

        /*public static void SetPosition(IViewContent viewContent, int makerStartLine, int makerStartColumn, int makerEndLine, int makerEndColumn)
        {
            ITextEditorControlProvider tecp = viewContent as ITextEditorControlProvider;
            if (tecp != null)
                SetPosition(tecp.TextEditorControl.FileName, tecp.TextEditorControl.Document, makerStartLine, makerStartColumn, makerEndLine, makerEndColumn);
            else
                Remove();
        }*/

        public static void SetPosition(string fileName, IDocument document, int makerStartLine, int makerStartColumn, int makerEndLine, int makerEndColumn)
        {
            Remove();

            startLine = makerStartLine;
            startColumn = makerStartColumn;
            endLine = makerEndLine;
            endColumn = makerEndColumn;

            LineSegment line = document.GetLineSegment(startLine - 1);
            instance = new CurrentLineBookmark(fileName, document, startLine - 1);
            document.BookmarkManager.AddMark(instance);
            document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.LinesBetween, startLine - 1, endLine - 1));
            document.CommitUpdate();
        }

        public static void Remove()
        {
            if (instance != null)
            {
                instance.Document.BookmarkManager.RemoveMark(instance);
                instance.RemoveMarker();
                instance = null;
            }
        }

        public override bool CanToggle
        {
            get
            {
                return false;
            }
        }

        public CurrentLineBookmark(string fileName, IDocument document, int startLine)
            : base(fileName, document, startLine)
        {
            this.IsSaved = false;
            this.IsVisibleInBookmarkPad = false;
        }

        public override void Draw(IconBarMargin margin, Graphics g, Point p)
        {
            margin.DrawArrow(g, p.Y);
        }

        protected override TextMarker CreateMarker()
        {
            LineSegment lineSeg = Document.GetLineSegment(startLine - 1);
            //TextMarker marker = new TextMarker(lineSeg.Offset + startColumn - 1, Math.Max(endColumn - startColumn, 1), TextMarkerType.SolidBlock, Color.Yellow, Color.Blue);
            TextMarker marker = new TextMarker(lineSeg.Offset + startColumn - 1, Math.Max(endColumn - startColumn, 1), TextMarkerType.SolidBlock, Color.Yellow, Color.Blue);
            Document.MarkerStrategy.InsertMarker(0, marker);
            return marker;
        }
    }

    public enum DebugStatus
    {
        None,
        StepOver,
        StepIn
    }

    public class BreakPointFactory
    {
        public static Dictionary<CurrentBreakpointBookmark, Breakpoint> breakpoints = new Dictionary<CurrentBreakpointBookmark, Breakpoint>();
        public static DebugHelper dbg_helper;

        static BreakPointFactory()
        {

        }

        public static void AddBreakpoints(string FileName, TextArea text_area)
        {
            List<Breakpoint> br_list = dbg_helper.GetBreakpointsInFile(FileName);
            foreach (Breakpoint br in br_list)
            {
                ToggleBreakpointAtByOpen(text_area.Document, br.SourcecodeSegment.SourceFullFilename, br.SourcecodeSegment.StartLine-1, br);
            }
        }

        public static void ToggleBreakpointAtByOpen(IDocument document, string fileName, int lineNumber, Breakpoint br)
        {
            foreach (char ch in document.GetText(document.GetLineSegment(lineNumber)))
            {
                if (!char.IsWhiteSpace(ch))
                {
                    CurrentBreakpointBookmark cbb = new CurrentBreakpointBookmark(fileName, document, lineNumber);
                    breakpoints.Add(cbb, br);
                    document.BookmarkManager.AddMark(cbb);
                    document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, lineNumber));
                    document.CommitUpdate();
                    break;
                }
            }
        }

        public static void ToggleBreakpointAt(IDocument document, string fileName, int lineNumber)
        {
            foreach (Bookmark m in document.BookmarkManager.Marks)
            {
                CurrentBreakpointBookmark breakpoint = m as CurrentBreakpointBookmark;
                if (breakpoint != null)
                {
                    if (breakpoint.LineNumber == lineNumber)
                    {
                        document.BookmarkManager.RemoveMark(m);
                        try
                        {
                            dbg_helper.RemoveBreakpoint(breakpoints[breakpoint]);
                        }
                        catch (System.Exception e)
                        {
                        }
                        breakpoints.Remove(breakpoint);
                        breakpoint.RemoveMarker();
                        return;
                    }
                }
            }
            foreach (char ch in document.GetText(document.GetLineSegment(lineNumber)))
            {
                if (!char.IsWhiteSpace(ch))
                {
                    CurrentBreakpointBookmark cbb = new CurrentBreakpointBookmark(fileName, document, lineNumber);
                    breakpoints.Add(cbb, dbg_helper.AddBreakPoint(fileName, lineNumber + 1));
                    document.BookmarkManager.AddMark(cbb);
                    document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.SingleLine, lineNumber));
                    document.CommitUpdate();
                    break;
                }
            }
        }

        public static void IconBarMouseDown(AbstractMargin iconBar, Point mousepos, MouseButtons mouseButtons)
        {
            try
            {
                if (mouseButtons != MouseButtons.Left) return;
                Rectangle viewRect = iconBar.TextArea.TextView.DrawingPosition;
                //Point logicPos = iconBar.TextArea.TextView.GetLogicalPosition(0, mousepos.Y - viewRect.Top);
                ICSharpCode.TextEditor.TextLocation logicPos = iconBar.TextArea.TextView.GetLogicalPosition(0, mousepos.Y - viewRect.Top);
                if (logicPos.Y >= 0 && logicPos.Y < iconBar.TextArea.Document.TotalNumberOfLines)
                {
                    string s = iconBar.TextArea.MotherTextEditorControl.FileName;
                    if (s == null) s = TextAreaHelper.GetFileNameByTextArea(iconBar.TextArea);
                    ToggleBreakpointAt(iconBar.TextArea.Document, s, logicPos.Y);
                    iconBar.TextArea.Refresh(iconBar);
                }
            }
            catch (System.Exception e)
            {
            }
        }
    }

}