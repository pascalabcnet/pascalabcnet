#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;

#endregion Using Directives


namespace ScintillaNET
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class HeaderInformation : PageInformation
    {
        #region Methods

        private void ResetBorder()
        {
            Border = PageInformationBorder.Bottom;
        }


        private void ResetCenter()
        {
            Center = InformationType.Nothing;
        }


        private void ResetFont()
        {
            Font = DefaultFont;
        }


        private void ResetLeft()
        {
            Left = InformationType.DocumentName;
        }


        private void ResetMargin()
        {
            Margin = 3;
        }


        private void ResetRight()
        {
            Right = InformationType.PageNumber;
        }


        internal bool ShouldSerialize()
        {
            return ShouldSerializeBorder() ||
                ShouldSerializeCenter() ||
                ShouldSerializeFont() ||
                ShouldSerializeLeft() ||
                ShouldSerializeMargin() ||
                ShouldSerializeRight();
        }


        private bool ShouldSerializeBorder()
        {
            return Border != PageInformationBorder.Bottom;
        }


        private bool ShouldSerializeCenter()
        {
            return Center != InformationType.Nothing;
        }


        private bool ShouldSerializeFont()
        {
            return !DefaultFont.Equals(Font);
        }


        private bool ShouldSerializeLeft()
        {
            return Left != InformationType.DocumentName;
        }


        private bool ShouldSerializeMargin()
        {
            return Margin != 3;
        }


        private bool ShouldSerializeRight()
        {
            return Right != InformationType.PageNumber;
        }

        #endregion Methods


        #region Properties

        public override PageInformationBorder Border
        {
            get
            {
                return base.Border;
            }
            set
            {
                base.Border = value;
            }
        }


        public override InformationType Center
        {
            get
            {
                return base.Center;
            }
            set
            {
                base.Center = value;
            }
        }


        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }


        public override InformationType Left
        {
            get
            {
                return base.Left;
            }
            set
            {
                base.Left = value;
            }
        }


        public override int Margin
        {
            get
            {
                return base.Margin;
            }
            set
            {
                base.Margin = value;
            }
        }


        public override InformationType Right
        {
            get
            {
                return base.Right;
            }
            set
            {
                base.Right = value;
            }
        }

        #endregion Properties


        #region Constructors

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public HeaderInformation() : base(PageInformationBorder.None, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing)
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
        public HeaderInformation(int iMargin, Font oFont, PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight) : base(iMargin, oFont, eBorder, eLeft, eCenter, eRight)
        {
        }


        /// <summary>
        ///     Normal Use Constructor
        /// </summary>
        /// <param name="eBorder">Border style</param>
        /// <param name="eLeft">What to print on the left side of the page</param>
        /// <param name="eCenter">What to print in the center of the page</param>
        /// <param name="eRight">What to print on the right side of the page</param>
        public HeaderInformation(PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight) : base(3, DefaultFont, eBorder, eLeft, eCenter, eRight)
        {
        }

        #endregion Constructors
    }
}
