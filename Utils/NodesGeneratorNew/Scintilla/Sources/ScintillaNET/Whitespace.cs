#region Using Directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Reflection;

#endregion Using Directives


namespace ScintillaNET
{
    /// <summary>
    ///     Determines how whitespace should be displayed in a <see cref="Scintilla"/> control.
    /// </summary>
    /// <remarks>
    ///     By default, whitespace is determined by the lexer in use. Setting the <see cref="ForeColor"/>
    ///     or <see cref="BackColor"/> properties overrides the lexer behavior.
    /// </remarks>
    [TypeConverterAttribute(typeof(WhitespaceConverter))]
    public class Whitespace : TopLevelHelper
    {
        #region Constants

        private const string BACK_COLORBAG = "Whitespace.BackColor";
        private const string FORE_COLORBAG = "Whitespace.ForeColor";
        private const string ALPHA_EXCEPTION = "Transparent colors are not supported."; // TODO Move into resource file.

        #endregion Constants


        #region Properties

        /// <summary>
        ///     Gets or sets the whitespace background color.
        /// </summary>
        /// <remarks>
        ///     By default, the whitespace background color is determined by the lexer in use.
        ///     Setting the <c>BackColor</c> to anything other than <see cref="Color.Empty"/> overrides the lexer behavior.
        ///     Transparent colors are not supported.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Color"/> that represents the background color of whitespace characters.
        ///     The default is <see cref="Color.Empty"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRange">
        ///     The specified <paramref name="value"/> has an alpha value that is less that <see cref="Byte.MaxValue"/>.
        /// </exception>
        [DefaultValue(typeof(Color), ""), NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Appearance"), Description("The background color of whitespace characters.")]
        public Color BackColor
        {
            get
            {
                Color value;
                Scintilla.ColorBag.TryGetValue(BACK_COLORBAG, out value);
                return value;
            }
            set
            {
                if (value != Color.Empty && value.A < Byte.MaxValue)
                    throw new ArgumentException(ALPHA_EXCEPTION);

                if (value != BackColor)
                {
                    if (value == Color.Empty)
                    {
                        Scintilla.ColorBag.Remove(BACK_COLORBAG);
                        NativeScintilla.SetWhitespaceBack(false, 0);
                    }
                    else
                    {
                        Scintilla.ColorBag[BACK_COLORBAG] = value;
                        NativeScintilla.SetWhitespaceBack(true, Utilities.ColorToRgb(value));
                    }
                }
            }
        }


        /// <summary>
        ///     Gets or sets the whitespace foreground color.
        /// </summary>
        /// <remarks>
        ///     By default, the whitespace foreground color is determined by the lexer in use.
        ///     Setting the <c>ForeColor</c> to anything other than <see cref="Color.Empty"/> overrides the lexer behavior.
        ///     Transparent colors are not supported.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Color"/> that represents the foreground color of whitespace characters.
        ///     The default is <see cref="Color.Empty"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRange">
        ///     The specified <paramref name="value"/> has an alpha value that is less that <see cref="Byte.MaxValue"/>.
        /// </exception>
        [DefaultValue(typeof(Color), ""), NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Appearance"), Description("The foreground color of whitespace characters.")]
        public Color ForeColor
        {
            get
            {
                Color value;
                Scintilla.ColorBag.TryGetValue(FORE_COLORBAG, out value);
                return value;
            }
            set
            {
                if (value != Color.Empty && value.A < Byte.MaxValue)
                    throw new ArgumentException(ALPHA_EXCEPTION);

                if (value != ForeColor)
                {
                    if (value == Color.Empty)
                    {
                        Scintilla.ColorBag.Remove(FORE_COLORBAG);
                        NativeScintilla.SetWhitespaceFore(false, 0xFFFFFF);
                    }
                    else
                    {
                        Scintilla.ColorBag[FORE_COLORBAG] = value;
                        NativeScintilla.SetWhitespaceFore(true, Utilities.ColorToRgb(value));
                    }
                }
            }
        }


        /// <summary>
        ///     Gets or sets the whitespace display mode.
        /// </summary>
        /// <returns>One of the <see cref="WhitespaceMode"/> values. The default is <see cref="WhitespaceMode.Invisible"/></returns>
        /// <exception cref="InvalidEnumArgumentException">
        ///     The specified value is not a valid <see cref="WhitespaceMode"/> value.
        /// </exception>
        [DefaultValue(WhitespaceMode.Invisible), NotifyParentProperty(true), RefreshProperties(RefreshProperties.Repaint)]
        [Category("Appearance"), Description("The mode used to display whitespace.")]
        public WhitespaceMode Mode
        {
            get { return (WhitespaceMode)NativeScintilla.GetViewWs(); }
            set
            {
                if (!Enum.IsDefined(typeof(WhitespaceMode), value))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(WhitespaceMode));

                if (value != Mode)
                    NativeScintilla.SetViewWs((int)value);
            }
        }

        #endregion Properties


        #region Constructors

        internal Whitespace(Scintilla scintilla) : base(scintilla)
        {
        }

        #endregion Constructors


        #region Types

        // This is an experimental type converter that will automatically build a string representation
        // of all the public properties on the object. It provides better user feedback than the standard ExpandableObjectConverter.
        // It is currently private to the Whitespace class because that is the only place it has been tested
        // but it has obvious applications outside of this one class.
        private class WhitespaceConverter : ExpandableObjectConverter
        {
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                // We only care about string representations...
                if (destinationType != typeof(String))
                    base.ConvertTo(context, culture, value, destinationType);

                try
                {
                    // Enumerate all the public properties
                    StringBuilder sb = new StringBuilder();
                    foreach (PropertyInfo pi in context.PropertyDescriptor.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        // Look for a DefaultAttribute and see if the value has changed
                        object[] attributes = pi.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                        if (attributes != null && attributes.Length > 0)
                        {
                            DefaultValueAttribute dv = (DefaultValueAttribute)attributes[0];
                            object propertyValue = pi.GetValue(value, null);
                            if (!dv.Value.Equals(propertyValue))
                            {
                                // Create a string representation of the value
                                if (sb.Length > 0)
                                    sb.Append("; ");

                                sb.Append(pi.Name);
                                sb.Append("=");

                                TypeConverter tc = TypeDescriptor.GetConverter(pi.PropertyType);
                                if (tc != null)
                                    sb.Append(tc.ConvertToString(propertyValue));
                                else if (propertyValue != null)
                                    sb.Append(propertyValue.ToString());
                            }
                        }
                        else
                        {
                            // TODO Look for a "ShouldSerialize..." method to determine if the value has changed
                        }
                    }

                    return sb.ToString();
                }
                catch
                {
                    // Don't throw an exception, just don't display anything
                    return String.Empty;
                }
            }
        }

        #endregion Types
    }
}
