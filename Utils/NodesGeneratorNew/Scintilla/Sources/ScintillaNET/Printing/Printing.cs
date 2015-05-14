#region Using Directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class Printing : TopLevelHelper
    {
        #region Fields

        private PrintDocument _printDocument;

        #endregion Fields


        #region Methods

        public bool Print()
        {
            return Print(true);
        }


        public bool Print(bool showPrintDialog)
        {
            if (showPrintDialog)
            {
                PrintDialog pd = new PrintDialog();
                pd.Document = _printDocument;
                pd.UseEXDialog = true;
                pd.AllowCurrentPage = true;
                pd.AllowSelection = true;
                pd.AllowSomePages = true;
                pd.PrinterSettings = PageSettings.PrinterSettings;

                if (pd.ShowDialog(Scintilla) == DialogResult.OK)
                {
                    _printDocument.PrinterSettings = pd.PrinterSettings;
                    _printDocument.Print();
                    return true;
                }

                return false;
            }

            _printDocument.Print();
            return true;
        }


        public DialogResult PrintPreview()
        {
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.WindowState = FormWindowState.Maximized;

            ppd.Document = _printDocument;
            return ppd.ShowDialog();
        }


        public DialogResult PrintPreview(IWin32Window owner)
        {
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.WindowState = FormWindowState.Maximized;

            if (owner is Form)
                ppd.Icon = ((Form)owner).Icon;

            ppd.Document = _printDocument;
            return ppd.ShowDialog(owner);
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializePageSettings() || ShouldSerializePrintDocument();
        }


        private bool ShouldSerializePageSettings()
        {
            return PageSettings.ShouldSerialize();
        }


        private bool ShouldSerializePrintDocument()
        {
            return _printDocument.ShouldSerialize();
        }


        public DialogResult ShowPageSetupDialog()
        {
            PageSetupDialog psd = new PageSetupDialog();
            psd.PageSettings = PageSettings;
            psd.PrinterSettings = PageSettings.PrinterSettings;
            return psd.ShowDialog();
        }


        public DialogResult ShowPageSetupDialog(IWin32Window owner)
        {
            PageSetupDialog psd = new PageSetupDialog();
            psd.AllowPrinter = true;
            psd.PageSettings = PageSettings;
            psd.PrinterSettings = PageSettings.PrinterSettings;

            return psd.ShowDialog(owner);
        }

        #endregion Methods


        #region Properties

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PageSettings PageSettings
        {
            get
            {
                return _printDocument.DefaultPageSettings as PageSettings;
            }
            set
            {
                _printDocument.DefaultPageSettings = value;
            }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PrintDocument PrintDocument
        {
            get
            {
                return _printDocument;
            }
            set
            {
                _printDocument = value;
            }
        }

        #endregion Properties


        #region Constructors

        internal Printing(Scintilla scintilla) : base(scintilla)
        {
            _printDocument = new PrintDocument(scintilla);
        }

        #endregion Constructors
    }
}
