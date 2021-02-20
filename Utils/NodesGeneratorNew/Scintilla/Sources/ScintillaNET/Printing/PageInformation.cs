#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Class for determining how and what to print for a header or footer.
    /// </summary>
    public class PageInformation
    {
        #region Fields

        /// <summary>
        ///     Default font used for Page Information sections
        /// </summary>
        public static readonly Font DefaultFont = new Font(FontFamily.GenericSansSerif, 8F);

        private const int _iBorderSpace = 2;

        private int _iMargin;
        private Font _oFont;
        private PageInformationBorder _eBorder;
        private InformationType _eLeft;
        private InformationType _eCenter;
        private InformationType _eRight;

        #endregion Fields


        #region Methods

        /// <summary>
        ///     Draws the page information section in the specified rectangle
        /// </summary>
        /// <param name="oGraphics"></param>
        /// <param name="oBounds"></param>
        /// <param name="strDocumentName"></param>
        /// <param name="iPageNumber"></param>
        public void Draw(Graphics oGraphics, Rectangle oBounds, String strDocumentName, int iPageNumber)
        {
            StringFormat oFormat = new StringFormat(StringFormat.GenericDefault);
            Pen oPen = Pens.Black;
            Brush oBrush = Brushes.Black;

            // Draw border
            switch (_eBorder)
            {
                case PageInformationBorder.Top:
                    oGraphics.DrawLine(oPen, oBounds.Left, oBounds.Top, oBounds.Right, oBounds.Top);
                    break;
                case PageInformationBorder.Bottom:
                    oGraphics.DrawLine(oPen, oBounds.Left, oBounds.Bottom, oBounds.Right, oBounds.Bottom);
                    break;
                case PageInformationBorder.Box:
                    oGraphics.DrawRectangle(oPen, oBounds);
                    oBounds = new Rectangle(oBounds.Left + _iBorderSpace, oBounds.Top, oBounds.Width - (2 * _iBorderSpace), oBounds.Height);
                    break;
                case PageInformationBorder.None:
                default:
                    break;
            }

            // Center vertically
            oFormat.LineAlignment = StringAlignment.Center;

            // Draw left side
            oFormat.Alignment = StringAlignment.Near;
            switch (_eLeft)
            {
                case InformationType.DocumentName:
                    oGraphics.DrawString(strDocumentName, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    oGraphics.DrawString("Page " + iPageNumber, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }

            // Draw center
            oFormat.Alignment = StringAlignment.Center;
            switch (_eCenter)
            {
                case InformationType.DocumentName:
                    oGraphics.DrawString(strDocumentName, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    oGraphics.DrawString("Page " + iPageNumber, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }

            // Draw right side
            oFormat.Alignment = StringAlignment.Far;
            switch (_eRight)
            {
                case InformationType.DocumentName:
                    oGraphics.DrawString(strDocumentName, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    oGraphics.DrawString("Page " + iPageNumber, _oFont, oBrush, oBounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }
        }

        #endregion Methods


        #region Properties

        /// <summary>
        ///     Border style used for the Page Information section
        /// </summary>
        public virtual PageInformationBorder Border
        {
            get { return _eBorder; }
            set { _eBorder = value; }
        }


        /// <summary>
        ///     Information printed in the center of the Page Information section
        /// </summary>
        public virtual InformationType Center
        {
            get { return _eCenter; }
            set { _eCenter = value; }
        }


        /// <summary>
        ///     Whether there is a need to display this item, true if left, center, or right are not nothing.
        /// </summary>
        [Browsable(false)]
        public bool Display
        {
            get
            {
                return (_eLeft != InformationType.Nothing) ||
                    (_eCenter != InformationType.Nothing) ||
                    (_eRight != InformationType.Nothing);
            }
        }


        /// <summary>
        ///     Font used in printing the Page Information section
        /// </summary>
        public virtual Font Font
        {
            get { return _oFont; }
            set { _oFont = value; }
        }


        /// <summary>
        ///     Height required to draw the Page Information section based on the options selected.
        /// </summary>
        [Browsable(false)]
        public int Height
        {
            get
            {
                int iHeight = Font.Height;

                switch (_eBorder)
                {
                    case PageInformationBorder.Top:
                    case PageInformationBorder.Bottom:
                        iHeight += _iBorderSpace;
                        break;

                    case PageInformationBorder.Box:
                        iHeight += 2 * _iBorderSpace;
                        break;

                    case PageInformationBorder.None:
                    default:
                        break;
                }

                return iHeight;
            }
        }


        /// <summary>
        ///     Information printed on the left side of the Page Information section
        /// </summary>
        public virtual InformationType Left
        {
            get { return _eLeft; }
            set { _eLeft = value; }
        }


        /// <summary>
        ///     Space between the Page Information section and the rest of the page
        /// </summary>
        public virtual int Margin
        {
            get { return _iMargin; }
            set { _iMargin = value; }
        }


        /// <summary>
        ///     Information printed on the right side of the Page Information section
        /// </summary>
        public virtual InformationType Right
        {
            get { return _eRight; }
            set { _eRight = value; }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public PageInformation() : this(PageInformationBorder.None, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing)
        {
        }


        /// <summary>
        ///     Full Constructor
        /// </summary>
        /// <param name="iMargin">Margin to use</param>
        /// <param name="oFont">Font to use </param>
        /// <param name="eBorder">Border style</param>
        /// <param name="eLeft">What to print on the left side of the page</param>
        /// <param name="eCenter">What to print in the center of the page</param>
        /// <param name="eRight">What to print on the right side of the page</param>
        public PageInformation(int iMargin, Font oFont, PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight)
        {
            _iMargin = iMargin;
            _oFont = oFont;
            _eBorder = eBorder;
            _eLeft = eLeft;
            _eCenter = eCenter;
            _eRight = eRight;
        }


        /// <summary>
        ///     Normal Use Constructor
        /// </summary>
        /// <param name="eBorder">Border style</param>
        /// <param name="eLeft">What to print on the left side of the page</param>
        /// <param name="eCenter">What to print in the center of the page</param>
        /// <param name="eRight">What to print on the right side of the page</param>
        public PageInformation(PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight)
            : this(3, DefaultFont, eBorder, eLeft, eCenter, eRight)
        {
        }

        #endregion Constructors
    }
}
