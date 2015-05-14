#region Using Directives

using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ScintillaNET;

#endregion Using Directives


namespace SCide
{
    internal sealed partial class MainForm : Form
    {
        #region Constants

        private const string NEW_DOCUMENT_TEXT = "Untitled";
        private const int LINE_NUMBERS_MARGIN_WIDTH = 35; // TODO Don't hardcode this

        #endregion Constants


        #region Fields

        private int _newDocumentCount = 0;
        private string[] _args;
        private int _zoomLevel;

        #endregion Fields


        #region Methods

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
                aboutForm.ShowDialog(this);
        }


        private void autoCompleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.AutoComplete.Show();
        }


        private void clearBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Markers.DeleteAll(0);
        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Close();
        }


        private void collectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.DropMarkers.Collect();
        }


        private void commentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LineComment);
        }


        private void commentStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.StreamComment);
        }


        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Copy();
        }


        private void csToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("cs");
        }


        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Cut();
        }


        private void dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            // Update the main form _text to show the current document
            if (ActiveDocument != null)
                this.Text = String.Format(CultureInfo.CurrentCulture, "{0} - {1}", ActiveDocument.Text, Program.Title);
            else
                this.Text = Program.Title;
        }


        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.DropMarkers.Drop();
        }


        private void endOfLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle EOL visibility for all open files
            endOfLineToolStripMenuItem.Checked = !endOfLineToolStripMenuItem.Checked;
            foreach (DocumentForm doc in dockPanel.Documents)
            {
                doc.Scintilla.EndOfLine.IsVisible = endOfLineToolStripMenuItem.Checked;
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void exportAsHtmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.ExportAsHtml();
        }


        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Coming someday...
        }


        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowFind();
        }


        private void foldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                foreach (Line l in ActiveDocument.Scintilla.Lines)
                {
                    l.FoldExpanded = true;
                }
            }
        }


        private void foldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Lines.Current.FoldExpanded = true;
        }


        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.GoTo.ShowGoToDialog();
        }


        private void htmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("html");
        }


        private void iniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("ini");
        }


        private void insertSnippetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Snippets.ShowSnippetList();
        }


        private void lineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the line numbers margin for all documents
            lineNumbersToolStripMenuItem.Checked = !lineNumbersToolStripMenuItem.Checked;
            foreach (DocumentForm docForm in dockPanel.Documents)
            {
                if (lineNumbersToolStripMenuItem.Checked)
                    docForm.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
                else
                    docForm.Scintilla.Margins.Margin0.Width = 0;
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            if (_args != null && _args.Length != 0)
            {
                // Open the document specified on the command line
                FileInfo fi = new FileInfo(_args[0]);
                if (fi.Exists)
                    OpenFile(fi.FullName);
            }
            else
            {
                // Create a new document
                NewDocument();
            }
        }


        private void makeLowerCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LowerCase);
        }


        private void makeUpperCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.UpperCase);
        }


        private void mssqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("mssql");
        }


        private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.DocumentNavigation.NavigateBackward();
        }


        private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.DocumentNavigation.NavigateForward();
        }


        private DocumentForm NewDocument()
        {
            DocumentForm doc = new DocumentForm();
            SetScintillaToCurrentOptions(doc);
            doc.Text = String.Format(CultureInfo.CurrentCulture, "{0}{1}", NEW_DOCUMENT_TEXT, ++_newDocumentCount);
            doc.Show(dockPanel);
            toolIncremental.Searcher.Scintilla = doc.Scintilla;
            return doc;
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewDocument();
        }


        private void nextBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Scintilla.Lines.Current.FindNextMarker(1);
            if (l != null)
                l.Goto();
        }


        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            foreach (string filePath in openFileDialog.FileNames)
            {
                // Ensure this file isn't already open
                bool isOpen = false;
                foreach (DocumentForm documentForm in dockPanel.Documents)
                {
                    if (filePath.Equals(documentForm.FilePath, StringComparison.OrdinalIgnoreCase))
                    {
                        documentForm.Select();
                        isOpen = true;
                        break;
                    }
                }

                // Open the files
                if (!isOpen)
                    OpenFile(filePath);
            }
        }


        private DocumentForm OpenFile(string filePath)
        {
            DocumentForm doc = new DocumentForm();
            SetScintillaToCurrentOptions(doc);
            doc.Scintilla.Text = File.ReadAllText(filePath);
            doc.Scintilla.UndoRedo.EmptyUndoBuffer();
            doc.Scintilla.Modified = false;
            doc.Text = Path.GetFileName(filePath);
            doc.FilePath = filePath;
            doc.Show(dockPanel);
            toolIncremental.Searcher.Scintilla = doc.Scintilla;

            return doc;
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }


        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Clipboard.Paste();
        }


        private void plainTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage(String.Empty);
        }


        private void previosBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Scintilla.Lines.Current.FindPreviousMarker(1);
            if (l != null)
                l.Goto();
        }


        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.PrintPreview();
        }


        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Printing.Print();
        }


        private void pythonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("python");
        }


        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Redo();
        }


        private void replaceInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //	Coming someday...
        }


        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.FindReplace.ShowReplace();
        }


        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _zoomLevel = 0;
            UpdateAllScintillaZoom();
        }


        private void saveAllStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DocumentForm doc in dockPanel.Documents)
            {
                doc.Activate();
                doc.Save();
            }
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.SaveAs();
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Save();
        }


        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Selection.SelectAll();
        }


        private void SetLanguage(string language)
        {
            if ("ini".Equals(language, StringComparison.OrdinalIgnoreCase))
            {
                // Reset/set all styles and prepare _scintilla for custom lexing
                ActiveDocument.IniLexer = true;
                IniLexer.Init(ActiveDocument.Scintilla);
            }
            else
            {
                // Use a built-in lexer and configuration
                ActiveDocument.IniLexer = false;
                ActiveDocument.Scintilla.ConfigurationManager.Language = language;

                // Smart indenting...
                if ("cs".Equals(language, StringComparison.OrdinalIgnoreCase))
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = SmartIndent.CPP;
                else
                    ActiveDocument.Scintilla.Indentation.SmartIndentType = SmartIndent.None;
            }
        }


        private void SetScintillaToCurrentOptions(DocumentForm doc)
        {
            // Turn on line numbers?
            if (lineNumbersToolStripMenuItem.Checked)
                doc.Scintilla.Margins.Margin0.Width = LINE_NUMBERS_MARGIN_WIDTH;
            else
                doc.Scintilla.Margins.Margin0.Width = 0;

            // Turn on white space?
            if (whitespaceToolStripMenuItem.Checked)
                doc.Scintilla.Whitespace.Mode = WhitespaceMode.VisibleAlways;
            else
                doc.Scintilla.Whitespace.Mode = WhitespaceMode.Invisible;

            // Turn on word wrap?
            if (wordWrapToolStripMenuItem.Checked)
                doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
            else
                doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;

            // Show EOL?
            doc.Scintilla.EndOfLine.IsVisible = endOfLineToolStripMenuItem.Checked;

            // Set the zoom
            doc.Scintilla.Zoom = _zoomLevel;
        }


        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the visibility of the status strip
            statusStrip.Visible = !statusStrip.Visible;
            statusBarToolStripMenuItem.Checked = statusStrip.Visible;
        }


        private void surroundWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Snippets.ShowSurroundWithList();
        }


        private void toggleBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Line currentLine = ActiveDocument.Scintilla.Lines.Current;
            if (ActiveDocument.Scintilla.Markers.GetMarkerMask(currentLine) == 0)
            {
                currentLine.AddMarker(0);
            }
            else
            {
                currentLine.DeleteMarker(0);
            }
        }


        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the visibility of the tool bar
            toolStrip.Visible = !toolStrip.Visible;
            toolBarToolStripMenuItem.Checked = toolStrip.Visible;
        }


        private void uncommentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveDocument.Scintilla.Commands.Execute(BindableCommand.LineUncomment);
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.UndoRedo.Undo();
        }


        private void unfoldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
            {
                foreach (Line l in ActiveDocument.Scintilla.Lines)
                {
                    l.FoldExpanded = true;
                }
            }
        }


        private void unfoldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveDocument != null)
                ActiveDocument.Scintilla.Lines.Current.FoldExpanded = false;
        }


        private void UpdateAllScintillaZoom()
        {
            // Update zoom level for all files
            foreach (DocumentForm doc in dockPanel.Documents)
                doc.Scintilla.Zoom = _zoomLevel;
        }


        private void vbScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("vbscript");
        }


        private void whitespaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle the whitespace mode for all open files
            whitespaceToolStripMenuItem.Checked = !whitespaceToolStripMenuItem.Checked;
            foreach (DocumentForm doc in dockPanel.Documents)
            {
                if (whitespaceToolStripMenuItem.Checked)
                    doc.Scintilla.Whitespace.Mode = WhitespaceMode.VisibleAlways;
                else
                    doc.Scintilla.Whitespace.Mode = WhitespaceMode.Invisible;
            }
        }


        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Toggle word wrap for all open files
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;
            foreach (DocumentForm doc in dockPanel.Documents)
            {
                if (wordWrapToolStripMenuItem.Checked)
                    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.Word;
                else
                    doc.Scintilla.LineWrapping.Mode = LineWrappingMode.None;
            }
        }


        private void xmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetLanguage("xml");
        }


        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Increase the zoom for all open files
            _zoomLevel++;
            UpdateAllScintillaZoom();
        }


        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _zoomLevel--;
            UpdateAllScintillaZoom();
        }

        #endregion Methods


        #region Properties

        public DocumentForm ActiveDocument
        {
            get
            {
                return dockPanel.ActiveDocument as DocumentForm;
            }
        }

        #endregion Properties


        #region Constructors

        public MainForm()
        {
            // The VS designer doesn't allow the form icon to be specified
            // from a resource, so we'll do it manually here
            this.Icon = Properties.Resources.IconApplication;

            InitializeComponent();
        }


        public MainForm(string[] args) : this()
        {
            // Store the command line args
            this._args = args;

            //	I personally really dislike the OfficeXP look on Windows XP with the blue.
            ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderer();
            renderer.ColorTable.UseSystemColors = true;
            renderer.RoundedEdges = false;
            ToolStripManager.Renderer = renderer;

            // Set the application title
            Text = Program.Title;
            aboutToolStripMenuItem.Text = String.Format(CultureInfo.CurrentCulture, "&About {0}", Program.Title);
        }

        #endregion Constructors
    }
}